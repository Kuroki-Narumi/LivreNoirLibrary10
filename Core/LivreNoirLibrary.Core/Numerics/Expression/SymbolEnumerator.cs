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
        private readonly IExpressionInterpreter _interpreter;
        private int _index;
        private SymbolInfo _current;

        public SymbolEnumerator(ReadOnlySpan<char> expression, IExpressionInterpreter interpreter)
        {
            _expression = expression;
            _interpreter = interpreter;
            var index = 0;
            // 先頭の空白は飛ばす
            SkipWhile(ref index, interpreter.IsWhiteSpace);
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
                var interpreter = _interpreter;
                var i = index;
                var c = span[index];
                index++;
                var type = SymbolType.Unknown;
                string? symbol = null;
                if (interpreter.IsOpenBracket(c))
                {
                    type = SymbolType.OpenBracket;
                }
                else if (interpreter.IsCloseBracket(c))
                {
                    type = SymbolType.CloseBracket;
                }
                else if (interpreter.IsArgumentDelimiter(c))
                {
                    type = SymbolType.ArgumentDelimiter;
                }
                else if (interpreter.IsIdentifierCharacter(c))
                {
                    type = interpreter.IsNumberCharacter(c) ? SymbolType.Number : SymbolType.Variable;
                    SkipWhile(ref index, interpreter.IsIdentifierCharacter);
                    symbol = new(span[i..index]);
                }
                else if (interpreter.IsOperatorCharacter(c))
                {
                    type = SymbolType.Operator;
                    SkipWhile(ref index, interpreter.IsOperatorCharacter);
                    symbol = new(span[i..index]);
                }
                symbol ??= c.ToString();
                SkipWhile(ref index, interpreter.IsWhiteSpace);
                // 暫定が値シンボルで、次の文字が開き括弧なら、これは関数シンボル
                if (type is SymbolType.Variable && index < length && interpreter.IsOpenBracket(span[index]))
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
        private readonly void SkipWhile(ref int index, Predicate<char> selector)
        {
            var span = _expression;
            for (; index < span.Length && selector(span[index]); index++) ;
        }

        public readonly SymbolEnumerator GetEnumerator() => this;
    }
}
