using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface ISelectionRect
    {
        public SelectionMode Mode { get; set; }
        public void SetHorizontal(double x, double width);
        public void SetVertical(double y, double height);
        public void Hide();
    }
}
