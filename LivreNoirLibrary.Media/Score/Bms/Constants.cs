using System;
using System.Text;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static class Constants
    {
        public const string DefType_Wav = "WAV";
        public const string DefType_Bmp = "BMP";
        public const string DefType_Bga = "BGA";
        public const string DefType_ExWav = "EXWAV";
        public const string DefType_ExBmp = "EXBMP";
        public const string DefType_AtBga = "@BGA";

        public const string BarTextFormat = "#{0:D3}";

        public const int DefaultBarLength = 1;
        public const ulong BarLengthDenominatorLimit = 10_000_000_000;
        public const int StopResolution = 192;
        public const int MaxBarNumber = 999;
        public static readonly BarPosition MaxBarPosition = new(MaxBarNumber + 1, Rational.Zero);

        public const int MaxKeyLane = 72;
        public const int MetaOffset = 1000;
        public const int MaxLane = MetaOffset + Base_Default * Base_Default - 1;

        public const PlayerType DefaultPlayer = PlayerType.Single;
        public const decimal DefaultBpm = 130;
        public static readonly Rational DefaultBpmRational = (Rational)DefaultBpm;
        public const Rank DefaultRank = Rank.Easy;
        public const double DefaultTotal = 260;
        public const int DefaultLevel = 1;
        public const string DefaultDifficulty = "1";

        public const int Base_Default = 36;
        public const int Base_Legacy = BasedIndex.HexRadix;
        public const int Base_Extended = BasedIndex.MaximumRadix;
        public const int DefMax_Default = Base_Default * Base_Default;
        public const int DefMax_Legacy = Base_Legacy * Base_Legacy;
        public const int DefMax_Extended = Base_Extended * Base_Extended;

        public const LongNoteMode DefaultLnMode = LongNoteMode.Auto;
        public const double DefaultExRank = 100;

        public const string DefaultStageFile = "_stagefile.png";
        public const string DefaultBanner = "_banner.png";
        public const string DefaultBackBmp = "_backbmp.png";
        public const string DefaultPreview = "preview.ogg";

        public static Encoding DefaultEncoding { get; } = Encodings.Get("shift-jis",  EncoderFallback.ExceptionFallback, DecoderFallback.ReplacementFallback);
        public static UTF8Encoding Utf8Encoding { get; } = new(false, true);
    }

    public static class KeyIndexes
    {
        public const int Beat_1P_1 = 1;
        public const int Beat_1P_2 = 2;
        public const int Beat_1P_3 = 3;
        public const int Beat_1P_4 = 4;
        public const int Beat_1P_5 = 5;
        public const int Beat_1P_6 = 8;
        public const int Beat_1P_7 = 9;
        public const int Beat_1P_Scratch = 6;
        public const int Beat_1P_FootPedal = 7;

        public const int Beat_2P_1 = 37;
        public const int Beat_2P_2 = 38;
        public const int Beat_2P_3 = 39;
        public const int Beat_2P_4 = 40;
        public const int Beat_2P_5 = 41;
        public const int Beat_2P_6 = 44;
        public const int Beat_2P_7 = 45;
        public const int Beat_2P_Scratch = 42;
        public const int Beat_2P_FootPedal = 43;

        public const int Pop_1 = 1;
        public const int Pop_2 = 2;
        public const int Pop_3 = 3;
        public const int Pop_4 = 4;
        public const int Pop_5 = 5;
        public const int Pop_6 = 38;
        public const int Pop_7 = 39;
        public const int Pop_8 = 40;
        public const int Pop_9 = 41;

        public const int Pop_1P_1 = Pop_1;
        public const int Pop_1P_2 = Pop_2;
        public const int Pop_1P_3 = Pop_3;
        public const int Pop_1P_4 = Pop_4;
        public const int Pop_1P_5 = Pop_5;
        public const int Pop_1P_6 = 8;
        public const int Pop_1P_7 = 9;
        public const int Pop_1P_8 = 6;
        public const int Pop_1P_9 = 7;
        public const int Pop_2P_1 = 37;
        public const int Pop_2P_2 = 38;
        public const int Pop_2P_3 = 39;
        public const int Pop_2P_4 = 40;
        public const int Pop_2P_5 = 41;
        public const int Pop_2P_6 = 44;
        public const int Pop_2P_7 = 45;
        public const int Pop_2P_8 = 42;
        public const int Pop_2P_9 = 43;
    }

    public static partial class SeparatorComments
    {
        public const string Header = "*---------------------- HEADER FIELD";
        public const string Others = "*---------------------- COMMENT FIELD";
        public const string Def = "*---------------------- DEFINITION FIELD";
        public const string Data = "*---------------------- MAIN DATA FIELD";
        public const string Flows = "*---------------------- FLOW FIELD";

        public static bool Is(string text)
        {
            return Regex.IsMatch(text);
        }

        [GeneratedRegex("\\*-+[^-]+FIELD")]
        private static partial Regex Regex { get; }
    }
}
