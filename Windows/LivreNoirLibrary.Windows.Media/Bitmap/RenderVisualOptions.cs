using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly struct RenderVisualOptions(Brush? background = null, Rect rect = default, int sizeUnit = 1, bool waitForUpdate = true)
    {
        public Brush? Background { get; } = background;
        public Rect Rect { get; } = rect;

        private readonly int _sizeUnitMinusOne = sizeUnit - 1;
        public int SizeUnit => _sizeUnitMinusOne + 1;

        private readonly bool _notWait = !waitForUpdate;
        public bool WaitForUpdate => !_notWait;

        public static RenderVisualOptions Black(Rect sourceRect = default, int sizeUnit = 1, bool waitForUpdate = true)
        {
            return new(Brushes.Black, sourceRect, sizeUnit, waitForUpdate);
        }

        public static RenderVisualOptions White(Rect sourceRect = default, int sizeUnit = 1, bool waitForUpdate = true)
        {
            return new(Brushes.White, sourceRect, sizeUnit, waitForUpdate);
        }
    }
}
