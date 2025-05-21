using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Collections;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class RadarChart
    {
        public const bool DefaultIsValueVisible = true;
        public const double DefaultRadius = 100;
        public const double DefaultDoubleValue = double.NaN;
        public const int DefaultStep = 4;
        public const double DefaultAngle = 0.25;
        public const SweepDirection DefaultSweepDirection = SweepDirection.Clockwise;
        public const bool DefaultIsFilled = true;
        public const bool DefaultIsGradient = true;
        public const double DefaultFillOpacity = 0.5;

        public static readonly SolidColorBrush DefaultFrameBrush = GetBrush(255, 224, 224, 224);
        public static readonly SolidColorBrush DefaultStepBrush = GetBrush(255, 192, 224, 255);
        public const double DefaultFrameThickness = 1;
        public const double DefaultStepThickness = 1;

        public const double DefaultLineThickness = 2;
        public static readonly Color DefaultMaxColor = Color.FromArgb(255, 255, 0, 0);
        public static readonly Color DefaultMinColor = Color.FromArgb(255, 0, 0, 255);
        public static readonly Color DefaultMidColor = Color.FromArgb(255, 0, 255, 0);

        protected readonly ObservableList<object?> _captions = [];
        protected readonly ObservableList<double> _values = [];

        public IEnumerable<object?> Captions
        {
            get => _captions;
            set
            {
                _captions.ClearWithoutNotify();
                _captions.AddRange(value);
            }
        }

        public IEnumerable<double> Values
        {
            get => _values;
            set
            {
                _values.ClearWithoutNotify();
                _values.AddRange(value);
            }
        }

        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private object? _title;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private bool _isValueVisible = DefaultIsValueVisible;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private string? _valueFormat;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private double _radius = DefaultRadius;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _offsetX = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _offsetY = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private double _maxValue = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private double _minValue = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private double _midValue = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _step = DefaultStep;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private double _angle = DefaultAngle;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private SweepDirection _sweepDirection = DefaultSweepDirection;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _verticalCaptionOffset = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _horizontalCaptionOffset = DefaultDoubleValue;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private bool _isFilled = DefaultIsFilled;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _fillOpacity = DefaultFillOpacity;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private bool _isGradient = DefaultIsGradient;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Brush? _frameBrush = DefaultFrameBrush;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _frameThickness = DefaultFrameThickness;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Brush? _stepBrush = DefaultStepBrush;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _stepThickness = DefaultStepThickness;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _lineThickness = DefaultLineThickness;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private Color _maxColor = DefaultMaxColor;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private Color _minColor = DefaultMinColor;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsMeasure = true)]
        private Color _midColor = DefaultMidColor;

        private void OnIsValueVisibleChanged(bool value) => ReserveRefresh();
        private void OnMaxValueChanged(double value) => ReserveRefresh();
        private void OnMinValueChanged(double value) => ReserveRefresh();
        private void OnMidValueChanged(double value) => ReserveRefresh();
        private void OnAngleChanged(double value) => ReserveRefresh();
        private void OnSweepDirectionChanged(SweepDirection value) => ReserveRefresh();
        private void OnIsFilledChanged(bool value) => ReserveRefresh();
        private void OnIsGradientChanged(bool value) => ReserveRefresh();
        private void OnMaxColorChanged(Color value) => ReserveRefresh();
        private void OnMinColorChanged(Color value) => ReserveRefresh();
        private void OnMidColorChanged(Color value) => ReserveRefresh();
    }
}
