using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class NoteOn(int channel, int nn, int velocity) : NoteEvent(channel, nn, velocity)
    {
        public override StatusType Type => StatusType.NoteOn;

        public static NoteOn Load(int channel, BinaryReader reader)
        {
            var (nn, vel) = LoadContents(reader);
            return new(channel, nn, vel);
        }

        public override string ToString() => $"{nameof(NoteOn)}{{{nameof(Channel)}={_channel}, {nameof(Number)}={_nn}, {nameof(Velocity)}={_vel}}}";
    }
}
