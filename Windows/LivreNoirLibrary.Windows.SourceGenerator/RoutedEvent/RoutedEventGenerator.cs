using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LivreNoirLibrary.Windows.SourceGenerator
{
    [Generator]
    public class RoutedEventGenerator : IIncrementalGenerator
    {
        public const string RoutedEvent = nameof(RoutedEvent);
        public const string AttributeName = "LivreNoirLibrary.Windows.RoutedEventAttribute";
        public const string BaseTypeName = "System.Windows.IInputElement";

        public const string Event = nameof(Event);
        public const string EventName = nameof(EventName);
        public const string Strategy = nameof(Strategy);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // ObservablePropertyAttribute が付与されたフィールドを収集
            var provider = context.SyntaxProvider
                .ForAttributeWithMetadataName(AttributeName, IsMatch, CreateInfo)
                .Where(f => f is not null)
                .Collect();

            // 収集したフィールド情報をもとにコードを生成
            context.RegisterSourceOutput(provider, Emit);
        }

        private bool IsMatch(SyntaxNode node, CancellationToken c) => node is VariableDeclaratorSyntax;

        private RoutedEventInfo? CreateInfo(GeneratorAttributeSyntaxContext context, CancellationToken c)
        {
            RoutedEventInfo? result = new();
            switch (context.TargetSymbol)
            {
                case IFieldSymbol fieldSymbol:
                    break;
                case IEventSymbol eventSymbol:
                    result.IsEvent = true;
                    break;
                default:
                    return null;
            }
            // 名前空間とクラス名
            var targetSymbol = context.TargetSymbol;
            var containingType = targetSymbol.ContainingType;
            result.IsStatic = containingType.IsStatic;
            result.Namespace = targetSymbol.ContainingNamespace.IsGlobalNamespace ? "" : targetSymbol.ContainingNamespace.ToDisplayString();
            var keyword = containingType.TypeKind switch
            {
                TypeKind.Struct => containingType.IsRecord ? "record struct" : "struct",
                TypeKind.Interface => "interface",
                _ => containingType.IsRecord ? "record" : "class",
            };
            var containingTypeName = containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            result.ContainingType = $"{keyword} {containingTypeName}";
            // 出力ファイル名
            var targetNode = context.TargetNode;
            var path = Path.GetFileNameWithoutExtension(targetNode.SyntaxTree.FilePath);
            result.FilePath = $"{path}.g.gs";

            // 引数の整理
            var attr = context.Attributes[0];
            // オーナー/ハンドラー型
            ITypeSymbol? ownerType, handlerType;
            if (result.IsEvent)
            {
                ownerType = containingType;
                handlerType = (targetSymbol as IEventSymbol)!.Type;
            }
            else
            {
                var ctorArgs = attr.ConstructorArguments;
                switch (ctorArgs.Length)
                {
                    case 1:
                        handlerType = null;
                        break;
                    case 2:
                        if (ctorArgs[1].Value is not ITypeSymbol ca3)
                        {
                            return null;
                        }
                        handlerType = ca3;
                        break;
                    default:
                        return null;
                }
                if (ctorArgs[0].Value is not ITypeSymbol ca2)
                {
                    return null;
                }
                ownerType = ca2;
            }
            // IInputElementを継承していないオーナータイプは不正
            if (!Utils.IsDerivedFrom(ownerType, BaseTypeName))
            {
                return null;
            }
            result.OwnerType = ownerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            result.HandlerType = handlerType is not null ? handlerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) : "RoutedEventHandler" ;
            // その他の引数
            foreach (var arg in context.Attributes[0].NamedArguments)
            {
                switch (arg.Key)
                {
                    case EventName when arg.Value.Value is string s:
                        result.EventName = s;
                        break;
                    case Strategy when arg.Value.Value is int v:
                        result.RoutingStragegy = (RoutingStrategy)v;
                        break;
                }
            }
            // イベント名
            if (string.IsNullOrEmpty(result.EventName))
            {
                var memberName = targetSymbol.Name;
                if (memberName.EndsWith(Event, StringComparison.Ordinal))
                {
                    result.EventName = memberName.Substring(0, memberName.Length - Event.Length);
                }
                else
                {
                    result.EventName = memberName;
                }
            }

            return result;
        }

        private void Emit(SourceProductionContext context, ImmutableArray<RoutedEventInfo?> infos)
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();

            // ソースファイル名でグループ化
            foreach (var group in infos.GroupBy(f => f!.FilePath))
            {
                sb.Length = 0;
                // ヘッダー
                sb.AppendLine($$"""
// <auto-generated/>
#nullable enable
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604

using System;
using System.Windows;
using System.Runtime.CompilerServices;

""");
                // 名前空間でグループ化
                foreach (var namespaceGroup in group.GroupBy(f => f!.Namespace))
                {
                    var @namespace = namespaceGroup.Key;
                    var global = string.IsNullOrEmpty(@namespace);
                    // 名前空間の開始
                    if (!global)
                    {
                        sb.AppendLine($$"""
namespace {{@namespace}}
{
""");
                    }

                    // クラス名でグループ化
                    foreach (var classGroup in namespaceGroup.GroupBy(f => f!.ContainingType))
                    {
                        // クラスごとの内容
                        sb.AppendLine($$"""
    partial {{classGroup.Key}}
    {
""");
                        AppendClassInfo(sb, sb2, classGroup);
                        sb.AppendLine("""
    }
""");
                    }

                    // 名前空間の終了
                    if (!global)
                    {
                        sb.AppendLine("}");
                    }
                }

                // ソースコードを追加
                context.AddSource(group.Key, sb.ToString());
            }
        }

        private void AppendClassInfo(StringBuilder sb, StringBuilder sb2, IEnumerable<RoutedEventInfo?> infos)
        {
            // RegisterEvent メソッド
            sb2.Length = 0;
            var registerCount = 0;
            foreach (var info in infos)
            {
                if (info is not null && !info.IsEvent)
                {
                    sb2.AppendLine($$"""
                nameof({{info.EventName}}Event) => EventManager.RegisterRoutedEvent("{{info.EventName}}", RoutingStrategy.{{info.RoutingStragegy}}, typeof({{info.HandlerType}}), typeof({{info.OwnerType}})),
""");
                    registerCount++;
                }
            }
            if (registerCount > 0)
            {
                sb.AppendLine($$"""
        private static RoutedEvent RegisterEvent([CallerMemberName] string eventName = "")
        {
            return eventName switch
            {
""");
                sb.Append(sb2.ToString());
                sb.AppendLine($$"""
                _ => throw new NotImplementedException(),
            };
        }

""");
            }

            // Add/Remove
            foreach (var info in infos)
            {
                if (info!.IsEvent)
                {
                    // RoutedEventの登録
                    sb.AppendLine($$"""
        public static readonly RoutedEvent {{info.EventName}}Event = EventManager.RegisterRoutedEvent("{{info.EventName}}", RoutingStrategy.{{info.RoutingStragegy}}, typeof({{info.HandlerType}}), typeof({{info.OwnerType}}));

""");
                    // add/remove
                    sb.AppendLine($$"""
        public partial event {{info.HandlerType}}? {{info.EventName}} { add => AddHandler({{info.EventName}}Event, value); remove => RemoveHandler({{info.EventName}}Event, value); }

""");
                }
                else
                {
                    var prefix = info.IsStatic ? "this " : "";
                    sb.AppendLine($$"""
        public static void Add{{info.EventName}}Handler({{prefix}}DependencyObject d, {{info.HandlerType}} handler)
        {
            (d as {{info.OwnerType}})?.AddHandler({{info.EventName}}Event, handler);
        }
        public static void Remove{{info.EventName}}Handler({{prefix}}DependencyObject d, {{info.HandlerType}} handler)
        {
            (d as {{info.OwnerType}})?.RemoveHandler({{info.EventName}}Event, handler);
        }

""");
                }
            }
        }
    }
}
