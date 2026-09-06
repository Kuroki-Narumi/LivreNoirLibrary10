using System;

namespace LivreNoirLibrary.Media.Capture
{
    [Flags]
    public enum WindowSearchMode
    {
        None,
        Title = 1,
        File = 2,
        Complete = 4,

        TitleOrFile = Title | File,
        TitleAndFile = Title | File | Complete,
    }
}
