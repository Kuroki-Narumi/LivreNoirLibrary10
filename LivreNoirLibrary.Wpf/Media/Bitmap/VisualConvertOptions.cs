using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public class VisualConvertOptions(Brush? background = null, Rect rect = default, int sizeUnit = 1, bool wait = true)
    {
        public static VisualConvertOptions Default { get; } = new();

        public Brush? Background { get; init; } = background;
        public Rect Rect { get; init; } = rect;
        public int SizeUnit { get; init; } = sizeUnit;
        public bool WaitForUpdate { get; init; } = wait;

        public static VisualConvertOptions Black(Rect sourceRect = default, int sizeUnit = 1, bool wait = true)
        {
            return new(Brushes.Black, sourceRect, sizeUnit, wait);
        }

        public static VisualConvertOptions White(Rect sourceRect = default, int sizeUnit = 1, bool wait = true)
        {
            return new(Brushes.White, sourceRect, sizeUnit, wait);
        }
    }
}
