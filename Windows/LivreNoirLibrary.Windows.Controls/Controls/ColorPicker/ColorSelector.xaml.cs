using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// ColorPicker.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorSelector : UserControl
    {
        public const float HueInvertFactor_Part = 1f / 60f;

        public static readonly DependencyProperty SelectedColorProperty = ColorPicker.SelectedColorProperty.AddOwner(typeof(ColorSelector), default(Color));

        private readonly Dictionary<ImageMode, UIElement> _images;
        private readonly Dictionary<object, ImageMode> _radioMap;
        private ImageMode _mode;
        private bool _colorCodeEditing;

        public Color SelectedColor { get => (Color)GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }
        public ColorInfo ColorInfo { get; } = new();

        public ColorSelector()
        {
            DataContext = this;
            InitializeComponent();
            _images = new()
            {
                { ImageMode.Rgb, Image_RGB },
                { ImageMode.Grb, Image_GRB },
                { ImageMode.Brg, Image_BRG },
                { ImageMode.Hsv, Image_HSV },
                { ImageMode.Shv, Image_SHV },
                { ImageMode.Vhs, Image_VHS },
            };
            _radioMap = new()
            {
                { Radio_R, ImageMode.Rgb },
                { Radio_G, ImageMode.Grb },
                { Radio_B, ImageMode.Brg },
                { Radio_H, ImageMode.Hsv },
                { Radio_S, ImageMode.Shv },
                { Radio_V, ImageMode.Vhs },
            };
            InitializePalettes();
            UpdateImage(Radio_H);
            ColorInfo.ColorChanged += OnColorChanged;
        }

        public void Setup(Color color, bool alpha)
        {
            ColorInfo.IsAlphaEnabled = alpha;
            ColorInfo.SetColor(color);
        }

        private void OnColorChanged(Color color)
        {
            Update();
        }

        private void UpdateImage(object sender)
        {
            if (_radioMap.TryGetValue(sender, out var index))
            {
                _images[_mode].Visibility = Visibility.Collapsed;
                _mode = index;
                _images[_mode].Visibility = Visibility.Visible;
                UpdateCursor();
            }
        }

        private void Update()
        {
            SelectedColor = ColorInfo.Color;
            UpdateCursor();
            UpdatePalettes();
            UpdateHsvRect();
            UpdateSliderRect();
            if (!_colorCodeEditing)
            {
                TextBox_ColorCode.Text = ColorInfo.GetColorCode();
            }
        }

        private void UpdateCursor()
        {
            var info = ColorInfo;
            var (x, y, z) = _mode switch
            {
                ImageMode.Rgb => (info.IntG, 255 - info.IntB, 255 - info.IntR),
                ImageMode.Grb => (info.IntR, 255 - info.IntB, 255 - info.IntG),
                ImageMode.Brg => (info.IntR, 255 - info.IntG, 255 - info.IntB),
                ImageMode.Hsv => (info.IntS, 255 - info.IntV, info.ScaledIntH),
                ImageMode.Shv => (info.ScaledIntH, 255 - info.IntV, 255 - info.IntS),
                ImageMode.Vhs => (info.ScaledIntH, 255 - info.IntS, 255 - info.IntV),
                _ => (0, 0, 0),
            };
            Canvas.SetLeft(Pointer, x);
            Canvas.SetTop(Pointer, y);
            Canvas.SetTop(SlideIndicator, z);
        }

        private void UpdateHsvRect()
        {
            var hue = ColorInfo.H;
            byte r, g, b;
            switch (hue)
            {
                case < 60:
                    r = 255;
                    g = ColorUtils.GetByte(hue * HueInvertFactor_Part);
                    b = 0;
                    break;
                case < 120:
                    r = ColorUtils.GetByte((120 - hue) * HueInvertFactor_Part);
                    g = 255;
                    b = 0;
                    break;
                case < 180:
                    r = 0;
                    g = 255;
                    b = ColorUtils.GetByte((hue - 120) * HueInvertFactor_Part);
                    break;
                case < 240:
                    r = 0;
                    g = ColorUtils.GetByte((240 - hue) * HueInvertFactor_Part);
                    b = 255;
                    break;
                case < 300:
                    r = ColorUtils.GetByte((hue - 240) * HueInvertFactor_Part);
                    g = 0;
                    b = 255;
                    break;
                default:
                    r = 255;
                    g = 0;
                    b = ColorUtils.GetByte((360 - hue) * HueInvertFactor_Part);
                    break;
            }
            Brush_HSV.Color = Color.FromArgb(255, r, g, b);
        }

        private void UpdateSliderRect()
        {
            var c = ColorInfo.Color;
            var brush = Brush_RGB;
            brush.GradientStops[0].Color = Color.FromRgb(255, c.G, c.B);
            brush.GradientStops[1].Color = Color.FromRgb(0, c.G, c.B);
            brush = Brush_GRB;
            brush.GradientStops[0].Color = Color.FromRgb(c.R, 255, c.B);
            brush.GradientStops[1].Color = Color.FromRgb(c.R, 0, c.B);
            brush = Brush_BRG;
            brush.GradientStops[0].Color = Color.FromRgb(c.R, c.G, 255);
            brush.GradientStops[1].Color = Color.FromRgb(c.R, c.G, 0);
        }

        private void OnCheck_Color(object sender, RoutedEventArgs e) => UpdateImage(sender);

        private void OnMouseDownCore(object sender, MouseEventHandler move, MouseButtonEventArgs e)
        {
            if (sender is not Canvas c)
            {
                return;
            }
            c.CaptureMouse();

            void mouseUp(object s, MouseButtonEventArgs e)
            {
                e.Handled = true;
                c.ReleaseMouseCapture();
                c.MouseMove -= move;
                c.MouseLeftButtonUp -= mouseUp;
            }

            c.MouseMove += move;
            c.MouseLeftButtonUp += mouseUp;
            move(c, e);
        }

        private void OnMouseDown_Canvas(object sender, MouseButtonEventArgs e) => OnMouseDownCore(sender, OnMouseMove_Main, e);
        private void OnMouseDown_Slider(object sender, MouseButtonEventArgs e) => OnMouseDownCore(sender, OnMouseMove_Slider, e);

        private void OnMouseMove_Main(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition((sender as IInputElement)!);
            var x = Math.Clamp(pos.X.RoundToInt(), 0, 255);
            var y = Math.Clamp(255 - pos.Y.RoundToInt(), 0, 255);
            var info = ColorInfo;
            switch (_mode)
            {
                case ImageMode.Rgb:
                    info.IntG = x;
                    info.IntB = y;
                    break;
                case ImageMode.Grb:
                    info.IntR = x;
                    info.IntB = y;
                    break;
                case ImageMode.Brg:
                    info.IntR = x;
                    info.IntG = y;
                    break;
                case ImageMode.Hsv:
                    info.IntS = x;
                    info.IntV = y;
                    break;
                case ImageMode.Shv:
                    info.ScaledIntH = x;
                    info.IntV = y;
                    break;
                case ImageMode.Vhs:
                    info.ScaledIntH = x;
                    info.IntS = y;
                    break;
            }
        }

        private void OnMouseMove_Slider(object sender, MouseEventArgs e)
        {
            var y = Math.Clamp((int)e.GetPosition((sender as IInputElement)!).Y, 0, 255);
            var info = ColorInfo;
            switch (_mode)
            {
                case ImageMode.Rgb:
                    info.IntR = 255 - y;
                    break;
                case ImageMode.Grb:
                    info.IntG = 255 - y;
                    break;
                case ImageMode.Brg:
                    info.IntB = 255 - y;
                    break;
                case ImageMode.Hsv:
                    info.ScaledIntH = y;
                    break;
                case ImageMode.Shv:
                    info.IntS = 255 - y;
                    break;
                case ImageMode.Vhs:
                    info.IntV = 255 - y;
                    break;
            }
        }

        private void OnWheel_Slider(object sender, MouseWheelEventArgs e)
        {
            (sender as Slider)?.ChangeByWheel(e, 5);
        }

        private bool OnVerify_ColorCode(string text)
        {
            _colorCodeEditing = true;
            var result = ColorInfo.TrySetColorCode(text);
            _colorCodeEditing = false;
            return result;
        }
    }
}
