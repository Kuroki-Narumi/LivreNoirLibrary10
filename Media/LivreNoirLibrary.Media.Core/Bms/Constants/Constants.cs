using System;
using System.Text;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static class Constants
    {
        public const string Chid_Root = "LNBmsR";
        public const string Chid_FlowContainer = "LNBFlC";
        public const string Chid_FlowBranch = "LNBFlB";

        public const string BarTextFormat = "#{0:D3}";

        public const int DefaultBarLength = 1;
        public static readonly Rational DefaultBarLengthR = Rational.One;
        public const int StopResolution = 192;
        public const int MaxBarNumber = 999;
        public static readonly BarPosition MaxBarPosition = new(MaxBarNumber + 1, Rational.Zero);

        public const int MaxKeyLane = 72;
        public const int MetaOffset = 1000;
        public const int MaxLane = MetaOffset + Base_Default * Base_Default - 1;

        public const string DefaultTitle = "(untitled)";
        public const PlayerType DefaultPlayer = PlayerType.Single;
        public const double DefaultBpm = 130;
        public static readonly Rational DefaultBpmRational = new((long)DefaultBpm);
        public const Rank DefaultRank = Rank.Easy;
        public const double DefaultTotal = 999;
        public const string DefaultPlayLevel = "0";
        public const string DefaultDifficulty = "1";

        public const int Base_Default = 36;
        public const int Base_Legacy = BasedNumber.HexRadix;
        public const int Base_Extended = BasedNumber.MaximumRadix;
        public const int DefMax_Default = Base_Default * Base_Default;
        public const int DefMax_Legacy = Base_Legacy * Base_Legacy;
        public const int DefMax_Extended = Base_Extended * Base_Extended;

        public const LongNoteMode DefaultLnMode = LongNoteMode.Auto;
        public const double DefaultExRank = 100;

        public const string DefaultStageFile = "_stagefile.png";
        public const string DefaultBanner = "_banner.png";
        public const string DefaultBackBmp = "_backbmp.png";
        public const string DefaultPreview = "preview.ogg";

        public const int DefaultCondition = -1;

        public static Encoding DefaultEncoding { get; } = Encodings.Get("shift-jis",  EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
        public static UTF8Encoding Utf8Encoding { get; } = new(false, true);
    }
}
