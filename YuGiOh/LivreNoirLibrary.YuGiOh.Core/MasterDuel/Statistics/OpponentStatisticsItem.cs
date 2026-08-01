using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class OpponentStatisticsItem : StatisticsItemBase
    {
        public string? Tag { get; set; }

        public override void AppendLine(StringBuilder sb)
        {
            sb.Append(Tag);
            sb.Append('\t');
            base.AppendLine(sb);
        }
    }
}
