using System;

namespace LivreNoirLibrary.Numerics
{
    public partial class ExpressionBase
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
        public const string PowerSymbol = "**";
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

        public const string ShiftLeftSymbol = "<<";
        public const string ShiftRightArithmeticSymbol = ">>";
        public const string ShiftRightLogicalSymbol = ">>>";
        public const string OnesComplementSymbol = "~";

        public const string UnarySymbol = "@";
        public const string UnaryPlusSymbol = PlusSymbol + UnarySymbol;
        public const string UnaryMinusSymbol = MinusSymbol + UnarySymbol;

        public const string Conditional1Symbol = "?";
        public const string Conditional2Symbol = ":";
        public const string ConditionalSymbol = Conditional1Symbol + Conditional2Symbol;

        public const string MaxSymbol = "max";
        public const string MinSymbol = "min";
        public const string AbsSymbol = "abs";
        public const string SignSymbol = "sign";
        public const string FloorSymbol = "floor";
        public const string CeilingSymbol = "ceil";
        public const string TruncateSymbol = "trunc";
        public const string RoundSymbol = "round";
        public const string SquareRootSymbol = "sqrt";
        public const string CubeRootSymbol = "cbrt";
        public const string HypotSymbol = "hypot";
        public const string ExponentSymbol = "exp";
        public const string ScaleBSymbol = "ScaleB";
        public const string ILogBSymbol = "ILogB";
        public const string LogSymbol = "log";
        public const string Log2Symbol = "log2";
        public const string Log10Symbol = "log10";
        public const string SinSymbol = "sin";
        public const string CosSymbol = "cos";
        public const string TanSymbol = "tan";
        public const string AsinSymbol = "asin";
        public const string AcosSymbol = "acos";
        public const string AtanSymbol = "atan";
        public const string SinhSymbol = "sinh";
        public const string CoshSymbol = "cosh";
        public const string TanhSymbol = "tanh";
        public const string AsinhSymbol = "asinh";
        public const string AcoshSymbol = "acosh";
        public const string AtanhSymbol = "atanh";
        public const string RadianSymbol = "rad";
        public const string DegreeSymbol = "deg";

        public const string PiSymbol = "PI";
        public const string NapierSymbol = "E";

        public virtual bool IsWhiteSpace(char c) => char.IsWhiteSpace(c);
        public virtual bool IsOpenBracket(char c) => c is '(' or '[' or '{';
        public virtual bool IsCloseBracket(char c) => c is ')' or ']' or '}';
        public virtual bool IsArgumentDelimiter(char c) => c is ',';
        public virtual bool IsNumberCharacter(char c) => c is '.' or (>= '0' and <= '9');
        public virtual bool IsIdentifierCharacter(char c) => c is '.' or (>= '0' and <= '9') or (>= 'A' and <= 'Z') or '_' or (>= 'a' and <= 'z');
        public virtual bool IsOperatorCharacter(char c) => c is '!' or '"' or '#' or '$' or '%' or '&' or '\'' or '+' or '*' or '-' or '/' or ':' or ';' or '<' or '=' or '>' or '?' or '@' or '\\' or '^' or '`' or '|' or '~';
    }
}
