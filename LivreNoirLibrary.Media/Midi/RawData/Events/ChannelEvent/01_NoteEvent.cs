using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public abstract class NoteEvent(int channel, int number, int velocity) : ChannelEvent(channel)
    {
        protected byte _nn = GetMax127(number);
        protected byte _vel = GetMax127(velocity);

        public int Number { get => _nn; set => _nn = GetMax127(value); }
        public int Velocity { get => _vel; set => _vel = GetMax127(value); }

        public static (int Number, int Velocity) LoadContents(BinaryReader reader) => (reader.ReadByte(), reader.ReadByte());

        protected sealed override void DumpContents(BinaryWriter writer)
        {
            writer.Write(_nn);
            writer.Write(_vel);
        }
    }
}
