using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimingList : ITimeCounter
    {
        private readonly List<decimal> _beat_list = [];
        private readonly List<TimingInfo> _beat_item_list = [];
        private readonly List<decimal> _time_list = [];
        private readonly List<TimingInfo> _time_item_list = [];

        private readonly SortedDictionary<int, List<SoundInfo>> _bgm = [];
        private readonly LongKeyTimeline<int, KeyInfo> _key = [];
        private readonly Dictionary<Channel, LongTimeline<string>> _bga = [];
        private readonly Dictionary<Channel, LongMultiTimeline<int>> _meta = [];

        public string Directory { get; private set; } = "";
        public bool AutoPlay { get; private set; }
        public IEnumerable<(int, long, KeyInfo)> KeyInfos => _key;

        public void Clear()
        {
            _beat_list.Clear();
            _beat_item_list.Clear();
            _time_list.Clear();
            _time_item_list.Clear();
            _key.Clear();
            _bgm.Clear();
            _bga.Clear();
            _meta.Clear();
        }

        public void Load(IBmsData data, string directory, bool autoPlay)
        {
            Clear();
            Directory = directory;
            AutoPlay = autoPlay;
            // initialize timing info
            var tempo = (decimal)data.Bpm;

            var timeReferenceBeat = 0m;
            var timeReference = 0m;
            var timePerBeat = 240 / tempo;

            var positionReference = 0m;
            var scroll = 1m;

            TimingInfo timingInfo = new(0, 0, 0, tempo, scroll, 0);
            var beatList = _beat_list;
            var beatItemList = _beat_item_list;
            var timeList = _time_list;
            var timeItemList = _time_item_list;
            beatList.Add(0);
            beatItemList.Add(timingInfo);
            timeList.Add(0);
            timeItemList.Add(timingInfo);

            var keyList = _key;
            var bgmList = _bgm;
            var bgaList = _bga;
            var metaList = _meta;
            var lastNoteLane = ObjectPool.Rent<Dictionary<int, KeyInfo>>();
            var wavFilenames = ObjectPool.Rent<Dictionary<int, string>>();
            var bgaFilenames = ObjectPool.Rent<Dictionary<int, string>>();
            try
            {
                foreach (var (pos, list) in data.Timeline.EachList())
                {
                    // テンポが正でない場合は終了
                    if (tempo is <= 0)
                    {
                        break;
                    }
                    // 現在値
                    var beat = (decimal)data.GetAbsolutePosition(pos);
                    var time = timeReference + (beat - timeReferenceBeat) * timePerBeat;
                    var tick = TimeUtils.Seconds2Ticks(time);
                    var position = positionReference + (beat - timeReferenceBeat) * scroll;
                    // テンポ変化フラグ
                    var tempoExists = false;
                    var newTempo = 0m;
                    var totalStop = 0m;
                    var scrollExists = false;
                    var newScroll = 0m;
                    foreach (var note in CollectionsMarshal.AsSpan(list))
                    {
                        switch (note)
                        {
                            case IConductorNote conductor:
                                switch (conductor.Channel)
                                {
                                    case Channel.Bpm:
                                        tempoExists = true;
                                        newTempo = conductor.Value;
                                        break;
                                    case Channel.Stop:
                                        totalStop += conductor.Value;
                                        break;
                                    case Channel.Scroll:
                                        scrollExists = true;
                                        newScroll = conductor.Value;
                                        break;
                                    case Channel.Speed:
                                        // ハイスピ変更はメタノート扱いとする
                                        // %単位に変換、0.01未満の変化量は無視される
                                        AddMeta(tick, new MetaNote(Channel.Speed, (short)Math.Clamp(conductor.DoubleValue * 100, 1, 10000)));
                                        break;
                                }
                                break;
                            case ISoundNote sound:
                                var lane = sound.Lane;
                                switch (sound.Type)
                                {
                                    case NoteType.Normal:
                                        var value = sound.Value;
                                        if (!wavFilenames.TryGetValue(value, out var path))
                                        {
                                            data.TryGetWavePath(value, directory, out _, out path);
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
                                        SoundInfo soundInfo = new(time, lane, path);
                                        bList.Add(soundInfo);
                                        if (lane is > 0)
                                        {
                                            if (!keyList.TryGet(lane, tick, SearchMode.Equal, out _, out var keyInfo))
                                            {
                                                keyInfo = new(time);
                                                keyList.Set(lane, tick, keyInfo);
                                                lastNoteLane[lane] = keyInfo;
                                            }
                                            keyInfo.Sounds.Add(soundInfo);
                                        }
                                        break;
                                    case NoteType.Mine:
                                        if (lane is > 0)
                                        {
                                            keyList.Set(lane, tick, new(time, true));
                                        }
                                        break;
                                    case NoteType.LongEnd:
                                        if (lastNoteLane.TryGetValue(lane, out var info))
                                        {
                                            info.Length = time - info.Time;
                                            lastNoteLane.Remove(lane);
                                        }
                                        break;
                                }
                                break;
                            case IMetaNote meta:
                                AddMeta(tick, meta);
                                break;
                        }
                    }
                    var tempoChanged = tempoExists && newTempo != tempo;
                    var scrollChanged = scrollExists && newScroll != scroll;
                    var stopExists = totalStop is not 0;
                    if (tempoChanged || scrollChanged || stopExists)
                    {
                        if (tempoChanged)
                        {
                            tempo = newTempo;
                            timePerBeat = 240 / tempo;
                        }
                        if (scrollChanged)
                        {
                            scroll = newScroll;
                        }
                        var stop = totalStop * Constants.StopUnit * timePerBeat;
                        timingInfo = new(beat, position, time, tempo, scroll, stop);
                        // beat to info
                        if (beat is 0)
                        {
                            beatItemList[0] = timingInfo;
                        }
                        else
                        {
                            beatList.Add(beat);
                            beatItemList.Add(timingInfo);
                        }
                        // time to info
                        if (time is 0)
                        {
                            timeItemList[0] = timingInfo;
                        }
                        else
                        {
                            timeList.Add(time);
                            timeItemList.Add(timingInfo);
                        }
                        if (stopExists)
                        {
                            time += stop;
                            timingInfo = new(beat, position, time, tempo, scroll, stop);
                            timeList.Add(time);
                            timeItemList.Add(timingInfo);
                        }
                        timeReferenceBeat = beat;
                        timeReference = time;
                        positionReference = position;
                    }
                }

                void AddMeta(long tick, IMetaNote note)
                {
                    var channel = note.Channel;
                    var value = note.Value;
                    if (note.Channel.IsBga())
                    {
                        if (!bgaFilenames.TryGetValue(value, out var path))
                        {
                            data.TryGetMediaPath(value, directory, out _, out path);
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
            }
            finally
            {
                lastNoteLane.Clear();
                wavFilenames.Clear();
                bgaFilenames.Clear();
                ObjectPool.Return(lastNoteLane);
                ObjectPool.Return(wavFilenames);
                ObjectPool.Return(bgaFilenames);
            }
        }

        public List<TempoInfo<decimal>> GetTempoInfos()
        {
            List<TempoInfo<decimal>> list = [];
            var seconds = _time_list;
            var items = _time_item_list;
            var c = seconds.Count;
            for (int i = 1; i < c; i++)
            {
                var curSec = seconds[i - 1];
                var nextSec = seconds[i];
                var item = items[i - 1];
                list.Add(new(item.Tempo, curSec, nextSec));
            }
            list.Add(new(items[^1].Tempo, seconds[^1], -1, true));
            return list;
        }

        public decimal Beat2Time(decimal beat)
        {
            var index = _beat_list.BinarySearch(beat);
            if (index is >= 0)
            {
                return _beat_item_list[index].Time;
            }
            else
            {
                index = Math.Max(~index - 1, 0);
                var beatReference = _beat_list[index];
                var item = _beat_item_list[index];
                return item.Time + item.Stop + (beat - beatReference) * item.SecondsPerBeat;
            }
        }

        public bool TryGetTime2Info(decimal time, out TimingInfo info)
        {
            var index = _time_list.BinarySearch(time);
            var found = index is >= 0;
            if (!found)
            {
                index = Math.Max(~index - 1, 0);
            }
            info = _time_item_list[index];
            return found;
        }

        public decimal Time2Tempo(decimal time)
        {
            TryGetTime2Info(time, out var info);
            return info.Tempo;
        }

        public decimal Time2Beat(decimal time)
        {
            if (TryGetTime2Info(time, out var info))
            {
                return info.Beat;
            }
            else
            {
                return info.Beat + (time - info.Time) * info.BeatsPerSecond;
            }
        }

        public decimal Time2Position(decimal time)
        {
            if (TryGetTime2Info(time, out var info))
            {
                return info.Position;
            }
            else
            {
                return info.Position + (time - info.Time) * info.BeatsPerSecond * info.Scroll;
            }
        }

        public bool TryGetBgaInfo(Channel channel, long tick, out long startTick, [MaybeNullWhen(false)] out string path)
        {
            startTick = default;
            path = default;
            return _bga.TryGetValue(channel, out var timeline) &&
                timeline.TryGet(tick, SearchMode.PreviousOrEqual, out startTick, out path);
        }

        public class TimingInfo(decimal beat, decimal position, decimal time, decimal tempo, decimal scroll, decimal stop)
        {
            /// <summary>
            /// 拍数基準の絶対位置
            /// </summary>
            public decimal Beat { get; } = beat;
            /// <summary>
            /// 描画位置
            /// </summary>
            public decimal Position { get; } = position;
            /// <summary>
            /// 絶対時刻
            /// </summary>
            public decimal Time { get; } = time;
            public decimal Tempo { get; } = tempo;
            public decimal Scroll { get; } = scroll;
            public decimal Stop { get; } = stop;
            public decimal SecondsPerBeat { get; } = 240 / tempo;
            public decimal BeatsPerSecond { get; } = tempo / 240;
        }

        public class SoundInfo(decimal time, int lane, string path)
        {
            public decimal Time { get; } = time;
            public decimal Length { get; internal set; } = -1;
            public int Lane { get; } = lane;
            public string Path { get; } = path;
            public bool IsKey => Lane is > 0;
        }

        public class KeyInfo(decimal time, bool isMine = false)
        {
            public decimal Time { get; } = time;
            public decimal Length { get; internal set; }
            public bool IsMine { get; } = isMine;
            public List<SoundInfo> Sounds { get; } = [];
        }
    }
}
