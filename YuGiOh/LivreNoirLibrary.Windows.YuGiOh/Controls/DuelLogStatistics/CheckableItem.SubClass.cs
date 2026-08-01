using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls.DuelLogStatistics
{
    public class RankItem(Rank value) : CheckableItem
    {
        public Rank Value { get; } = value;
        public override Brush? Background => AltBackgroundComboItem.GetBackground(0, Column);
    }

    public class OrderItem(Order value) : CheckableItem
    {
        public Order Value { get; } = value;
        public required bool IsOptionMarkVisible { get; init; }
    }

    public class ResultItem(Result value) : CheckableItem
    {
        public Result Value { get; } = value;
    }
}
