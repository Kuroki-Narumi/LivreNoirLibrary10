using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimingList : TimeCounterBase
    {
        private readonly DoubleKeyTimeline<Channel, KeyInfo> _key = [];
        private readonly Dictionary<Channel, BgaInfo> _bga = [];
        private readonly Dictionary<Channel, DoubleMultiTimeline<int>> _meta = [];

        public string Directory { get; private set; } = "";
        public bool AutoPlay { get; private set; }
        public BgmTimeline BgmTimeline { get; } = [];
        public IEnumerable<(Channel, double, KeyInfo)> KeyInfos => _key;
        public int NoteCount { get; private set; }

        public override void Clear()
        {
            base.Clear();
            BgmTimeline.Clear();
            _key.Clear();
            _bga.Clear();
            _meta.Clear();
            NoteCount = 0;
        }

        public void Load(IBmsViewModel source, string directory, bool autoPlay)
        {
            const float colorFactor = 1f / 255f;

            Directory = directory;
            AutoPlay = autoPlay;
            var initialTempo = source.Bpm;
            BeginInit(initialTempo);
            TimingInfoState state = new(initialTempo);
            var keyList = _key;
            var bgmList = BgmTimeline;
            var bgaList = _bga;
            var metaList = _meta;
            var lastBgmNotes = ObjectPool.Rent<Dictionary<int, SoundInfo>>();
            var lastNoteLane = ObjectPool.Rent<Dictionary<Channel, KeyInfo>>();
            var wavFilenames = ObjectPool.Rent<Dictionary<int, string>>();
            var bgaFilenames = ObjectPool.Rent<Dictionary<int, string>>();
            var count = 0;
            var firstPos = double.NaN;
            var lastPos = double.NaN;
            try
            {
                foreach (var (pos, list) in source.CurrentTimeline.EnumerateList())
                {
                    // 現在値
                    var time = state.Setup(source.GetAbsolutePosition(pos));
                    var objectExists = false;
                    foreach (var note in list.AsSpan())
                    {
                        if (state.Update(note))
                        {
                            continue;
                        }
                        var channel = note.Channel;
                        var value = (int)note.Value;
                        if (note.IsSoundLane())
                        {
                            switch (note.Type)
                            {
                                case NoteType.Normal:
                                    if (!wavFilenames.TryGetValue(value, out var path))
                                    {
                                        source.TryGetWavePath(value, directory, out _, out path);
                                        path ??= "";
                                        wavFilenames.Add(value, path);
                                    }
                                    if (lastBgmNotes.Remove(value, out var previous))
                                    {
                                        previous.Length = time - previous.Time;
                                    }
                                    SoundInfo soundInfo = new(time, channel);
                                    lastBgmNotes[value] = soundInfo;
                                    if (!string.IsNullOrEmpty(path))
                                    {
                                        objectExists = true;
                                        if (double.IsNaN(firstPos))
                                        {
                                            firstPos = time;
                                        }
                                        bgmList.Add(path, soundInfo);
                                    }
                                    if (channel.IsKey())
                                    {
                                        if (!keyList.TryGetValue(channel, time, SearchMode.Equal, out _, out var keyInfo))
                                        {
                                            keyInfo = new(time);
                                            keyList.Set(channel, time, keyInfo);
                                            lastNoteLane[channel] = keyInfo;
                                            count++;
                                        }
                                    }
                                    break;
                                case NoteType.Mine:
                                    if (channel.IsKey())
                                    {
                                        keyList.Set(channel, time, new(time, true));
                                    }
                                    break;
                                case NoteType.LongEnd:
                                    if (lastNoteLane.Remove(channel, out var info))
                                    {
                                        info.Length = time - info.Time;
                                    }
                                    break;
                            }
                        }
                        else if (note.IsBga())
                        {
                            if (!bgaFilenames.TryGetValue(value, out var path))
                            {
                                source.TryGetMediaPath(value, directory, out _, out path);
                                path ??= "";
                                bgaFilenames.Add(value, path);
                            }
                            objectExists = true;
                            bgaList.GetOrAdd(channel).Layer.Set(time, path);
                        }
                        else if (channel.IsArgb())
                        {
                            if (TupleStringConverter.TryConvertFromString<int, int, int, int>(source.GetDefValue(DefType.Argb, (int)note.Value), out var tuple))
                            {
                                var item = bgaList.GetOrAdd(channel.ToBga());
                                item.Rgb.Set(time, new(tuple.Item2 * colorFactor, tuple.Item3 * colorFactor, tuple.Item4 * colorFactor));
                                item.Opacity.Set(time, tuple.Item1 * colorFactor);
                            }
                        }
                        else if (channel.IsOpacity())
                        {
                            bgaList.GetOrAdd(channel.ToBga()).Opacity.Set(time, value * colorFactor);
                        }
                        else
                        {
                            metaList.GetOrAdd(channel).Add(time, value);
                        }
                    }
                    if (objectExists)
                    {
                        lastPos = time;
                    }
                    ApplyTimeInfo(ref state);
                }
                FirstSoundTime = firstPos;
                LastSoundTime = lastPos;
                EndInit(ref state);
                // 小節線
                foreach (var (_, head, length) in source.EnumerateBars())
                {
                    var time = Beat2Time(head);
                    //barList.Set(time, (head, length));
                    keyList.Set(Channel.Bar, time, new(time, false));
                }
                NoteCount = count;
            }
            finally
            {
                ObjectPool.Return(lastBgmNotes);
                ObjectPool.Return(lastNoteLane);
                ObjectPool.Return(wavFilenames);
                ObjectPool.Return(bgaFilenames);
            }
        }

        public bool TryGetBgaLayer(Channel channel, double time, out double start, [MaybeNullWhen(false)] out string path)
        {
            start = default;
            path = default;
            return _bga.TryGetValue(channel, out var bga) &&
                bga.Layer.TryGetValue(time, SearchMode.PreviousOrEqual, out start, out path);
        }

        public bool TryGetColorCorrection(Channel channel, double time, out Vector<float> vector)
        {
            if (_bga.TryGetValue(channel, out var bga))
            {
                var opacity = bga.Opacity.TryGetValue(time, SearchMode.PreviousOrEqual, out _, out var a);
                if (bga.Rgb.TryGetValue(time, SearchMode.PreviousOrEqual, out _, out var rgb))
                {
                    var (r, g, b) = rgb;
                    vector = VectorUtils.CreateRepeating([b, g, r, a]);
                    return true;
                }
                vector = VectorUtils.CreateRepeating([1, 1, 1, a]);
                return opacity;
            }
            vector = default;
            return false;
        }
        private class BgaInfo
        {
            public DoubleTimeline<string> Layer { get; } = [];
            public DoubleTimeline<Rgb> Rgb { get; } = [];
            public DoubleTimeline<float> Opacity { get; } = [];
        }

        private readonly record struct Rgb(float R, float G, float B);
    }

    public record SoundInfo(double Time, Channel Channel)
    {
        public double Length { get; internal set; } = -1;
        public bool IsKey => Channel.IsKey();
    }

    public class KeyInfo(double time, bool isMine = false)
    {
        public double Time { get; } = time;
        public double Length { get; internal set; }
        public bool IsMine { get; } = isMine;
    }
}
