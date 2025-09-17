using System.Windows;

namespace LivreNoirLibrary.ObjectModel
{
    public partial class WindowInfo : ObservableObjectBase
    {
        public bool IsValid { get; set => SetValue(ref field, value); }
        public double Left { get; set => SetValue(ref field, value); }
        public double Top { get; set => SetValue(ref field, value); }
        public double Width { get; set => SetValue(ref field, value); }
        public double Height { get; set => SetValue(ref field, value); }

        public WindowInfo() { }
        public WindowInfo(Window window)
        {
            SaveFromWindow(window);
        }

        public virtual void ApplyToWindow(Window window)
        {
            if (IsValid)
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
            IsValid = other.IsValid;
            Left = other.Left;
            Top = other.Top;
            Width = other.Width;
            Height = other.Height;
        }
    }
}
