using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounter
    {
        private readonly List<Rational> _beat_list = [];
        private readonly List<Beat2SecondItem> _beat_item_list = [];

        private readonly List<Rational> _second_list = [];
        private readonly List<Second2BeatItem> _second_item_list = [];

        public TimeCounter(BaseData data)
        {
            var tempo = (Rational)data.Bpm;
            var rZero = Rational.Zero;
            var lastBeat = rZero;
            var spb = 240 / tempo;
            var second = Rational.Zero;
            InitList(tempo);
            foreach (var (pos, list) in data.Timeline.EachList())
            {
                // abort process if tempo(BPM) is non-positive number.
                if (tempo.IsNegativeOrZero())
                {
                    break;
                }
                var tempoExists = false;
                var curTempo = rZero;
                var curStop = rZero;
                foreach (var note in CollectionsMarshal.AsSpan(list))
                {
                    if (note.IsTempo())
                    {
                        curTempo = note.Value;
                        tempoExists = true;
                    }
                    else if (note.IsStop())
                    {
                        curStop += note.Value;
                    }
                }
                var tempoChanged = tempoExists && curTempo != tempo;
                var stopExists = !curStop.IsZero();
                if (tempoChanged || stopExists)
                {
                    var beat = data.GetBeat(pos);
                    second += spb * (beat - lastBeat);
                    lastBeat = beat;
                    if (tempoChanged)
                    {
                        tempo = curTempo;
                        spb = 240 / tempo;
                    }
                    var ss = curStop * spb;
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

        protected void InitList(Rational tempo)
        {
            var zero = Rational.Zero;
            _beat_list.Add(zero);
            _beat_item_list.Add(new(zero, tempo, zero));
            _second_list.Add(zero);
            _second_item_list.Add(new(zero, tempo));
        }

        protected void AddBeat2Second(Rational beat, Rational second, Rational tempo, Rational stop)
        {
            Beat2SecondItem item = new(second, tempo, stop);
            if (beat.IsZero())
            {
                _beat_item_list[0] = item;
            }
            else
            {
                _beat_list.Add(beat);
                _beat_item_list.Add(item);
            }
        }

        protected void AddSecond2Beat(Rational second, Rational beat, Rational tempo)
        {
            Second2BeatItem item = new(beat, tempo);
            if (second.IsZero())
            {
                _second_item_list[0] = item;
            }
            else
            {
                _second_list.Add(second);
                _second_item_list.Add(item);
            }
        }

        protected void AddStop(Rational second, Rational duration)
        {
            var index = _second_list.Count - 1;
            var preSec = _second_list[index];
            var prev = _second_item_list[index];
            var beat = prev.Beat;
            var tempo = prev.Tempo;
            var zero = Rational.Zero;
            var bps = prev.BeatsPerSecond;
            if (preSec == second)
            {
                _second_item_list[index] = new(beat, zero);
            }
            else
            {
                beat += (second - preSec) * bps;
                _second_list.Add(second);
                _second_item_list.Add(new(beat, zero));
            }
            _second_list.Add(second + duration);
            _second_item_list.Add(new(beat, tempo));
        }

        public IEnumerable<TempoInfo> EnumTempoInfo()
        {
            var seconds = _second_list;
            var items = _second_item_list;
            var c = seconds.Count;
            for (var i = 1; i < c; i++)
            {
                var curSec = seconds[i - 1];
                var nextSec = seconds[i];
                var item = items[i - 1];
                yield return new(item.Tempo, curSec, nextSec);
            }
            yield return new(items[^1].Tempo, seconds[^1], Rational.MinusOne, true);
        }

        public Rational Beat2Tempo(Rational beat)
        {
            var index = _beat_list.BinarySearch(beat);
            if (index is < 0)
            {
                index = Math.Max(~index - 1, 0);
            }
            return _beat_item_list[index].Tempo;
        }

        public Rational Beat2Second(Rational beat)
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

        public long Beat2Ticks(Rational beat) => Beat2Second(beat).ToTicks();
        public TimeSpan Beat2TimeSpan(Rational beat) => Beat2Second(beat).ToTimeSpan();

        public Rational Interval(Rational firstBeat, Rational lastBeat) => Beat2Second(lastBeat) - Beat2Second(firstBeat);
        public long IntervalTicks(Rational firstBeat, Rational lastBeat) => Beat2Ticks(lastBeat) - Beat2Ticks(firstBeat);
        public TimeSpan IntervalTimeSpan(Rational firstBeat, Rational lastBeat) => Beat2TimeSpan(lastBeat) - Beat2TimeSpan(firstBeat);

        public Rational Second2Tempo(Rational second)
        {
            var index = _second_list.BinarySearch(second);
            if (index is < 0)
            {
                index = Math.Max(~index - 1, 0);
            }
            return _second_item_list[index].Tempo;
        }

        public Rational Second2Beat(Rational second)
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

        public readonly struct Beat2SecondItem(Rational second, Rational tempo, Rational stop)
        {
            public readonly Rational Second = second;
            public readonly Rational Tempo = tempo;
            public readonly Rational SecondsPerBeat = 240 / tempo;
            public readonly Rational Stop = stop;
        }

        public readonly struct Second2BeatItem(Rational beat, Rational tempo)
        {
            public readonly Rational Beat = beat;
            public readonly Rational Tempo = tempo;
            public readonly Rational BeatsPerSecond = tempo / 240;
        }
    }
}
