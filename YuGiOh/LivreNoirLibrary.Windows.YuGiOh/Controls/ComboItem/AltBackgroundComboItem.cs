using LivreNoirLibrary.Windows.Media;
using System;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract class AltBackgroundComboItem<T>(T value, IVocabData? name) : WithVocabComboItem<T>(value, name)
        where T : struct, Enum
    {
        protected override Brush? GetBackground(int row, int column) => AltBackgroundComboItem.GetBackground(row, column);
    }
}
