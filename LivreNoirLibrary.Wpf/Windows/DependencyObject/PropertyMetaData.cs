using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class PropertyUtils
    {
        public static FrameworkPropertyMetadata GetMeta(object? defaultValue, PropertyChangedCallback? changed = null, CoerceValueCallback? coerce = null)
        {
            return new(defaultValue, FrameworkPropertyMetadataOptions.AffectsRender, changed, coerce);
        }

        public static FrameworkPropertyMetadata GetMetaTwoWay(object? defaultValue, PropertyChangedCallback? changed = null, CoerceValueCallback? coerce = null)
        {
            return new(defaultValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, changed, coerce);
        }

        private static void EX_OnMeasurePropertyChanged(this object? obj, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UIElement)?.InvalidateMeasure();
        }

        private static void EX_OnVisualPropertyChanged(this object? obj, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UIElement)?.InvalidateVisual();
        }

        public static PropertyChangedCallback OnMeasurePropertyChanged { get; } = new(default(object).EX_OnMeasurePropertyChanged);
        public static PropertyChangedCallback OnVisualPropertyChanged { get; } = new(default(object).EX_OnVisualPropertyChanged);
    }
}
