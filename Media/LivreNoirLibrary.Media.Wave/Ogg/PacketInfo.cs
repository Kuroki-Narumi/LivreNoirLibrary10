using System;

namespace LivreNoirLibrary.Media.Ogg
{
    public readonly struct PacketInfo(byte[] data, long granulePosition, bool isEOF)
    {
        public readonly byte[] Data = data;
        public readonly long GranulePosition = granulePosition;
        public readonly bool IsEOF = isEOF;

        public void Deconstruct(out byte[] data, out long granulePosition, out bool isEOF)
        {
            data = Data;
            granulePosition = GranulePosition;
            isEOF = IsEOF;
        }
    }
}
