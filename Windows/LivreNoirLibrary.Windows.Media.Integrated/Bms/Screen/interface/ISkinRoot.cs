using System;
using System.Drawing;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface ISkinRoot
    {
        Size BaseSize { get; }
        double FadeInTime { get; }
        double FadeOutTime { get; }
    }

    public interface IPlaySkinRoot : ISkinRoot
    {
        double LoadTime { get; }
        double ReadyTime { get; }
        double MarginTime { get; }
    }
}
