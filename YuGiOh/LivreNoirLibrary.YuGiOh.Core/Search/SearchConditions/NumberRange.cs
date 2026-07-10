using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [JsonConverter(typeof(Converters.NumberRangeJsonConverter))]
    public class NumberRange(int lower, int upper, bool isEnabled, bool exclusive) : RangeBase(isEnabled, exclusive)
    {
        public int LowerBound { get; set => SetValue(ref field, value); } = lower;
        public int UpperBound { get; set => SetValue(ref field, value); } = upper;

        public NumberRange(NumberRange source) : this(source.LowerBound, source.UpperBound, source.IsEnabled, source.Exclusive) { }

        public bool IsOutOfRange(int value) => Exclusive ^ (value < LowerBound || value > UpperBound);

        public void Set(int lower, int upper, bool isEnabled, bool exclusive)
        {
            LowerBound = lower;
            UpperBound = upper;
            IsEnabled = isEnabled;
            Exclusive = exclusive;
        }

        public void CopyFrom(NumberRange other)
        {
            base.CopyFrom(other);
            LowerBound = other.LowerBound;
            UpperBound = other.UpperBound;
        }
    }
}
