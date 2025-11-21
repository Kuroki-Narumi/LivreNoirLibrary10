using System;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class DestinationTimeline : LongTimeline<double>
    {
        private readonly double _defaultValue;

        public DestinationTimeline(double defaultValue)
        {
            _defaultValue = defaultValue;
            Set(0, defaultValue);
        }

        /// <inheritdoc cref="ITimeline{TX, TValue}.Clear"/>
        public new void Clear()
        {
            base.Clear();
            Set(0, _defaultValue);
        }

        public double GetBlended(long tick, double slope)
        {
            var poss = _pos_list;
            var values = _value_list;
            var index = poss.BinarySearch(tick);
            if (index is >= 0)
            {
                return values[index];
            }
            else
            {
                var rightIndex = ~index;
                var leftIndex = rightIndex - 1;
                var value = values[leftIndex];
                if (slope is > 0 && rightIndex < values.Count)
                {
                    var target = values[rightIndex];
                    var leftPos = poss[leftIndex];
                    var dif = (double)(tick - leftPos) / (poss[rightIndex] - leftPos);
                    if (slope is not 1)
                    {
                        dif = Math.Pow(dif, 1d / slope);
                    }
                    value = (target - value) * dif + value;
                }
                return value;
            }
        }
    }
}
