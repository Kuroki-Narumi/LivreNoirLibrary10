using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class TypedHeader(HeaderType type, string value) : HeaderBase(type, value), IDumpable<TypedHeader>
    {
        public static TypedHeader Load(BinaryReader reader)
        {
            var (key, value) = LoadKV(reader);
            TryGetType(key, out var type);
            return new(type, value);
        }

        public static bool TryGetString(string? text, [MaybeNullWhen(false)] out string value)
        {
            value = text;
            return !string.IsNullOrEmpty(value);
        }

        public static bool TryGetInt(string? text, out int value) => TryGetNumber(text, out value);
        public static bool TryGetDouble(string? text, out double value) => TryGetNumber(text, out value);
        public static bool TryGetDecimal(string? text, out decimal value) => TryGetNumber(text, out value);

        private static bool TryGetNumber<T>(string? text, out T value)
            where T : struct, INumber<T>
        {
            if (!string.IsNullOrEmpty(text) && T.TryParse(text, null, out value))
            {
                return true;
            }
            value = default;
            return false;
        }

        public static bool TryGetEnum<T>(string? text, out T value)
            where T : struct, Enum
        {
            if (!string.IsNullOrEmpty(text) && Enum.TryParse(text, out value))
            {
                return true;
            }
            value = default;
            return false;
        }
    }
}
