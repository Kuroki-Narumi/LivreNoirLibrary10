using System;

namespace LivreNoirLibrary.Windows.Controls
{
    [Flags]
    public enum SelectionType
    {
        None,
        Horizontal,
        Vertical,
        Both = Horizontal | Vertical,
        Hide,
    }

    public static class SelectionTypeExtensions
    {
        public static bool IsHorizontal(this SelectionType type) => (type & SelectionType.Horizontal) is not 0;
        public static bool IsVertical(this SelectionType type) => (type & SelectionType.Vertical) is not 0;
        public static bool NeedsHide(this SelectionType type) => (type & SelectionType.Hide) is not 0;
    }
}
