using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly struct RenderVisualOptions(Brush? background = null, Rect rect = default, Size scale = default, int sizeUnit = 1, bool waitForUpdate = true)
    {
        public Brush? Background { get; } = background;
        public Rect Rect { get; } = rect;

        private readonly int _sizeUnitMinusOne = sizeUnit - 1;
        private readonly bool _notWait = !waitForUpdate;

        public Size Scale { get; } = scale;
        public int SizeUnit => _sizeUnitMinusOne + 1;
        public bool WaitForUpdate => !_notWait;
    }
}
