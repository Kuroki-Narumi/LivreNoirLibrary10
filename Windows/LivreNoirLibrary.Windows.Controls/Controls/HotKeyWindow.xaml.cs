using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Windows.Input;
using InputManager = LivreNoirLibrary.Windows.Input.InputManager;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// HotKeyWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HotKeyWindow : Window
    {
        public bool IsDecided { get; private set; }
        public KeyInput KeyInput { get; private set; }

        public static (bool IsDecided, KeyInput KeyInput) StartDialog(Window owner, string? message = null)
        {
            InputManager.StopHotKey();
            HotKeyWindow window = new(owner, message);
            window.ShowDialog();
            InputManager.RestartHotKey();
            return (window.IsDecided, window.KeyInput);
        }

        public HotKeyWindow(Window owner, string? message = null)
        {
            Owner = owner;
            InitializeComponent();
            var bounds = owner.GetScaledScreenBounds();
            Left = bounds.X;
            Top = bounds.Y;
            Width = bounds.Width;
            Height = bounds.Height;
            if (!string.IsNullOrEmpty(message))
            {
                TextBlock.Inlines.Clear();
                TextBlock.Text = message;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key is Key.Escape)
            {
                e.Handled = true;
                Close();
            }
            else
            {
                KeyInput = new(e);
                if (!KeyInput.IsSystemKey(KeyInput.Key))
                {
                    IsDecided = true;
                    e.Handled = true;
                    Close();
                }
            }
        }

        protected override void OnDeactivated(EventArgs e)
        {
            Activate();
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            Close();
        }
    }
}
