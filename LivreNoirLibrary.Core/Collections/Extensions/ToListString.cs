using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static string ToListString<T>(this Span<T> span)
        {
            StringBuilder sb = new();
            BuildString(sb, (ReadOnlySpan<T>)span);
            return sb.ToString();
        }

        public static string ToListString<T>(this ReadOnlySpan<T> span)
        {
            StringBuilder sb = new();
            BuildString(sb, span);
            return sb.ToString();
        }

        public static string ToListString<T>(this IEnumerable<T> obj)
        {
            StringBuilder sb = new();
            BuildString(sb, obj);
            return sb.ToString();
        }

        public static string ToListString<TKey, TValue>(this IDictionary<TKey, TValue> dic)
        {
            StringBuilder sb = new();
            BuildString(sb, dic);
            return sb.ToString();
        }

        private static void BuildString<T>(StringBuilder builder, T obj)
        {
            if (obj is null)
            {
                builder.Append("(null)");
            }
            else if (typeof(T) == typeof(string))
            {
                builder.Append((string)(object)obj);
            }
            else if (obj is IEnumerable enumer)
            {
                BuildString(builder, enumer);
            }
            else
            {
                builder.Append(obj.ToString());
            }
        }

        private static void BuildString<T>(StringBuilder builder, ReadOnlySpan<T> span)
        {
            builder.Append('[');
            var i = 0;
            foreach (var item in span)
            {
                if (i++ > 0)
                {
                    builder.Append(", ");
                }
                BuildString(builder, item);
            }
            builder.Append(']');
        }

        private static void BuildString<T>(StringBuilder builder, IEnumerable<T> obj)
        {
            builder.Append('[');
            var i = 0;
            foreach (var item in obj)
            {
                if (i++ > 0)
                {
                    builder.Append(", ");
                }
                BuildString(builder, item);
            }
            builder.Append(']');
        }

        private static void BuildString<TKey, TValue>(StringBuilder builder, IDictionary<TKey, TValue> obj)
        {
            builder.Append('{');
            var i = 0;
            foreach (var (key, value) in obj)
            {
                if (i++ > 0)
                {
                    builder.Append(", ");
                }
                BuildString(builder, key);
                builder.Append(": ");
                BuildString(builder, value);
            }
            builder.Append('}');
        }

        public static string ToListString(this IEnumerable obj)
        {
            StringBuilder sb = new();
            BuildString(sb, obj);
            return sb.ToString();
        }

        private static void BuildString(StringBuilder builder, object? obj)
        {
            if (obj is null)
            {
                builder.Append("null");
            }
            else if (obj is string s)
            {
                builder.Append(s);
            }
            else if (obj is IEnumerable enumer)
            {
                BuildString(builder, enumer);
            }
            else
            {
                builder.Append(obj.ToString());
            }
        }

        private static void BuildString(StringBuilder builder, IEnumerable obj)
        {
            builder.Append('[');
            var i = 0;
            foreach (var item in obj)
            {
                if (i++ > 0)
                {
                    builder.Append(", ");
                }
                BuildString(builder, item);
            }
            builder.Append(']');
        }
    }
}
