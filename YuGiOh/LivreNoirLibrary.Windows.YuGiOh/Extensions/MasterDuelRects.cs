using LivreNoirLibrary.Win32Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class MasterDuelRects
    {
        public const int WindowWidth = 1920;
        public const int WindowHeight = 1080;

        public static readonly Int32Rect CardImage = new(132, 166, 447, 652);
        public static readonly Int32Rect Hand = new(0, 880, 1920, 200);
        public static readonly Int32Rect OpponentHand = new(480, 780, 960, 200);
        public static readonly Int32Rect DeckCardList = new(492, 108, 788, 896);

        public static readonly Rect Ratio_CardImage = GetRatio(CardImage);
        public static readonly Rect Ratio_Hand = GetRatio(Hand);
        public static readonly Rect Ratio_OpponentHand = GetRatio(OpponentHand);
        public static readonly Rect Ratio_DeckCardList = GetRatio(DeckCardList);

        public static Rect GetRatio(Int32Rect r) => new((double)r.X / WindowWidth, (double)r.Y / WindowHeight, (double)r.Width / WindowWidth, (double)r.Height / WindowHeight);
        public static Int32Rect ApplyRatio(int width, int height, Rect ratio)
            => new((int)Math.Floor(width * ratio.X), (int)Math.Floor(height * ratio.Y), (int)Math.Ceiling(width * ratio.Width), (int)Math.Ceiling(height * ratio.Height));

        public static bool IsMasterDuelApp(WindowInfo info) => info.ExeFileName.Equals("masterduel.exe", StringComparison.OrdinalIgnoreCase);


        public const string CardMaskPath = "/LivreNoirLibrary.Windows.YuGiOh;component/Resources/mask_card.png";

        public static BitmapImage CardImageMask { get; } = new(new Uri(CardMaskPath, UriKind.Relative));
    }
}
