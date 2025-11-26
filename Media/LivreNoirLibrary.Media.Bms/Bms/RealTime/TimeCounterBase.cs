using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private TempoInfo? _lastTempoInfo;
        private readonly List<int> _tempoList = [];
        private readonly List<TempoInfo> _tempoInfoList = [];

        public double MinTempo => _tempoList.Count is 0 ? -1 : _tempoList[0];
        public double MaxTempo => _tempoList.Count is 0 ? -1 : _tempoList[^1];
        public double MainTempo => _tempoList.Count is 0 ? -1 : SortedList.MaxKeyBy(_tempoList, _tempoInfoList, value => value.BeatLength);
        public double MainTimeTempo => _tempoList.Count is 0 ? -1 : SortedList.MaxKeyBy(_tempoList, _tempoInfoList, value => value.TimeLength);

        public virtual void Clear()
        {
            _beatList.Clear();
            _beatItemList.Clear();
            _timeList.Clear();
            _timeItemList.Clear();
            _speedTimeList.Clear();
            _speedValueList.Clear();
            _tempoList.Clear();
            _tempoInfoList.Clear();
        }

        public void BeginInit(double initialTempo)
        {
            Clear();

            TimingInfo timingInfo = new(0, 0, 0, initialTempo, 0, 1);
            _beatList.Add(0);
            _beatItemList.Add(timingInfo);
            _timeList.Add(0);
            _timeItemList.Add(timingInfo);

            _speedTimeList.Add(0);
            _speedValueList.Add(1);

            _lastTempoInfo = new();
            _tempoList.Add((int)initialTempo);
            _tempoInfoList.Add(_lastTempoInfo);
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

                var tempo = (int)Math.Max(state.CurrentTempo, 0);
                _lastTempoInfo?.Add(beat, time);
                _lastTempoInfo = SortedList.GetOrAdd(_tempoList, _tempoInfoList, tempo);

                var stop = info.Stop;
                if (stop is not 0)
                {
                    _timeItemList[^1] = _timeItemList[^1].AsStop();
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

        public void EndInit(ref TimingInfoState state)
        {
            _lastTempoInfo?.Add(state.CurrentBeat, state.CurrentTime);
            _lastTempoInfo = null;
            SortedList.Remove(_tempoList, _tempoInfoList, 0);
            ExConsole.Write($"Min={MinTempo}bpm, Max={MaxTempo}bpm, Main={MainTempo}bpm, MainTime={MainTimeTempo}bpm");
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

        public double GetHighSpeed(double time) => SortedList.TryGetValue(_speedTimeList, _speedValueList, time, out var value) ? value : 1;

        private class TempoInfo
        {
            public double LastBeat { get; set; }
            public double LastTime { get; set; }
            public double BeatLength { get; set; }
            public double TimeLength { get; set; }

            public TempoInfo Init(double beat, double time)
            {
                LastBeat = beat;
                LastTime = time;
                return this;
            }

            public void Add(double beat, double time)
            {
                BeatLength += beat - LastBeat;
                TimeLength += time - LastTime;
            }
        }
    }
}
