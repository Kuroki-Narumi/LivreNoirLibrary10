using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [JsonConverter(typeof(Converters.NumberRangeJsonConverter))]
    public class NumberRange(int lower, int upper, bool isEnabled, bool exclusive) : RangeBase(isEnabled, exclusive)
    {
        public int LowerBound { get; set => SetValue(ref field, value); } = lower;
        public int UpperBound { get; set => SetValue(ref field, value); } = upper;

        public bool IsOutOfRange(int value) => Exclusive ^ (value < LowerBound || value > UpperBound);

        public void CopyFrom(NumberRange other)
        {
            base.CopyFrom(other);
            LowerBound = other.LowerBound;
            UpperBound = other.UpperBound;
        }
    }
}
