using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Core.Generator;
using static Utils;

[Generator]
internal class CollectionExtensions : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(Generate);
    }

    private static readonly (string, string, string)[] CollectionTypes = [
            ($"ReadOnlySpan<{PH_Type1}>", "span", $"ReadOnlySpan<{PH_Type1}>"),
            ($"Span<{PH_Type1}>", "span", $"ReadOnlySpan<{PH_Type1}>"),
            ($"{PH_Type1}[]", "array", $"ReadOnlySpan<{PH_Type1}>"),
            ($"List<{PH_Type1}>", "list", $"ReadOnlySpan<{PH_Type1}>"),
            ($"IList<{PH_Type1}>", "list", $"IEnumerable<{PH_Type1}>"),
        ];

    private static readonly string[] Comparable1 = [Int, Long, Float, Double, Decimal, Rational];
    private static readonly string[] Comparable2 = [Double, Decimal, Rational];

    private static void Generate(IncrementalGeneratorPostInitializationContext context)
    {
        StringBuilder sb = new();
        sb.AppendLine("""
            using System;
            using System.Collections.Generic;
            using System.Runtime.InteropServices;
            using LivreNoirLibrary.Numerics;

            namespace LivreNoirLibrary.Collections
            {
                public static partial class CollectionExtensions
                {
            """);

        Regex replacer = new($"({PH_Source}|{PH_Destination}|{PH_Target}|{PH_Type1}|{PH_Type2})");

        foreach (var type1 in Comparable1)
        {
            foreach (var type2 in Comparable2)
            {
                if (type1 == type2)
                {
                    continue;
                }
                foreach (var (collectionType, target, rangeType) in CollectionTypes)
                {
                    var colType = collectionType.Replace(PH_Type1, type1);
                    var rngType = rangeType.Replace(PH_Type1, type1);

                    var text = replacer.Replace(Template, match => match.Value switch
                    {
                        PH_Source => colType,
                        PH_Destination => rngType,
                        PH_Target => target,
                        PH_Type1 => type1,
                        PH_Type2 => type2,
                        _ => match.Value
                    });
                    sb.AppendLine(text);
                }
            }
        }

        sb.AppendLine("""
                }
            }
            """);
        context.AddSource("TrySearch.g.cs", sb.ToString());
    }

    public const string Template = $$"""
                public static bool TrySearch(this {{PH_Source}}{{PH_Target}}, {{PH_Type2}} value, SearchMode mode, out int index, out {{PH_Type1}} actualValue)
                {
                    return TrySearch<{{PH_Type1}}, {{PH_Type2}}, Comparer_{{PH_Type1}}_{{PH_Type2}}>({{PH_Target}}, value, mode, out index, out actualValue);
                }

                public static int FindIndex(this {{PH_Source}}{{PH_Target}}, {{PH_Type2}} value, SearchMode mode)
                {
                    return FindIndex<{{PH_Type1}}, {{PH_Type2}}, Comparer_{{PH_Type1}}_{{PH_Type2}}>({{PH_Target}}, value, mode);
                }

                public static int FindNearestIndex(this {{PH_Source}}{{PH_Target}}, {{PH_Type2}} value)
                {
                    return FindNearestIndex<{{PH_Type1}}, {{PH_Type2}}, Comparer_{{PH_Type1}}_{{PH_Type2}}>({{PH_Target}}, value);
                }

                public static {{PH_Destination}} Range(this {{PH_Source}}{{PH_Target}}, Range<{{PH_Type2}}> range)
                {
                    return Range<{{PH_Type1}}, {{PH_Type2}}, Comparer_{{PH_Type1}}_{{PH_Type2}}>({{PH_Target}}, range);
                }

                public static (int Start, int Length) IndexRange(this {{PH_Source}}{{PH_Target}}, Range<{{PH_Type2}}> range)
                {
                    return IndexRange<{{PH_Type1}}, {{PH_Type2}}, Comparer_{{PH_Type1}}_{{PH_Type2}}>({{PH_Target}}, range);
                }

        """;
}