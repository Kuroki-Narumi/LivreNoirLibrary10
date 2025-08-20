using System;
using System.Windows;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoirLibrary.Windows
{
    public interface IResize
    {
        public const double DefaultMoveThreshold = 4;
        public const int DefaultSnapDivision = 2;
        public const double DefaultSnapThreshold = 12;

        public static readonly DependencyProperty MoveThresholdProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultMoveThreshold);
        public static readonly DependencyProperty SnapDivisionProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultSnapDivision);
        public static readonly DependencyProperty SnapThresholdProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultSnapThreshold);

        public double MoveThreshold { get; set; }
        public int SnapDivision { get; set; }
        public double SnapThreshold { get; set; }
    }
}
