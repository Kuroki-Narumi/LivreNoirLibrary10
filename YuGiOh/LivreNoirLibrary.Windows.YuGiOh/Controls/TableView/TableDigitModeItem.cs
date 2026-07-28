using LivreNoirLibrary.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class TableDigitModeItem(TableViewDigitMode value, IVocabData? name) : WithVocabComboItem<TableViewDigitMode>(value, name)
    {
        protected override Brush? GetBackground(int row, int column) => null;

        public static TableDigitModeItem[] Items { get; } =
        [
            new(TableViewDigitMode.DecimalPart, Vocab.Current.Table_Decimal),
            new(TableViewDigitMode.Entire, Vocab.Current.Table_Entire),
        ];

        public static TableDigitModeItem GetItem(TableViewDigitMode item) => Items[(int)item];
    }
}
