using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class ValueResult
    {
        public List<double> Values { get; } = [];
        public double Average { get; private set; }
        public double Median { get; private set; }
        public int ZeroCount { get; private set; }
        public double ZeroProb { get; private set; }

        public void EndInit()
        {
            var list = Values;
            var count = list.Count;
            if (count is 0)
            {
                Average = Median = ZeroProb = 0;
                ZeroCount = 0;
            }
            else
            {
                list.Sort();
                Average = list.Average();
                var index = count / 2;
                Median = count % 2 is 0 ? (list[index] + list[index - 1]) / 2 : list[index];
                ZeroCount = list.Count(v => v <= 0);
                ZeroProb = (double)ZeroCount / count;
            }
        }

        public void AppendTo(StringBuilder sb)
        {
            sb.AppendLine($"  Average: {Average:F2}");
            sb.AppendLine($"  Median: {Median:F2}");
            sb.AppendLine($"  Zero: {ZeroCount}({ZeroProb:P2})");
        }
    }
}
