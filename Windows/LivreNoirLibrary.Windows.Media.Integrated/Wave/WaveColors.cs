using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public static class WaveColors
    {
        public static Color Background { get; } = Color.FromArgb(255, 0, 0, 0);

        public static Color Time { get; } = Color.FromArgb(128, 192, 192, 255); // 時間スケール
        public static Color TimeText { get; } = Color.FromArgb(255, 255, 255, 255); // 時間スケール 文字色

        public static Color Gain { get; } = Color.FromArgb(128, 192, 64, 255); // レベルライン
        public static Color GainZero { get; } = Color.FromArgb(160, 0, 128, 255); // 0dB ライン
        public static Color GainText { get; } = Color.FromArgb(192, 255, 255, 255); // レベル 文字色

        public static Color Marker { get; } = Color.FromArgb(128, 0, 224, 0);  // マーカー
        public static Color MarkerText { get; } = Color.FromArgb(224, 0, 224, 0);  // マーカー 文字色

        public static Color TextOutline { get; } = Color.FromArgb(127, 0, 0, 0); // 文字 縁取り

        public static Color FreqLine { get; } = Color.FromArgb(128, 128, 255, 0); // 周波数ライン
        public static Color FreqText { get; } = Color.FromArgb(192, 255, 255, 255); // 周波数 文字色
    }
}
