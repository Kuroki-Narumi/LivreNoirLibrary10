using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows
{
    public abstract class ComboItemBase<T>
        where T : struct, Enum
    {
        public T Value { get; }
        public string Name { get; }
        public int Row { get; }
        public int Column { get; }

        protected ComboItemBase(T value, string name)
        {
            Value = value;
            Name = name;
            Row = GetRow(value);
            Column = GetColumn(value);
        }

        protected static Dictionary<T, TValue> CreateItems<TValue>(Func<T, TValue> creator)
        {
            Dictionary<T, TValue> result = [];
            foreach (var value in Enum.GetValues<T>())
            {
                result.Add(value, creator(value));
            }
            return result;
        }

        protected virtual int GetRow(T value) => 0;
        protected virtual int GetColumn(T value) => 0;

        public override string ToString() => Name;
    }
}
