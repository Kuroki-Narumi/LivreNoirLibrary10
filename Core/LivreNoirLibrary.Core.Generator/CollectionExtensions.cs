using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

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

    private void Generate(IncrementalGeneratorPostInitializationContext context)
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

        foreach (var type1 in Comparable)
        {
            foreach (var type2 in Comparable)
            {
                if (type1 == type2)
                    continue;
                foreach (var (collectionType, target, rangeType) in CollectionTypes)
                {
                    sb.AppendLine(Template.Replace(PH_Source, collectionType)
                                          .Replace(PH_Destination, rangeType)
                                          .Replace(PH_Target, target)
                                          .Replace(PH_Type1, type1)
                                          .Replace(PH_Type2, type2));
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
                public static bool TrySearch(this {{PH_Source}}{{PH_Target}}, {{PH_Type2}} value, SearchMode type, out int index, out {{PH_Type1}} actualValue)
                {
                    return TrySearch({{PH_Target}}, value, new Comparer_{{PH_Type1}}_{{PH_Type2}}(), type, out index, out actualValue);
                }

                public static int FindIndex(this {{PH_Source}}{{PH_Target}}, {{PH_Type2}} value, SearchMode type)
                {
                    return FindIndex({{PH_Target}}, value, new Comparer_{{PH_Type1}}_{{PH_Type2}}(), type);
                }

                public static int FindNearestIndex(this {{PH_Source}}{{PH_Target}}, {{PH_Type2}} value)
                {
                    return FindNearestIndex({{PH_Target}}, value, new Comparer_{{PH_Type1}}_{{PH_Type2}}());
                }

                public static {{PH_Destination}} Range(this {{PH_Source}}{{PH_Target}}, Range<{{PH_Type2}}> range)
                {
                    return Range({{PH_Target}}, range, new Comparer_{{PH_Type1}}_{{PH_Type2}}());
                }

                public static (int Start, int Length) IndexRange(this {{PH_Source}}{{PH_Target}}, Range<{{PH_Type2}}> range)
                {
                    return IndexRange({{PH_Target}}, range, new Comparer_{{PH_Type1}}_{{PH_Type2}}());
                }
        """;
}