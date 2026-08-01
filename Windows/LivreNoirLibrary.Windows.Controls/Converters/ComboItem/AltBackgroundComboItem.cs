using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public static class AltBackgroundComboItem
    {
        public static SolidColorBrush AltBackground { get; set; } = MediaUtils.GetBrush("#08000080");
        public static SolidColorBrush? GetBackground(int row, int column) => ((row + column) % 2) is 1 ? AltBackground : Brushes.Transparent;
    }
}
