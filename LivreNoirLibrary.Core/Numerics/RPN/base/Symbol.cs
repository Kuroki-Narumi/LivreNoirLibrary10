using System;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation
    {
        public const string OpenBracketSymbol1 = "(";
        public const string OpenBracketSymbol2 = "{";
        public const string OpenBracketSymbol3 = "[";
        public const string CloseBracketSymbol1 = ")";
        public const string CloseBracketSymbol2 = "}";
        public const string CloseBracketSymbol3 = "]";

        public const string PlusSymbol = "+";
        public const string MinusSymbol = "-";
        public const string MultiplySymbol = "*";
        public const string DivideSymbol = "/";
        public const string ModuloSymbol = "%";
        public const string AndSymbol = "&";
        public const string AndSymbol2 = "&&";
        public const string OrSymbol = "|";
        public const string OrSymbol2 = "||";
        public const string XorSymbol = "^";
        public const string NotSymbol = "!";
        public const string LessSymbol = "<";
        public const string LessOrEqualSymbol = "<=";
        public const string GreaterSymbol = ">";
        public const string GreaterOrEqualSymbol = ">=";
        public const string CompareSymbol = "<=>";
        public const string EqualSymbol = "=";
        public const string EqualSymbol2 = "==";
        public const string NotEqualSymbol = "!=";

        public const string UnarySymbol = "@";
        public const string UnaryPlusSymbol = PlusSymbol + UnarySymbol;
        public const string UnaryMinusSymbol = MinusSymbol + UnarySymbol;

        public const string Conditional1Symbol = "?";
        public const string Conditional2Symbol = ":";
        public const string ConditionalSymbol = Conditional1Symbol + Conditional2Symbol;

        public const string MaxSymbol = "max";
        public const string MinSymbol = "min";

        public virtual bool IsWhiteSpace(char c) => char.IsWhiteSpace(c);
        public virtual bool IsOpenBracket(char c) => c is '(' or '[' or '{';
        public virtual bool IsCloseBracket(char c) => c is ')' or ']' or '}';
        public virtual bool IsArgumentDelimiter(char c) => c is ',';
        public virtual bool IsNumberCharacter(char c) => c is '.' or (>= '0' and <= '9');
        public virtual bool IsIdentifierCharacter(char c) => c is '.' or (>= '0' and <= '9') or (>= 'A' and <= 'Z') or '_' or (>= 'a' and <= 'z');
        public virtual bool IsOperatorCharacter(char c) => c is '!' or '"' or '#' or '$' or '%' or '&' or '\'' or '+' or '*' or '-' or '/' or ':' or ';' or '<' or '=' or '>' or '?' or '@' or '\\' or '^' or '`' or '|' or '~';
    }
}
