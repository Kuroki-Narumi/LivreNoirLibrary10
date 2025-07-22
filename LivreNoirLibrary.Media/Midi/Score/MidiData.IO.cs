using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class MidiData
    {
        public static MidiData Open(string path) => General.Open(path, Load);

        public static MidiData Load(BinaryReader reader)
        {
            var raw = RawData.RawData.Load(reader);
            MidiData data = new();
            raw.ParseTo(data);
            return data;
        }

        public void Save(string path, int format = RawData.RawData.DefaultFormat, int resolution = RawData.RawData.DefaultResolution)
        {
            General.Save(path, writer => Dump(writer, format, resolution), ExtRegs.Midi, Exts.Mid);
        }

        public void Dump(BinaryWriter stream, int format, int resolution)
        {
            RawData.RawData raw = new(resolution, format);
            raw.ComposeFrom(this);
            raw.Dump(stream);
        }
    }
}
