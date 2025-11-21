using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimingList : TimeCounterBase
    {
        private readonly SortedDictionary<int, List<SoundInfo>> _bgm = [];
        private readonly LongKeyTimeline<Channel, KeyInfo> _key = [];
        private readonly Dictionary<Channel, LongTimeline<string>> _bga = [];
        private readonly Dictionary<Channel, LongMultiTimeline<int>> _meta = [];

        public string Directory { get; private set; } = "";
        public bool AutoPlay { get; private set; }
        public IEnumerable<(Channel, long, KeyInfo)> KeyInfos => _key;

        public override void Clear()
        {
            base.Clear();
            _key.Clear();
            _bgm.Clear();
            _bga.Clear();
            _meta.Clear();
        }

        public void Load(IBmsViewModel source, string directory, bool autoPlay)
        {
            Clear();
            Directory = directory;
            AutoPlay = autoPlay;
            var initialTempo = source.Bpm;
            InitializeTimeInfo(initialTempo);
            TimingInfoState state = new(initialTempo);
            var keyList = _key;
            var bgmList = _bgm;
            var bgaList = _bga;
            var metaList = _meta;
            var lastNoteLane = ObjectPool.Rent<Dictionary<Channel, KeyInfo>>();
            var wavFilenames = ObjectPool.Rent<Dictionary<int, string>>();
            var bgaFilenames = ObjectPool.Rent<Dictionary<int, string>>();
            try
            {
                foreach (var (pos, list) in source.CurrentTimeline.EnumerateList())
                {
                    // テンポが正でない場合は終了
                    if (state.IsInvalidTempo)
                    {
                        break;
                    }
                    // 現在値
                    var (time, tick) = state.Setup(source.GetAbsolutePosition(pos));
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
                                    if (bgmList.TryGetValue(value, out var bList))
                                    {
                                        bList[0].Length = time - bList[0].Time;
                                    }
                                    else
                                    {
                                        bList = [];
                                        bgmList.Add(value, bList);
                                    }
                                    SoundInfo soundInfo = new(time, channel, path);
                                    bList.Add(soundInfo);
                                    if (channel is > 0)
                                    {
                                        if (!keyList.TryGetValue(channel, tick, SearchMode.Equal, out _, out var keyInfo))
                                        {
                                            keyInfo = new(time);
                                            keyList.Set(channel, tick, keyInfo);
                                            lastNoteLane[channel] = keyInfo;
                                        }
                                        keyInfo.Sounds.Add(soundInfo);
                                    }
                                    break;
                                case NoteType.Mine:
                                    if (channel is > 0)
                                    {
                                        keyList.Set(channel, tick, new(time, true));
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
                            bgaList.GetOrAdd(channel).Set(tick, path);
                        }
                        else
                        {
                            metaList.GetOrAdd(channel).Add(tick, value);
                        }
                    }
                    ApplyTimeInfo(ref state);
                }
                // 小節線
                foreach (var (_, head, length) in source.EnumerateBars())
                {
                    var time = Beat2Time(head);
                    keyList.Set(Channel.Bar, TimeUtils.Seconds2Ticks(time), new(time, false) { Length = length });
                }
            }
            finally
            {
                ObjectPool.Return(lastNoteLane);
                ObjectPool.Return(wavFilenames);
                ObjectPool.Return(bgaFilenames);
            }
        }

        public bool TryGetBgaInfo(Channel channel, long tick, out long startTick, [MaybeNullWhen(false)] out string path)
        {
            startTick = default;
            path = default;
            return _bga.TryGetValue(channel, out var timeline) &&
                timeline.TryGetValue(tick, SearchMode.PreviousOrEqual, out startTick, out path);
        }

        public class SoundInfo(double time, Channel channel, string path)
        {
            public double Time { get; } = time;
            public double Length { get; internal set; } = -1;
            public Channel Channel { get; } = channel;
            public string Path { get; } = path;
            public bool IsKey => Channel is > 0;
        }

        public class KeyInfo(double time, bool isMine = false)
        {
            public double Time { get; } = time;
            public double Length { get; internal set; }
            public bool IsMine { get; } = isMine;
            public List<SoundInfo> Sounds { get; } = [];
        }
    }
}
