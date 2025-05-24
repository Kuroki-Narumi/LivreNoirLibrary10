using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public partial class RawData(int resolution = RawData.DefaultResolution, int format = RawData.DefaultFormat) : IJsonWriter, IDumpable<RawData>
    {
        public const string DataHeader = "MThd";
        public const string TrackHeader = "MTrk";

        public const ushort DefaultFormat = 1;
        public const short DefaultResolution = 240;

        private readonly List<RawTimeline> _tracks = [];
        private ushort _format = (ushort)format;
        private short _resolution = (short)resolution;

        public int Format { get => _format; set => _format = value is 0 ? (ushort)0 : (ushort)1; }

        public int Resolution
        {
            get => _resolution;
            set
            {
                var v = (short)value;
                var den = _resolution;
                if (den != v)
                {
                    foreach (var track in CollectionsMarshal.AsSpan(_tracks))
                    {
                        track.ChangeResolution(v, den);
                    }
                    _resolution = v;
                }
            }
        }

        public short TrackCount => _format is 0 ? (short)1 : (short)_tracks.Count;

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("format", _format);
            writer.WriteNumber("resolution", _resolution);
            writer.WriteNumber("track_count", _tracks.Count);
            writer.WritePropertyName("tracks");
            writer.WriteStartArray();
            foreach (var track in CollectionsMarshal.AsSpan(_tracks))
            {
                writer.WriteStartArray();
                foreach (var (pos, ev) in track)
                {
                    writer.WriteStartObject();
                    writer.WriteNumber("tick", pos);
                    writer.WritePropertyName("content");
                    ev.WriteJson(writer, options);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public static RawData Load(BinaryReader reader)
        {
            RawData data = new();
            data.ProcessLoad(reader);
            return data;
        }

        private void ProcessLoad(BinaryReader reader)
        {
            var trackCount = ReadHeader(reader);
            if (_format is 0)
            {
                for (var i = 0; i <= 16; i++)
                {
                    _tracks.Add([]);
                }
            }
            var stream = reader.BaseStream;
            for (var i = 0; i < trackCount && (stream.Position != stream.Length);)
            {
                if (FourLetterHeader.Check(reader, TrackHeader))
                {
                    ReadTrack(reader);
                    i++;
                }
            }
        }

        private int ReadHeader(BinaryReader reader)
        {
            FourLetterHeader.CheckAndThrow(reader, DataHeader);
            var length = reader.ReadUInt32BigEndian();
            if (length is not 6)
            {
                throw new InvalidOperationException($"Invalid header size ({length}, expected 6)");
            }
            var format = reader.ReadUInt16BigEndian();
            var trackCount = reader.ReadUInt16BigEndian();
            var resol = reader.ReadInt16BigEndian();
            _format = format;
            _resolution = resol;
            return trackCount;
        }

        private void ReadTrack(BinaryReader reader)
        {
            var length = reader.ReadUInt32BigEndian();
            var tick = 0L;
            var runningStatus = StatusType.None;
            var stream = reader.BaseStream;
            var end = stream.Position + length;
            RawTimeline track;
            if (_format is 0)
            {
                track = _tracks[0];
            }
            else
            {
                track = [];
                _tracks.Add(track);
            }
            ControlChange.InitSize();
            while (stream.Position < end)
            {
                tick += reader.Read7BitEncodedIntBigEndian();
                var statusByte = reader.ReadByte();
                if (statusByte is >= 0x80)
                {
                    runningStatus = (StatusType)statusByte;
                }
                else
                {
                    stream.Position -= 1;
                }
                var ev = Event.Load(runningStatus, reader);
                if (ev is TrackEnd)
                {
                    stream.Position = end;
                    break;
                }
                else
                {
                    if (_format is 0 && ev is ChannelEvent c)
                    {
                        _tracks[c.Channel + 1].Add(tick, ev);
                    }
                    else
                    {
                        track.Add(tick, ev);
                    }
                }
            }
        }

        public void Dump(BinaryWriter writer)
        {
            FourLetterHeader.Write(writer, DataHeader);
            writer.WriteBigEndian((uint)6);
            writer.WriteBigEndian(_format);
            writer.WriteBigEndian(TrackCount);
            writer.WriteBigEndian(_resolution);
            if (_format is 0)
            {
                RawTimeline timeline = [];
                foreach (var track in CollectionsMarshal.AsSpan(_tracks))
                {
                    track.CopyTo(timeline);
                }
                WriteTrack(timeline, writer);
            }
            else
            {
                foreach (var track in CollectionsMarshal.AsSpan(_tracks))
                {
                    WriteTrack(track, writer);
                }
            }
        }

        private static void WriteTrack(RawTimeline track, BinaryWriter writer)
        {
            var stream = writer.BaseStream;
            FourLetterHeader.Write(writer, TrackHeader);
            writer.ProcessWrite(() =>
            {
                var lastTick = 0L;
                foreach (var (tick, ev) in track)
                {
                    writer.Write7BitEncodedIntBigEndian((int)(tick - lastTick));
                    ev.Dump(writer);
                    lastTick = tick;
                }
                writer.Write((byte)0);
                TrackEnd.Instance.Dump(writer);
            }, p => writer.WriteBigEndian((uint)p));
        }
    }
}
