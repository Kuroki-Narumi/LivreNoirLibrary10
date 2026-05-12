using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Linq;

namespace LivreNoirLibrary.Windows.SourceGenerator
{
    public class DependencyPropertyGenerator : IIncrementalGenerator
    {
        public const string DependencyProperty = nameof(DependencyProperty);
        public const string AttributeName = "LivreNoirLibrary.Windows.DependencyPropertyAttribute";
        public const string BaseTypeName = "System.Windows.DependencyObject";

        public static bool IsFlagSet(FrameworkPropertyMetadataOptions options, FrameworkPropertyMetadataOptions flag) => (options & flag) == flag;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // ObservablePropertyAttribute が付与されたフィールドを収集
            var provider = context.SyntaxProvider
                .ForAttributeWithMetadataName(AttributeName, IsMatch, CreatePropertyInfo)
                .Where(f => f is not null)
                .Collect();

            // 収集したフィールド情報をもとにコードを生成
            context.RegisterSourceOutput(provider, Emit);
        }

        private bool IsMatch(SyntaxNode node, CancellationToken c) => node is PropertyDeclarationSyntax { AttributeLists.Count: > 0 };

        private static readonly List<string> _attr_temp_list = [];

        private static DependencyPropertyInfo? CreatePropertyInfo(GeneratorAttributeSyntaxContext context, CancellationToken c)
        {
            INamedTypeSymbol? containingType;
            // プロパティである
            if (context.TargetSymbol is not IPropertySymbol propertySymbol ||
                // 対象のクラスが DependencyObject を継承している
                !Utils.IsDerivedFrom(containingType = propertySymbol.ContainingType, BaseTypeName))
            {
                return null;
            }


            var node = context.TargetNode;
            var filename = node.GetFilename();
            var usings = node.GetUsingList();
            var @namespace = containingType.GetNamespace();
            var containingTypeName = containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            var property = new TypeInfo(propertySymbol);
            var propertyType = property.Type;
            var propertyName = property.Name;

            Debug.WriteLine($"owner={containingTypeName}, {property.Type} {property.Name}");

            // メタデータオプション
            FrameworkPropertyMetadataOptions options = 0;
            foreach (var arg in context.Attributes[0].NamedArguments)
            {
                var key = arg.Key;
                var value = arg.Value;
                if (key is nameof(DependencyPropertyInfo.DefaultValue))
                {
                    continue;
                }
                if (key is "Options" && value.Value is int opt)
                {
                    options |= (FrameworkPropertyMetadataOptions)opt;
                    continue;
                }
                if (Enum.TryParse<FrameworkPropertyMetadataOptions>(key, out var enumOpt))
                {
                    if (value.Value is false)
                    {
                        options &= ~enumOpt;
                    }
                    else
                    {
                        options |= enumOpt;
                    }
                }
            }

            DependencyPropertyInfo info = new()
            {
                Filename = filename,
                Usings = usings,
                Namespace = @namespace,
                ContainingType = containingTypeName,
                Property = property,
                FieldName = $"__f__{propertyName}",
                Options = options,
            };
            if (node is PropertyDeclarationSyntax syntax)
            {
                // DependencyProperty以外の属性
                if (syntax.Ancestors().OfType<PropertyDeclarationSyntax>().FirstOrDefault() is { } anscestor)
                {
                    var list = _attr_temp_list;
                    foreach (var attr in anscestor.AttributeLists)
                    {
                    }
                    list.Clear();
                    info.Attributes = [.. anscestor.AttributeLists
                                                   .Select(attr => attr.ToString())
                                                   .Where(attr => !attr.Contains(DependencyProperty))];
                }
                // setter スコープ
                if (syntax.AccessorList?.Accessors.FirstOrDefault(a => a.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SetAccessorDeclaration)) is { } setterSyntax)
                {
                    info.SetterScope = setterSyntax.Modifiers.ToString();
                }
                // デフォルト値
            }

