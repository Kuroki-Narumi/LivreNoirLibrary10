using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IIconUI
    {
        const double DefaultIconSize = 16;
        const Stretch DefaultIconStretch = Stretch.Uniform;

        static readonly DependencyProperty IconProperty = PropertyUtils.RegisterAttachedTwoWay<object>(typeof(PropertyHolder));
        static readonly DependencyProperty IconSizeProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIconSize);
        static readonly DependencyProperty IconStretchProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIconStretch);
        static readonly DependencyProperty IconFontFamilyProperty = PropertyUtils.RegisterAttachedTwoWay<FontFamily>(typeof(PropertyHolder));

        object? Icon { get; set; }
        double IconSize { get; set; }
        Stretch IconStretch { get; set; }
        FontFamily? IconFontFamily { get; set; }
    }
}
