using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public class AudioComposer<TKey>(IWaveBufferProvider<TKey> provider, IAudioTimeline<TKey> timeline) : IAudioBuffer
    {
        private readonly WaveBuffer _composeTarget = new();

        public IWaveBufferProvider<TKey> Provider { get; set; } = provider;
        public IAudioTimeline<TKey> Timeline { get; set; } = timeline;

        public int SampleRate => _composeTarget.SampleRate;
        public int Channels => _composeTarget.Channels;
        public int TotalSample => _composeTarget.TotalSample;

        public bool IgnoreItemDuration { get; set; } = false;
        public float MasterVolume { get; set; } = 1f;
        public Dictionary<int, float> TagToVolume { get; } = [];

        private double _currentSeconds;

        public void Clear()
        {
            _currentSeconds = 0;
            _composeTarget.Clear();
            Timeline.Rewind();
        }

        public void SetLayout(int sampleRate, int channels)
        {
            _composeTarget.SetLayout(sampleRate, channels);
            Provider.OutputSampleRate = sampleRate;
            Provider.OutputChannels = channels;
        }

        public bool SetLayoutByFirstItem()
        {
            var provider = Provider;
            foreach (var list in Timeline)
            {
                if (provider.TryGetWaveBuffer(list.Key, out var buffer))
                {
                    SetLayout(buffer.SampleRate, buffer.Channels);
                    return true;
                }
            }
            return false;
        }

        public void SetVolume(float masterVolume, params ReadOnlySpan<(int, float)> tagToVolumes)
        {
            MasterVolume = masterVolume;
            var t = TagToVolume;
            foreach (var (tag, volume) in tagToVolumes)
            {
                t[tag] = volume;
            }
        }

        public void Setup(int sampleRate, int channels, double delay)
        {
            Clear();
            SetLayout(sampleRate, channels);
            _currentSeconds -= delay;
        }

        public void Read(Span<float> span)
        {
            var buffer = _composeTarget;
            var bufferSampleRate = buffer.SampleRate;
            var bufferChannels = buffer.Channels;
            var requiredSampleCount = span.Length / bufferChannels;
            var requiredSeconds = (double)requiredSampleCount / bufferSampleRate;

            var currentSeconds = _currentSeconds;
            var provider = Provider;
            var notIgnore = !IgnoreItemDuration;
            var masterVolume = MasterVolume;
            var tagToVolume = TagToVolume;
            var until = currentSeconds + requiredSeconds;
            foreach (var list in Timeline)
            {
                if (provider.TryGetWaveBuffer(list.Key, out var source))
                {
                    var rate = source.SampleRate;
                    var sourceSamples = source.SampleLength;
                    foreach (var (itemTime, itemDuration, tag) in list.Advance(until))
                    {
                        if (notIgnore && itemDuration is >= 0)
                        {
                            sourceSamples = Math.Min((int)Math.Ceiling(itemDuration * rate), sourceSamples);
                        }
                        var offset = itemTime - currentSeconds;
                        var sourceOffset = 0;
                        if (offset is < 0)
                        {
                            sourceOffset = -(int)(offset * rate);
                            sourceSamples -= sourceOffset;
                            offset = 0;
                        }
                        if (sourceSamples is > 0)
                        {
                            var destOffset = (int)(offset * bufferSampleRate);
                            var volume = masterVolume * (tagToVolume.TryGetValue(tag, out var value) ? value : 1);
                            buffer.Append(source, destOffset, sourceOffset, sourceSamples, volume);
                        }
                    }
                }
            }
            _currentSeconds = until;
            var sampleCount = Math.Min(buffer.SampleLength, requiredSampleCount);
            var totalSample = sampleCount * bufferChannels;
            if (totalSample is > 0)
            {
                span.CopyFrom(buffer.Data);
                buffer.RemoveRange(0, sampleCount);
            }
            var remain = span.Length - totalSample;
            if (remain is > 0)
            {
                span.Clear(totalSample, remain);
            }
        }
    }
}
