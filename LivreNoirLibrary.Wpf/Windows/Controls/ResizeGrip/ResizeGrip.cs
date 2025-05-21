using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ResizeGrip : Canvas
    {
        static ResizeGrip()
        {
            PropertyUtils.OverrideDefaultStyleKey<ResizeGrip>();
        }

        public const double DefaultMinimumSize = 20;

        public static readonly SolidColorBrush DefaultGripBackground = MediaUtils.GetBrush(255, 255, 255, 255);
        public static readonly SolidColorBrush DefaultGripForeground = MediaUtils.GetBrush(255, 127, 127, 127);

        public event RectChangedEventHandler? RectChanged;
        public event RectChangedEventHandler? ResizeFinished;

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _baseWidth = double.NaN;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _baseHeight = double.NaN;
        [DependencyProperty]
        private Brush? _gripBackground = DefaultGripBackground;
        [DependencyProperty]
        private Brush? _gripForeground = DefaultGripForeground;

        private Window? _root;
        private readonly Dictionary<int, Shape> _grips = [];

        public ResizeGrip()
        {
            CreateGrip_Top();
            CreateGrip_Left();
            CreateGrip_Right();
            CreateGrip_Bottom();
            CreateGrip_UpperLeft();
            CreateGrip_UpperRight();
            CreateGrip_LowerLeft();
            CreateGrip_LowerRight();
            UpdateBrush();
        }
    }
}
