using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ObservablePropertyGenerator : IIncrementalGenerator
{
    private const string Namespace = "LivreNoirLibrary.Observable";
    private const string ObservablePropertyAttribute = nameof(ObservablePropertyAttribute);
    private const string ObservableObjectBase = nameof(ObservableObjectBase);
    private const string PropertyName = nameof(PropertyName);
    private const string Related = nameof(Related);
    private const string SetterScope = nameof(SetterScope);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // ObservablePropertyAttribute が付与されたフィールドを収集
        var fieldProvider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{Namespace}.{ObservablePropertyAttribute}",
                predicate: (node, _) => node is VariableDeclaratorSyntax,
                transform: (ctx, _) => GetFieldInfo(ctx))
            .Where(f => f is not null)
            .Collect();

        // 収集したフィールド情報をもとにコードを生成
        context.RegisterSourceOutput(fieldProvider, GenerateSource);
    }

    private class FieldInfo(string nameSpace, string className, string fieldType, string fieldName, string propName, int setterScope, bool validate, IEnumerable<string?> related)
    {
        public string Namespace { get; } = nameSpace;
        public string ClassName { get; } = className;
        public string FieldType { get; } = fieldType;
        public string FieldName { get; } = fieldName;
        public string PropertyName { get; } = propName;
        public int SetterScope { get; } = setterScope;
        public bool Validate { get; } = validate;
        public string[] Related { get; } = [.. related.Where(v => !string.IsNullOrEmpty(v) && v != propName).Select(v => v!)];
    }

    private static bool IsDerivedFromObservableObject(INamedTypeSymbol? classSymbol)
    {
        while (classSymbol is not null)
        {
            if (classSymbol.ToDisplayString() is $"{Namespace}.{ObservableObjectBase}")
            {
                return true;
            }
            classSymbol = classSymbol.BaseType;
        }
        return false;
    }

    private static readonly Dictionary<INamedTypeSymbol, IEnumerable<IMethodSymbol>> _method_cache = [];

    private static IEnumerable<IMethodSymbol> GetMethods(INamedTypeSymbol typeSymbol)
    {
        if (!_method_cache.TryGetValue(typeSymbol, out var methods))
        {
            methods = typeSymbol.GetMembers().OfType<IMethodSymbol>();
            _method_cache.Add(typeSymbol, methods);
        }
        return methods;
    }

    private FieldInfo? GetFieldInfo(GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetSymbol is not IFieldSymbol fieldSymbol)
        {
            return null;
        }
        var classSymbol = fieldSymbol.ContainingType;
        if (!IsDerivedFromObservableObject(classSymbol))
        {
            return null;
        }

        var propertyName = fieldSymbol.Name.TrimStart('_');
        if (string.IsNullOrEmpty(propertyName))
        {
            return null;
        }
        propertyName = $"{char.ToUpper(propertyName[0])}{propertyName.Substring(1)}";
        IEnumerable<string?> related = [];
        var setterScope = 0;
        foreach (var arg in context.Attributes[0].NamedArguments)
        {
            if (arg.Key is PropertyName && arg.Value.Value is string prop)
            {
                propertyName = prop;
            }
            else if (arg.Key is Related)
            {
                related = arg.Value.Values.Select(v => v.Value as string);
            }
            else if (arg.Key is SetterScope && arg.Value.Value is int scp)
            {
                setterScope = scp;
            }
        }

        var validate = false;
        foreach (var method in GetMethods(classSymbol))
        {
            if (method.Name == $"Validate{propertyName}" &&
                method.Parameters.Length is 1 && method.Parameters[0].Type.Equals(fieldSymbol.Type, SymbolEqualityComparer.Default) &&
                method.ReturnType.Equals(fieldSymbol.Type, SymbolEqualityComparer.Default)
                )
            {
                validate = true;
                break;
            }
        }

        return new FieldInfo(
            classSymbol.ContainingNamespace.IsGlobalNamespace ? "" : classSymbol.ContainingNamespace.ToDisplayString(),
            classSymbol.Name,
            fieldSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            fieldSymbol.Name,
            propertyName,
            setterScope,
            validate,
            related
            );
    }

    private void GenerateSource(SourceProductionContext context, ImmutableArray<FieldInfo?> fields)
    {
        // 名前空間ごとにグループ化
        foreach (var group in fields.Where(f => f is not null).GroupBy(f => f!.Namespace))
        {
            var sb = new StringBuilder();
            var @namespace = group.Key;
            var global = string.IsNullOrEmpty(@namespace);
            // 名前空間の開始
            if (!global)
            {
                sb.AppendLine($$"""
                    namespace {{@namespace}}
                    {
                    """);
            }
            // クラスごとにグループ化
            foreach (var classGroup in group.GroupBy(f => f!.ClassName))
            {
                var className = classGroup.Key;
                sb.AppendLine($$"""
                        partial class {{className}}
                        {
                    """);

                // プロパティ生成
                foreach (var field in classGroup)
                {
                    var scopeText = field!.SetterScope switch
                    {
                        1 => "private ",
                        2 => "protected ",
                        3 => "internal ",
                        4 => "internal protected ",
                        _ => "",
                    };
                    sb.AppendLine($$"""
                                public {{field.FieldType}} {{field.PropertyName}}
                                {
                                    get => {{field.FieldName}};
                                    {{scopeText}}set
                                    {
                        """);
                    if (field.Validate)
                    {
                        sb.AppendLine($"                value = Validate{field.PropertyName}(value);");
                    }
                    if (field.Related.Length is > 0)
                    {
                        sb.AppendLine($$"""
                                            if (SetProperty(ref {{field.FieldName}}, value))
                                            {
                            """);
                        foreach (var name in field.Related)
                        {
                            sb.AppendLine($"                    SendPropertyChanged(\"{name}\");");
                        }
                        sb.AppendLine("                }");
                    }
                    else
                    {
                        sb.AppendLine($"                SetProperty(ref {field.FieldName}, value);");
                    }
                    sb.AppendLine("""
                                        }
                                    }
                            """);
                }

                sb.AppendLine("    }");
            }
            // 名前空間の終了
            if (!global)
            {
                sb.AppendLine("}");
            }

            // ソースコードを追加
            context.AddSource($"{(global ? "__global__" : @namespace)}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }
}