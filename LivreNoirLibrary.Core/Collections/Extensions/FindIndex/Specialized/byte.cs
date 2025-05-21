using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindIndex(this ReadOnlySpan<byte> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_int(), type);
        }

        public static int FindIndex(this Span<byte> span, int value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_int(), type);
        }

        public static int FindIndex(this byte[] array, int value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_int(), type);
        }

        public static int FindIndex(this List<byte> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_int(), type);
        }

        public static int FindIndex(this IList<byte> list, int value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_int(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_int());
        }

        public static int FindNearestIndex(this Span<byte> span, int value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_int());
        }

        public static int FindNearestIndex(this byte[] array, int value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_int());
        }

        public static int FindNearestIndex(this List<byte> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_int());
        }

        public static int FindNearestIndex(this IList<byte> list, int value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_int());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<int> range)
        {
            return Range(span, range, new Comparer_byte_int());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<int> range)
        {
            return Range(span, range, new Comparer_byte_int());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<int> range)
        {
            return Range(array, range, new Comparer_byte_int());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<int> range)
        {
            return Range(list, range, new Comparer_byte_int());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<int> range)
        {
            return Range(list, range, new Comparer_byte_int());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_byte_int());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<int> range)
        {
            return IndexRange(span, range, new Comparer_byte_int());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<int> range)
        {
            return IndexRange(array, range, new Comparer_byte_int());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_byte_int());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<int> range)
        {
            return IndexRange(list, range, new Comparer_byte_int());
        }

        public static int FindIndex(this ReadOnlySpan<byte> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_long(), type);
        }

        public static int FindIndex(this Span<byte> span, long value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_long(), type);
        }

        public static int FindIndex(this byte[] array, long value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_long(), type);
        }

        public static int FindIndex(this List<byte> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_long(), type);
        }

        public static int FindIndex(this IList<byte> list, long value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_long(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_long());
        }

        public static int FindNearestIndex(this Span<byte> span, long value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_long());
        }

        public static int FindNearestIndex(this byte[] array, long value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_long());
        }

        public static int FindNearestIndex(this List<byte> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_long());
        }

        public static int FindNearestIndex(this IList<byte> list, long value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_long());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<long> range)
        {
            return Range(span, range, new Comparer_byte_long());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<long> range)
        {
            return Range(span, range, new Comparer_byte_long());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<long> range)
        {
            return Range(array, range, new Comparer_byte_long());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<long> range)
        {
            return Range(list, range, new Comparer_byte_long());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<long> range)
        {
            return Range(list, range, new Comparer_byte_long());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_byte_long());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<long> range)
        {
            return IndexRange(span, range, new Comparer_byte_long());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<long> range)
        {
            return IndexRange(array, range, new Comparer_byte_long());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_byte_long());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<long> range)
        {
            return IndexRange(list, range, new Comparer_byte_long());
        }

        public static int FindIndex(this ReadOnlySpan<byte> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_ulong(), type);
        }

        public static int FindIndex(this Span<byte> span, ulong value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_ulong(), type);
        }

        public static int FindIndex(this byte[] array, ulong value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_ulong(), type);
        }

        public static int FindIndex(this List<byte> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_ulong(), type);
        }

        public static int FindIndex(this IList<byte> list, ulong value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_ulong(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_ulong());
        }

        public static int FindNearestIndex(this Span<byte> span, ulong value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_ulong());
        }

        public static int FindNearestIndex(this byte[] array, ulong value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_ulong());
        }

        public static int FindNearestIndex(this List<byte> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_ulong());
        }

        public static int FindNearestIndex(this IList<byte> list, ulong value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_ulong());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_byte_ulong());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<ulong> range)
        {
            return Range(span, range, new Comparer_byte_ulong());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<ulong> range)
        {
            return Range(array, range, new Comparer_byte_ulong());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_byte_ulong());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<ulong> range)
        {
            return Range(list, range, new Comparer_byte_ulong());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_byte_ulong());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<ulong> range)
        {
            return IndexRange(span, range, new Comparer_byte_ulong());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<ulong> range)
        {
            return IndexRange(array, range, new Comparer_byte_ulong());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_byte_ulong());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<ulong> range)
        {
            return IndexRange(list, range, new Comparer_byte_ulong());
        }

        public static int FindIndex(this ReadOnlySpan<byte> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_float(), type);
        }

        public static int FindIndex(this Span<byte> span, float value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_float(), type);
        }

        public static int FindIndex(this byte[] array, float value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_float(), type);
        }

        public static int FindIndex(this List<byte> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_float(), type);
        }

        public static int FindIndex(this IList<byte> list, float value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_float(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_float());
        }

        public static int FindNearestIndex(this Span<byte> span, float value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_float());
        }

        public static int FindNearestIndex(this byte[] array, float value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_float());
        }

        public static int FindNearestIndex(this List<byte> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_float());
        }

        public static int FindNearestIndex(this IList<byte> list, float value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_float());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<float> range)
        {
            return Range(span, range, new Comparer_byte_float());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<float> range)
        {
            return Range(span, range, new Comparer_byte_float());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<float> range)
        {
            return Range(array, range, new Comparer_byte_float());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<float> range)
        {
            return Range(list, range, new Comparer_byte_float());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<float> range)
        {
            return Range(list, range, new Comparer_byte_float());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_byte_float());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<float> range)
        {
            return IndexRange(span, range, new Comparer_byte_float());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<float> range)
        {
            return IndexRange(array, range, new Comparer_byte_float());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_byte_float());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<float> range)
        {
            return IndexRange(list, range, new Comparer_byte_float());
        }

        public static int FindIndex(this ReadOnlySpan<byte> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_double(), type);
        }

        public static int FindIndex(this Span<byte> span, double value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_double(), type);
        }

        public static int FindIndex(this byte[] array, double value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_double(), type);
        }

        public static int FindIndex(this List<byte> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_double(), type);
        }

        public static int FindIndex(this IList<byte> list, double value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_double(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_double());
        }

        public static int FindNearestIndex(this Span<byte> span, double value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_double());
        }

        public static int FindNearestIndex(this byte[] array, double value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_double());
        }

        public static int FindNearestIndex(this List<byte> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_double());
        }

        public static int FindNearestIndex(this IList<byte> list, double value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_double());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<double> range)
        {
            return Range(span, range, new Comparer_byte_double());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<double> range)
        {
            return Range(span, range, new Comparer_byte_double());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<double> range)
        {
            return Range(array, range, new Comparer_byte_double());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<double> range)
        {
            return Range(list, range, new Comparer_byte_double());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<double> range)
        {
            return Range(list, range, new Comparer_byte_double());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_byte_double());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<double> range)
        {
            return IndexRange(span, range, new Comparer_byte_double());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<double> range)
        {
            return IndexRange(array, range, new Comparer_byte_double());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_byte_double());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<double> range)
        {
            return IndexRange(list, range, new Comparer_byte_double());
        }

        public static int FindIndex(this ReadOnlySpan<byte> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_decimal(), type);
        }

        public static int FindIndex(this Span<byte> span, decimal value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_decimal(), type);
        }

        public static int FindIndex(this byte[] array, decimal value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_decimal(), type);
        }

        public static int FindIndex(this List<byte> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_decimal(), type);
        }

        public static int FindIndex(this IList<byte> list, decimal value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_decimal(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_decimal());
        }

        public static int FindNearestIndex(this Span<byte> span, decimal value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_decimal());
        }

        public static int FindNearestIndex(this byte[] array, decimal value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_decimal());
        }

        public static int FindNearestIndex(this List<byte> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_decimal());
        }

        public static int FindNearestIndex(this IList<byte> list, decimal value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_decimal());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_byte_decimal());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<decimal> range)
        {
            return Range(span, range, new Comparer_byte_decimal());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<decimal> range)
        {
            return Range(array, range, new Comparer_byte_decimal());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_byte_decimal());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<decimal> range)
        {
            return Range(list, range, new Comparer_byte_decimal());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_byte_decimal());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<decimal> range)
        {
            return IndexRange(span, range, new Comparer_byte_decimal());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<decimal> range)
        {
            return IndexRange(array, range, new Comparer_byte_decimal());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_byte_decimal());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<decimal> range)
        {
            return IndexRange(list, range, new Comparer_byte_decimal());
        }

        public static int FindIndex(this ReadOnlySpan<byte> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_Rational(), type);
        }

        public static int FindIndex(this Span<byte> span, Rational value, SearchMode type)
        {
            return FindIndex(span, value, new Comparer_byte_Rational(), type);
        }

        public static int FindIndex(this byte[] array, Rational value, SearchMode type)
        {
            return FindIndex(array, value, new Comparer_byte_Rational(), type);
        }

        public static int FindIndex(this List<byte> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_Rational(), type);
        }

        public static int FindIndex(this IList<byte> list, Rational value, SearchMode type)
        {
            return FindIndex(list, value, new Comparer_byte_Rational(), type);
        }

        public static int FindNearestIndex(this ReadOnlySpan<byte> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_Rational());
        }

        public static int FindNearestIndex(this Span<byte> span, Rational value)
        {
            return FindNearestIndex(span, value, new Comparer_byte_Rational());
        }

        public static int FindNearestIndex(this byte[] array, Rational value)
        {
            return FindNearestIndex(array, value, new Comparer_byte_Rational());
        }

        public static int FindNearestIndex(this List<byte> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_Rational());
        }

        public static int FindNearestIndex(this IList<byte> list, Rational value)
        {
            return FindNearestIndex(list, value, new Comparer_byte_Rational());
        }

        public static ReadOnlySpan<byte> Range(this ReadOnlySpan<byte> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_byte_Rational());
        }

        public static ReadOnlySpan<byte> Range(this Span<byte> span, Range<Rational> range)
        {
            return Range(span, range, new Comparer_byte_Rational());
        }

        public static ReadOnlySpan<byte> Range(this byte[] array, Range<Rational> range)
        {
            return Range(array, range, new Comparer_byte_Rational());
        }

        public static ReadOnlySpan<byte> Range(this List<byte> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_byte_Rational());
        }

        public static IEnumerable<byte> Range(this IList<byte> list, Range<Rational> range)
        {
            return Range(list, range, new Comparer_byte_Rational());
        }

        public static (int Start, int Length) IndexRange(this ReadOnlySpan<byte> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_byte_Rational());
        }

        public static (int Start, int Length) IndexRange(this Span<byte> span, Range<Rational> range)
        {
            return IndexRange(span, range, new Comparer_byte_Rational());
        }

        public static (int Start, int Length) IndexRange(this byte[] array, Range<Rational> range)
        {
            return IndexRange(array, range, new Comparer_byte_Rational());
        }

        public static (int Start, int Length) IndexRange(this List<byte> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_byte_Rational());
        }

        public static (int Start, int Length) IndexRange(this IList<byte> list, Range<Rational> range)
        {
            return IndexRange(list, range, new Comparer_byte_Rational());
        }

    }
}