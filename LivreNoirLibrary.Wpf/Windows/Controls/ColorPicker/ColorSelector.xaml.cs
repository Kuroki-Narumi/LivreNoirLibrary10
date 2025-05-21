using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// ColorPicker.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorSelector : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty = ColorPicker.SelectedColorProperty.AddOwner(typeof(ColorSelector), default(Color));
        public static readonly DependencyProperty IsAlphaEnabledProperty = ColorPicker.IsAlphaEnabledProperty.AddOwner(typeof(ColorSelector));

        public Color SelectedColor { get => (Color)GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }
        public bool IsAlphaEnabled { get => (bool)GetValue(IsAlphaEnabledProperty); set => SetValue(IsAlphaEnabledProperty, value); }
        public ColorInfo ColorInfo => _color_info;

        private readonly ColorInfo _color_info = new();
        private readonly Dictionary<ColorImageIndex, UIElement> _images;
        private readonly Dictionary<object, ColorImageIndex> _radio_indexes;
        private ColorImageIndex _current_image;

        public ColorSelector()
        {
            DataContext = this;
            InitializeComponent();
            _images = new()
            {
                { ColorImageIndex.Rgb, Image_RGB },
                { ColorImageIndex.Grb, Image_GRB },
                { ColorImageIndex.Brg, Image_BRG },
                { ColorImageIndex.Hsv, Image_HSV },
                { ColorImageIndex.Shv, Image_SHV },
                { ColorImageIndex.Vhs, Image_VHS },
            };
            _radio_indexes = new()
            {
                { Radio_R, ColorImageIndex.Rgb },
                { Radio_G, ColorImageIndex.Grb },
                { Radio_B, ColorImageIndex.Brg },
                { Radio_H, ColorImageIndex.Hsv },
                { Radio_S, ColorImageIndex.Shv },
                { Radio_V, ColorImageIndex.Vhs },
            };
            InitializePalettes();
            UpdateImage(Radio_H);
            Update();
            _color_info.PropertyChanged += OnPropertyChanged_ColorInfo;
        }

        private void OnPropertyChanged_ColorInfo(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(ColorInfo.Color))
            {
                Update();
            }
        }

        private void UpdateImage(object sender)
        {
            if (_radio_indexes.TryGetValue(sender, out var index))
            {
                _images[_current_image].Visibility = Visibility.Collapsed;
                _current_image = index;
                _images[_current_image].Visibility = Visibility.Visible;
                UpdatePointer();
                UpdateSliderIndicator();
            }
        }

        private void Update()
        {
            SelectedColor = _color_info.Color;
            UpdatePointer();
            UpdateSliderIndicator();
            UpdatePalettes();
            UpdateHsvRect();
            UpdateSliderRect();
        }

        private void UpdatePointer()
        {
            var (x, y) = _current_image switch
            {
                ColorImageIndex.Rgb => (ColorUtils.GetByte(_color_info.G), ColorUtils.GetByte(1 - _color_info.B)),
                ColorImageIndex.Grb => (ColorUtils.GetByte(_color_info.R), ColorUtils.GetByte(1 - _color_info.B)),
                ColorImageIndex.Brg => (ColorUtils.GetByte(_color_info.R), ColorUtils.GetByte(1 - _color_info.G)),
                ColorImageIndex.Hsv => (ColorUtils.GetByte(_color_info.S), ColorUtils.GetByte(1 - _color_info.V)),
                ColorImageIndex.Shv => (ColorUtils.GetByte(_color_info.H / 359), ColorUtils.GetByte(1 - _color_info.V)),
                ColorImageIndex.Vhs => (ColorUtils.GetByte(_color_info.H / 359), ColorUtils.GetByte(1 - _color_info.S)),
                _ => (0, 0),
            };
            Canvas.SetLeft(Pointer, x);
            Canvas.SetTop(Pointer, y);
        }

        private void UpdateSliderIndicator()
        {
            int y = _current_image switch
            {
                ColorImageIndex.Rgb => ColorUtils.GetByte(1 - _color_info.R),
                ColorImageIndex.Grb => ColorUtils.GetByte(1 - _color_info.G),
                ColorImageIndex.Brg => ColorUtils.GetByte(1 - _color_info.B),
                ColorImageIndex.Hsv => ColorUtils.GetByte(_color_info.H / 359),
                ColorImageIndex.Shv => ColorUtils.GetByte(1 - _color_info.S),
                ColorImageIndex.Vhs => ColorUtils.GetByte(1 - _color_info.V),
                _ => 0,
            };
            Canvas.SetTop(SlideIndicator, y);
        }

        private void UpdateHsvRect()
        {
            var hue = _color_info.H;
            byte r, g, b;
            switch (_color_info.H)
            {
                case < 60:
                    r = 255;
                    g = ColorUtils.GetByte(hue / 60);
                    b = 0;
                    break;
                case < 120:
                    r = ColorUtils.GetByte((120 - hue) / 60);
                    g = 255;
                    b = 0;
                    break;
                case < 180:
                    r = 0;
                    g = 255;
                    b = ColorUtils.GetByte((hue - 120) / 60);
                    break;
                case < 240:
                    r = 0;
                    g = ColorUtils.GetByte((240 - hue) / 60);
                    b = 255;
                    break;
                case < 300:
                    r = ColorUtils.GetByte((hue - 240) / 60);
                    g = 0;
                    b = 255;
                    break;
                default:
                    r = 255;
                    g = 0;
                    b = ColorUtils.GetByte((360 - hue) / 60);
                    break;
            }
            Brush_HSV.Color = Color.FromArgb(255, r, g, b);
        }

        private void UpdateSliderRect()
        {
            var c = _color_info.GetColor();
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
            var x = (float)(pos.X / 255);
            var y = (float)(pos.Y / 255);
            switch (_current_image)
            {
                case ColorImageIndex.Rgb:
                    _color_info.G = x;
                    _color_info.B = 1 - y ;
                    break;
                case ColorImageIndex.Grb:
                    _color_info.R = x ;
                    _color_info.B = 1 - y;
                    break;
                case ColorImageIndex.Brg:
                    _color_info.R = x;
                    _color_info.G = 1 - y;
                    break;
                case ColorImageIndex.Hsv:
                    _color_info.S = x;
                    _color_info.V = 1 - y;
                    break;
                case ColorImageIndex.Shv:
                    _color_info.H = x * 359;
                    _color_info.V = 1 - y;
                    break;
                case ColorImageIndex.Vhs:
                    _color_info.H = x * 359;
                    _color_info.S = 1 - y;
                    break;
            }
        }

        private void OnMouseMove_Slider(object sender, MouseEventArgs e)
        {
            var y = (float)(e.GetPosition((sender as IInputElement)!).Y / 255);
            switch (_current_image)
            {
                case ColorImageIndex.Rgb:
                    _color_info.R = 1 - y;
                    break;
                case ColorImageIndex.Grb:
                    _color_info.G = 1 - y;
                    break;
                case ColorImageIndex.Brg:
                    _color_info.B = 1 - y;
                    break;
                case ColorImageIndex.Hsv:
                    _color_info.H = y * 359;
                    break;
                case ColorImageIndex.Shv:
                    _color_info.S = 1 - y;
                    break;
                case ColorImageIndex.Vhs:
                    _color_info.V = 1 - y;
                    break;
            }
        }

        private void OnWheel_Slider(object sender, MouseWheelEventArgs e)
        {
            (sender as Slider)?.ChangeByWheel(e, 5);
        }

        private bool OnVerify_TextBox(string text) => ColorUtils.IsValidColorCode(text);
    }
}
