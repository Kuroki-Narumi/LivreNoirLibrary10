using System;

namespace LivreNoirLibrary.Windows
{
    public readonly struct DragMoveOptions(
        int moveThreshold = DragMoveOptions.DefaultMoveThreshold,
        int snapThreshold = DragMoveOptions.DefaultSnapThreshold,
        RectChangedEventHandler? changing = null,
        RectChangedEventHandler? finished = null
        )
    {
        public const int DefaultMoveThreshold = 10;
        public const int DefaultSnapThreshold = 16;

        // default(DragMoveOptions)でデフォルト値を適用するため、内部的にはデフォルト値を引いた値で保持する
        private readonly int _moveThreshold = moveThreshold - DefaultMoveThreshold;
        private readonly int _snapThreshold = snapThreshold - DefaultSnapThreshold;

        public readonly int MoveThreshold => _moveThreshold + DefaultMoveThreshold;
        public readonly int SnapThreshold => _snapThreshold + DefaultSnapThreshold;
        public readonly RectChangedEventHandler? Changing = changing;
        public readonly RectChangedEventHandler? Finished = finished;

        public void Deconstruct(out int moveThreshold, out int snapThreshold, out RectChangedEventHandler? changing, out RectChangedEventHandler? finished)
        {
            moveThreshold = MoveThreshold;
            snapThreshold = SnapThreshold;
            changing = Changing;
            finished = Finished;
        }
    }
}
