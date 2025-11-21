using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsViewModel : IBarPositionProvider<double>
    {
        IBmsData Root { get; }
        IBmsDataUnit CurrentData { get; }
        IListEnumerable<BarPosition, Note> CurrentTimeline => CurrentData.Timeline;
        ITimeCounter TimeCounter { get; }
        DoubleBarLengthCache BarLengthCache { get; }

        /// <summary>
        /// Returns an enumerable object that iterates through the data in ascending order(from the last descendant to the root).
        /// </summary>
        /// <returns>an enumerable that can be used to iterate data.</returns>
        IEnumerable<IBmsDataUnit> EnumerateParents();

        /// <summary>
        /// Returns an enumerable object that iterates through the data in descending order(from the root to the last descendant).
        /// </summary>
        /// <returns>an enumerable that can be used to iterate data.</returns>
        IEnumerable<IBmsDataUnit> ReverseEnumerateParents() => EnumerateParents().Reverse();

        void InvalidateTimeCounter();
        void OnModified() { }

        double IBarLengthProvider<double>.GetBarLength(int number)
        {
            if (CurrentData.BarDefs.TryGetValue(number, out var value))
            {
                return value;
            }
            foreach (var data in EnumerateParents())
            {
                if (data.BarDefs.TryGetValue(number, out value))
                {
                    return value; 
                }
            }
            return BmsConstants.DefaultBarLength;
        }

        double IBarPositionProvider<double>.GetAbsolutePosition(BarPosition position) => BarLengthCache.GetAbsolutePosition(position, this);
        BarPosition IBarPositionProvider<double>.GetBarPosition(double absolutePosition) => BarLengthCache.GetBarPosition(absolutePosition, this);
    }
}
