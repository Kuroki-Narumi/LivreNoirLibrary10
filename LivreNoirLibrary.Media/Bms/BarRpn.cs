using LivreNoirLibrary.Numerics;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarRpn : ReversePolishNotation<Rational>
    {
        public const char Char_BarLength = 'l';
        public const char Char_FirstLength = 'f';
        public const char Char_MaxCount = 'm';
        public const char Char_Index = 'i';
        public const char Char_Previous = 'p';
        public const char Char_PrePrevious = 'q';

        public static readonly string Symbol_BarLength = $"{Char_BarLength}";
        public static readonly string Symbol_FirstLength = $"{Char_FirstLength}";
        public static readonly string Symbol_MaxCount = $"{Char_MaxCount}";
        public static readonly string Symbol_Index = $"{Char_Index}";
        public static readonly string Symbol_Previous = $"{Char_Previous}";
        public static readonly string Symbol_PrePrevious = $"{Char_PrePrevious}";

        public static RpnVariableToken BarLengthToken { get; } = new(Symbol_BarLength);
        public static RpnVariableToken FirstLengthToken { get; } = new(Symbol_FirstLength);
        public static RpnVariableToken MaxCountToken { get; } = new(Symbol_MaxCount);
        public static RpnVariableToken IndexToken { get; } = new(Symbol_Index);
        public static RpnVariableToken PreviousToken { get; } = new(Symbol_Previous);
        public static RpnVariableToken PrePreviousToken { get; } = new(Symbol_PrePrevious);

        protected override void InitializeParsers()
        {
            base.InitializeParsers();
            AddCharToken(Char_BarLength, BarLengthToken);
            AddCharToken(Char_FirstLength, FirstLengthToken);
            AddCharToken(Char_MaxCount, MaxCountToken);
            AddCharToken(Char_Index, IndexToken);
            AddCharToken(Char_Previous, PreviousToken);
            AddCharToken(Char_PrePrevious, PrePreviousToken);
        }
    }

    public class BarRpnVariables : Dictionary<string, Rational>
    {
        public BarRpnVariables()
        {
            Add(BarRpn.Symbol_BarLength, Rational.One);
            Add(BarRpn.Symbol_FirstLength, new(1,2));
            Add(BarRpn.Symbol_MaxCount, new(8));
            Add(BarRpn.Symbol_Index, Rational.One);
            Add(BarRpn.Symbol_Previous, Rational.One);
            Add(BarRpn.Symbol_PrePrevious, Rational.One);
        }

        public void Setup(Rational barLength, Rational firstLength, int maxCount)
        {
            this[BarRpn.Symbol_BarLength] = barLength;
            this[BarRpn.Symbol_FirstLength] = firstLength;
            this[BarRpn.Symbol_MaxCount] = maxCount;
            this[BarRpn.Symbol_Previous] = firstLength;
            this[BarRpn.Symbol_PrePrevious] = Rational.Zero;
        }

        public void UpdatePrevious(Rational value)
        {
            this[BarRpn.Symbol_PrePrevious] = this[BarRpn.Symbol_Previous];
            this[BarRpn.Symbol_Previous] = value;
        }

        public Rational BarLength => this[BarRpn.Symbol_BarLength];
        public Rational FirstLength => this[BarRpn.Symbol_FirstLength];
        public int MaxCount => (int)this[BarRpn.Symbol_MaxCount];
        public int Index { get => (int)this[BarRpn.Symbol_Index]; set => this[BarRpn.Symbol_Index] = new(value); }
        public Rational Previous => this[BarRpn.Symbol_Previous];
        public Rational PrePrevious => this[BarRpn.Symbol_PrePrevious];
    }
}
