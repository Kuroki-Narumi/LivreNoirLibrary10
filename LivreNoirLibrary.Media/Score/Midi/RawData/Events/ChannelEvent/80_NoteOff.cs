using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class NoteOff(int channel, int nn) : NoteEvent(channel, nn, 0)
    {
        public override StatusType Type => StatusType.NoteOff;

        public static NoteOff Load(int channel, BinaryReader reader)
        {
            var (nn, _) = LoadContents(reader);
            return new(channel, nn);
        }

        public override string ToString() => $"{nameof(NoteOff)}{{{nameof(Channel)}={_channel}, {nameof(Number)}={_nn}, {nameof(Velocity)}={_vel}}}";
    }
}
