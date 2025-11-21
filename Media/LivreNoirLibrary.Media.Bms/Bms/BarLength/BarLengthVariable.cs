
namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthVariable
    {
        public const string BarLengthSymbol = "l";
        public const string BarLengthSymbol2 = "Length";
        public const string FirstLengthSymbol = "f";
        public const string FirstLengthSymbol2 = "First";
        public const string MaxCountSymbol = "m";
        public const string MaxCountSymbol2 = "Count";
        public const string IndexSymbol = "i";
        public const string PreviousSymbol = "p";
        public const string PrePreviousSymbol = "q";

        public double BarLength { get; private set; } = 1;
        public double FirstLength { get; private set; } = 0.5;
        public int MaxCount { get; private set; } = 8;
        public int Index { get; set; } = 1;
        public double Previous { get; private set; } = 1;
        public double PrePrevious { get; private set; } = 1;

        public void Setup(double barLength, double firstLength, int maxCount)
        {
            BarLength  = barLength;
            FirstLength = firstLength;
            MaxCount = maxCount;
            Previous = firstLength;
            PrePrevious = 0;
        }

        public void UpdatePrevious(double value)
        {
            PrePrevious = Previous;
            Previous = value;
        }

        public bool TryGetValue(string symbol, out double value)
        {
            value = symbol switch
            {
                BarLengthSymbol or BarLengthSymbol2 => BarLength,
                FirstLengthSymbol or FirstLengthSymbol2 => FirstLength,
                MaxCountSymbol or MaxCountSymbol2 => MaxCount,
                IndexSymbol => Index,
                PreviousSymbol => Previous,
                PrePreviousSymbol => PrePrevious,
                _ => -1,
            };
            return value is > 0;
        }
    }
}
