using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows
{
    public abstract class ComboItemBase<T>
        where T : struct, Enum
    {
        public T Value { get; }
        public Brush? Background { get; }
        public int Row { get; }
        public int Column { get; }

        protected ComboItemBase(T value)
        {
            Value = value;
            Row = GetRow(value);
            Column = GetColumn(value);
            Background = GetBackground(Row, Column);
        }

        protected static Dictionary<T, TValue> CreateMap<TValue>(TValue[] items)
            where TValue : ComboItemBase<T>
        {
            Dictionary<T, TValue> result = [];
            foreach (var item in items)
            {
                result[item.Value] = item;
            }
            return result;
        }

        protected virtual int GetRow(T value) => 0;
        protected virtual int GetColumn(T value) => 0;
        protected virtual Brush? GetBackground(int row, int column) => null;
    }
}
