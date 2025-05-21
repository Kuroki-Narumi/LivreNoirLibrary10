using System.Windows;

namespace LivreNoirLibrary.ObjectModel
{
    public partial class WindowInfo : ObservableObjectBase
    {
        [ObservableProperty]
        protected bool _isValid = false;
        [ObservableProperty]
        protected double _left = 0;
        [ObservableProperty]
        protected double _top = 0;
        [ObservableProperty]
        protected double _width = 0;
        [ObservableProperty]
        protected double _height = 0;

        public WindowInfo() { }
        public WindowInfo(Window window)
        {
            SaveFromWindow(window);
        }

        public virtual void ApplyToWindow(Window window)
        {
            if (_isValid)
            {
                if (window.Owner is Window owner)
                {
                    window.Left = Left + owner.Left;
                    window.Top = Top + owner.Top;
                }
                else
                {
                    window.Left = Left;
                    window.Top = Top;
                }
                window.Width = Width;
                window.Height = Height;
            }
        }

        public virtual void SaveFromWindow(Window window)
        {
            IsValid = true;
            if (window.Owner is Window owner)
            {
                Left = window.Left - owner.Left;
                Top = window.Top - owner.Top;
            }
            else
            {
                Left = window.Left;
                Top = window.Top;
            }
            Width = window.Width;
            Height = window.Height;
        }

        public void Load(WindowInfo other)
        {
            IsValid = other._isValid;
            Left = other._left;
            Top = other._top;
            Width = other._width;
            Height = other._height;
        }
    }
}
