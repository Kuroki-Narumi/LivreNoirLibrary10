using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Media.Midi.RawData;

namespace LivreNoirLibrary.Media.BM3
{
    using PackedTrackList = IDictionary<int, PackedTrack>;

    public partial class BM3Score : MidiData<BM3Track> , IStreamDumpable<BM3Score>
    {
        public const string DumpHeader = "BM3Scr";

        public ScoreOptions Options { get; set; } = new();

        public static BM3Score Open(string path) => General.Open<BM3Score>(path);

        public static BM3Score Load(Stream stream)
        {
            BM3Score data = new();
            data.ProcessLoad(stream);
            return data;
        }

        public void ProcessLoad(Stream stream)
        {
            stream.CheckChid(DumpHeader);
            try
            {
                using DeflateStream deflate = new(stream, CompressionMode.Decompress, true);
                using BinaryReader reader = new(deflate);
                LoadContents(reader);
            }
            catch
            {
                using DeflateStream deflate = new(stream, CompressionMode.Decompress, true);
                using BinaryReader reader = new(deflate);
                LoadContents_Legacy(reader);
            }
        }

        private void LoadContents(BinaryReader reader)
        {
            Copyright = reader.ReadString();
            var json = reader.ReadString();
            if (Json.TryParse<ScoreOptions>(json, out var options))
            {
                Options = options;
            }
            _signatures.ProcessLoad(reader);
            var count = reader.ReadInt32();
            if (count < _tracks.Count)
            {
                _tracks.RemoveRange(count, _tracks.Count - count);
            }
            else
            {
                InitializeTracks(count);
            }
            for (var i = 0; i < count; i++)
            {
                _tracks[i].ProcessLoad(reader);
            }
        }

        private void LoadContents_Legacy(BinaryReader reader)
        {
            Copyright = reader.ReadString();
            var json = reader.ReadString();
            if (Json.TryParse<ScoreOptions>(json, out var options))
            {
                Options = options;
            }
            TempoTimeline tempo = [];
            TonalityTimeline tonality = [];
            tempo.ProcessLoad(reader);
            _signatures.ProcessLoad(reader);
            tonality.ProcessLoad(reader);
            var count = reader.ReadInt32();
            if (count < _tracks.Count)
            {
                _tracks.RemoveRange(count, _tracks.Count - count);
            }
            else
            {
                InitializeTracks(count);
            }
            for (int i = 0; i < count; i++)
            {
                _tracks[i].ProcessLoad(reader);
            }
            var conductor = _tracks[0].Timeline;
            foreach (var (pos, value) in tempo)
            {
                conductor.Add(pos, new TempoEvent(value));
            }
            foreach (var (pos, value) in tonality)
            {
                conductor.Add(pos, new TonalityEvent(value));
            }
        }

        public void Save(string path) => General.Save(path, this, ExtRegs.BM3Score, Exts.BM3Score);

        public void Dump(Stream stream)
        {
            stream.WriteChid(DumpHeader);
            using DeflateStream deflate = new(stream, CompressionMode.Compress, true);
            using BinaryWriter writer = new(deflate, Encoding.UTF8, true);
            DumpContents(writer);
        }

        private void DumpContents(BinaryWriter writer)
        {
            writer.Write(Copyright ?? string.Empty);
            writer.Write(Options.GetJsonText(false));
            _signatures.Dump(writer);
            writer.Write(_tracks.Count);
            foreach (var track in CollectionsMarshal.AsSpan(_tracks))
            {
                track.Dump(writer);
            }
        }

        public static BM3Score OpenMidi(string path) => General.Open(path, LoadMidi);

        public static BM3Score LoadMidi(BinaryReader reader)
        {
            var raw = RawData.Load(reader);
            BM3Score data = new();
            data.Options.Resolution = raw.Resolution;
            raw.ParseTo(data);
            return data;
        }

        public void SaveMidi(string path) => General.Save(path, DumpMidi, ExtRegs.Midi, Exts.Mid);

        public void DumpMidi(BinaryWriter writer)
        {
            RawData raw = new(Options.Resolution, Options.Format);
            raw.ComposeFrom(this);
            raw.Dump(writer);
        }

        public bool MoveTrackUp(int index)
        {
            if (index is > 1 && _tracks.MoveUp(index))
            {
                foreach (var track in CollectionsMarshal.AsSpan(_tracks))
                {
                    track.Options.SwapSideChain(index, index - 1);
                }
                return true;
            }
            return false;
        }

