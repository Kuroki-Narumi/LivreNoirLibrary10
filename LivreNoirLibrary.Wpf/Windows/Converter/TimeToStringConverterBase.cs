using System;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows
{
    public abstract class TimeToStringConverterBase<T> : IValueConverter
    {
        public abstract string Format { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            DateTime time => time.ToString(Format, culture),
            DateOnly time => time.ToString(Format, culture),
            TimeOnly time => time.ToString(Format, culture),
            _ => $"{value}",
        };

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is string text ? ConvertBack(text, culture) : value;
        protected abstract T ConvertBack(string text, CultureInfo culture);
    }

    public class DateTimeToStringConverter : TimeToStringConverterBase<DateTime>
    {
        public const string DefaultFormat = "G";
        public override string Format { get; set; } = DefaultFormat;
        protected override DateTime ConvertBack(string text, CultureInfo culture) => DateTime.Parse(text, culture);
    }

    public class DateOnlyToStringConverter : TimeToStringConverterBase<DateOnly>
    {
        public const string DefaultFormat = "d";
        public override string Format { get; set; } = DefaultFormat;
        protected override DateOnly ConvertBack(string text, CultureInfo culture) => DateOnly.Parse(text, culture);
    }

    public class TimeOnlyToStringConverter : TimeToStringConverterBase<TimeOnly>
    {
        public const string DefaultFormat = "T";
        public override string Format { get; set; } = DefaultFormat;
        protected override TimeOnly ConvertBack(string text, CultureInfo culture) => TimeOnly.Parse(text, culture);
    }
}
