using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Text.Convert
{
    public static class ConvertingStringComparer
    {
        public static ConvertingStringComparer<T> Create<T>(T converter) where T : IStringConverter => new(converter);
    }

    public class ConvertingStringComparer<T>(T converter) : IEqualityComparer<string>, IAlternateEqualityComparer<ReadOnlySpan<char>, string>
        where T : IStringConverter
    {
        private readonly T _converter = converter;

        public int GetHashCode([DisallowNull] string obj) => GetHashCode(obj.AsSpan());

        public bool Equals(string? x, string? y) => x is not null ? y is not null && Equals(x.AsSpan(), y) : y is null;

        public string Create(ReadOnlySpan<char> alternate) => _converter.Convert(alternate);

        public int GetHashCode(ReadOnlySpan<char> alternate)
        {
            var hashCode = new HashCode();
            foreach (var c in _converter.GetEnumerator(alternate))
            {
                hashCode.Add(c);
            }
            return hashCode.ToHashCode();
        }

        public bool Equals(ReadOnlySpan<char> alternate, string other)
        {
            var enum1 = _converter.GetEnumerator(alternate);
            var enum2 = _converter.GetEnumerator(other);
            
            while (enum1.MoveNext() && enum2.MoveNext())
            {
                if (!enum1.Current.Equals(enum2.Current))
                {
                    return false;
                }
            }

            return !enum1.MoveNext() && !enum2.MoveNext();
        }
    }
}
