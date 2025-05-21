using System;

namespace LivreNoirLibrary.Media.Ogg
{
    public enum OVResult
    {
        Finished = 0,
        Continue = 1,

        FALSE = -1,
        EOF = -2,
        HOLE = -3,

        EREAD = -128,
        EFAULT = -129,
        EIMPL = -130,
        EINVAL = -131,
        ENOTVORBIS = -132,
        EBADHEADER = -133,
        EVERSION = -134,
        ENOTAUDIO = -135,
        EBADPACKET = -136,
        EBADLINK = -137,
        ENOSEEK = -138,
    }
}
