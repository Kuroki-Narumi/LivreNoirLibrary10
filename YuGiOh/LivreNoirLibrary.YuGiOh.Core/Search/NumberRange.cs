using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class NumberRange : RangeBase
    {
        public int LowerBound { get; set => SetValue(ref field, value); }
        public int UpperBound { get; set => SetValue(ref field, value); }

        public bool IsOutOfRange(int value) => Exclusive ^ (value < LowerBound || value > UpperBound);
    }
}
