using System;

namespace LivreNoirLibrary.Media.Ogg.Vorbis
{
    public enum PacketType : byte
    {
        Audio = 0,
        Identification = 1,
        Comment = 3,
        Setup = 5,
    }
}
