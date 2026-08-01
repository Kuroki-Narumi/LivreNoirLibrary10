using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class OpponentStatisticsCollection : StatisticsCollectionBase<string, OpponentStatisticsItem>
    {
        public OpponentStatisticsItem Total { get; } = new() { Tag = "(Total)" };

        public override void Clear()
        {
            Total.Clear();
            base.Clear();
        }

        public void Append(string tag, DuelLog log)
        {
            var item = GetOrAdd(tag);
            item.Tag = tag;
            item.Append(log);
        }

        public override void UpdateRatio(int totalCount)
        {
            Total.CountRatio = 100;
            CountValidRow(Total, _rowTotals);
            base.UpdateRatio(totalCount);
        }

        public override void AppendItemLines(StringBuilder sb)
        {
            Total.AppendLine(sb);
            base.AppendItemLines(sb);
        }

        public override IEnumerator<OpponentStatisticsItem> GetEnumerator()
        {
            yield return Total;
            var enumer = base.GetEnumerator();
            while (enumer.MoveNext())
            {
                yield return enumer.Current;
            }
        }
    }
}