            // 検証及び変更通知メソッド
            var methods = MethodCache.Get(containingType);
            info.CoerceType = methods.CheckCoerce(propertyName, property.Type, propertyType);
            info.OnChangedArgCount = methods.CheckOnChange(propertyName, propertyType);

            return info;
        }

        const string BasicUsing = "using System.Windows;";

        private void Emit(SourceProductionContext context, ImmutableArray<DependencyPropertyInfo?> infos)
        {
            var sb_general = new StringBuilder();
            var sb_dp = new StringBuilder();
            var sb_coerce = new StringBuilder();
            var sb_changed = new StringBuilder();
            var sb_clr = new StringBuilder();

            // ソースファイル名でグループ化
            foreach (var group in infos.GroupBy(f => f!.Filename))
            {
                var filePath = group.Key;
                sb_general.Clear();
                sb_general.AppendLine($$"""
// <auto-generated/>
#nullable enable
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604
""");
                var usings = group.First()!.Usings;
                foreach (var @using in usings)
                {
                    sb_general.AppendLine(@using);
                }
                if (!usings.Contains(BasicUsing))
                {
                    sb_general.AppendLine(BasicUsing);
                }
                sb_general.AppendLine();

                // 名前空間でさらにグループ化
                foreach (var namespaceGroup in group.GroupBy(f => f!.Namespace))
                {
                    var @namespace = namespaceGroup.Key;
                    var global = string.IsNullOrEmpty(@namespace);
                    // 名前空間の開始
                    if (!global)
                    {
                        sb_general.AppendLine($$"""
namespace {{@namespace}}
{
""");
                    }
                    // クラス名でグループ化
                    foreach (var classGroup in namespaceGroup.GroupBy(f => f!.ContainingType))
                    {
                        // クラス本体
                        sb_general.AppendLine($$"""
    partial class {{classGroup.Key}}
    {
""");
                        AppendCore(classGroup, sb_general, sb_dp, sb_coerce, sb_changed, sb_clr);
                        sb_general.AppendLine("""
    }
""");
                    }
                    // 名前空間の終了
                    if (!global)
                    {
                        sb_general.AppendLine("}");
                    }
                }

                // ソースコードを追加
                try
                {
                    context.AddSource($"{filePath}.g.cs", sb_general.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(sb_general.ToString());
                    Console.WriteLine(ex);
                }
            }
        }

        private void AppendCore(
            IEnumerable<DependencyPropertyInfo?> infos,
            StringBuilder sb_general,
            StringBuilder sb_dp,
            StringBuilder sb_coerce,
            StringBuilder sb_changed,
            StringBuilder sb_clr
            )
        {
            sb_dp.Clear();
            sb_coerce.Clear();
            sb_changed.Clear();
            sb_clr.Clear();
            var second = false;
            var coerce_second = false;
            foreach (var info in infos)
            {
                if (second)
                {
                    sb_dp.AppendLine();
                    sb_changed.AppendLine();
                    sb_clr.AppendLine();
                }
                second = true;
                var ownerType = info!.ContainingType;
                var (type, propertyName, isValueType, isNullable) = info.Property;
                var defaultValue = info.DefaultValue;
                var fieldName = info.FieldName;
                var setterScope = $"{info.SetterScope} ";
                var isPublicSetter = string.IsNullOrWhiteSpace(setterScope);
                var coerceType = info.CoerceType;
                var onChangedArgCount = info.OnChangedArgCount;
                var options = info.Options;
                var dpType = type.TrimEnd('?');
                var convertFormat = isValueType ? $"({type}){{0}}" : $"{{0}} as {dpType}";
                var defaultText = string.IsNullOrEmpty(defaultValue) ? $"default({type})" : string.Format(convertFormat, defaultValue);

                // DependencyProperty
                if (isPublicSetter)
                {
                    sb_dp.AppendLine($$"""
        public static readonly DependencyProperty {{propertyName}}Property = DependencyProperty.Register(
""");
                }
                else
                {
                    sb_dp.AppendLine($$"""
        {{setterScope}}static readonly DependencyPropertyKey {{propertyName}}PropertyKey = DependencyProperty.RegisterReadOnly(
""");
                }
                sb_dp.AppendLine($$"""
            "{{propertyName}}",
            typeof({{dpType}}),
            typeof({{ownerType}}),
            new FrameworkPropertyMetadata(
                {{defaultText}},
                (FrameworkPropertyMetadataOptions){{(int)options}},
                On{{propertyName}}Changed{{(coerceType is not CoerceType.None ? $", Coerce{propertyName}" : "")}}
                )
""");
                if (!isValueType && !isNullable)
                {
                    sb_dp.AppendLine($$"""
            , value => value is not null
""");
                }
                sb_dp.AppendLine($$"""
                                    );
                        """);
                if (!isPublicSetter)
                {
                    sb_dp.AppendLine($$"""
        public static readonly DependencyProperty {{propertyName}}Property = {{propertyName}}PropertyKey.DependencyProperty;
""");
                }

                // coerce method
                if (coerceType is not CoerceType.None)
                {
                    if (coerce_second)
                    {
                        sb_coerce.AppendLine();
                    }
                    coerce_second = true;
                    sb_coerce.AppendLine($$"""
        public static object Coerce{{propertyName}}(DependencyObject d, object baseValue)
        {
""");
                    if (coerceType is CoerceType.Static)
                    {
                        sb_coerce.AppendLine($$"""
            return Coerce{{propertyName}}({{string.Format(convertFormat, "baseValue")}});
""");
                    }
                    else
                    {
                        sb_coerce.AppendLine($$"""
            return (d as {{ownerType}})!.Coerce{{propertyName}}({{string.Format(convertFormat, "baseValue")}});
""");
                    }
                    sb_coerce.AppendLine($$"""
        }
""");
                }

                // on changed handler
                sb_changed.AppendLine($$"""  
        private static void On{{propertyName}}Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is {{ownerType}} obj)
            {
                var value = {{string.Format(convertFormat, "e.NewValue")}};
                obj.{{fieldName}} = value;
""");
                switch (onChangedArgCount)
                {
                    case 0:
                        sb_changed.AppendLine($$"""
                obj.On{{propertyName}}Changed();
""");
                        break;
                    case 1:
                        sb_changed.AppendLine($$"""
                obj.On{{propertyName}}Changed(value);
""");
                        break;
                    case 2:
                        sb_changed.AppendLine($$"""
                obj.On{{propertyName}}Changed({{string.Format(convertFormat, "e.OldValue")}}, value);
""");
                        break;
                }
                if (IsFlagSet(options, FrameworkPropertyMetadataOptions.AffectsMeasure))
                {
                    sb_changed.AppendLine($$"""
                obj.InvalidateMeasure();
""");
                }
                else if (IsFlagSet(options, FrameworkPropertyMetadataOptions.AffectsArrange))
                {
                    sb_changed.AppendLine($$"""
                obj.InvalidateArrange();
""");
                }
                else if (IsFlagSet(options, FrameworkPropertyMetadataOptions.AffectsRender))
                {
                    sb_changed.AppendLine($$"""
                obj.InvalidateVisual();
""");
                }
                sb_changed.AppendLine($$"""
            }
        }
""");

                // clr property
                sb_clr.AppendLine($$"""
        private {{type}} {{fieldName}} = {{defaultText}};
""");
                foreach (var attribute in info.Attributes)
                {
                    sb_clr.AppendLine($$"""
        {{attribute}}
""");
                }
                sb_clr.AppendLine($$"""
        public partial {{type}} {{propertyName}}
        {
            get => {{fieldName}};
            {{setterScope}}set => SetValue({{propertyName}}Property{{(isPublicSetter ? "" : "Key")}}, value);
        }
""");
            }

            sb_general.AppendLine(sb_dp.ToString());
            if (sb_coerce.Length is > 0)
            {
                sb_general.AppendLine(sb_coerce.ToString());
            }
            sb_general.AppendLine(sb_changed.ToString());
            sb_general.Append(sb_clr.ToString());
        }
    }
}
