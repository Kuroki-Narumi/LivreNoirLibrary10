using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.SourceGenerator
{
    public static partial class Utils
    {
        public static TypeKey BuiltTypeKey(INamedTypeSymbol type, List<string> levels)
        {
            INamedTypeSymbol? t = type;
            while (t is not null)
            {
                var keyword = t.TypeKind switch
                {
                    TypeKind.Struct => t.IsRecord ? "record struct" : "struct",
                    TypeKind.Interface => "interface",
                    _ => t.IsRecord ? "record" : "class",
                };

                var tp = "";
                var typeParams = t.TypeParameters;
                if (typeParams.Length is > 0)
                {
                    tp = $"<{string.Join(", ", typeParams.Select(tp => tp.Name))}>";
                }
                levels.Add($"{keyword} {t.Name}{tp}");
                t = t.ContainingType;
            }

            levels.Reverse();

            var ns = type.ContainingNamespace is { IsGlobalNamespace: false } nsSym ? $"{nsSym.ToDisplayString()}." : "";

            var nestingKey = string.Join("|", levels);

            var hintNameCore = string.Join(".", levels.Select(l =>
                {
                    var span = l.AsSpan();
                    var nameOnly = span.Slice(span.LastIndexOf(' ') + 1);
                    var lt = nameOnly.IndexOf('<');
                    return (lt >= 0 ? nameOnly.Slice(0, lt) : nameOnly).ToString();
                }));

            var hintName = $"{ns}{hintNameCore}.RoutedEvents.g.cs";

            levels.Clear();
            return new TypeKey(ns, nestingKey, hintName);
        }

    }

    public readonly struct TypeKey(string @namespace, string nestingKey, string hintName): IEquatable<TypeKey>
    {
        public string Namespace { get; } = @namespace;
        public string NestingKey { get; } = nestingKey;
        public string HintName { get; } = hintName;

        public bool Equals(TypeKey other) => Namespace == other.Namespace && NestingKey == other.NestingKey && HintName == other.HintName;
    }
}
