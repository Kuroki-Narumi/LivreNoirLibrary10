using System.Windows.Media;
using static LivreNoirLibrary.Windows.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Media
{
    public static class WaveBrushes
    {
        public static readonly SolidColorBrush Background = GetBrush(WaveColors.Background);

        public static readonly SolidColorBrush Time = GetBrush(WaveColors.Time);
        public static readonly DrawingBrush TimeDashed = CreateVerticalDashBrush(WaveColors.Time, Colors.Transparent, 4, 4);
        public static readonly SolidColorBrush TimeText = GetBrush(WaveColors.TimeText);

        public static readonly SolidColorBrush Gain = GetBrush(WaveColors.Gain);
        public static readonly DrawingBrush GainDashed = CreateHorizontalDashBrush(WaveColors.Gain, Colors.Transparent, 4, 4);
        public static readonly SolidColorBrush GainZero = GetBrush(WaveColors.GainZero);
        public static readonly SolidColorBrush GainText = GetBrush(WaveColors.GainText);

        public static readonly SolidColorBrush Marker = GetBrush(WaveColors.Marker);
        public static readonly SolidColorBrush MarkerText = GetBrush(WaveColors.MarkerText);

        public static readonly SolidColorBrush TextOutline = GetBrush(WaveColors.TextOutline);
        public static readonly Pen TextOutlinePen = new(TextOutline, 2);

        public static readonly SolidColorBrush FreqLine = GetBrush(WaveColors.FreqLine);
        public static readonly DrawingBrush FreqLineDashed = CreateVerticalDashBrush(WaveColors.FreqLine, Colors.Transparent, 4, 4);
        public static readonly SolidColorBrush FreqText = GetBrush(WaveColors.FreqText);
    }
}