        public bool MoveTrackDown(int index)
        {
            if (index is >= 1 && _tracks.MoveDown(index))
            {
                foreach (var track in CollectionsMarshal.AsSpan(_tracks))
                {
                    track.Options.SwapSideChain(index, index + 1);
                }
                return true;
            }
            return false;
        }

        public bool DeleteTrack(int index)
        {
            if (index is >= 1 && index < _tracks.Count)
            {
                _tracks.RemoveAt(index);
                foreach (var track in CollectionsMarshal.AsSpan(_tracks))
                {
                    track.Options.RemoveSideChain(index);
                }
                return true;
            }
            return false;
        }

        public PackOptions GetPackOptions(int trackId)
        {
            PackOptions? options = null;
            if (TryGetTrack(trackId, out var track))
            {
                options = track.Options.PackOptions;
            }
            return options ?? Options.DefaultPackOptions;
        }

        public SliceOptions GetSliceOptions(int trackId)
        {
            SliceOptions? options = null;
            if (TryGetTrack(trackId, out var track))
            {
                options = track.Options.SliceOptions;
            }
            return options ?? Options.DefaultSliceOptions;
        }

        public PackedTrack PackTrack(int trackId, SysExPrefixCollection sysExPrefixes) => new(this, trackId, GetPackOptions(trackId), sysExPrefixes);

        public Dictionary<int, PackedTrack> PackAllTracks(SysExPrefixCollection sysExPrefixes)
        {
            Dictionary<int, PackedTrack> result = [];
            foreach (var (id, track) in EachTrack())
            {
                if (track.NeedsPack())
                {
                    var packed = PackTrack(id, sysExPrefixes);
                    result.Add(id, packed);
                }
            }
            return result;
        }

        public (MidiData Data, string Filename) CreatePackedMidi(PackedTrackList packed, string baseFilename, int trackId) => CreatePackedMidi(packed[trackId], baseFilename, trackId);
        public (MidiData Data, string Filename) CreatePackedMidi(PackedTrack packed, string baseFilename, int trackId)
        {
            return packed.CreateMidiData(this, baseFilename, trackId, GetPackOptions(trackId));
        }

        public (BmsData Data, int MaxDefIndex) CreateBms(string originalFilename, PackedTrackList packedList)
        {
            string GetBasename(string format) => PackUtils.Format(format, originalFilename, this, 0);

            var options = Options.BmsConvertOptions;
            var data = BmsData.Create();
            data.Genre = GetBasename(options.Genre);
            data.Title = GetBasename(options.Title);
            data.Artist = GetBasename(options.Artist);
            data.ExtendConductor(this, GetLastPosition());
            if (!Options.SetupBar)
            {
                data.InsertBar(0, 1);
            }
            var lane = options.LaneStart;
            var defId = options.DefStart;
            defId = CreateBmsCore(data, originalFilename, lane, defId, packedList);
            return (data, defId);
        }

        public int AppendToBms(BmsData target, bool removeDef, string originalFilename, PackedTrackList packedList)
        {
            var defStart = Options.BmsConvertOptions.DefStart;
            if (removeDef)
            {
                foreach (var (id, _) in packedList)
                {
                    if (_tracks[id].NeedsPack())
                    {
                        var basename = PackUtils.Format(GetSliceOptions(id).BasenameWithDefault, originalFilename, this, id);
                        target.RemoveDefWithBasename(DefType.Wav, basename);
                    }
                }
                target.DefSort(new() { Headroom = defStart });
                target.RemoveUnusedBgmLane();
            }
            var lane = target.GetMaxBgmLane() + 1;
            var defId = target.DefLists.FindFreeIndex(DefType.Wav, defStart);
            defId = CreateBmsCore(target, originalFilename, lane, defId, packedList);
            return defId;
        }

        private int CreateBmsCore(BmsData target, string originalFilename, int lane, int defId, PackedTrackList packedList)
        {
            var options = Options.BmsConvertOptions;
            var defInterval = options.DefInterval;
            var oneOrigin = !Options.SetupBar;
            foreach (var (id, packed) in packedList)
            {
                if (_tracks[id].NeedsPack())
                {
                    var basename = PackUtils.Format(GetSliceOptions(id).BasenameWithDefault, originalFilename, this, id);
                    (defId, lane) = packed.CreateBmsData(target, basename, defId, lane, oneOrigin);
                    defId += defInterval;
                }
            }
            defId -= defInterval;
            if (defId >= Constants.DefMax_Default)
            {
                target.Base = Constants.Base_Extended;
            }
            return defId;
        }
    }
}
