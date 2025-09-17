using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media.Bms
{
    public static class Tags
    {
        public const string Wav = "#WAV";
        public const string Bmp = "#BMP";
        public const string Bga = "#BGA";
        public const string Bpm = "#BPM";
        public const string ExBpm = "#EXBPM";
        public const string Stop = "#STOP";
        public const string Text = "#TEXT";
        public const string ExWav = "#EXWAV";
        public const string ExBmp = "#EXBMP";
        public const string AtBga = "#@BGA";
        public const string Argb = "#ARGB";
        public const string SwBga = "#SWBGA";
        public const string ExRank = "#EXRANK";
        public const string ChangeOption = "#CHANGEOPTION";
        public const string Scroll = "#SCROLL";
        public const string Speed = "#SPEED";

        public static string ToTag(this DefType type) => _defType2Tag.TryGetValue(type, out var value) ? value : type.ToString().ToUpper();
        private static readonly Dictionary<DefType, string> _defType2Tag = new()
        {
            [DefType.Wav] = Wav,
            [DefType.Bmp] = Bmp,
            [DefType.Bga] = Bga,
            [DefType.Bpm] = Bpm,
            [DefType.Stop] = Stop,
            [DefType.Text] = Text,
            [DefType.ExWav] = ExWav,
            [DefType.ExBmp] = ExBmp,
            [DefType.AtBga] = AtBga,
            [DefType.Argb] = Argb,
            [DefType.SwBga] = SwBga,
            [DefType.ExRank] = ExRank,
            [DefType.ChangeOption] = ChangeOption,
            [DefType.Scroll] = Scroll,
            [DefType.Speed] = Speed,
        };

        public const string Random = "#RANDOM";
        public const string SetRandom = "#SETRANDOM";
        public const string If = "#IF";
        public const string ElseIf = "#ELSEIF";
        public const string Else = "#ELSE";
        public const string EndIf = "#ENDIF";
        public const string EndRandom = "#ENDRANDOM";
        public const string Switch = "#SWITCH";
        public const string SetSwitch = "#SETSWITCH";
        public const string Case = "#CASE";
        public const string Default = "#DEF";
        public const string Skip = "#SKIP";
        public const string EndSwitch = "#ENDSW";
    }
}
