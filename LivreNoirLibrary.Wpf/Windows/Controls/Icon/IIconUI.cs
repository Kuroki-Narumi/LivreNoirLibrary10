using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IIconUI
    {
        public const double DefaultIconSize = 16;
        public const Stretch DefaultIconStretch = Stretch.UniformToFill;

        public static readonly DependencyProperty IconProperty = PropertyUtils.RegisterAttachedTwoWay<object>(typeof(PropertyHolder));
        public static readonly DependencyProperty IconSizeProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIconSize);
        public static readonly DependencyProperty IconStretchProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIconStretch);
        public static readonly DependencyProperty IconFontFamilyProperty = PropertyUtils.RegisterAttachedTwoWay<FontFamily>(typeof(PropertyHolder));

        public object? Icon { get; set; }
        public double IconSize { get; set; }
        public Stretch IconStretch { get; set; }
        public FontFamily? IconFontFamily { get; set; }
    }
}
