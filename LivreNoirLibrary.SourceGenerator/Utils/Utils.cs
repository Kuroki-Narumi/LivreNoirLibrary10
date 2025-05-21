using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

public static class Utils
{
    public static bool IsDerivedFrom(ITypeSymbol? classSymbol, string fullName)
    {
        while (classSymbol is not null)
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
}