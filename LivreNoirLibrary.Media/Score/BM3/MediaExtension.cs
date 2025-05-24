using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public static partial class MediaExtension
    {
        public static void ExtendConductor(this BaseData data, IScore score, Rational length)
        {
            var bars = data.Bars;
            bars.Clear();
            data.Timeline.RemoveIf((_, item) => item.IsTempo() || item.IsStop());
            TempoTimeline conductor = new(score);
            data.Bpm = conductor.GetBpmM(Rational.Zero);
            var @enum = conductor.GetEnumerator();
            var exists = @enum.MoveNext();
            foreach (var info in score.EachBar(length))
            {
                var head = info.Head;
                var len = info.Length;
                var number = info.Number;
                bars.Set(number, len);
                var limit = head + len;
                while (exists)
                {
                    var (pos, tempo) = @enum.Current;
                    if (pos >= limit) { break; }
                    var bpm = MediaUtils.MicroSeconds2BpmM(tempo);
                    if (pos.IsZero())
                    {
                        data.Bpm = bpm;
                    }
                    else
                    {
                        BarPosition p = new(number, pos - head);
                        data.AddTempo(p, (Rational)bpm);
                    }
                    exists = @enum.MoveNext();
                }
            }
        }

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
            foreach (var (pos, obj) in CollectionsMarshal.AsSpan(remove))
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
                    n = Marker.IgnoreName;
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
            where T : IWaveBuffer, IMarker
        {
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
                offset += wave.FindFirstSound(WaveBuffer.Level2Value(options.CutoffLeft));
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
                    var samples = (seconds * sampleRate).RoundToInt() + offset;
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
            if (options.MoveToMinimum)
            {
                wave.MoveMarkerToMinimum();
            }
            return true;
        }

        public static void SaveSliced_General(WaveBuffer source, string directory, string baseName, SliceOptions options, ProgressReporter? p, CancellationToken c)
        {
            var cLeft = WaveBuffer.Level2Value(options.CutoffLeft);
            var mLeft = (int)options.MarginLeft;
            var fadein = (int)options.FadeIn;
            var cRight = WaveBuffer.Level2Value(options.CutoffRight);
            var mRight = (int)options.MarginRight;
            var fadeout = (int)options.FadeOut;
            var crossfade = (int)options.CrossFade;
            var needCrossfade = false;
            var format = options.Format;
            if (!format.IsValid())
            {
                format = source is WaveData wd ? wd.SampleFormat : WaveEncodeOptions.DefaultFormat;
            }
            var rate = source.SampleRate;
            var ch = source.Channels;
            double i = 0;
            double max = source.GetSliceCount();
            using UnmanagedSharedBuffer<float> buffer = new(rate * ch);
            foreach (var slice in source.EachSlice())
            {
                c.ThrowIfCancellationRequested();
                var fullPath = Path.GetFullPath($"{baseName}{slice.Name}", directory);
                p?.ReportFraction(i, max);
                try
                {
                    var t0 = Stopwatch.GetTimestamp();
                    var (srcSlice, ro, rl, ao, al) = source.SliceWithCutSilence((int)slice.Offset, (int)slice.Length, cLeft, cRight, mLeft, mRight);
                    buffer.SetData(srcSlice);
                    if (needCrossfade)
                    {
                        WaveBuffer.FadeIn(buffer, 0, crossfade, 0.5f, ch);
                        needCrossfade = false;
                    }
                    else if (fadein is > 0)
                    {
                        WaveBuffer.FadeIn(buffer, 0, fadein, 0.5f, ch);
                    }
                    if (crossfade is > 0 && ao + al >= ro + rl)
                    {
                        buffer.Append(source.Slice(ao + al, crossfade));
                        WaveBuffer.FadeOut(buffer, -crossfade, crossfade, 0.5f, ch);
                        needCrossfade = true;
                    }
                    else if (fadeout is > 0)
                    {
                        WaveBuffer.FadeOut(buffer, -fadeout, fadeout, 0.5f, ch);
                    }
                    using (WaveEncoder encoder = new($"{fullPath}.wav", new(rate, ch, format)))
                    {
                        encoder.Software = nameof(LivreNoirLibrary);
                        encoder.Write(buffer);
                    }
                    ExConsole.Write($"Saved slice: {fullPath} in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds:F3}ms");
                }
                catch (Exception e)
                {
                    ExConsole.Write($"Failed to save: {fullPath} ({e.GetType()}: {e.Message})");
                }
                i++;
            }
        }

        public static void SaveSliced(this WaveBuffer wave, string directory, string baseFilename, SliceOptions options, ProgressReporter? p = null, CancellationToken c = default)
        {
            var baseName = PackUtils.Format(options.BasenameWithDefault, baseFilename);
            SaveSliced_General(wave, directory, baseName, options, p, c);
        }

        public static void SaveSliced(this WaveBuffer wave, string directory, string baseFilename, IScore score, int trackId, SliceOptions options, ProgressReporter? p = null, CancellationToken c = default)
        {
            var baseName = PackUtils.Format(options.BasenameWithDefault, baseFilename, score, trackId);
            SaveSliced_General(wave, directory, baseName, options, p, c);
        }
    }
}
