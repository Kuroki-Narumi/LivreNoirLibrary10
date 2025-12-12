using LivreNoirLibrary.Collections;
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
        }

        public void SetLayout(int sampleRate, int channels)
        {
            _composeTarget.SetLayout(sampleRate, channels);
            Provider.OutputSampleRate = sampleRate;
            Provider.OutputChannels = channels;
        }

        public bool SetLayoutByFirstItem()
        {
            if (Timeline.TryGetFirstItem(out var item) && Provider.TryGetWaveBuffer(item.Key, out var buffer))
            {
                SetLayout(buffer.SampleRate, buffer.Channels);
                return true;
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
            foreach (var (key, itemTime, itemDuration, tag) in Timeline.Range(currentSeconds, requiredSeconds))
            {
                if (provider.TryGetWaveBuffer(key, out var source))
                {
                    var offset = itemTime - currentSeconds;
                    var sourceSeconds = source.TotalSeconds;
                    var actualSourceSeconds = (notIgnore && itemDuration is >= 0) ? Math.Min(itemDuration, sourceSeconds) : sourceSeconds;
                    var sourceOffsetSeconds = 0d;
                    if (offset is < 0)
                    {
                        actualSourceSeconds += offset;
                        sourceOffsetSeconds = -offset;
                        offset = 0;
                    }
                    if (actualSourceSeconds is > 0)
                    {
                        var rate = source.SampleRate;
                        var destOffset = (int)(offset * bufferSampleRate);
                        var sourceOffset = (int)(sourceOffsetSeconds * rate);
                        var sourceLength = (int)(actualSourceSeconds * rate);
                        var volume = masterVolume * (tagToVolume.TryGetValue(tag, out var value) ? value : 1);
                        buffer.Append(source, destOffset, sourceOffset, sourceLength, volume);
                    }
                }
            }
            _currentSeconds = currentSeconds + requiredSeconds;
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
