using System;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [JsonConverter(typeof(Converters.DateRangeJsonConverter))]
    public class DateRange(DateTime since, DateTime until, bool isEnabled, bool exclusive) : RangeBase(isEnabled, exclusive)
    {
        public DateRange() : this(Utils.DateStart, DateTime.Now, false, false) { }

        public DateTime Since { get; set => SetValue(ref field, value); } = since;
        public DateTime Until { get; set => SetValue(ref field, value); } = until;

        public bool IsOutOfRange(DateTime value) => Exclusive ^ (value < Since || value > Until);

        public void CopyFrom(DateRange other)
        {
            base.CopyFrom(other);
            Since = other.Since;
            Until = other.Until;
        }
    }
}
