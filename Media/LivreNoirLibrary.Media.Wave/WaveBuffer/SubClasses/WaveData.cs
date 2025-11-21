using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class WaveData : WaveBuffer, IWaveMetaData, IFile<WaveData>, IStreamLoadable<WaveData>, IStreamDumpable
    {
        private SampleFormat _sample_format = WaveEncodeOptions.DefaultFormat;
        private FormatChunk _format;
        private readonly List<RiffChunk> _chunks = [];

        public SampleFormat SampleFormat
        {
            get => _sample_format;
            set
            {
                if (_sample_format != value)
                {
                    if (!value.IsValid())
                    {
                        throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not supported format.");
                    }
                    _sample_format = value;
                    _format = FormatChunk.Create(SampleRate, Channels, value);
                }
            }
        }

        public FormatChunk Format => _format;

        public List<RiffChunk> Chunks => _chunks;

        public WaveData(int sampleRate = DefaultSampleRate, int channels = DefaultChannels, SampleFormat format = WaveEncodeOptions.DefaultFormat)
        {
            _sample_format = format.IsValid() ? format : WaveEncodeOptions.DefaultFormat;
            SetLayout(sampleRate, channels);
        }

        public override void Clear()
        {
            base.Clear();
            _chunks.Clear();
        }

        protected override void OnLayoutChanged(int oldSampleRate, int newSampleRate, int oldChannels, int newChannels)
        {
            _format = FormatChunk.Create(newSampleRate, newChannels, _sample_format);
        }

        protected override void LoadMetaData(object source)
        {
            base.LoadMetaData(source);
            if (source is IWaveMetaData w)
            {
                _chunks.AddRange(w.Chunks);
                SampleFormat = w.Format.TryGetSampleFormat(out var sf) ? sf : WaveEncodeOptions.DefaultFormat;
            }
            _format = FormatChunk.Create(SampleRate, Channels, SampleFormat);
        }

        public static WaveData Open(string path) => General.Open(path, Load);
        public static WaveData Load(Stream stream)
        {
            WaveData data = new();
            using WaveDecoder decoder = new(stream, true, 0, 0);
            data.Load(decoder);
            return data;
        }

        public void Save(string path) => General.Save(path, Dump, ExtRegs.Wav, Exts.Wav);
        public void Save(string path, bool ext = true) => General.Save(path, Dump, ext ? ExtRegs.Wav : null, Exts.Wav);
        public void Save(string path, SampleFormat sampleFormat, bool ext = true)
        {
            if (sampleFormat.IsValid())
            {
                SampleFormat = sampleFormat;
            }
            else if (!SampleFormat.IsValid())
            {
                SampleFormat = SampleFormat.Int16;
            }
            Save(path, ext);
        }

        public void Dump(Stream stream)
        {
            using WaveEncoder encoder = new(stream, new(SampleRate, Channels, _sample_format), true);
            encoder.Chunks.AddRange(_chunks);
            encoder.SetCueMarkers(_markers, TotalSample);
            encoder.Write(Data);
            encoder.Flush();
        }

        public static void SaveAsWave(string path, WaveBuffer source, SampleFormat format = SampleFormat.Invalid, bool ext = true)
        {
            if (source is WaveData w)
            {
                if (format.IsValid())
                {
                    w.SampleFormat = format;
                }
                w.Save(path, ext);
                return;
            }
            General.PrepareSave(ref path, ext ? ExtRegs.Wav : null, Exts.Wav);
            using var sw = ExStopwatch.SaveProcessTime(path, General.Tweet);
            using WaveEncoder encoder = new(path, new(source.SampleRate, source.Channels, format));
            encoder.SetCueMarkers(source.Markers, source.TotalSample);
            encoder.Software = nameof(LivreNoirLibrary);
            encoder.Write(source.Data);
            encoder.Flush();
        }

        public override void WriteMetaTags(IMetaTag format, IMetaTag stream)
        {
            void Set(string tagName, string? value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    stream.SetMetaTag(tagName, value);
                }
            }
            Set("Genre", this.Genre);
            Set("Title", this.Title);
            Set("Artist", this.Artist);
            Set("Comment", this.Comment);
            if (this.IsTempoSet())
            {
                Set("BPM", this.Tempo.ToString());
            }
        }

        protected override void WriteJsonMetaData(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteString("type", "RIFF WAVE");

            writer.WritePropertyName("format");
            Format.WriteJson(writer, options);
            writer.WritePropertyName("chunks");
            writer.WriteStartArray();
            foreach (var chunk in _chunks.AsSpan())
            {
                chunk.WriteJson(writer, options);
            }
            writer.WriteEndArray();
        }
    }
}