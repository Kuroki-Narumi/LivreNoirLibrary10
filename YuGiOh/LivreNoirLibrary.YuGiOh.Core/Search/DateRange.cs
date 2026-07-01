using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class DateRange : RangeBase
    {
        public DateTime Since { get; set => SetValue(ref field, value); }
        public DateTime Until { get; set => SetValue(ref field, value); }

        public bool IsOutOfRange(DateTime value) => Exclusive ^ (value < Since || value > Until);
    }
}
