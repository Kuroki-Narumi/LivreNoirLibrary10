using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounterBase : ITimeCounter
    {
        private readonly List<double> _beatList = [];
        private readonly List<TimingInfo> _beatItemList = [];
        private readonly List<double> _timeList = [];
        private readonly List<TimingInfo> _timeItemList = [];
        private readonly List<double> _speedTimeList = [];
        private readonly List<double> _speedValueList = [];

        public virtual void Clear()
        {
            _beatList.Clear();
            _beatItemList.Clear();
            _timeList.Clear();
            _timeItemList.Clear();
            _speedTimeList.Clear();
            _speedValueList.Clear();
        }

        public void InitializeTimeInfo(double initialTempo)
        {
            _beatList.Clear();
            _beatItemList.Clear();
            _timeList.Clear();
            _timeItemList.Clear();

            TimingInfo timingInfo = new(0, 0, 0, initialTempo, 0, 1);
            _beatList.Add(0);
            _beatItemList.Add(timingInfo);
            _timeList.Add(0);
            _timeItemList.Add(timingInfo);

            _speedTimeList.Add(0);
            _speedValueList.Add(1);
        }

        public void ApplyTimeInfo(ref TimingInfoState state)
        {
            if (state.Finalize(out var info, out var speedChanged, out var newSpeed))
            {
                // beat to info
                var beat = info.Beat;
                if (beat is 0)
                {
                    _beatItemList[0] = info;
                }
                else
                {
                    _beatList.Add(beat);
                    _beatItemList.Add(info);
                }

                // time to info
                var time = info.Time;
                if (time is 0)
                {
                    _timeItemList[0] = info;
                }
                else
                {
                    _timeList.Add(time);
                    _timeItemList.Add(info);
                }

                var stop = info.Stop;
                if (stop is not 0)
                {
                    time += stop;
                    info = new(beat, info.Position, time, info.Tempo, 0, info.Scroll);
                    _timeList.Add(time);
                    _timeItemList.Add(info);
                }
            }
            if (speedChanged)
            {
                var time = state.CurrentTime;
                if (time is 0)
                {
                    _speedValueList[0] = newSpeed;
                }
                else
                {
                    _speedTimeList.Add(time);
                    _speedValueList.Add(newSpeed);
                }
            }
        }

        public List<TempoInfo<double>> GetTempoInfos(List<TempoInfo<double>>? list = null)
        {
            list ??= [];
            var seconds = _timeList;
            var items = _timeItemList;
            var c = seconds.Count;
            for (var i = 1; i < c; i++)
            {
                var curSec = seconds[i - 1];
                var nextSec = seconds[i];
                var item = items[i - 1];
                list.Add(new(item.Tempo, curSec, nextSec));
            }
            list.Add(new(items[^1].Tempo, seconds[^1], -1, true));
            return list;
        }

        public double Beat2Time(double absolutePosition)
        {
            var index = _beatList.BinarySearch(absolutePosition);
            if (index is >= 0)
            {
                return _beatItemList[index].Time;
            }
            else
            {
                index = Math.Max(~index - 1, 0);
                var beatReference = _beatList[index];
                var item = _beatItemList[index];
                return item.Time + item.Stop + (absolutePosition - beatReference) * item.SecondsPerBeat;
            }
        }

        public bool TryGetTime2Info(double time, out TimingInfo info)
        {
            var index = _timeList.BinarySearch(time);
            var found = index is >= 0;
            if (!found)
            {
                index = Math.Max(~index - 1, 0);
            }
            info = _timeItemList[index];
            return found;
        }

        public double Time2Tempo(double time)
        {
            TryGetTime2Info(time, out var info);
            return info.Tempo;
        }

        public double Time2Beat(double time)
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

        public double Time2Position(double time)
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

        public double GetHighSpeed(double time) => Collections.SortedList.TryGetValue(_speedTimeList, _speedValueList, time, out var value) ? value : 1;
    }
}
