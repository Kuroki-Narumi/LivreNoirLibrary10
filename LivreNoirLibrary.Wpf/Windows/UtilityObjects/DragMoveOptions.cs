using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public readonly struct DragMoveOptions(
        double moveThreshold = DragMoveOptions.DefaultMoveThreshold,
        double snapThreshold = DragMoveOptions.DefaultSnapThreshold,
        RectChangedEventHandler? changing = null,
        RectChangedEventHandler? finished = null
        )
    {
        public const double DefaultMoveThreshold = 10;
        public const double DefaultSnapThreshold = 16;

        private readonly double _moveThreshold = moveThreshold - DefaultMoveThreshold;
        private readonly double _snapThreshold = snapThreshold - DefaultSnapThreshold;

        public readonly double MoveThreshold => _moveThreshold + DefaultMoveThreshold;
        public readonly double SnapThreshold => _snapThreshold + DefaultSnapThreshold;
        public readonly RectChangedEventHandler? Changing = changing;
        public readonly RectChangedEventHandler? Finished = finished;

        public void Deconstruct(out double moveThreshold, out double snapThreshold, out RectChangedEventHandler? changing, out RectChangedEventHandler? finished)
        {
            moveThreshold = MoveThreshold;
            snapThreshold = SnapThreshold;
            changing = Changing;
            finished = Finished;
        }
    }
}
