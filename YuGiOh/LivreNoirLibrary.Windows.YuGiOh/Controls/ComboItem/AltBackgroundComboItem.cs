using LivreNoirLibrary.Windows.Media;
using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract class AltBackgroundComboItem<T>(T value, IVocabData? name) : WithVocabComboItem<T>(value, name)
        where T : struct, Enum
    {
        protected override Brush? GetBackground(int row, int column) => AltBackgroundComboItem.GetBackground(row, column);
    }

    public static class AltBackgroundComboItem
    {
        private static SolidColorBrush AltBackground { get; } = MediaUtils.GetBrush("#08000080");
        internal static SolidColorBrush? GetBackground(int row, int column) => ((row + column) % 2) is 1 ? AltBackground : Brushes.Transparent;
    }
}
