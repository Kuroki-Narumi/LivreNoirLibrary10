using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Xml.Linq;

public static class Utils
{
    public static bool IsDerivedFrom(ITypeSymbol? classSymbol, string fullName)
    {
        while (classSymbol is not null && classSymbol.SpecialType is not SpecialType.System_Object)
        {
            if (classSymbol.ToDisplayString() == fullName)
            {
                return true;
            }
            classSymbol = classSymbol.BaseType;
        }
        return false;
    }

    public static string GetTypeFullname(ITypeSymbol symbol)
    {
        var name = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (!symbol.IsValueType && symbol.NullableAnnotation is NullableAnnotation.Annotated)
        {
            name += "?";
        }
        return name;
    }

    public static string GetNamespace(this ISymbol symbol)
    {
        var containingNamespace = symbol.ContainingNamespace;
        return containingNamespace.IsGlobalNamespace ? "" : containingNamespace.ToDisplayString();
    }

    public static string GetFilename(this SyntaxNode node) => node.SyntaxTree.FilePath;

    private static readonly Dictionary<string, string[]> _usings = [];

    public static string[] GetUsingList(this SyntaxNode node)
    {
        var filename = node.GetFilename();
        if (!_usings.TryGetValue(filename, out var usings))
        {
            if (node.AncestorsAndSelf().OfType<CompilationUnitSyntax>().FirstOrDefault() is { } comp)
            {
                usings = [.. comp.Usings.Select(s => s.ToString()).Where(s => !string.IsNullOrWhiteSpace(s))];
            }
            usings ??= [];
            _usings.Add(filename, usings);
        }
        return usings;
    }

    private static readonly Dictionary<string, HashSet<string>> _alias_usings = [];

    public static string GetAlias(string filename, string suffix)
    {
        if (!_alias_usings.TryGetValue(suffix, out var used))
        {
            used = [];
            _alias_usings.Add(suffix, used);
        }
        var parts = filename.Split('\\', '.');
        var index = parts.Length - 1;
        if (parts[index] == "cs")
        {
            index--;
        }
        var alias = parts[index];
        if (!used.Add(alias))
        {
            do
            {
                index--;
                alias = $"{parts[index]}.{alias}";
            } while (!used.Add(alias));
        }
        return alias;
    }
}