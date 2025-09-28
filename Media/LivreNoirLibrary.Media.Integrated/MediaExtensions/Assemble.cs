using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Integrated
{
    public static partial class MediaExtensions
    {
        public static WaveData Assemble(this IBmsData data, AssembleOptions option, string directory) => Assemble(data, option, directory, null, CancellationToken.None);
        public static WaveData Assemble(this IBmsData data, AssembleOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var selector = options.Target.GetSelector([], options.PlayLongEnd && data.LnObj is 0);
            var list = SoundTimingList.Create(data, selector);
            return AssembleCore(data, options, directory, list, options.Adjust ? list.FirstTick : 0, 0, reporter, c);
        }

        public static WaveData AssembleSelection(this IBmsData data, AssembleOptions option, Selection selection, string directory) => AssembleSelection(data, option, selection, directory, null, CancellationToken.None);
        public static WaveData AssembleSelection(this IBmsData data, AssembleOptions options, Selection selection, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var lnEnd = options.PlayLongEnd && data.LnObj is 0;
            var list = SoundTimingList.Create(data, selection, n => n.IsPlayableSound(lnEnd, out _));
            return AssembleCore(data, options, directory, list, list.FirstTick, 0, reporter, c);
        }

        public static WaveData AssembleForPreview(this IBmsData data, AssembleOptions options, string directory) => AssembleForPreview(data, options, directory, null, CancellationToken.None);
        public static WaveData AssembleForPreview(this IBmsData data, AssembleOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            TimeCounter counter = new(data);
            var start = counter.Beat2Ticks(data.GetAbsolutePosition(options.PreviewStart));
            static long ToTicks(decimal duration) => (duration * TimeSpan.TicksPerSecond).RoundToLong();
            var b = ToTicks(options.PreviewBody);
            var fo = ToTicks(options.PreviewFadeOut);

            var list = SoundTimingList.Create(data, counter, null, start + b + fo);

            var fi = ToTicks(options.PreviewFadeIn);
            if (fi > start)
            {
                fi = start;
            }
            var result = AssembleCore(data, options, directory, list, start - fi, fi + b + fo, reporter, c);
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

            return result;
        }

        private static WaveData AssembleCore(IBmsData data, AssembleOptions options, string directory, SoundTimingList timings, long headroom, long lengthLimit, ProgressReporter? reporter, CancellationToken c)
        {
            c.ThrowIfCancellationRequested();
            var ogain = options.Gain;
            var gain = WaveBuffer.Level2Value(ogain);
            var normalize = options.Normalize;
            var needGain = ogain is not 0 || normalize;
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
                foreach (var item in CollectionsMarshal.AsSpan(list))
                {
                    Append(name!, item.Position, overlap ? 0 : item.Length);
                }
                current++;
                reporter?.Report($"Assembling ({current} of {count})", 1.0 + current * max, 100);
            }
            if (needGain)
            {
                reporter?.Report("Normalizing ...", 99, 100);
                if (normalize)
                {
                    gain /= result.GetMaxMagnitude();
                }
                result.Multiply(gain);
            }
            foreach (var (pos, names) in markerList)
            {
                result.Markers.Set(pos, string.Join(" + ", names));
            }
            result.Software = "LivreNoirLibrary";
            return result;
        }

        public static bool ReplaceToAssembled(this IBmsData data, AssembleOptions options, Selection selection, string defName, out Selection newSelection, out int defId)
        {
            if (!data.TryGetDefIndex(DefType.Wav, defName, out defId))
            {
                defId = data.FindFreeDefIndex(DefType.Wav);
            }
            bool flag;
            switch (options.ReplaceMode)
            {
                case AssembleReplaceMode.Selection:
                    flag = data.ReplaceSelection(selection, defId, out newSelection);
                    break;
                case AssembleReplaceMode.All:
                    flag = data.ReplaceSelectionAll(selection, defId, options.ReplaceMargin, out newSelection);
                    break;
                default:
                    newSelection = selection;
                    return false;
            }
            if (flag)
            {
                data.DefLists.Set(DefType.Wav, defId, defName);
            }
            return flag;
        }
    }
}
