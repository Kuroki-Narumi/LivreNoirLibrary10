using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface ISelectionRect
    {
        ElementSelectionMode Mode { get; set; }
        void SetHorizontal(double x, double width);
        void SetVertical(double y, double height);
        void Hide();
    }
}
