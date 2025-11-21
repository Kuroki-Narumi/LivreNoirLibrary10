using System;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public record ProblemItem(BarPosition Position, Note Note, ProblemType Type)
    {
        public string BarText => Position.Bar.GetBarText();
        public string BeatText => Position.Offset.ToString();
        public string LaneText => Note.GetLaneText();
    }
}
