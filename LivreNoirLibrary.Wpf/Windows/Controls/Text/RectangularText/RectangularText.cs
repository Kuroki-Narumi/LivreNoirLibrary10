using LivreNoirLibrary.Media;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LivreNoirLibrary.Windows.Controls
{
    public sealed partial class RectangularText : FrameworkElement
    {
        static RectangularText()
        {
            PropertyUtils.OverrideDefaultStyleKey<RectangularText>();
            InitializePathData();
        }

        public static readonly SolidColorBrush DefaultFill = Brushes.Black;
        public static readonly Brush? DefaultStroke = null;
        public const double DefaultThickness = 2;
        public const double DefaultStrokeThickness = 1;
        public const VerticalAlignment DefaultVerticalAlignment = VerticalAlignment.Top;
        public const HorizontalAlignment DefaultHorizontalAlignment = HorizontalAlignment.Left;

        public static readonly DependencyProperty TextProperty = TextBlock.TextProperty.AddOwner<string>(typeof(RectangularText), null, OnTextChanged);
        public static readonly DependencyProperty FillProperty = Shape.FillProperty.AddOwner(typeof(RectangularText), DefaultFill, OnFillChanged);
        public static readonly DependencyProperty StrokeProperty = Shape.StrokeProperty.AddOwner(typeof(RectangularText), DefaultStroke, OnStrokeChanged);
        public static readonly DependencyProperty ThicknessProperty = PropertyUtils.Register(typeof(RectangularText), DefaultThickness, OnThicknessChanged);
        public static readonly DependencyProperty StrokeThicknessProperty = Shape.StrokeThicknessProperty.AddOwner(typeof(RectangularText), DefaultStrokeThickness, OnStrokeThicknessChanged);

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RectangularText r)
            {
                r._text = e.NewValue as string;
                r.InvalidateMeasure();
                r.InvalidateVisual();
            }
        }

        private static void OnFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RectangularText r)
            {
                r._fill = e.NewValue as Brush;
                r.InvalidateVisual();
            }
        }

        private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RectangularText r)
            {
                r._stroke = e.NewValue as Brush;
                r.InvalidateVisual();
            }
        }

        private static void OnThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RectangularText r)
            {
                r._thickness = (double)e.NewValue;
                r.InvalidateMeasure();
            }
        }

        private static void OnStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RectangularText r)
            {
                r._stroke_thickness = (double)e.NewValue;
                r.InvalidateMeasure();
            }
        }

        private string? _text;
        private Brush? _fill = DefaultFill;
        private Brush? _stroke = DefaultStroke;
        private double _thickness = DefaultThickness;
        private double _stroke_thickness = DefaultStrokeThickness;
        private double _width;
        private double _height;

        public string? Text { get => _text; set => SetValue(TextProperty, value); }
        public Brush? Fill { get => _fill; set => SetValue(FillProperty, value); }
        public Brush? Stroke { get => _stroke; set => SetValue(StrokeProperty, value); }
        public double Thickness { get => _thickness; set => SetValue(ThicknessProperty, Math.Max(value, 0)); }
        public double StrokeThickness { get => _stroke_thickness; set => SetValue(StrokeThicknessProperty, Math.Max(value, 0)); }

        public RectangularText()
        {
            UseLayoutRounding = true;
            SnapsToDevicePixels = true;
        }

        public (double Width, double Height) GetSize() => string.IsNullOrEmpty(_text) ? (0, 0) : CalcSize(_text, _thickness, _stroke_thickness);

        protected override Size MeasureOverride(Size availableSize)
        {
            (_width, _height) = GetSize();
            return new(_width, _height);
        }

        protected override void OnRender(DrawingContext dc)
        {
            Render(dc, 0, 0, _text, _fill, _stroke, _thickness, _stroke_thickness);
        }

        public static void Render(DrawingContext dc, double x, double y, string? text, Brush? fill, Brush? stroke, double th = DefaultThickness, double th2 = DefaultStrokeThickness)
        {
            if (string.IsNullOrEmpty(text)) { return; }
            var translate = x is not 0 || y is not 0;
            if (translate)
            {
                dc.PushTransform(new TranslateTransform(x, y));
            }
            var g = CreateGeometry(text, th, th2);
            var pen = MediaUtils.GetPen(stroke, th2 * 2);
            if (pen is not null)
            {
                dc.DrawGeometry(null, pen, g);
            }
            if (fill is not null)
            {
                dc.DrawGeometry(fill, null, g);
            }
            if (translate)
            {
                dc.Pop();
            }
        }
    }
}
