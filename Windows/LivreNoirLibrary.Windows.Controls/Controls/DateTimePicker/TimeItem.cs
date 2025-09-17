using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls
{
    public class TimeItem
    {
        public int Value { get; }
        public int Row { get; }
        public int Column { get; }

        private TimeItem(int value, int row, int column)
        {
            Value = value;
            Row = row;
            Column = column;
        }

        static TimeItem Create(TimeItemType type, int value) => type switch
        {
            TimeItemType.Hour => Hour(value),
            _ => Minute(value),
        };

        public static TimeItem Hour(int value) => new(value, value % 6, value / 6);
        public static TimeItem Minute(int value) => new(value, value % 10, value / 10);

        static TimeItem[] GetArray(TimeItemType type, int start, int end)
        {
            var length = end - start + 1;
            if (length <= 0) return [];
            var result = new TimeItem[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Create(type, start + i);
            }
            return result;
        }
        public static TimeItem[] Hours { get; } = GetArray(TimeItemType.Hour, 0, 23);
        public static TimeItem[] Minutes { get; } = GetArray(TimeItemType.Minute, 0, 59);

        public static TimeItem GetItem(TimeItemType type, int value) => type switch
        {
            TimeItemType.Hour => Hours[value],
            _ => Minutes[value],
        };
    }
}
