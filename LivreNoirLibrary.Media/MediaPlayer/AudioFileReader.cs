using System;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Media.Wave;
using NAudio.Wave;

namespace LivreNoirLibrary.Media
{
    public sealed class AudioFileReader : WaveStream, IWaveProvider, ISampleProvider, IMediaDecoder
    {
        private readonly IAudioDecoder _decoder;
        private readonly WaveFormat _waveFormat;
        private float _volume = 1;

        public override WaveFormat WaveFormat => _waveFormat;
        public override long Length => SampleCount * _waveFormat.BlockAlign;
        public override long Position
        {
            get => _decoder.SamplePosition * _waveFormat.BlockAlign;
            set => _decoder.SampleSeek(value / _waveFormat.BlockAlign);
        }
        public long SampleCount => _decoder.SampleLength;

        public Rational Duration => _decoder.Duration;
        public double TotalSeconds => (double)_decoder.Duration;
        public override TimeSpan TotalTime => TimeSpan.FromSeconds(TotalSeconds);
        public long TotalTicks => _decoder.Duration.ToTicks();

        public float Volume
        {
            get => _volume;
            set => _volume = Math.Max(value, 0);
        }

        private AudioFileReader(IAudioDecoder decoder)
        {
            _decoder = decoder;
            _waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(decoder.OutputSampleRate, decoder.OutputChannels);
        }

        public AudioFileReader(string path, AudioDecodeOptions options = default) : this(new AudioDecoder(path, options)) { }
        public AudioFileReader(Stream stream, bool leaveOpen = true, AudioDecodeOptions options = default) : this(new AudioDecoder(stream, leaveOpen, options)) { }

        public static IAudioDecoder DecoderAutoOpen(string path)
        {
            var stream = File.OpenRead(path);
            return ExtRegs.Wav.IsMatch(path) && WaveDecoder.IsSupported(stream) ? new WaveDecoder(stream, false) : new AudioDecoder(stream, false);
        }

        public static AudioFileReader AutoOpen(string path) => new(DecoderAutoOpen(path));
        public static AudioFileReader AutoOpen(Stream stream, bool leaveOpen = true)
        {
            IAudioDecoder decoder = WaveDecoder.IsSupported(stream) ? new WaveDecoder(stream, leaveOpen) : new AudioDecoder(stream, leaveOpen);
            return new(decoder);
        }

        void IMediaDecoder.Seek(Rational position) => _decoder.Seek(position);
        public void SampleSeek(int position) => _decoder.SampleSeek(position);
        public void SeekByTicks(long ticks) => _decoder.SampleSeek(ticks * _decoder.OutputSampleRate / TimeSpan.TicksPerSecond);
        public void SeekByTime(TimeSpan time) => SeekByTicks(time.Ticks);
        public void SeekByMicroseconds(long microseconds) => _decoder.SampleSeek(microseconds * _decoder.OutputSampleRate / TimeSpan.MicrosecondsPerSecond);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _decoder.Dispose();
            }
            base.Dispose(disposing);
        }

        public override unsafe int Read(byte[] buffer, int offset, int count)
        {
            fixed (byte* ptr = buffer)
            {
                var span = new Span<float>(ptr, count / sizeof(float));
                var read = ReadCore(span);
                return read * sizeof(float);
            }
        }

        public int Read(float[] buffer, int offset, int count) => ReadCore(buffer.AsSpan(offset, count));

        private int ReadCore(Span<float> span)
        {
            var read = _decoder.Read(span);
            if (_volume is not 1)
            {
                span.Multiply(_volume);
            }
            return read;
        }

        public float[] ReadAllSamples() => _decoder.ReadAllSamples();
    }
}
