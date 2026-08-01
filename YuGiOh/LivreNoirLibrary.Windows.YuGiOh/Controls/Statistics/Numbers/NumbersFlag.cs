using LivreNoirLibrary.ObjectModel;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class NumbersFlag : CheckableObject, IClear, IGridComboItem
    {
        public const int ColumnCount = 10;

        public int Number { get; set => SetValue(ref field, value, [nameof(Column), nameof(Row), nameof(Background)]); }

        public void Clear()
        {
            IsChecked = false;
            Number = 0;
        }

        public int Column => Number % ColumnCount;
        public int Row => Number / ColumnCount;
        public Brush? Background => AltBackgroundComboItem.GetBackground(Row, Column);
    }
}
