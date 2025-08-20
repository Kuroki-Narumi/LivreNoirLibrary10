using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;
using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed partial class MidiData : MidiData<Track>
    {
        public void SetTrack(int index, Track track)
        {
            InitializeTracks(index + 1);
            _tracks[index] = track;
        }

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
