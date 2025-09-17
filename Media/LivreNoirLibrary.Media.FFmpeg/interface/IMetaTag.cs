using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe interface IMetaTag
    {
        internal AVDictionary** GetDictPointer();
    }
}
