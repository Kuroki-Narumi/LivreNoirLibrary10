using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Numerics
{
    public readonly record struct SymbolInfo(int Index, SymbolType Type, string Symbol);

    public ref struct SymbolEnumerator
    {
        private readonly ReadOnlySpan<char> _expression;
        private readonly ICharTypeProvider _provider;
        private int _index;
        private SymbolInfo _current;

        public SymbolEnumerator(ReadOnlySpan<char> expression, ICharTypeProvider provider)
        {
            _expression = expression;
            _provider = provider;
            var index = 0;
            // 先頭の空白は飛ばす
            SkipWhile(expression, ref index, provider, CharType.WhiteSpace);
            _index = index;
        }

        public readonly SymbolInfo Current => _current;

        public bool MoveNext()
        {
            var span = _expression;
            var index = _index;
            var length = span.Length;
            if (index < length)
            {
                var provider = _provider;
                var i = index;
                var c = span[index];
                index++;
                var type = SymbolType.Unknown;
                string? symbol = null;
                switch (provider.GetCharType(c))
                {
                    case CharType.OpenBracket:
                        type = SymbolType.OpenBracket;
                        break;
                    case CharType.CloseBracket:
                        type = SymbolType.CloseBracket;
                        break;
                    case CharType.ArgumentDelimiter:
                        type = SymbolType.ArgumentDelimiter;
                        break;
                    case CharType.Number:
                        type = SymbolType.Number;
                        symbol = GetIdentifier(span, i, ref index, provider);
                        break;
                    case CharType.StartVariable:
                    case CharType.Identifier:
                        type = SymbolType.Variable;
                        symbol = GetIdentifier(span, i, ref index, provider);
                        break;
                    case CharType.Operator:
                        type = SymbolType.Operator;
                        SkipWhile(span, ref index, provider, CharType.Operator);
                        symbol = new(span[i..index]);
                        break;
                }
                symbol ??= c.ToString();
                SkipWhile(span, ref index, provider, CharType.WhiteSpace);
                // 暫定が値シンボルで、次の文字が開き括弧なら、これは関数シンボル
                if (type is SymbolType.Variable && index < length && provider.GetCharType(span[index]) is CharType.OpenBracket)
                {
                    type = SymbolType.Function;
                }
                _current = new(i, type, string.Intern(symbol));
                _index = index;
                return true;
            }
            else
            {
                _current = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SkipWhile(in ReadOnlySpan<char> span, ref int index, ICharTypeProvider provider, CharType type)
        {
            for (; index < span.Length && provider.GetCharType(span[index]) == type; index++) ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetIdentifier(in ReadOnlySpan<char> span, int start, ref int index, ICharTypeProvider provider)
        {
            for (; index < span.Length && provider.GetCharType(span[index]) is (CharType.Number or CharType.Identifier); index++) ;
            return new(span[start..index]);
        }

        public readonly SymbolEnumerator GetEnumerator() => this;
    }
}
