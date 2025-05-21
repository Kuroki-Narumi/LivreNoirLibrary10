using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<decimal> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_int(), type);
        }

        public static int FindIndex(this Span<decimal> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_int(), type);
        }

        public static int FindIndex(this decimal[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_int(), type);
        }

        public static int FindIndex(this List<decimal> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_int(), type);
        }

        public static int FindIndex(this IList<decimal> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_int());
        }

        public static int FindNearestIndex(this Span<decimal> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_int());
        }

        public static int FindNearestIndex(this decimal[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_int());
        }

        public static int FindNearestIndex(this List<decimal> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_int());
        }

        public static int FindNearestIndex(this IList<decimal> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_int());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<int> range)
        {
            return Range(span, range, new Comparer_decimal_int());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<int> range)
        {
            return Range(span, range, new Comparer_decimal_int());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_decimal_int());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<int> range)
        {
            return Range(list, range, new Comparer_decimal_int());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<int> range)
        {
            return Range(list, range, new Comparer_decimal_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_decimal_int());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_decimal_int());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_decimal_int());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_decimal_int());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_decimal_int());
        }

        public static int FindIndex(this ReadOnlySpan<decimal> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_long(), type);
        }

        public static int FindIndex(this Span<decimal> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_long(), type);
        }

        public static int FindIndex(this decimal[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_long(), type);
        }

        public static int FindIndex(this List<decimal> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_long(), type);
        }

        public static int FindIndex(this IList<decimal> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_long());
        }

        public static int FindNearestIndex(this Span<decimal> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_long());
        }

        public static int FindNearestIndex(this decimal[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_long());
        }

        public static int FindNearestIndex(this List<decimal> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_long());
        }

        public static int FindNearestIndex(this IList<decimal> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_long());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<long> range)
        {
            return Range(span, range, new Comparer_decimal_long());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<long> range)
        {
            return Range(span, range, new Comparer_decimal_long());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_decimal_long());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<long> range)
        {
            return Range(list, range, new Comparer_decimal_long());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<long> range)
        {
            return Range(list, range, new Comparer_decimal_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_decimal_long());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_decimal_long());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_decimal_long());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_decimal_long());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_decimal_long());
        }

        public static int FindIndex(this ReadOnlySpan<decimal> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_ulong(), type);
        }

        public static int FindIndex(this Span<decimal> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_ulong(), type);
        }

        public static int FindIndex(this decimal[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_ulong(), type);
        }

        public static int FindIndex(this List<decimal> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_ulong(), type);
        }

        public static int FindIndex(this IList<decimal> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_ulong());
        }

        public static int FindNearestIndex(this Span<decimal> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_ulong());
        }

        public static int FindNearestIndex(this decimal[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_ulong());
        }

        public static int FindNearestIndex(this List<decimal> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_ulong());
        }

        public static int FindNearestIndex(this IList<decimal> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_ulong());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_decimal_ulong());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_decimal_ulong());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_decimal_ulong());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_decimal_ulong());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_decimal_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_decimal_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_decimal_ulong());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_decimal_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_decimal_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_decimal_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<decimal> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_float(), type);
        }

        public static int FindIndex(this Span<decimal> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_float(), type);
        }

        public static int FindIndex(this decimal[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_float(), type);
        }

        public static int FindIndex(this List<decimal> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_float(), type);
        }

        public static int FindIndex(this IList<decimal> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_float());
        }

        public static int FindNearestIndex(this Span<decimal> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_float());
        }

        public static int FindNearestIndex(this decimal[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_float());
        }

        public static int FindNearestIndex(this List<decimal> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_float());
        }

        public static int FindNearestIndex(this IList<decimal> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_float());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<float> range)
        {
            return Range(span, range, new Comparer_decimal_float());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<float> range)
        {
            return Range(span, range, new Comparer_decimal_float());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_decimal_float());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<float> range)
        {
            return Range(list, range, new Comparer_decimal_float());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<float> range)
        {
            return Range(list, range, new Comparer_decimal_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_decimal_float());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_decimal_float());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_decimal_float());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_decimal_float());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_decimal_float());
        }

        public static int FindIndex(this ReadOnlySpan<decimal> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_double(), type);
        }

        public static int FindIndex(this Span<decimal> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_double(), type);
        }

        public static int FindIndex(this decimal[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_double(), type);
        }

        public static int FindIndex(this List<decimal> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_double(), type);
        }

        public static int FindIndex(this IList<decimal> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_double());
        }

        public static int FindNearestIndex(this Span<decimal> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_double());
        }

        public static int FindNearestIndex(this decimal[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_double());
        }

        public static int FindNearestIndex(this List<decimal> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_double());
        }

        public static int FindNearestIndex(this IList<decimal> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_double());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<double> range)
        {
            return Range(span, range, new Comparer_decimal_double());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<double> range)
        {
            return Range(span, range, new Comparer_decimal_double());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_decimal_double());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<double> range)
        {
            return Range(list, range, new Comparer_decimal_double());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<double> range)
        {
            return Range(list, range, new Comparer_decimal_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_decimal_double());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_decimal_double());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_decimal_double());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_decimal_double());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_decimal_double());
        }

        public static int FindIndex(this ReadOnlySpan<decimal> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_decimal(), type);
        }

        public static int FindIndex(this Span<decimal> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_decimal(), type);
        }

        public static int FindIndex(this decimal[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_decimal(), type);
        }

        public static int FindIndex(this List<decimal> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_decimal(), type);
        }

        public static int FindIndex(this IList<decimal> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_decimal());
        }

        public static int FindNearestIndex(this Span<decimal> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_decimal());
        }

        public static int FindNearestIndex(this decimal[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_decimal());
        }

        public static int FindNearestIndex(this List<decimal> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_decimal());
        }

        public static int FindNearestIndex(this IList<decimal> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_decimal());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_decimal_decimal());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_decimal_decimal());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_decimal_decimal());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_decimal_decimal());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_decimal_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_decimal_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_decimal_decimal());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_decimal_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_decimal_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_decimal_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<decimal> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_Rational(), type);
        }

        public static int FindIndex(this Span<decimal> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_decimal_Rational(), type);
        }

        public static int FindIndex(this decimal[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_decimal_Rational(), type);
        }

        public static int FindIndex(this List<decimal> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_Rational(), type);
        }

        public static int FindIndex(this IList<decimal> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_decimal_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<decimal> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_Rational());
        }

        public static int FindNearestIndex(this Span<decimal> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_decimal_Rational());
        }

        public static int FindNearestIndex(this decimal[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_decimal_Rational());
        }

        public static int FindNearestIndex(this List<decimal> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_Rational());
        }

        public static int FindNearestIndex(this IList<decimal> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_decimal_Rational());
        }

        public static ReadOnlySpan<decimal> Range(this ReadOnlySpan<decimal> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_decimal_Rational());
        }

        public static ReadOnlySpan<decimal> Range(this Span<decimal> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_decimal_Rational());
        }

        public static ReadOnlySpan<decimal> Range(this decimal[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_decimal_Rational());
        }

        public static ReadOnlySpan<decimal> Range(this List<decimal> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_decimal_Rational());
        }

        public static IEnumerable<decimal> Range(this IList<decimal> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_decimal_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<decimal> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_decimal_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<decimal> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_decimal_Rational());
        }

        public static (int Start, int Length) IndexRange(this decimal[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_decimal_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<decimal> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_decimal_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<decimal> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_decimal_Rational());
        }

    }
}