using LivreNoirLibrary.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows
{
    public interface IGridComboItem
    {
        Brush? Background => AltBackgroundComboItem.GetBackground(Row, Column);
        int Row => 0;
        int Column => 0;
    }
}
