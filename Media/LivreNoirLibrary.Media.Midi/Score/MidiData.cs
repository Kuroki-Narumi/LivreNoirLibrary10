using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed partial class MidiData : MidiData<Track>, IDumpable, ILoadable<MidiData>, IFile<MidiData>
    {
        public static MidiData Open(string path) => General.Open(path, Load);

        public static MidiData Load(BinaryReader reader)
        {
            var raw = RawData.Load(reader);
            MidiData data = new();
            data.ParseRawData(raw);
            return data;
        }

        public void Save(string path) => Save(path, RawData.DefaultFormat, RawData.DefaultResolution);
        public void Save(string path, int format, int resolution) => General.Save(path, writer => Dump(writer, format, resolution), ExtRegs.Midi, Exts.Mid);

        public void Dump(BinaryWriter writer) => Dump(writer, RawData.DefaultFormat, RawData.DefaultResolution);
        public void Dump(BinaryWriter writer, int format, int resolution)
        {
            var raw = this.ComposeRawData(format, resolution);
            raw.Dump(writer);
        }
    }
}
