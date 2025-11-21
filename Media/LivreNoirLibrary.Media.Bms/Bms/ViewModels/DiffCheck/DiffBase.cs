using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class DiffBase<T> : IDifference
    {
        public DiffType DiffType
        {
            get
            {
                if (EqualityComparer<T>.Default.Equals(OldValue, NewValue))
                {
                    return DiffType.NoChange;
                }
                else if (OldValue is null)
                {
                    return DiffType.Added;
                }
                else if (NewValue is null)
                {
                    return DiffType.Removed;
                }
                else
                {
                    return DiffType.Changed;
                }
            }
        }

        public T? OldValue { get; init; }
        public T? NewValue { get; init; }
    }
}
