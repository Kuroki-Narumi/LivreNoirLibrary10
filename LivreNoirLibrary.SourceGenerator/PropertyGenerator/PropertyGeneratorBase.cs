using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

public abstract class PropertyGeneratorBase<T> : IIncrementalGenerator
    where T : FieldInfoBase, new()
{
    public abstract string AttributeName { get; }
    public abstract string BaseTypeName { get; }
    private static readonly Regex _regex_attribute_slice = new(@"[^.]+(?=Attribute$)");
    private string _attribute_slice = "";
    private string _generator_name = "";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var match = _regex_attribute_slice.Match(AttributeName);
        if (match.Success)
        {
            _attribute_slice = match.Value;
        }
        _generator_name = GetType().FullName;
        // ObservablePropertyAttribute が付与されたフィールドを収集
        var fieldProvider = context.SyntaxProvider
            .ForAttributeWithMetadataName(AttributeName, IsMatch, GetFieldInfo)
            .Where(f => f is not null)
            .Collect();

        // 収集したフィールド情報をもとにコードを生成
        context.RegisterSourceOutput(fieldProvider, Emit);
    }

    private bool IsMatch(SyntaxNode node, CancellationToken c)
    {
        return node is VariableDeclaratorSyntax;
    }

    private const string Name = nameof(Name);
    private const string SetterScope = nameof(SetterScope);

    private static readonly Dictionary<string, string[]> _usings = [];
    private static readonly Dictionary<string, string> _path_alias = [];
    private static readonly Dictionary<string, HashSet<string>> _alias_used = [];

    private T? GetFieldInfo(GeneratorAttributeSyntaxContext context, CancellationToken c)
    {
        if (context.TargetSymbol is not IFieldSymbol fieldSymbol)
        {
            return null;
        }
        var classSymbol = fieldSymbol.ContainingType;
        if (!Utils.IsDerivedFrom(classSymbol, BaseTypeName))
        {
            return null;
        }
        var node = context.TargetNode;
        var path = node.SyntaxTree.FilePath;
        var generatorName = _generator_name;
        var vPath = $"{_generator_name};{path}";
        if (!_path_alias.TryGetValue(vPath, out var alias))
        {
            if (!_alias_used.TryGetValue(generatorName, out var used))
            {
                used = [];
                _alias_used.Add(generatorName, used);
            }
            var parts = path.Split('\\', '.');
            var index = parts.Length - 1;
            if (parts[index] == "cs")
            {
                index--;
            }
            alias = parts[index];
            if (!used.Add(alias))
            {
                do
                {
                    index--;
                    alias = $"{parts[index]}.{alias}";
                } while (!used.Add(alias));
            }
            _path_alias.Add(vPath, alias);
        }
        if (!_usings.TryGetValue(path, out var usings))
        {
            if (node.AncestorsAndSelf().OfType<CompilationUnitSyntax>().FirstOrDefault() is CompilationUnitSyntax comp)
            {
                usings = [.. comp.Usings.Select(s => s.ToString()).Where(s => !string.IsNullOrWhiteSpace(s))];
            }
            usings ??= [];
            _usings.Add(path, usings);
        }
        TypeInfo field = new(fieldSymbol);
        TypeInfo property = new(field);
        var fieldType = field.Type;
        T info = new()
        {
            FilePath = alias,
            Usings = usings,
            Namespace = classSymbol.ContainingNamespace.IsGlobalNamespace ? "" : classSymbol.ContainingNamespace.ToDisplayString(),
            ClassName = classSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            Field = field,
            Property = property,
        };
        if (node is VariableDeclaratorSyntax syntax)
        {
            if (syntax.Initializer is not null)
            {
                info.DefaultValue = syntax.Initializer.Value.ToString();
            }
            if (syntax.Ancestors().OfType<FieldDeclarationSyntax>().FirstOrDefault() is FieldDeclarationSyntax fs)
            {
                info.Attributes = [.. fs.AttributeLists
                      .Select(attr => attr.ToString())
                      .Where(attr => !attr.Contains(_attribute_slice))];
            }
        }

        foreach (var arg in context.Attributes[0].NamedArguments)
        {
            var (argName, value) = (arg.Key, arg.Value);
            if (argName is Name)
            {
                if (value.Value is string prop)
                {
                    property.Name = prop;
                }
                continue;
            }
            if (argName is SetterScope)
            {
                if (value.Value is int scp)
                {
                    info.SetterScope = scp;
                }
                continue;
            }
            ApplyAttributeArg(arg.Key, arg.Value, info);
        }
        var propertyName = info.Property.Name;
        if (string.IsNullOrEmpty(propertyName) || propertyName == info.Field.Name)
        {
            propertyName = fieldSymbol.Name.TrimStart('_');
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            propertyName = $"{char.ToUpper(propertyName[0])}{propertyName.Substring(1)}";
            info.Property.Name = propertyName;
        }
        var methods = MethodCache.Get(classSymbol);
        info.CoerceHandler = methods.CheckCoerce(propertyName, info.Property.Type, fieldType);
        info.OnChangedHandler = methods.CheckOnChange(propertyName, fieldType);
        return ApplyExtraInfo(fieldSymbol, info);
    }

    protected virtual void ApplyAttributeArg(string argName, in TypedConstant value, T info) { }
    protected virtual T? ApplyExtraInfo(IFieldSymbol fieldSymbol, T info) => info;

    protected void Emit(SourceProductionContext context, ImmutableArray<T?> infos)
    {
        /*
        context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.GenericType, typeNode.Identifier.GetLocation(), typeSymbol.Name));
        return;
        //*/

        // ソースファイル名でグループ化
        foreach (var group in infos.GroupBy(f => f!.FilePath))
        {
            var filePath = group.Key;
            var usings = group.First()!.Usings;
            var sb = new StringBuilder();
            sb.AppendLine($$"""
                    // <auto-generated/>
                    #nullable enable
                    #pragma warning disable CS8600
                    #pragma warning disable CS8601
                    #pragma warning disable CS8602
                    #pragma warning disable CS8603
                    #pragma warning disable CS8604
                    """);
            foreach (var @using in usings)
            {
                sb.AppendLine(@using);
            }
            sb.AppendLine();

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
                foreach (var classGroup in namespaceGroup.GroupBy(f => f!.ClassName))
                {
                    // クラス本体
                    sb.AppendLine($$"""
                            partial class {{classGroup.Key}}
                            {
                        """);
                    AppendClass(sb, classGroup);
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
            context.AddSource($"{filePath}.g.cs", sb.ToString());
        }
    }

    protected virtual void AppendUsing(StringBuilder sb) { }

    protected virtual void AppendClass(StringBuilder sb, IEnumerable<T?> infos)
    {
        var second = false;
        foreach (var info in infos)
        {
            if (second)
            {
                sb.AppendLine();
            }
            AppendProperty(sb, info!);
            second = true;
        }
    }

    protected virtual void AppendProperty(StringBuilder sb, T info) { }
}