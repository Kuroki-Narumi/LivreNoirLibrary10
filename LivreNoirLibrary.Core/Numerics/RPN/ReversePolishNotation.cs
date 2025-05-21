using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static LivreNoirLibrary.Numerics.RpnConstants;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T>
        where T : struct, INumber<T>
    {
        private readonly List<IRpnToken> _tokens = [];

        private readonly Dictionary<char, IRpnToken> _char_tokens = new()
        {
            { Plus, AdditionToken },
            { Minus, SubtractionToken },
            { Multiply, MultipicationToken },
            { Divide, DivisionToken },
            { Modulo, ModuloToken },
        };

        private readonly Dictionary<Regex, IRpnToken> _regex_tokens = new()
        {
            { MaxToken.GetRegex(), MaxToken },
            { MinToken.GetRegex(), MinToken },
        };

        public ReversePolishNotation()
        {
            InitializeParsers();
        }

        public ReversePolishNotation(string expression) : this()
        {
            TryParse(expression);
        }

        protected virtual void InitializeParsers() { }

        protected void AddCharToken(char c, IRpnToken token)
        {
            _char_tokens.Add(c, token);
        }

        protected void AddRegexToken(IRpnToken token)
        {
            _regex_tokens.Add(token.GetRegex(), token);
        }

        public void Clear()
        {
            _tokens.Clear();
        }

        public bool IsEffective() => _tokens.Count is > 0;

        public override string ToString()
        {
            return $"{GetType()}{{{string.Join(", ", _tokens.Select(s => s.Symbol))}}}";
        }

        public static InfixOperatorToken AdditionToken { get; } = new($"{Plus}", Priority_AddSub, true, (a, b) => a + b);
        public static InfixOperatorToken SubtractionToken { get; } = new($"{Minus}", Priority_AddSub, true, (a, b) => a - b);
        public static InfixOperatorToken MultipicationToken { get; } = new($"{Multiply}", Priority_MulDiv, true, (a, b) => a * b);
        public static InfixOperatorToken DivisionToken { get; } = new($"{Divide}", Priority_MulDiv, true, (a, b) => a / b);
        public static InfixOperatorToken ModuloToken { get; } = new($"{Modulo}", Priority_MulDiv, true, (a, b) => a % b);

        public static FunctionToken MaxToken { get; } = new("max", 2, op => T.Max(op[0], op[1]));
        public static FunctionToken MinToken { get; } = new("min", 2, op => T.Min(op[0], op[1]));
    }
}
