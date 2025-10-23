using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.BM3;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Integrated
{
    public partial class BM3Score : MidiData<BM3Track>, IStreamDumpable, IStreamLoadable<BM3Score>
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
            using DeflateStream deflate = new(stream, CompressionMode.Decompress, true);
            using BinaryReader reader = new(deflate);
            LoadContents(reader);
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
            data.ParseRawData(raw);
            return data;
        }

        public void SaveMidi(string path) => General.Save(path, DumpMidi, ExtRegs.Midi, Exts.Mid);

        public void DumpMidi(BinaryWriter writer)
        {
            var raw = this.ComposeRawData(Options.Format, Options.Resolution);
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
    }
}
