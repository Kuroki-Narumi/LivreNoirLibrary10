using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class HandStatisticsItem : StatisticsItemBase, ICard
    {
        public Card? Card { get; set; }
        public Card ThisCard => Card!;

        public override void Clear()
        {
            Card = null;
            base.Clear();
        }

        public override void AppendLine(StringBuilder sb)
        {
            sb.Append(Card?.Name);
            sb.Append('\t');
            base.AppendLine(sb);
        }
    }
}
