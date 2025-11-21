using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi
{
    /// <summary>
    /// "Tempo" is expressed in the unit of "microseconds per beat".
    /// To convert to "beats per minute", divide 60_000_000(microseconds per minute) by the value, and truncate to the 4th double place.
    /// </summary>
    public class TempoTimeline : RationalTimeline<int>, IDumpable, ILoadable<TempoTimeline>
    {
        public const string Chid = "LNMdTm";
        public const int DefaultTempo = Events.Tempo.DefaultValue;
        /// <summary>
        /// 1 / (Microseconds * beats per bar)
        /// </summary>
        public const double SecondsMultiplier = 4d / 1_000_000;

        private readonly List<double> _seconds_list = [];

        public TempoTimeline() { }
        public TempoTimeline(IScore score)
        {
            foreach (var (pos, obj) in score.ConductorTrack.Timeline)
            {
                if (obj is TempoEvent t)
                {
                    Set(pos, t.Value);
                }
            }
        }

        public static TempoTimeline Load(BinaryReader reader)
        {
            TempoTimeline timeline = [];
            timeline.ProcessLoad(reader);
            return timeline;
        }

        public void ProcessLoad(BinaryReader reader) => ProcessLoad(reader, IOExtensions.LoadInt32, Chid);
        public void Dump(BinaryWriter writer) => ProcessDump(writer, IOExtensions.Dump, Chid);

        public override void Clear()
        {
            base.Clear();
            _seconds_list.Clear();
        }

        public void CopyTo(TempoTimeline target, Range<Rational> range, Rational offset)
        {
            target.Set(Rational.Zero, range.IsStartEnabled ? Get(range.Start) : Get(Rational.Zero));
            base.CopyTo(target, range, offset);
        }

        protected override void OnItemChanged(int index)
        {
            base.OnItemChanged(index);
            var c = _seconds_list.Count - index;
            if (c is > 0)
            {
                _seconds_list.RemoveRange(index, c);
            }
        }

        public int Get(Rational position) => Get(position, DefaultTempo);
        public double GetBpm(Rational position) => TimeUtils.MicroSeconds2Bpm(Get(position));
        public void SetBpm(Rational position, double value) => Set(position, TimeUtils.Bpm2MicroSeconds(value));

        public IEnumerable<(Rational Position, double Value)> EachBpm()
        {
            var c = _pos_list.Count;
            for (int i = 0; i < c; i++)
            {
                yield return (_pos_list[i], TimeUtils.MicroSeconds2Bpm(_value_list[i]));
            }
        }

        public IEnumerable<(Rational Position, double Value)> EachBpm(Range<Rational> range)
        {
            var (s, l) = GetPositionIndex(range);
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                yield return (_pos_list[i], TimeUtils.MicroSeconds2Bpm(_value_list[i]));
            }
        }

        public double GetSeconds(Rational position)
        {
            EnsureSecondsList();
            if (TryGetIndex(position, out var index))
            {
                return _seconds_list[index];
            }
            else
            {
                var (pos, value, head) = GetValues(~index - 1);
                return head + CalcSeconds(value, position - pos);
            }
        }

        public double GetSeconds(Rational start, Rational length) => GetSeconds(start + length) - GetSeconds(start);

        private void EnsureSecondsList()
        {
            var c = _pos_list.Count;
            var c2 = _seconds_list.Count;
            if (c2 >= c) { return; }
            var (lastPos, value, head) = GetValues(c2 - 1);
            for (int i = c2; i < c; i++)
            {
                var pos = _pos_list[i];
                head += CalcSeconds(value, pos - lastPos);
                _seconds_list.Add(head);
                lastPos = pos;
                value = _value_list[i];
            }
        }

        private (Rational Position, int Value, double Head) GetValues(int index)
        {
            if (index is >= 0)
            {
                return (_pos_list[index], _value_list[index], _seconds_list[index]);
            }
            else
            {
                return (Rational.Zero, DefaultTempo, 0);
            }
        }

        private static double CalcSeconds(int value, Rational length) => value * length * SecondsMultiplier;
    }
}
