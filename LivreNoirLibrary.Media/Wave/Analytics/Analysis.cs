using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class Analysis : IEnumerable<Analysis.Channel>
    {
        public int Channels { get; }
        public int SampleRate { get; }
        public long Length { get; }
        public long FFTLength { get; }

        private readonly Channel[] _channelData;
        public Channel this[int channel] => _channelData[channel];

        public Analysis(int sampleCount, int channels, int sampleRate)
        {
            Channels = channels;
            SampleRate = sampleRate;
            _channelData = new Channel[channels];
            Length = sampleCount;
            FFTLength = FFT.CalcFittingLength(sampleCount);
            for (int c = 0; c < Channels; c++)
            {
                _channelData[c] = new(sampleCount);
            }
        }

        public Analysis(IWaveBuffer data)
        {
            var length = data.SampleLength;
            var ch = data.Channels;
            var sRate = data.SampleRate;
            Channels = ch;
            SampleRate = sRate;
            Length = length;
            _channelData = new Channel[ch];
            FFTLength = FFT.CalcFittingLength(length);
            for (int c = 0; c < ch; c++)
            {
                _channelData[c] = new(data, c);
            }
        }

        public float[] GetInverse()
        {
            var result = new float[Channels][];
            for (int c = 0; c < Channels; c++)
            {
                result[c] = FFT.Backward(_channelData[c].FFTResult);
            }
            return WaveBuffer.MergeChannels(result);
        }

        public IEnumerator<Channel> GetEnumerator()
        {
            for (int c = 0; c < Channels; c++)
            {
                yield return _channelData[c];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
