using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        public WaveData Assemble(AssembleOptions option, string directory) => Assemble(option, directory, null, CancellationToken.None);
        public WaveData Assemble(AssembleOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var selector = options.Target.GetSelector([], options.PlayLongEnd && LnObj is 0);
            var list = TimingList.Create(this, selector);
            return AssembleCore(options, directory, list, options.Adjust ? list.FirstTick : 0, 0, reporter, c);
        }

        public WaveData AssembleSelection(AssembleOptions option, Selection selection, string directory) => AssembleSelection(option, selection, directory, null, CancellationToken.None);
        public WaveData AssembleSelection(AssembleOptions options, Selection selection, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var lnEnd = options.PlayLongEnd && LnObj is 0;
            var list = TimingList.Create(this, selection, n => n.IsPlayableSound(lnEnd));
            return AssembleCore(options, directory, list, list.FirstTick, 0, reporter, c);
        }

        public WaveData AssembleForPreview(AssembleOptions options, string directory) => AssembleForPreview(options, directory, null, CancellationToken.None);
        public WaveData AssembleForPreview(AssembleOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            TimeCounter counter = new(this);
            var start = counter.Beat2Ticks(GetBeat(options.PreviewStart));
            static long ToTicks(decimal duration) => (duration * TimeSpan.TicksPerSecond).RoundToLong();
            var b = ToTicks(options.PreviewBody);
            var fo = ToTicks(options.PreviewFadeOut);

            var list = TimingList.Create(this, counter, null, start + b + fo);

            var fi = ToTicks(options.PreviewFadeIn);
            if (fi > start)
            {
                fi = start;
            }
            var data = AssembleCore(options, directory, list, start - fi, fi + b + fo, reporter, c);
            reporter?.Report("Apply fade ...", 100, 100);
            var rate = data.SampleRate;
            if (fi is > 0)
            {
                var fadein = (int)(fi * rate / TimeSpan.TicksPerSecond);
                data.FadeIn(0, fadein, 2);
            }
            var fadeout = data.SampleLength - (int)((fi + b) * rate / TimeSpan.TicksPerSecond);
            if (fadeout is > 0)
            {
                data.FadeOut(-fadeout, fadeout, 2);
            }

            return data;
        }

        private WaveData AssembleCore(AssembleOptions options, string directory, TimingList timings, long headroom, long lengthLimit, ProgressReporter? reporter, CancellationToken c)
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
            WaveData data = new()
            {
                Tempo = (double)Bpm
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
                    data.Append(buffer, offset, srcOffset, length);
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
            var max = 98.0 / timings.Count;
            foreach (var (index, list) in timings)
            {
                c.ThrowIfCancellationRequested();
                if (TryGetWavePath(index, directory, out var name, out var path))
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
                    data.SetLayout(sampleRate, buffer.Channels);
                    data.EnsureSampleLength(Math.Min(sampleLimit, ToOffset(timings.LastTick)));
                    needInitialize = false;
                }
                foreach (var item in CollectionsMarshal.AsSpan(list))
                {
                    Append(name!, item.Position, overlap ? 0 : item.Length);
                }
                current++;
                reporter?.Report($"Assembling ({current} of {max})", 1.0 + current * max, 100);
            }
            if (needGain)
            {
                reporter?.Report("Normalizing ...", 99, 100);
                if (normalize)
                {
                    gain /= data.GetMaxMagnitude();
                }
                data.Multiply(gain);
            }
            foreach (var (pos, names) in markerList)
            {
                data.Markers.Set(pos, string.Join(" + ", names));
            }
            data.Software = "LivreNoirLibrary";
            return data;
        }

        public bool ReplaceToAssembled(AssembleOptions options, Selection selection, string defName, out Selection newSelection, out int defId)
        {
            defId = DefLists.FindIndex(DefType.Wav, defName);
            if (defId is < 0)
            {
                defId = DefLists.FindFreeIndex(DefType.Wav);
            }
            bool flag;
            switch (options.ReplaceMode)
            {
                case AssembleReplaceMode.Selection:
                    flag = ReplaceSelection(selection, defId, out newSelection);
                    break;
                case AssembleReplaceMode.All:
                    flag = ReplaceSelectionAll(selection, defId, options.ReplaceMargin, out newSelection);
                    break;
                default:
                    newSelection = selection;
                    return false;
            }
            if (flag)
            {
                DefLists.Set(DefType.Wav, defId, defName);
            }
            return flag;
        }
    }
}
