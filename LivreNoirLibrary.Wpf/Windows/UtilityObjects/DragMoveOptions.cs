using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public class DragMoveOptions
    {
        public const double DefaultMoveThreshold = 10;
        public const double DefaultSnapThreshold = 16;

        public double MoveThreshold { get; set; } = DefaultMoveThreshold;
        public double SnapThreshold { get; set; } = DefaultSnapThreshold;

        public RectChangedEventHandler? Changing { get; set; }
        public RectChangedEventHandler? Finished { get; set; }
    }
}
