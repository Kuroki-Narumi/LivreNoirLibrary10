using LivreNoirLibrary.Collections;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public class AudioComposer<TKey> : ISampleProvider
    {
        public IWaveBufferProvider<TKey> Provider { get; set; }
        public IAudioTimeline<TKey> Timeline { get; set; }

        public bool IgnoreItemDuration { get; set; } = false;
        public float MasterVolume { get; set; } = 1f;
        public Dictionary<int, float> TagToVolume { get; } = [];

        public WaveFormat WaveFormat { get; private set; }

        private List<ComposeInfo> _currentList = new(64);
        private List<ComposeInfo> _nextList = new(64);
        private double _currentSeconds;

        public AudioComposer(IWaveBufferProvider<TKey> provider, IAudioTimeline<TKey> timeline)
        {
            Provider = provider;
            Timeline = timeline;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(Provider.SampleRate, Provider.Channels);
        }

        public void Clear()
        {
            _currentSeconds = 0;
            _currentList.Clear();
            _nextList.Clear();
            Timeline.Rewind();
        }

        public void SetLayout(int sampleRate, int channels)
        {
            Provider.SampleRate = sampleRate;
            Provider.Channels = channels;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
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

        public readonly record struct ComposeInfo(TKey Key, IWaveBuffer Source, int SourceOffset, int SourceSamples, int DestOffset, int Tag);

        private static void Append(
            int ch, List<ComposeInfo> queue, float masterVolume, Dictionary<int, float> tagToVolume,
            in Span<float> span, int requiredSampleCount, in ComposeInfo info)
        {
            var (key, source, sourceOffset, sourceSamples, destOffset, tag) = info;
            // destOffsetが負の場合
            // 注: 以下の条件は保証されている
            //    requiredSampleCount = span.Length / ch
            //    sourceSamples <= source.SampleCount

            // 例1: requiredSampleCount=10, destOffset=-10, sourceOffset=10, sourceSamples=100
            // 例2: requiredSampleCount=10, destOffset=20, sourceOffset=0, sourceSamples=100
            if (destOffset is < 0)
            {
                // 例1: source[0..].CopyTo(dest[-10..]) は source[10..].CopyTo(dest[0..]) と等価
                sourceOffset -= destOffset;
                sourceSamples += destOffset;
                destOffset = 0;
                // 例1: この時点で destOffset=0, sourceOffset=20, sourceSampels=90
            }
            // 実際にコピーされるサンプル数
            // 例1: Math.Min(10 - 0, 90) = Math.Min(10, 90) = 10
            // 例2: Math.Min(10 - 20, 100) = Math.Min(-10, 100) = -10
            var destSamples = Math.Max(Math.Min(requiredSampleCount - destOffset, sourceSamples), 0);
            if (destSamples is > 0)
            {
                var destSpan = span[(destOffset * ch)..];
                var sourceSpan = source.Data.Slice(sourceOffset * ch, destSamples * ch);
                var volume = masterVolume * tagToVolume.GetValueOrDefault(tag, 1f);
                destSpan.Add(sourceSpan, volume);
            }
            // 未処理のサンプル数
            // 例1: sourceSamples = 90 - 10 = 80
            // 例2: sourceSamples = 100 - 0 = 100
            sourceSamples -= destSamples;
            if (sourceSamples is > 0)
            {
                // 次に適用される sourceOffset
                // 例1: sourceOffset = 20 + 10 = 30
                // 例2: sourceOffset = 0 + 0 = 0
                sourceOffset += destSamples;
                // 次に適用される destOffset
                // 例1: destOffset = Math.Max(0 - 10, 0) = 0
                // 例2: destOffset = Math.Max(20 - 10, 0) = 10
                destOffset = Math.Max(destOffset - requiredSampleCount, 0);
                queue.Add(new(key, source, sourceOffset, sourceSamples, destOffset, tag));
            }
        }

        public void Read(Span<float> span)
        {
            var provider = Provider;
            var rate = provider.SampleRate;
            var ch = provider.Channels;
            var requiredSampleCount = span.Length / ch;

            SimdOperations.Clear(span);
            var currentSeconds = _currentSeconds;
            var currentQueue = _currentList;
            var nextQueue = _nextList;
            var masterVolume = MasterVolume;
            var tagToVolume = TagToVolume;

            // 既存リストの消化
            foreach (var info in currentQueue.AsSpan())
            {
                Append(ch, nextQueue, masterVolume, tagToVolume, span, requiredSampleCount, info);
            }
            currentQueue.Clear();

            var notIgnore = !IgnoreItemDuration;
            var until = currentSeconds + (double)requiredSampleCount / rate;
            foreach (var list in Timeline)
            {
                var key = list.Key;
                if (provider.TryGetWaveBuffer(key, out var source))
                {
                    var sourceLength = source.SampleLength;
                    while (list.MoveNext(until, out var info))
                    {
                        var (itemTime, itemDuration, tag) = info;
                        var sourceSamples = (notIgnore && itemDuration is >= 0) ? Math.Min((int)Math.Ceiling(itemDuration * rate), sourceLength) : sourceLength;
                        var destOffset = (int)((itemTime - currentSeconds) * rate);
                        Append(ch, nextQueue, masterVolume, tagToVolume, span, requiredSampleCount, new(key, source, 0, sourceSamples, destOffset, tag));
                    }
                }
            }
            _currentSeconds = until;
            // リストの参照を入れ替える (list1: 空になったリスト, list2: 未処理のソースが追加されたリスト)
            _currentList = nextQueue;
            _nextList = currentQueue;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var ch = Provider.Channels;
            var span = buffer.AsSpan(offset, count / ch * ch);
            Read(span);
            return span.Length;
        }
    }
}
