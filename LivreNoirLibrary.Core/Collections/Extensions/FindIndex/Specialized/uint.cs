using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<uint> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_int(), type);
        }

        public static int FindIndex(this Span<uint> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_int(), type);
        }

        public static int FindIndex(this uint[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_int(), type);
        }

        public static int FindIndex(this List<uint> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_int(), type);
        }

        public static int FindIndex(this IList<uint> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_int());
        }

        public static int FindNearestIndex(this Span<uint> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_int());
        }

        public static int FindNearestIndex(this uint[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_int());
        }

        public static int FindNearestIndex(this List<uint> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_int());
        }

        public static int FindNearestIndex(this IList<uint> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_int());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<int> range)
        {
            return Range(span, range, new Comparer_uint_int());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<int> range)
        {
            return Range(span, range, new Comparer_uint_int());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_uint_int());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<int> range)
        {
            return Range(list, range, new Comparer_uint_int());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<int> range)
        {
            return Range(list, range, new Comparer_uint_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_uint_int());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_uint_int());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_uint_int());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_uint_int());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_uint_int());
        }

        public static int FindIndex(this ReadOnlySpan<uint> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_long(), type);
        }

        public static int FindIndex(this Span<uint> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_long(), type);
        }

        public static int FindIndex(this uint[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_long(), type);
        }

        public static int FindIndex(this List<uint> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_long(), type);
        }

        public static int FindIndex(this IList<uint> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_long());
        }

        public static int FindNearestIndex(this Span<uint> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_long());
        }

        public static int FindNearestIndex(this uint[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_long());
        }

        public static int FindNearestIndex(this List<uint> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_long());
        }

        public static int FindNearestIndex(this IList<uint> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_long());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<long> range)
        {
            return Range(span, range, new Comparer_uint_long());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<long> range)
        {
            return Range(span, range, new Comparer_uint_long());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_uint_long());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<long> range)
        {
            return Range(list, range, new Comparer_uint_long());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<long> range)
        {
            return Range(list, range, new Comparer_uint_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_uint_long());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_uint_long());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_uint_long());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_uint_long());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_uint_long());
        }

        public static int FindIndex(this ReadOnlySpan<uint> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_ulong(), type);
        }

        public static int FindIndex(this Span<uint> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_ulong(), type);
        }

        public static int FindIndex(this uint[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_ulong(), type);
        }

        public static int FindIndex(this List<uint> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_ulong(), type);
        }

        public static int FindIndex(this IList<uint> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_ulong());
        }

        public static int FindNearestIndex(this Span<uint> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_ulong());
        }

        public static int FindNearestIndex(this uint[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_ulong());
        }

        public static int FindNearestIndex(this List<uint> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_ulong());
        }

        public static int FindNearestIndex(this IList<uint> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_ulong());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_uint_ulong());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_uint_ulong());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_uint_ulong());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_uint_ulong());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_uint_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_uint_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_uint_ulong());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_uint_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_uint_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_uint_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<uint> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_float(), type);
        }

        public static int FindIndex(this Span<uint> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_float(), type);
        }

        public static int FindIndex(this uint[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_float(), type);
        }

        public static int FindIndex(this List<uint> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_float(), type);
        }

        public static int FindIndex(this IList<uint> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_float());
        }

        public static int FindNearestIndex(this Span<uint> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_float());
        }

        public static int FindNearestIndex(this uint[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_float());
        }

        public static int FindNearestIndex(this List<uint> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_float());
        }

        public static int FindNearestIndex(this IList<uint> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_float());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<float> range)
        {
            return Range(span, range, new Comparer_uint_float());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<float> range)
        {
            return Range(span, range, new Comparer_uint_float());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_uint_float());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<float> range)
        {
            return Range(list, range, new Comparer_uint_float());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<float> range)
        {
            return Range(list, range, new Comparer_uint_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_uint_float());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_uint_float());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_uint_float());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_uint_float());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_uint_float());
        }

        public static int FindIndex(this ReadOnlySpan<uint> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_double(), type);
        }

        public static int FindIndex(this Span<uint> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_double(), type);
        }

        public static int FindIndex(this uint[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_double(), type);
        }

        public static int FindIndex(this List<uint> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_double(), type);
        }

        public static int FindIndex(this IList<uint> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_double());
        }

        public static int FindNearestIndex(this Span<uint> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_double());
        }

        public static int FindNearestIndex(this uint[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_double());
        }

        public static int FindNearestIndex(this List<uint> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_double());
        }

        public static int FindNearestIndex(this IList<uint> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_double());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<double> range)
        {
            return Range(span, range, new Comparer_uint_double());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<double> range)
        {
            return Range(span, range, new Comparer_uint_double());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_uint_double());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<double> range)
        {
            return Range(list, range, new Comparer_uint_double());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<double> range)
        {
            return Range(list, range, new Comparer_uint_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_uint_double());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_uint_double());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_uint_double());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_uint_double());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_uint_double());
        }

        public static int FindIndex(this ReadOnlySpan<uint> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_decimal(), type);
        }

        public static int FindIndex(this Span<uint> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_decimal(), type);
        }

        public static int FindIndex(this uint[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_decimal(), type);
        }

        public static int FindIndex(this List<uint> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_decimal(), type);
        }

        public static int FindIndex(this IList<uint> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_decimal());
        }

        public static int FindNearestIndex(this Span<uint> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_decimal());
        }

        public static int FindNearestIndex(this uint[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_decimal());
        }

        public static int FindNearestIndex(this List<uint> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_decimal());
        }

        public static int FindNearestIndex(this IList<uint> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_decimal());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_uint_decimal());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_uint_decimal());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_uint_decimal());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_uint_decimal());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_uint_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_uint_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_uint_decimal());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_uint_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_uint_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_uint_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<uint> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_Rational(), type);
        }

        public static int FindIndex(this Span<uint> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_uint_Rational(), type);
        }

        public static int FindIndex(this uint[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_uint_Rational(), type);
        }

        public static int FindIndex(this List<uint> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_Rational(), type);
        }

        public static int FindIndex(this IList<uint> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_uint_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<uint> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_Rational());
        }

        public static int FindNearestIndex(this Span<uint> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_uint_Rational());
        }

        public static int FindNearestIndex(this uint[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_uint_Rational());
        }

        public static int FindNearestIndex(this List<uint> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_Rational());
        }

        public static int FindNearestIndex(this IList<uint> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_uint_Rational());
        }

        public static ReadOnlySpan<uint> Range(this ReadOnlySpan<uint> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_uint_Rational());
        }

        public static ReadOnlySpan<uint> Range(this Span<uint> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_uint_Rational());
        }

        public static ReadOnlySpan<uint> Range(this uint[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_uint_Rational());
        }

        public static ReadOnlySpan<uint> Range(this List<uint> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_uint_Rational());
        }

        public static IEnumerable<uint> Range(this IList<uint> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_uint_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<uint> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_uint_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<uint> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_uint_Rational());
        }

        public static (int Start, int Length) IndexRange(this uint[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_uint_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<uint> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_uint_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<uint> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_uint_Rational());
        }

    }
}