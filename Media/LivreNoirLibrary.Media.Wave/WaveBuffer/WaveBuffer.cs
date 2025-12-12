using System;
using System.Text.Json;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class WaveBuffer() : DisposableBase, IWaveBuffer, IMarkerContainer, IJsonWriter, IClear
    {
        public const int DefaultSampleRate = 44100;
        public const int DefaultChannels = 2;

        public static WaveBuffer Empty() => new();

        private readonly UnmanagedArray<float> _data = [];
        protected readonly MarkerCollection _markers = [];

        public Span<float> Data => _data.Slice(0, TotalSample);
        public int SampleRate { get; private set; } = DefaultSampleRate;
        public int Channels { get; private set; } = DefaultChannels;
        public int TotalSample { get; private set; }

        int IMarkerContainer.Length => this.SampleLength;

        public MarkerCollection Markers => _markers;

        public static WaveBuffer CreateUnsafe(int sampleRate, int channels) => new() { SampleRate = sampleRate, Channels = channels };

        protected override void DisposeUnmanaged()
        {
            _data.Free();
            base.DisposeUnmanaged();
        }

        public virtual void Clear()
        {
            TotalSample = 0;
            _markers.Clear();
        }

        public void SetTotalSample(int size, bool clear = true)
        {
            if (TotalSample < size)
            {
                _data.EnsureSize(size, false);
                if (clear)
                {
                    _data.Clear(TotalSample);
                }
            }
            TotalSample = size;
        }

        public void Load(ReadOnlySpan<float> source, int sampleRate = 0, int channels = 0)
        {
            Clear();
            SetLayout(sampleRate, channels);
            SetTotalSample(source.Length, false);
            source.CopyTo(_data);
        }

        public void Load(IWaveBuffer source, int sampleOffset = 0, int sampleCount = 0)
        {
            Clear();
            var span = source.Data;
            var channels = source.Channels;
            AdjustArgs(span, ref sampleOffset, ref sampleCount, source.Channels);
            span = span.Slice(sampleOffset * channels, sampleCount * channels);
            SetLayout(source.SampleRate, channels);
            SetTotalSample(span.Length, false);
            span.CopyTo(_data);
            LoadMetaData(source);
        }

        public WaveBuffer Clone()
        {
            WaveBuffer data = new();
            data.Load(this);
            return data;
        }

        public T Clone<T>()
            where T : WaveBuffer, new()
        {
            T data = new();
            data.Load(this);
            return data;
        }

        public void AutoDecode(string path, bool updateLayout = true)
        {
            if (WaveInfo.IsSupported(path))
            {
                DecodeWave(path, updateLayout);
            }
            else
            {
                Decode(path, updateLayout);
            }
        }

        public void Decode(string path, bool updateLayout = true)
        {
            if (updateLayout)
            {
                SampleRate = 0;
                Channels = 0;
            }
            using AudioDecoder decoder = new(path, new(SampleRate, Channels));
            Load(decoder);
        }

        public void DecodeWave(string path, bool updateLayout = true)
        {
            if (updateLayout)
            {
                SampleRate = 0;
                Channels = 0;
            }
            using WaveDecoder decoder = new(path, SampleRate, Channels);
            Load(decoder);
        }

        public void Load(IAudioDecodeContext decoder)
        {
            Clear();
            var rate = decoder.OutputSampleRate;
            var channels = decoder.OutputChannels;
            SetLayout(rate, channels);
            SetTotalSample((int)decoder.TotalSample, false);
            TotalSample = decoder.Read(_data);
            LoadMetaData(decoder);
        }

        protected virtual void LoadMetaData(object source)
        {
            if (source is IMarkerContainer m)
            {
                m.Markers.CopyTo(_markers);
            }
            if (source is IWaveMetaData w)
            {
                w.GetCueMarkers(_markers);
            }
        }

        public virtual void WriteMetaTags(IMetaTag format, IMetaTag stream) { }

        public void SetLayout(int sampleRate, int channels)
        {
            var rate = SampleRate;
            var ch = Channels;
            if (sampleRate is <= 0)
            {
                sampleRate = rate;
            }
            if (channels is <= 0)
            {
                channels = ch;
            }
            CheckLayout(sampleRate, channels);
            if (rate != sampleRate || ch != channels)
            {
                SampleRate = sampleRate;
                Channels = channels;
                OnLayoutChanged(rate, sampleRate, ch, channels);
            }
        }

        protected virtual void OnLayoutChanged(int oldSampleRate, int newSampleRate, int oldChannels, int newChannels) { }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber("channels", Channels);
            writer.WriteNumber("sample rate", SampleRate);
            writer.WriteNumber("sample length", this.SampleLength);
            writer.WriteString("duration", this.TotalTime.AutoFormat());

            WriteJsonMetaData(writer, options);

            writer.WriteEndObject();
        }

        protected virtual void WriteJsonMetaData(Utf8JsonWriter writer, JsonSerializerOptions options) { }
    }
}
