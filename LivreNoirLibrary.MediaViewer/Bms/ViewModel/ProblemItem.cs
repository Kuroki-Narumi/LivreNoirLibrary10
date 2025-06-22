using System;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public record ProblemItem(BarPosition Position, int Lane, ProblemType Type)
    {
        public string BarText => Position.Bar.GetBarText();
        public string BeatText => Position.Beat.ToString();
        public string LaneText => Lane.GetLaneName();
    }

    public enum ProblemType
    {
        None,
        ZeroPosition,
        Duplicated,
        InvalidMeta,
        AloneLongEnd,
    }
}
