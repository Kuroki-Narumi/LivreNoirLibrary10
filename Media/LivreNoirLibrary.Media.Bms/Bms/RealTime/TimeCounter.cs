using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounter : ITimeCounter
    {
        private readonly List<decimal> _beat_list = [];
        private readonly List<Beat2SecondItem> _beat_item_list = [];

        private readonly List<decimal> _second_list = [];
        private readonly List<Second2BeatItem> _second_item_list = [];

        public TimeCounter() { }

        public TimeCounter(IBmsData data)
        {
            LoadInternal(data);
        }

        public void Load(IBmsData data)
        {
            _beat_list.Clear();
            _beat_item_list.Clear();
            _second_list.Clear();
            _second_item_list.Clear();
            LoadInternal(data);
        }

        private void LoadInternal(IBmsData data)
        {
            var tempo = (decimal)data.Bpm;
            var lastBeat = 0m;
            var spb = 240 / tempo;
            var second = 0m;
            InitList(tempo);
            foreach (var (pos, list) in data.Timeline.EachList())
            {
                // abort process if tempo(BPM) is non-positive number.
                if (tempo is <= 0)
                {
                    break;
                }
                var tempoExists = false;
                var curTempo = 0m;
                var curStop = 0m;
                foreach (var note in CollectionsMarshal.AsSpan(list))
                {
                    switch (note)
                    {
                        case IConductorNote { Channel: Channel.Bpm } c:
                            curTempo = c.Value;
                            tempoExists = true;
                            break;
                        case IConductorNote { Channel: Channel.Stop } c:
                            curStop += c.Value;
                            break;
                    }
                }
                var tempoChanged = tempoExists && curTempo != tempo;
                var stopExists = curStop is not 0;
                if (tempoChanged || stopExists)
                {
                    var beat = (decimal)data.GetAbsolutePosition(pos);
                    second += spb * (beat - lastBeat);
                    lastBeat = beat;
                    if (tempoChanged)
                    {
                        tempo = curTempo;
                        spb = 240 / tempo;
                    }
                    var ss = curStop * Constants.StopUnit * spb;
                    AddBeat2Second(beat, second, tempo, ss);
                    AddSecond2Beat(second, beat, tempo);
                    if (stopExists)
                    {
                        AddStop(second, ss);
                        second += ss;
                    }
                }
            }
        }

        private void InitList(decimal tempo)
        {
            _beat_list.Add(0);
            _beat_item_list.Add(new(0, tempo, 0));
            _second_list.Add(0);
            _second_item_list.Add(new(0, tempo));
        }

        private void AddBeat2Second(decimal beat, decimal second, decimal tempo, decimal stop)
        {
            Beat2SecondItem item = new(second, tempo, stop);
            if (beat is 0)
            {
                _beat_item_list[0] = item;
            }
            else
            {
                _beat_list.Add(beat);
                _beat_item_list.Add(item);
            }
        }

        private void AddSecond2Beat(decimal second, decimal beat, decimal tempo)
        {
            Second2BeatItem item = new(beat, tempo);
            if (second is 0)
            {
                _second_item_list[0] = item;
            }
            else
            {
                _second_list.Add(second);
                _second_item_list.Add(item);
            }
        }

        private void AddStop(decimal second, decimal duration)
        {
            var index = _second_list.Count - 1;
            var preSec = _second_list[index];
            var prev = _second_item_list[index];
            var beat = prev.Beat;
            var tempo = prev.Tempo;
            var bps = prev.BeatsPerSecond;
            if (preSec == second)
            {
                _second_item_list[index] = new(beat, 0);
            }
            else
            {
                beat += (second - preSec) * bps;
                _second_list.Add(second);
                _second_item_list.Add(new(beat, 0));
            }
            _second_list.Add(second + duration);
            _second_item_list.Add(new(beat, tempo));
        }

        public List<TempoInfo<decimal>> GetTempoInfos()
        {
            List<TempoInfo<decimal>> list = [];
            var seconds = _second_list;
            var items = _second_item_list;
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
                return _beat_item_list[index].Second;
            }
            else
            {
                index = Math.Max(~index - 1, 0);
                var b = _beat_list[index];
                var item = _beat_item_list[index];
                var s = item.Second + item.Stop;
                return s + item.SecondsPerBeat * (beat - b);
            }
        }

        public decimal Time2Tempo(decimal second)
        {
            var index = _second_list.BinarySearch(second);
            if (index is < 0)
            {
                index = Math.Max(~index - 1, 0);
            }
            return _second_item_list[index].Tempo;
        }

        public decimal Time2Beat(decimal second)
        {
            var index = _second_list.BinarySearch(second);
            if (index is >= 0)
            {
                return _second_item_list[index].Beat;
            }
            else
            {
                index = Math.Max(~index - 1, 0);
                var s = _second_list[index];
                var item = _second_item_list[index];
                return item.Beat + (second - s) * item.BeatsPerSecond;
            }
        }

        public readonly struct Beat2SecondItem(decimal second, decimal tempo, decimal stop)
        {
            public readonly decimal Second = second;
            public readonly decimal Tempo = tempo;
            public readonly decimal SecondsPerBeat = 240 / tempo;
            public readonly decimal Stop = stop;

            public override string ToString() => $"{{Second={Second}, Tempo={Tempo}, Stop={Stop}}}";
        }

        public readonly struct Second2BeatItem(decimal beat, decimal tempo)
        {
            public readonly decimal Beat = beat;
            public readonly decimal Tempo = tempo;
            public readonly decimal BeatsPerSecond = tempo / 240;

            public override string ToString() => $"{{Beat={Beat}, Tempo={Tempo}}}";
        }
    }
}
