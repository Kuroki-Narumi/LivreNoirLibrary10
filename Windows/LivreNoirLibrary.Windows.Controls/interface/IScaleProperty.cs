using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IScaleProperty
    {
        public const double DefaultScale = 1.0;

        public static readonly DependencyProperty ScaleXProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultScale);
        public static readonly DependencyProperty ScaleYProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultScale);

        public static DependencyProperty RegisterScaleX<T>(PropertyChangedCallback callback, CoerceValueCallback? coerce = null)
            => ScaleXProperty.AddOwner(typeof(T), DefaultScale, callback, coerce: coerce);
        public static DependencyProperty RegisterScaleY<T>(PropertyChangedCallback callback, CoerceValueCallback? coerce = null)
            => ScaleYProperty.AddOwner(typeof(T), DefaultScale, callback, coerce: coerce);

        public double ScaleX { get; set; }
        public double ScaleY { get; set; }

        public static double ChangeScaleCore(double[] ary, double current, int delta)
        {
            var count = ary.Length;
            var index = Array.BinarySearch(ary, current);
            if (delta is > 0)
            {
                if (index is >= 0)
                {
                    index++;
                }
                else
                {
                    index = ~index;
                }
                return index < count ? ary[index] : ary[^1];
            }
            else
            {
                if (index is >= 0)
                {
                    index--;
                }
                else
                {
                    index = ~index - 1;
                }
                return index is >= 0 ? ary[index] : ary[0];
            }
        }
    }
}
