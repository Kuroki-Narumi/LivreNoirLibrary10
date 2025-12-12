using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LivreNoirLibrary.Media.Integrated
{
    using AssembleResult = (WaveData WaveData, float ActualGain);

    public static partial class MediaExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Predicate<Note> GetDefaultPredicate<T>(IBmsViewModel vm, T options) where T : IAssemblePlaysLongEndOptions
        {
            var includeLongEnd = options.PlaysLongEnd && vm.LnObj is 0;
            return n => n.IsMainSound(includeLongEnd);
        }

        public static AssembleResult Assemble<T>(
            this IBmsViewModel vm, IWaveBufferProvider<string> provider, T options,
            ProgressReporter? reporter = null, CancellationToken c = default)
            where T : IAssemblePlaysLongEndOptions, IConvertTargetOptions, IAdjustOptions
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var includeLongEnd = options.PlaysLongEnd && vm.LnObj is 0;
            Predicate<Note> selector = options.ConvertTarget.Type switch
            {
                ConvertTargetType.Key => n => n.IsVisibleKey(includeLongEnd),
                ConvertTargetType.Bgm => n => n.IsBgm(),
                _ => n => n.IsMainSound(includeLongEnd),
            };
            var list = SoundTimingList.Create(vm, selector);
            if (options.AdjustBeginning)
            {
                options.Offset = list.FirstTime;
            }
            options.Length = 0;
            return Assemble(vm, provider, list, options, reporter, c);
        }

        public static AssembleResult AssembleSelection<T>(
            this IBmsViewModel vm, IWaveBufferProvider<string> provider, T options, 
            Selection selection, ProgressReporter? reporter = null, CancellationToken c = default)
            where T : IAssemblePlaysLongEndOptions
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var list = SoundTimingList.Create(vm, selection, GetDefaultPredicate(vm, options));
            options.Offset = list.FirstTime;
            options.Length = 0;
            return Assemble(vm, provider, list, options, reporter, c);
        }

        public static AssembleResult AssembleForPreview<T>(
            this IBmsViewModel vm, IWaveBufferProvider<string> provider, T options, 
            ProgressReporter? reporter = null, CancellationToken c = default)
            where T : IAssemblePreviewOptions
        {
            reporter?.Report("Creating Timeline ...", 0, 100);
            var start = vm.Position2Tick(options.PreviewStart);
            var b = TimeUtils.Seconds2Ticks(options.PreviewBody);
            var fo = TimeUtils.Seconds2Ticks(options.PreviewFadeOut);

            var list = SoundTimingList.Create(vm, GetDefaultPredicate(vm, options), start + b + fo);

            var fi = TimeUtils.Seconds2Ticks(options.PreviewFadeIn);
            if (fi > start)
            {
                fi = start;
            }
            options.Offset = start - fi;
            options.Length = fi + b + fo;
            var (result, gain) = Assemble(vm, provider, list, options, reporter, c);
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

        public static AssembleResult Assemble<T>(this IBmsViewModel vm, 
            IWaveBufferProvider<string> provider, ISoundList soundList, T options, 
            ProgressReporter? p = null, CancellationToken c = default)
            where T : IAssembleCoreOptions
        {
            c.ThrowIfCancellationRequested();
            var gain = options.Gain;
            var actualGain = WaveBuffer.Level2Value(gain);
            var normalizeMode = options.NormalizeMode;
            var keyVolume = options.KeyVolume;
            var bgmVolume = options.BgmVolume;
            var notOverlap = !options.Overlap;

            var directory = options.RootDirectory;
            var headroom = options.Offset;
            var totalLength = options.Length;

            var setMarker = options.SetMarker;
            SortedSet<int> markers = [];

            var sampleRate = 0;
            var sampleLimit = int.MaxValue;
            var needInitialize = true;

            WaveData result = new()
            {
                Tempo = vm.Bpm
            };

            p?.Report("Assembling ...", 1, 100);
            var current = 0;
            var count = soundList.Count;
            var max = 98d / count;
            foreach (var (id, list) in soundList.EnumerateSoundList())
            {
                c.ThrowIfCancellationRequested();
                if (!(vm.TryGetWavePath(id, directory, out var name, out var path) && provider.TryGetWaveBuffer(path, out var buffer)))
                {
                    continue;
                }
                if (needInitialize)
                {
                    sampleRate = buffer.SampleRate;
                    if (totalLength is > 0)
                    {
                        sampleLimit = ToSamples(totalLength);
                    }
                    result.SetLayout(sampleRate, buffer.Channels);
                    result.EnsureSampleLength(Math.Min(sampleLimit, ToOffset(soundList.LastTime)));
                    needInitialize = false;
                }
                foreach (var (time, length, isBgm) in list.AsSpan())
                {
                    c.ThrowIfCancellationRequested();
                    var sourceOffset = 0;
                    var offset = ToOffset(time);
                    var sampleLength = (notOverlap && length is >= 0) ? Math.Min(ToSamples(length), buffer.SampleLength) : buffer.SampleLength;
                    if (offset is < 0)
                    {
                        sampleLength += offset;
                        sourceOffset = -offset;
                        offset = 0;
                    }
                    sampleLength = Math.Min(sampleLength, sampleLimit - offset);
                    if (sampleLength is > 0)
                    {
                        result.Append(buffer, offset, sourceOffset, sampleLength, isBgm ? bgmVolume : keyVolume);
                        if (setMarker)
                        {
                            markers.Add(offset);
                        }
                    }
                }
                current++;
                p?.Report($"Assembling ({current} of {count})", 1.0 + current * max, 100);
            }

            if (gain is not 0 || normalizeMode is not 0)
            {
                p?.Report("Normalizing ...", 99, 100);
                Console.WriteLine($"gain={gain}, mode={normalizeMode}");
                switch (normalizeMode)
                {
                    case NormalizeMode.Peak:
                        actualGain /= result.GetPeak();
                        break;
                    case NormalizeMode.Rms:
                        actualGain /= result.GetRms();
                        break;
                    case NormalizeMode.Lufs:
                        actualGain *= WaveBuffer.Level2Value(-result.GetLufs());
                        break;
                }
                result.Multiply(actualGain);
            }

            if (setMarker)
            {
                var format = SliceUtils.GetIndexFormat(markers.Count);
                current = 0;
                foreach (var pos in markers)
                {
                    current++;
                    result.Markers.Set(pos, current.ToString(format));
                }
            }
            result.Software = "LivreNoirLibrary";

            return (result, actualGain);

            int ToSamples(double time) => (int)(time * sampleRate);
            int ToOffset(double time) => (int)((time - headroom) * sampleRate);
        }

        public static void ReplaceToAssembled(this IBmsViewModel data, IAssembleReplaceOptions options, Selection selection, string defName, out Selection newSelection, out int defId)
        {
            switch (options.ReplaceMode)
            {
                case AssembleReplaceMode.Selection:
                    defId = EnsureDefId();
                    newSelection = data.CombineSequence(selection, defId);
                    break;
                case AssembleReplaceMode.All:
                    defId = EnsureDefId();
                    newSelection = data.CombineSequenceAll(selection, defId, options.ReplaceMargin);
                    break;
                default:
                    defId = 0;
                    newSelection = selection;
                    return;
            }

            int EnsureDefId()
            {
                if (!data.TryGetDefKey(DefType.Wav, defName, out var defId))
                {
                    defId = data.FindFreeDefIndex(DefType.Wav);
                    data.CurrentData.DefLists.Set(DefType.Wav, defId, defName);
                }
                return defId;
            }
        }
    }
}
