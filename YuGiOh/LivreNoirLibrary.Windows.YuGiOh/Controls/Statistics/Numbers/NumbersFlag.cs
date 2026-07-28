using LivreNoirLibrary.ObjectModel;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class NumbersFlag : CheckableObject, IClear
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
        public SolidColorBrush? Background => AltBackgroundComboItem.GetBackground(Row, Column);
    }
}
