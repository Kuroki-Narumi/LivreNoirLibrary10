using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LivreNoirLibrary.Media.Integrated
{
    public readonly record struct AssembleResult(WaveData WaveData, float ActualGain)
    {
        public static implicit operator AssembleResult((WaveData, float) tuple) => new(tuple.Item1, tuple.Item2);
    }

    public static partial class MediaExtensions
    {
        public static AssembleResult Assemble(this IBmsViewModel data, AssembleOptions option, string directory) => Assemble(data, option, directory, null, CancellationToken.None);
        public static AssembleResult Assemble(this IBmsViewModel data, AssembleOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var includeLongEnd = options.PlayLongEnd && data.LnObj is 0;
            Predicate<Note> selector = options.Target.Type switch
            {
                ConvertTargetType.Key => n => n.IsVisibleKey(includeLongEnd),
                ConvertTargetType.Bgm => n => n.IsBgm(),
                _ => n => n.IsMainSound(includeLongEnd),
            };
            var list = SoundTimingList.Create(data, selector);
            return AssembleCore(data, options, directory, list, options.Adjust ? list.FirstTick : 0, 0, reporter, c);
        }

        public static AssembleResult AssembleSelection(this IBmsViewModel data, AssembleOptions option, Selection selection, string directory) => AssembleSelection(data, option, selection, directory, null, CancellationToken.None);
        public static AssembleResult AssembleSelection(this IBmsViewModel data, AssembleOptions options, Selection selection, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var includeLongEnd = options.PlayLongEnd && data.LnObj is 0;
            var list = SoundTimingList.Create(data, selection, n => n.IsMainSound(includeLongEnd));
            return AssembleCore(data, options, directory, list, list.FirstTick, 0, reporter, c);
        }

        public static AssembleResult AssembleForPreview(this IBmsViewModel data, AssembleOptions options, string directory) => AssembleForPreview(data, options, directory, null, CancellationToken.None);
        public static AssembleResult AssembleForPreview(this IBmsViewModel data, AssembleOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var start = data.TimeCounter.Beat2Tick(data.GetAbsolutePosition(options.PreviewStart));
            var b = TimeUtils.Seconds2Ticks(options.PreviewBody);
            var fo = TimeUtils.Seconds2Ticks(options.PreviewFadeOut);

            var list = SoundTimingList.Create(data, null, start + b + fo);

            var fi = TimeUtils.Seconds2Ticks(options.PreviewFadeIn);
            if (fi > start)
            {
                fi = start;
            }
            var (result, gain) = AssembleCore(data, options, directory, list, start - fi, fi + b + fo, reporter, c);
            reporter?.Report("Apply fade ...", 100, 100);
            var rate = result.SampleRate;
            if (fi is > 0)
            {
                var fadein = (int)(fi * rate / TimeSpan.TicksPerSecond);
                result.FadeIn(0, fadein, 2);
            }
            var fadeout = result.SampleLength - (int)((fi + b) * rate / TimeSpan.TicksPerSecond);
            if (fadeout is > 0)
            {
                result.FadeOut(-fadeout, fadeout, 2);
            }

            return (result, gain);
        }

        private static AssembleResult AssembleCore(IBmsViewModel data, AssembleOptions options, string directory, SoundTimingList timings, long headroom, long lengthLimit, ProgressReporter? reporter, CancellationToken c)
        {
            c.ThrowIfCancellationRequested();
            var ogain = options.Gain;
            var gain = WaveBuffer.Level2Value(ogain);
            var normalize = options.NormalizeMode;
            var needGain = ogain is not 0 || normalize is not 0;
            var overlap = options.Overlap;

            var marker = options.Marker;
            SortedList<long, List<string>> markerList = [];
            var sampleRate = 0;
            var sampleLimit = int.MaxValue;
            var needInitialize = true;
            WaveData result = new()
            {
                Tempo = data.Bpm
            };
            WaveBuffer buffer = new();

            int ToSamples(long tick) => (int)(tick * sampleRate / TimeSpan.TicksPerSecond);
            int ToOffset(long tick) => (int)((tick - headroom) * sampleRate / TimeSpan.TicksPerSecond);
            void Append(string name, long position, long tickLength)
            {
                var srcOffset = 0;
                var offset = ToOffset(position);
                var length = tickLength is >= 0 ? Math.Min(ToSamples(tickLength), buffer.SampleLength) : buffer.SampleLength;
                if (offset is < 0)
                {
                    length += offset;
                    srcOffset = -offset;
                    offset = 0;
                }
                if (offset + length > sampleLimit)
                {
                    length = sampleLimit - offset;
                }
                if (length is > 0)
                {
                    result.Append(buffer, offset, srcOffset, length);
                    if (marker && offset is >= 0)
                    {
                        if (!markerList.TryGetValue(offset, out var list))
                        {
                            list = [];
                            markerList.Add(offset, list);
                        }
                        switch (list.Count)
                        {
                            case < 3:
                                list.Add(name);
                                break;
                            case 3:
                                list.Add("...");
                                break;
                        }
                    }
                }
            }

            reporter?.Report("Assembling ...", 1, 100);

            var current = 0;
            var count = timings.Count;
            var max = 98.0 / count;
            foreach (var (index, list) in timings)
            {
                c.ThrowIfCancellationRequested();
                if (data.TryGetWavePath(index, directory, out var name, out var path))
                {
                    try
                    {
                        buffer.AutoDecode(path, needInitialize);
                    }
                    catch
                    {
                        ExConsole.Write($"Failed to open {path}");
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                if (needInitialize)
                {
                    sampleRate = buffer.SampleRate;
                    if (lengthLimit is > 0)
                    {
                        sampleLimit = ToSamples(lengthLimit);
                    }
                    result.SetLayout(sampleRate, buffer.Channels);
                    result.EnsureSampleLength(Math.Min(sampleLimit, ToOffset(timings.LastTick)));
                    needInitialize = false;
                }
                foreach (var item in list.AsSpan())
                {
                    Append(name!, item.Position, overlap ? 0 : item.Length);
                }
                current++;
                reporter?.Report($"Assembling ({current} of {count})", 1.0 + current * max, 100);
            }
            if (needGain)
            {
                reporter?.Report("Normalizing ...", 99, 100);
                Console.WriteLine($"gain={gain}, mode={normalize}");
                switch (normalize)
                {
                    case NormalizeMode.Peak:
                        gain /= result.GetPeak();
                        break;
                    case NormalizeMode.Rms:
                        gain /= result.GetRms();
                        break;
                    case NormalizeMode.Lufs:
                        gain *= WaveBuffer.Level2Value(-result.GetLufs());
                        break;
                }
                result.Multiply(gain);
            }
            foreach (var (pos, names) in markerList)
            {
                result.Markers.Set(pos, string.Join(" + ", names));
            }
            result.Software = "LivreNoirLibrary";
            return (result, gain);
        }

        public static void ReplaceToAssembled(this IBmsViewModel data, AssembleOptions options, Selection selection, string defName, out Selection newSelection, out int defId)
        {
            if (!data.TryGetDefKey(DefType.Wav, defName, out defId))
            {
                defId = data.FindFreeDefIndex(DefType.Wav);
                data.CurrentData.DefLists.Set(DefType.Wav, defId, defName);
            }
            switch (options.ReplaceMode)
            {
                case AssembleReplaceMode.Selection:
                    newSelection = data.CombineSequence(selection, defId);
                    break;
                case AssembleReplaceMode.All:
                    newSelection = data.CombineSequenceAll(selection, defId, options.ReplaceMargin);
                    break;
                default:
                    newSelection = selection;
                    return;
            }
        }
    }
}
