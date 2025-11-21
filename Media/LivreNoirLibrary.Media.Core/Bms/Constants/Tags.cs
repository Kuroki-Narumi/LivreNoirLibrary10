using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public static class Tags
    {
        public const string Player = "#PLAYER";
        public const string Genre = "#GENRE";
        public const string Title = "#TITLE";
        public const string SubTitle = "#SUBTITLE";
        public const string Artist = "#ARTIST";
        public const string SubArtist = "#SUBARTIST";
        public const string Bpm = "#BPM";
        public const string PlayLevel = "#PLAYLEVEL";
        public const string Difficulty = "#DIFFICULTY";
        public const string Rank = "#RANK";
        public const string Total = "#TOTAL";
        public const string StageFile = "#STAGEFILE";
        public const string Banner = "#BANNER";
        public const string BackBmp = "#BACKBMP";
        public const string Preview = "#PREVIEW";
        public const string LnObj = "#LNOBJ";
        public const string LnMode = "#LNMODE";
        public const string DefExRank = "#DEFEXRANK";
        public const string Comment = "#COMMENT";
        public const string VolWav = "#VOLWAV";
        public const string Base = "#BASE";

        private static readonly Dictionary<HeaderType, string> _header2string = new()
        {
            [HeaderType.Player] = Player,
            [HeaderType.Genre] = Genre,
            [HeaderType.Title] = Title,
            [HeaderType.SubTitle] = SubTitle,
            [HeaderType.Artist] = Artist,
            [HeaderType.SubArtist] = SubArtist,
            [HeaderType.Bpm] = Bpm,
            [HeaderType.PlayLevel] = PlayLevel,
            [HeaderType.Difficulty] = Difficulty,
            [HeaderType.Rank] = Rank,
            [HeaderType.StageFile] = StageFile,
            [HeaderType.Banner] = Banner,
            [HeaderType.BackBmp] = BackBmp,
            [HeaderType.Preview] = Preview,
            [HeaderType.LnMode] = LnMode,
            [HeaderType.DefExRank] = DefExRank,
            [HeaderType.Comment] = Comment,
            [HeaderType.VolWav] = VolWav,
        };

        public static string ToString(HeaderType type)
        {
            if (!_header2string.TryGetValue(type, out var value))
            {
                value = type.ToString().ToUpperInvariant();
                _header2string[type] = value;
            }
            return value;
        }

        public const string Wav = "#WAV";
        public const string Bmp = "#BMP";
        public const string Bga = "#BGA";
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
