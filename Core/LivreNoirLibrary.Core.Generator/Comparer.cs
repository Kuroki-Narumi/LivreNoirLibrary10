using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Core.Generator;
using static Utils;

[Generator]
internal class Comparer : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(Generate);
    }

    private static string GetCastTarget(string type2, string fallback)
    {
        return type2 switch
        {
            Float => Float,
            Double or Rational => Double,
            Decimal => Decimal,
            _ => fallback,
        };
    }

    private static bool NeedsToCast(string type1, string type2, out string type)
    {
        type = type1;
        switch (type1)
        {
            case Rational:
                type = type2 is Float or Decimal ? type2 : Double;
                break;
            case Float:
                type = GetCastTarget(type2, Float);
                break;
            case Double:
                type = Double;
                break;
            case Decimal:
                type = Decimal;
                break;
            case Byte or SByte or Short or UShort or Int:
                type = GetCastTarget(type2, Int);
                break;
            case UInt or Long:
                type = GetCastTarget(type2, Long);
                break;
            case ULong:
                type = GetCastTarget(type2, ULong);
                break;
        }
        return type1 != type || type2 != type;
    }

    private static void Generate(IncrementalGeneratorPostInitializationContext context)
    {
        StringBuilder sb = new();
        sb.AppendLine("""
            using System;
            using System.Collections.Generic;
            using LivreNoirLibrary.Numerics;

            namespace LivreNoirLibrary.Collections
            {
            """);

        foreach (var type1 in Comparable)
        {
            foreach (var type2 in Comparable)
            {
                var template = NeedsToCast(type1, type2, out var type) ? Template_Cast : Template_Integer;
                sb.AppendLine(template.Replace(PH_Type1, type1).Replace(PH_Type2, type2).Replace(PH_Type, type));
            }
        }

        sb.AppendLine("""
            }
            """);
        context.AddSource("Comparer.g.cs", sb.ToString());
    }

    public const string Template_Integer = $$"""
            public readonly struct Comparer_{{PH_Type1}}_{{PH_Type2}} : IComparer<{{PH_Type1}}, {{PH_Type2}}>
            {
                public static int Compare({{PH_Type1}} x, {{PH_Type2}} y) => x.CompareTo(y);
                public static bool IsXCloserThanY({{PH_Type1}} x, {{PH_Type1}} y, {{PH_Type2}} z) => x + y - z * 2 is > 0;
            }
        """;

    public const string Template_Cast = $$"""
            public readonly struct Comparer_{{PH_Type1}}_{{PH_Type2}} : IComparer<{{PH_Type1}}, {{PH_Type2}}>
            {
                public static int Compare({{PH_Type1}} x, {{PH_Type2}} y) => (({{PH_Type}})x).CompareTo(({{PH_Type}})y);
                public static bool IsXCloserThanY({{PH_Type1}} x, {{PH_Type1}} y, {{PH_Type2}} z) => ({{PH_Type}})x + ({{PH_Type}})y - ({{PH_Type}})z * 2 is > 0;
            }
        """;
}