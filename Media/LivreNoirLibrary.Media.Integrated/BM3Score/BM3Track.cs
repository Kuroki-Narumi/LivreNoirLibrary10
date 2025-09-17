using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Integrated
{
    public class BM3Track : Track, IDumpable, ILoadable<BM3Track>
    {
        public const string DumpHeader = "BM3Trk";

        public TrackOptions Options { get; set; } = new();

        public bool NeedsPack() => Options.ApplyToBms && this.ContainsNote();

        public void Dump(BinaryWriter writer)
        {
            writer.WriteChid(DumpHeader);
            writer.Write((sbyte)Port);
            writer.Write((sbyte)Channel);
            writer.Write(Title ?? string.Empty);
            writer.Write(Options.GetJsonText(false));
            Timeline.Dump(writer);
            foreach (var b in _keySwitch)
            {
                b.Dump(writer);
            }
        }

        public void ProcessLoad(BinaryReader reader)
        {
            reader.CheckChid(DumpHeader);
            this.Clear();
            Port = reader.ReadSByte();
            Channel = reader.ReadSByte();
            Title = reader.ReadString().GetNullIfEmpty();
            var json = reader.ReadString();
            if (Json.TryParse<TrackOptions>(json, out var options))
            {
                Options = options;
            }
            Timeline.ProcessLoad(reader);
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
