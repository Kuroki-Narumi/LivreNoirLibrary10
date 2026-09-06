using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Numerics
{
    public interface ICharTypeProvider
    {
        CharType GetCharType(char c);
    }

    public class CharTypeProvider : ICharTypeProvider
    {
        public static CharTypeProvider Default { get; } = new();

        private static readonly CharType[] _typeMap;

        static CharTypeProvider()
        {
            var ary = new CharType[char.MaxValue + 1];
            var span = ary.AsSpan();
            for (var c = 0; c <= char.MaxValue; c++)
            {
                span[c] = c switch
                {
                    '(' or '[' or '{' => CharType.OpenBracket,
                    ')' or ']' or '}' => CharType.CloseBracket,
                    ',' => CharType.ArgumentDelimiter,
                    '.' or (>= '0' and <= '9') => CharType.Number,
                    '$' => CharType.StartVariable,
                    (>= 'A' and <= 'Z') or '_' or (>= 'a' and <= 'z') => CharType.Identifier,
                    '!' or '"' or '#' or '%' or '&' or '\'' or '+' or '*' or '-' or '/' or ':' or ';' or '<' or '=' or '>' or '?' or '@' or '\\' or '^' or '`' or '|' or '~' => CharType.Operator,
                    _ => char.IsWhiteSpace((char)c) ? CharType.WhiteSpace : CharType.Unknown,
                };
            }

            _typeMap = ary;
        }

        public static ReadOnlySpan<CharType> DefaultMap => _typeMap;

        public CharType GetCharType(char c)
        {
            ref var ptr = ref MemoryMarshal.GetArrayDataReference(_typeMap);
            return Unsafe.Add(ref ptr, c);
        }
    }
}
