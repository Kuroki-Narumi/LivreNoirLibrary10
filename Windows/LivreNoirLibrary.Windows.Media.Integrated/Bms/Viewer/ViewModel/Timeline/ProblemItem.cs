using System;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public record ProblemItem(BarPosition Position, Note Note, ProblemType Type)
    {
        public string BarText => Position.Bar.GetBarText();
        public string BeatText => Position.RationalOffset.ToString();
        public string LaneText => Note.GetLaneText();
    }
}
