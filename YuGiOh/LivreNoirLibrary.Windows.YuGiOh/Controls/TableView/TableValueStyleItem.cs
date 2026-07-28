using LivreNoirLibrary.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class TableValueStyleItem(TableViewValueStyle value, IVocabData? name) : WithVocabComboItem<TableViewValueStyle>(value, name)
    {
        protected override Brush? GetBackground(int row, int column) => null;

        public static TableValueStyleItem[] Items { get; } =
        [
            new(TableViewValueStyle.Normal, Vocab.Current.Table_Style_Count),
            new(TableViewValueStyle.RatioVertical, Vocab.Current.Table_Style_RatioV),
            new(TableViewValueStyle.RatioHorizontal, Vocab.Current.Table_Style_RatioH),
        ];

        public static TableValueStyleItem GetItem(TableViewValueStyle item) => Items[(int)item];
    }
}
