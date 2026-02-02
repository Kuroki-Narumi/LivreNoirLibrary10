using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimingList : TimeCounterBase
    {
        private readonly DoubleKeyTimeline<Channel, KeyInfo> _key = [];
        private readonly Dictionary<Channel, BgaInfo> _bga = [];
        private readonly Dictionary<Channel, DoubleMultiTimeline<int>> _meta = [];

        public string Directory { get; set; } = "";
        public BgmTimeline BgmTimeline { get; } = [];
        public IEnumerable<(Channel Channel, double Position, KeyInfo Info)> KeyInfos => _key;
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

        public override void Load(IBmsViewModel source)
        {
            const float colorFactor = 1f / 255f;

            var directory = Directory;
            var initialTempo = source.Bpm;
            BeginInit(initialTempo);
            TimingInfoState state = new(initialTempo);
            var keyList = _key;
            var bgmList = BgmTimeline;
            var bgaList = _bga;
            var metaList = _meta;
            using var obj1 = ObjectPool.Rent<Dictionary<int, SoundInfo>>();
            using var obj2 = ObjectPool.Rent<Dictionary<Channel, KeyInfo>>();
            using var obj3 = ObjectPool.Rent<Dictionary<int, string>>();
            using var obj4 = ObjectPool.Rent<Dictionary<int, string>>();
            var lastBgmNotes = obj1.Value;
            var lastNoteLane = obj2.Value;
            var wavFilenames = obj3.Value;
            var bgaFilenames = obj4.Value;
            var count = 0;
            foreach (var (pos, list) in source.CurrentTimeline.EnumerateList())
            {
                // 現在値
                var time = state.Setup(source.GetAbsolutePosition(pos));
                var isValidTempo = state.CurrentTempo is > 0;
                var position = state.CurrentPosition;
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
                        var type = note.Type;
                        var actualTime = isValidTempo ? time : double.PositiveInfinity;
                        switch (type)
                        {
                            case NoteType.Normal:
                                if (isValidTempo)
                                {
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
                                        state.UpdateFirstTime();
                                        bgmList.Add(path, soundInfo);
                                    }
                                }
                                if (UpdateKey(keyList, channel, actualTime, position, type, out var keyInfo))
                                {
                                    lastNoteLane[channel] = keyInfo;
                                    count++;
                                }
                                break;
                            case NoteType.Mine:
                            case NoteType.Invisible:
                                UpdateKey(keyList, channel, actualTime, position, type, out _);
                                break;
                            case NoteType.LongEnd:
                                if (lastNoteLane.Remove(channel, out var info))
                                {
                                    if (isValidTempo)
                                    {
                                        info.TimeLength = time - info.Time;
                                    }
                                    info.VisualLength = position - info.Position;
                                }
                                break;
                        }
                    }
                    else if (isValidTempo)
                    {
                        if (note.IsBga())
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
                }
                if (objectExists)
                {
                    state.UpdateLastTime();
                }
                ApplyTimeInfo(ref state);
            }
            EndInit(ref state);
            // 小節線
            foreach (var (_, head, length) in source.EnumerateBars())
            {
                var time = Beat2Time(head);
                var pos = Beat2Position(head);
                //barList.Set(time, (head, length));
                keyList.Set(Channel.Bar, pos, new(time, pos, NoteType.Invalid));
            }
            NoteCount = count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool UpdateKey(DoubleKeyTimeline<Channel, KeyInfo> keyList, Channel channel, double time, double position, NoteType type, [MaybeNullWhen(false)] out KeyInfo keyInfo)
        {
            if (channel.IsKey())
            {
                if (keyList.TryGetValue(channel, position, SearchMode.Equal, out _, out keyInfo))
                {
                    if (keyInfo.Type > type)
                    {
                        keyInfo.Type = type;
                        return true;
                    }
                }
                else
                {
                    keyInfo = new(time, position, type);
                    keyList.Set(channel, position, keyInfo);
                    return true;
                }
            }
            keyInfo = null;
            return false;
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

    public class KeyInfo(double time, double position, NoteType type)
    {
        public double Time { get; } = time;
        public double Position { get; } = position;
        public double TimeLength { get; internal set; }
        public double VisualLength { get; internal set; }
        public NoteType Type { get; internal set; } = type;
    }
}
