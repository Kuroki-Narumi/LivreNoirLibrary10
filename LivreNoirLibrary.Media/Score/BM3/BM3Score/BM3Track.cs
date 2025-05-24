using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.BM3
{
    public partial class BM3Track : Track, IDumpable<BM3Track>
    {
        public const string DumpHeader = "BM3Trk";

        public TrackOptions Options { get; set; } = new();

        public bool NeedsPack() => ContainsNote() && Options.ApplyToBms;

        public void Dump(BinaryWriter writer)
        {
            writer.WriteChid(DumpHeader);
            writer.Write((sbyte)_port);
            writer.Write((sbyte)_channel);
            writer.Write(_title ?? string.Empty);
            writer.Write(Options.GetJsonText(false));
            _timeline.Dump(writer);
            foreach (var b in _keySwitch)
            {
                b.Dump(writer);
            }
        }

        public void ProcessLoad(BinaryReader reader)
        {
            reader.CheckChid(DumpHeader);
            Clear();
            _port = reader.ReadSByte();
            _channel = reader.ReadSByte();
            _title = reader.ReadString().GetNullIfEmpty();
            var json = reader.ReadString();
            if (Json.TryParse<TrackOptions>(json, out var options))
            {
                Options = options;
            }
            _timeline.ProcessLoad(reader);
            for (var i = 0; i < 128; i++)
            {
                _keySwitch[i] = KeySwitchOption.Load(reader);
            }
        }

        public static BM3Track Load(BinaryReader reader)
        {
            BM3Track track = new();
            track.ProcessLoad(reader);
            return track;
        }
    }
}
