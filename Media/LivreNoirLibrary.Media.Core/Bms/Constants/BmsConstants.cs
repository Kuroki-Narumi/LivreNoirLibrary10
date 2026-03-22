using System;
using System.Text;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static class BmsConstants
    {
        public const string BarTextFormat = "#{0:D3}";

        public const double DefaultBarLength = 1;
        public const double StopUnit = 1d / 192d;
        public const int MaxBarNumber = 999;
        public const long MaxInnerResolution = 432000;

        public const int MaxKeyLane = 72;

        public const string DefaultTitle = "(untitled)";
        public const PlayerType DefaultPlayer = PlayerType.Single;
        public const double DefaultBpm = 130;
        public const Rank DefaultRank = Rank.Easy;
        public const double DefaultTotal = 999;
        public const int DefaultPlayLevel = 0;
        public const int DefaultDifficulty = 1;
        public const LongNoteMode DefaultLnMode = LongNoteMode.Auto;
        public const double DefaultExRank = 100;

        public const int Base_Default = 36;
        public const int Base_Legacy = BasedNumber.HexRadix;
        public const int Base_Extended = BasedNumber.MaximumRadix;
        public const int DefMax_Default = Base_Default * Base_Default;
        public const int DefMax_Legacy = Base_Legacy * Base_Legacy;
        public const int DefMax_Extended = Base_Extended * Base_Extended;

        public const string DefaultStageFile = "_stagefile.png";
        public const string DefaultBanner = "_banner.png";
        public const string DefaultBackBmp = "_backbmp.png";
        public const string DefaultPreview = "preview.ogg";

        public const int DefaultCondition = -1;

        public static Encoding DefaultEncoding { get; } = Encodings.Get("shift-jis",  EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
        public static UTF8Encoding Utf8Encoding { get; } = new(false, true);
    }
}
