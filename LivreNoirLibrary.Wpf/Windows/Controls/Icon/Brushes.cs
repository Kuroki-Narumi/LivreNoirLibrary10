using System.Windows.Media;
using MBrushes = System.Windows.Media.Brushes;
using static LivreNoirLibrary.Media.MediaUtils;

namespace LivreNoirLibrary.Windows.Controls.IconContent
{
    public static class Brushes
    {
        public static SolidColorBrush Transparent => MBrushes.Transparent;

        public static SolidColorBrush Black => MBrushes.Black;
        public static SolidColorBrush Gray_3 { get; } = GetBrush("#333");
        public static SolidColorBrush Gray_6 { get; } = GetBrush("#666");
        public static SolidColorBrush Gray_8 { get; } = GetBrush("#888");
        public static SolidColorBrush Gray_A { get; } = GetBrush("#aaa");
        public static SolidColorBrush White => MBrushes.White;

        public static SolidColorBrush Red_f0 => MBrushes.Red;
        public static SolidColorBrush Red_f8 { get; } = GetBrush("#f88");
        public static SolidColorBrush Red_f4 { get; } = GetBrush("#f44");
        public static SolidColorBrush Red_e4 { get; } = GetBrush("#e44");
        public static SolidColorBrush Red_d0 { get; } = GetBrush("#d00");
        public static SolidColorBrush Red_82 { get; } = GetBrush("#822");
        public static SolidColorBrush Red_80 { get; } = GetBrush("#800");
        public static SolidColorBrush Red_40 { get; } = GetBrush("#400");

        public static SolidColorBrush RedGreen_fc8 { get; } = GetBrush("#fc8");
        public static SolidColorBrush RedGreen_c62 { get; } = GetBrush("#c62");
        public static SolidColorBrush RedGreen_420 { get; } = GetBrush("#420");
        public static SolidColorBrush RedBlue_840 { get; } = GetBrush("#804");

        public static SolidColorBrush Yellow_f0 { get; } = GetBrush("#ff0");
        public static SolidColorBrush Yellow_Tf0 { get; } = GetBrush("#8ff0");

        public static SolidColorBrush Green_e0 { get; } = GetBrush("#0e0");
        public static SolidColorBrush Green_e4 { get; } = GetBrush("#4e4");
        public static SolidColorBrush Green_72 { get; } = GetBrush("#272");
        public static SolidColorBrush Green_40 { get; } = GetBrush("#040");

        public static SolidColorBrush GreenBlue_fc8 { get; } = GetBrush("#8fc");
        public static SolidColorBrush GreenBlue_fb3 { get; } = GetBrush("#3fb");

        public static SolidColorBrush Blue_fb { get; } = GetBrush("#bbf");
        public static SolidColorBrush Blue_e4 { get; } = GetBrush("#44e");
        public static SolidColorBrush Blue_d0 { get; } = GetBrush("#00d");
        public static SolidColorBrush Blue_a4 { get; } = GetBrush("#44a");
        public static SolidColorBrush Blue_84 { get; } = GetBrush("#448");

        public static SolidColorBrush BlueRed_fc4 { get; } = GetBrush("#c4f");
        public static SolidColorBrush BlueRed_fa6 { get; } = GetBrush("#a6f");

        public static SolidColorBrush BlueGreen_f80 { get; } = GetBrush("#08f");
        public static SolidColorBrush BlueGreen_fc8 { get; } = GetBrush("#8cf");
        public static SolidColorBrush BlueGreen_c84 { get; } = GetBrush("#48c");
        public static SolidColorBrush BlueGreen_b98 { get; } = GetBrush("#89b");
        public static SolidColorBrush BlueRed_840 { get; } = GetBrush("#408");

    }
}
