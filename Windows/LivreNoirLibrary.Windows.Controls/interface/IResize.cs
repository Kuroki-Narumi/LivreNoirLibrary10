using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IResize
    {
        const double DefaultMoveThreshold = 4;
        const int DefaultSnapDivision = 2;
        const double DefaultSnapThreshold = 12;

        static readonly DependencyProperty MoveThresholdProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultMoveThreshold);
        static readonly DependencyProperty SnapDivisionProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultSnapDivision);
        static readonly DependencyProperty SnapThresholdProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultSnapThreshold);

        double MoveThreshold { get; set; }
        int SnapDivision { get; set; }
        double SnapThreshold { get; set; }
    }
}
