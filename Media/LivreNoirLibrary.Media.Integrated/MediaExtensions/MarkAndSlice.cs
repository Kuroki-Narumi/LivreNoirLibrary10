using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace LivreNoirLibrary.Media.Integrated
{
    public static partial class MediaExtensions
    {
        public static void MarkEachNote(this ITrack track, bool cutTail, Rational tailMargin)
        {
            var needCheckTail = tailMargin.IsPositiveThanZero();
            var timeline = track.Timeline;
            List<(Rational, IObject)> remove = [];
            SortedDictionary<Rational, bool> add = [];
            var count = 0;
            foreach (var (pos, obj) in timeline)
            {
                if (obj is MetaText { Type: MetaType.Marker })
                {
                    remove.Add((pos, obj));
                }
                else if (obj is Midi.INote note && track.IsNormalNote(note))
                {
                    foreach (var (p, _) in note.EachNote(pos))
                    {
                        if (add.TryGetValue(p, out var current))
                        {
                            if (!current)
                            {
                                add[p] = true;
                                count++;
                            }
                        }
                        else
                        {
                            add.Add(p, true);
                            count++;
                        }
                        add.TryAdd(p + note.Length, false);
                    }
                }
            }
            foreach (var (pos, obj) in remove.AsSpan())
            {
                timeline.Remove(pos, obj);
            }
            var fmt = SliceUtils.GetIndexFormat(count);
            var flagList = add.ToList();
            var max = flagList.Count - 1;
            for (int i = 0, j = 0; i <= max; i++)
            {
                string n;
                var (pos, flag) = flagList[i];
                if (flag)
                {
                    j++;
                    n = string.Format(fmt, j);
                }
                else if (cutTail)
                {
                    n = Constants.IgnoreMarkerName;
                    if (needCheckTail)
                    {
                        pos += tailMargin;
                        if (i < max && pos >= flagList[i + 1].Key)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }
                timeline.AddToFront(pos, new MetaText(MetaType.Marker, n));
            }
        }

        public static bool MarkByTrack<T>(this T wave, IScore source, int trackIndex, SliceOptions options)
            where T : IWaveBuffer, IMarkerContainer
        {
            ExConsole.Write($"Mark By Track: {options.GetJsonText(false)}");
            var track = source.GetTrack(trackIndex);
            var firstPos = track.GetFirstMetaPosition(MetaType.Marker);
            if (firstPos.IsNegative())
            {
                track = source.ConductorTrack;
                firstPos = track.GetFirstMetaPosition(MetaType.Marker);
                if (firstPos.IsNegative())
                {
                    return false;
                }
            }
            var conductor = new TempoTimeline(source);
            var offset = (int)options.Offset;
            var midiOffset = 0d;
            var adjust = options.Adjust;
            if (adjust)
            {
                midiOffset = conductor.GetSeconds(firstPos);
                offset += wave.FindFirstSound(WaveBuffer.Level2Value(options.AdjustThreshold));
            }
            var adjustOffset = 0;
            var sampleRate = wave.SampleRate;
            var markers = wave.Markers;
            var limit = wave.SampleLength;
            markers.Clear();
            foreach (var (pos, note) in track.Timeline)
            {
                if (note is MetaText t && t.Type is MetaType.Marker)
                {
                    var seconds = conductor.GetSeconds(pos) - midiOffset;
                    var samples = Math.Max((seconds * sampleRate).RoundToInt() + offset, 0);
                    if (samples >= limit)
                    {
                        break;
                    }
                    if (adjust)
                    {
                        var fs = wave.FindFirstSound(0, samples);
                        if (fs == samples)
                        {
                            samples += adjustOffset;
                        }
                        else
                        {
                            adjustOffset = fs - samples;
                            samples = fs;
                        }
                    }
                    markers.Set(Math.Max(samples, 0), t.Text);
                }
            }
            return true;
        }

        public static void SaveSliced_General<T>(T source, string directory, string baseName, SliceOptions options, ProgressReporter? p, CancellationToken c)
            where T : IWaveBuffer, IMarkerContainer
        {
            var cLeft = WaveBuffer.Level2Value(options.CutoffLeft);
            var mLeft = (int)options.MarginLeft;
            var fadein = (int)options.FadeIn;
            var fiFactor = (float)options.FadeInFactor;
            var cRight = WaveBuffer.Level2Value(options.CutoffRight);
            var mRight = (int)options.MarginRight;
            var fadeout = (int)options.FadeOut;
            var foFactor = (float)options.FadeOutFactor;
            var crossfade = (int)options.CrossFade;
            var needCrossfade = false;
            var format = options.Format;
            if (!format.IsValid())
            {
                format = source is WaveData wd ? wd.SampleFormat : WaveEncodeOptions.DefaultFormat;
            }
            var rate = source.SampleRate;
            var ch = source.Channels;
            var i = 0d;
            var max = (double)source.GetSliceCount();
            using UnmanagedArray<float> buffer = new();
            foreach (var slice in source.EachSlice())
            {
                c.ThrowIfCancellationRequested();
                var fullPath = Path.GetFullPath($"{baseName}{slice.Name}", directory);
                p?.ReportFraction(i, max);
                try
                {
                    var t0 = Stopwatch.GetTimestamp();
                    var (srcSlice, ro, rl, ao, al) = source.SliceWithCutSilence((int)slice.Offset, (int)slice.Length, cLeft, cRight, mLeft, mRight);
                    var bufferSize = srcSlice.Length;
                    buffer.EnsureSize(bufferSize + crossfade * ch, false);
                    buffer.CopyFrom(srcSlice);
                    if (needCrossfade)
                    {
                        WaveBuffer.FadeIn(buffer, 0, crossfade, fiFactor, ch);
                        needCrossfade = false;
                    }
                    else if (fadein is > 0)
                    {
                        WaveBuffer.FadeIn(buffer, 0, fadein, fiFactor, ch);
                    }
                    if (crossfade is > 0 && ao + al >= ro + rl)
                    {
                        var crossSpan = source.Slice(ao + al, crossfade);
                        buffer.CopyFrom(crossSpan, bufferSize);
                        bufferSize += crossfade * ch;
                        WaveBuffer.FadeOut(buffer, -crossfade, crossfade, foFactor, ch);
                        needCrossfade = true;
                    }
                    else if (fadeout is > 0)
                    {
                        WaveBuffer.FadeOut(buffer, -fadeout, fadeout, foFactor, ch);
                    }
                    using (WaveEncoder encoder = new($"{fullPath}.wav", new(rate, ch, format)))
                    {
                        encoder.Software = nameof(LivreNoirLibrary);
                        encoder.Write(buffer.Slice(0, bufferSize));
                    }
                    ExConsole.Write($"Saved slice: {fullPath} in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds:F3}ms");
                }
                catch (Exception e)
                {
                    ExConsole.Write($"Failed to save: {fullPath} ({e.GetType()}: {e.Message})");
                }
                i++;
            }
            ExConsole.Write($"slice completed: {options.GetJsonText(false)}");
        }

        public static void SaveSliced<T>(this T wave, string directory, string baseFilename, SliceOptions options, ProgressReporter? p = null, CancellationToken c = default)
            where T : IWaveBuffer, IMarkerContainer
        {
            var baseName = PackUtils.Format(options.BasenameWithDefault, baseFilename);
            SaveSliced_General(wave, directory, baseName, options, p, c);
        }

        public static void SaveSliced<T>(this T wave, string directory, string baseFilename, IScore score, int trackId, SliceOptions options, ProgressReporter? p = null, CancellationToken c = default)
            where T : IWaveBuffer, IMarkerContainer
        {
            var baseName = PackFormat(options.BasenameWithDefault, baseFilename, score, trackId);
            SaveSliced_General(wave, directory, baseName, options, p, c);
        }
    }
}
