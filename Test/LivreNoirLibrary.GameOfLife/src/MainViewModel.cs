using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.GameOfLife
{
    public class MainViewModel : ObservableObjectBase
    {
        private bool _isRunning;

        public Settings Settings { get; } = new();
        public FieldState FieldState { get; } = new();
        public WriteableBitmap FieldBitmap { get; } = Bitmap.Create((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
        public int OffsetX { get => field; set => SetValue(ref field, value, UpdateBitmap); }
        public int OffsetY { get => field; set => SetValue(ref field, value, UpdateBitmap); }

        public void ClearField()
        {
            FieldState.LivingCells.Clear();
            UpdateBitmap();
        }

        public void UpdateBitmap()
        {
            using var p = FieldBitmap.BeginWrite();
            p.Fill(Settings.DeadCellColor.ToUInt());
            var livingColor = Settings.LivingCellColor.ToUInt();
            foreach (var (x, y) in FieldState.LivingCells)
            {
                p.SetPixel(x, y, livingColor);
            }
        }

        public void StartDrawing(UIElement element, MouseButtonEventArgs e)
        {
            var isLiving = e.LeftButton is MouseButtonState.Pressed;
            if (!isLiving && e.RightButton is not MouseButtonState.Pressed)
            {
                return;
            }
            var cells = FieldState.LivingCells;

            void MouseMove(object sender, MouseEventArgs e)
            {
                UpdateCell(e, isLiving);
                e.Handled = true;
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                element.ReleaseMouseCapture();
                element.MouseMove -= MouseMove;
                element.MouseUp -= MouseUp;
                e.Handled = true;
            }

            element.CaptureMouse();
            element.MouseMove += MouseMove;
            element.MouseUp += MouseUp;
            e.Handled = true;
            UpdateCell(e, isLiving);

            void UpdateCell(MouseEventArgs e, bool isLiving)
            {
                var point = e.GetPosition(element);
                var p = new Position((int)point.X, (int)point.Y);
                if (isLiving ? cells.Add(p) : cells.Remove(p))
                {
                    UpdateBitmap();
                }
            }
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                CompositionTarget.Rendering += Update;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                CompositionTarget.Rendering -= Update;
            }
        }

        private void Update(object? sender, EventArgs e)
        {
            FieldState.UpdateEffect();
            FieldState.UpdateCells();
            UpdateBitmap();
        }
    }
}
