using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface INoteRectContainer
    {
        double TextScale { get; }
        int HeadHeight { get; }
        bool DisplaysValueText { get; }
        Color MineColor { get; }
        Color LongEndColor { get; }
        Color SelectedColor { get; }
        Color SelectedLongColor { get; }
        Color InvalidColor { get; }
        Color NoteTextColor { get; }
    }
}
