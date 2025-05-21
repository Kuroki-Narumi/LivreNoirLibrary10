
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public static class SelectionColors
    {
        public static Color Fill { get; set; } = Color.FromArgb(32, 255, 255, 0); // 選択範囲 中
        public static Color Stroke { get; set; } = Color.FromRgb(255, 255, 0); // 選択範囲 枠
        public static Color Union { get; set; } = Color.FromArgb(32, 255, 0, 0); // 選択範囲(加算)
        public static Color Except { get; set; } = Color.FromArgb(32, 0, 255, 0); // 選択範囲(減算)
        public static Color Intersect { get; set; } = Color.FromArgb(32, 255, 0, 255); // 選択範囲(乗算)
    }
}
