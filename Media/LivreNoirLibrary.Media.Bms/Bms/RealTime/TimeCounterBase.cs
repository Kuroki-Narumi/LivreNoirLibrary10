using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class TimeCounterBase : ITimeCounter
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
        public double AverageTempo { get; private set; }
        public double MainTempo => _tempoList.Count is 0 ? -1 : SortedList.MaxKeyBy(_tempoList, _tempoInfoList, value => value.BeatLength);
        public double MainTimeTempo => _tempoList.Count is 0 ? -1 : SortedList.MaxKeyBy(_tempoList, _tempoInfoList, value => value.TimeLength);
        public double FirstSoundTime { get; private set; }
        public double LastSoundTime { get; private set; }

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

        public abstract void Load(IBmsViewModel vm);

        protected void BeginInit(double initialTempo)
        {
            Clear();

            var timingInfo = TimingInfo.Create(initialTempo);
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

        protected void ApplyTimeInfo(ref TimingInfoState state)
        {
            var (tempoChanged, info, speedChanged, newSpeed) = state.Finalize();
            if (tempoChanged)
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

                var tempo = state.CurrentTempo;
                if (tempo is > 0)
                {
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

                    _lastTempoInfo?.Add(beat, time);
                    _lastTempoInfo = SortedList.GetOrAdd(_tempoList, _tempoInfoList, (int)tempo).Init(beat, time);

                    if (info.StopTime is not 0)
                    {
                        (var before, info) = info.SplitStop();
                        _timeItemList[^1] = before;
                        _timeList.Add(info.Time);
                        _timeItemList.Add(info);
                    }
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

        protected void EndInit(ref TimingInfoState state)
        {
            _lastTempoInfo?.Add(state.CurrentBeat, state.CurrentTime);
            _lastTempoInfo = null;
            SortedList.Remove(_tempoList, _tempoInfoList, 0);
            var num = 0d;
            var den = 0d;
            foreach (var (tempo, item) in SortedList.GetEnumerator(_tempoList, _tempoInfoList))
            {
                var time = item.TimeLength;
                num += time * tempo;
                den += time;
            }
            AverageTempo = den is 0 ? 0 : (num / den).RoundToInt();
            FirstSoundTime = state.FirstTime.Validate(0);
            LastSoundTime = state.LastTime.Validate(0);
            ExConsole.Write($"Min={MinTempo}bpm, Max={MaxTempo}bpm, Avg={AverageTempo}bpm, Main={MainTempo}bpm, MainTime={MainTimeTempo}bpm, FirstSound={FirstSoundTime}, LastSound={LastSoundTime}");
        }

        public bool TryGetBeat2Info(double absolutePosition, out TimingInfo info)
        {
            var index = _beatList.BinarySearch(absolutePosition);
            var found = index is >= 0;
            if (!found)
            {
                index = Math.Max(~index - 1, 0);
            }
            info = _beatItemList[index];
            return found;
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

        public double Beat2Time(double absolutePosition)
        {
            return TryGetBeat2Info(absolutePosition, out var info)
                ? info.Time
                : info.Time + info.StopTime + (absolutePosition - info.Beat) * info.SecondsPerBeat;
        }

        public double Beat2Position(double absolutePosition)
        {
            return TryGetBeat2Info(absolutePosition, out var info) 
                ? info.Position 
                : info.Position + (absolutePosition - info.Beat) * info.Scroll;
        }

        public double Time2Tempo(double time)
        {
            TryGetTime2Info(time, out var info);
            return info.Tempo;
        }

        public double Time2Beat(double time)
        {
            return TryGetTime2Info(time, out var info) 
                ? info.Beat 
                : info.Beat + (time - info.Time) * info.BeatsPerSecond;
        }

        public double Time2Position(double time)
        {
            return TryGetTime2Info(time, out var info) 
                ? info.Position 
                : info.Position + (time - info.Time) * info.BeatsPerSecond * info.Scroll;
        }

        public double GetHighSpeed(double time) => SortedList.TryGetValue(_speedTimeList, _speedValueList, time, out var value) ? value : 1;

        private class TempoInfo
        {
            public double LastBeat { get; private set; }
            public double LastTime { get; private set; }
            public double BeatLength { get; private set; }
            public double TimeLength { get; private set; }

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

        public string GetTimingInfoText()
        {
            using var o = ObjectPool.Rent<StringBuilder>();
            var sb = o.Value;
            sb.AppendLine("Beat\tTime\tPosition\tTempo\tStopTime\tScroll\tSpB\tBpS");
            foreach (var item in _timeItemList)
            {
                sb.AppendLine($"{item.Beat}\t{item.Time}\t{item.Position}\t{item.Tempo}\t{item.StopTime}\t{item.Scroll}\t{item.SecondsPerBeat}\t{item.BeatsPerSecond}");
            }
            return sb.ToString();
        }

        public string GetTempoInfoText()
        {
            using var o = ObjectPool.Rent<StringBuilder>();
            var sb = o.Value;
            sb.AppendLine($"Tempo\tBeats\tSeconds");
            foreach (var (tempo, item) in SortedList.GetEnumerator(_tempoList, _tempoInfoList))
            {
                sb.AppendLine($"{tempo}\t{item.BeatLength}\t{item.TimeLength}");
            }
            return sb.ToString();
        }
    }
}
