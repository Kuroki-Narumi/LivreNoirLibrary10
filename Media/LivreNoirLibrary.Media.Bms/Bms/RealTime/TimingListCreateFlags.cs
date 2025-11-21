using System;

namespace LivreNoirLibrary.Media.Bms
{
    [Flags]
    public enum TimingListCreateFlags
    {
        None = 0,
        AutoPlay = 1,
        TimeCounter = 2,
        Bgm = 4,
        Key = 8,
        Bga = 16,
        Meta = 32,

        Play = TimeCounter | Bgm | Key | Bga | Meta,
    }
}
