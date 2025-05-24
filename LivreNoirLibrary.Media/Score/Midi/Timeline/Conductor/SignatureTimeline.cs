using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class SignatureTimeline : RationalTimeline<TimeSignature>, IDumpable<SignatureTimeline>, IBarPositionProvider
    {
        public const string Chid = "LNMdSg";

        private readonly List<int> _number_list = [];

        public static SignatureTimeline Load(BinaryReader reader)
        {
            SignatureTimeline timeline = new();
            timeline.ProcessLoad(reader);
            return timeline;
        }

        public void ProcessLoad(BinaryReader reader) => ProcessLoad(reader, TimeSignature.Load, Chid);
        public void Dump(BinaryWriter writer) => ProcessDump(writer, Dumpable.Dump, Chid);

        public void ExtendToEvent(RawTimeline timeline, long ticksPerWholeNote)
        {
            foreach (var (pos, value) in this)
            {
                var tick = IObject.GetTick(pos, ticksPerWholeNote);
                timeline.Add(tick, new RawData.TimeSignature(value));
            }
        }

        public TimeSignature Get(Rational position) => Get(position, TimeSignature.Default);

        public TimeSignature GetByNumber(int number)
        {
            EnsureNumberList();
            var index = _number_list.BinarySearch(number);
            if (index is < 0)
            {
                index = ~index - 1;
            }
            if ((uint)index < (uint)_value_list.Count)
            {
                return _value_list[index];
            }
            else
            {
                return TimeSignature.Default;
            }
        }

        public void SetByNumber(int number, TimeSignature value) => Set(GetHead(number), value);

        protected override void OnItemChanged(int index)
        {
            base.OnItemChanged(index);
            var c = _number_list.Count - index;
            if (c is > 0)
            {
                _number_list.RemoveRange(index, c);
            }
        }

        public int GetNumber(Rational position)
        {
            EnsureNumberList();
            if (TryGetIndex(position, out var index))
            {
                return _number_list[index];
            }
            else
            {
                var (pos, value, number) = GetValues(~index - 1);
                return number + CalcNumber(value, position - pos);
            }
        }

        public Rational GetHead(int number)
        {
            EnsureNumberList();
            var index = _number_list.BinarySearch(number);
            if (index is >= 0)
            {
                return _pos_list[index];
            }
            else
            {
                var (pos, value, num) = GetValues(~index - 1);
                return pos + new Rational((number - num) * value.Numerator, value.Denominator);
            }
        }

        public Rational GetBarLength(int number) => Get(number).ToRational();

        public BarPosition GetPosition(Rational beat)
        {
            var number = GetNumber(beat);
            var pos = beat - GetHead(number);
            return new(number, pos);
        }

        public Rational GetBeat(BarPosition position)
        {
            var head = GetHead(position.Bar);
            return head + position.Beat;
        }

        private void EnsureNumberList()
        {
            var poss = _pos_list;
            var values = _value_list;
            var nums = _number_list;
            var c = poss.Count;
            var c2 = nums.Count;
            if (c2 >= c) { return; }
            var (lastPos, value, number) = GetValues(c2 - 1);
            for (var i = c2; i < c; i++)
            {
                var pos = poss[i];
                var len = CalcNumber(value, pos - lastPos);
                if (len is 0)
                {
                    number++;
                }
                else
                {
                    number += len;
                }
                nums.Add(number);
                lastPos = pos;
                value = values[i];
            }
        }

        private (Rational Position, TimeSignature Value, int Number) GetValues(int index)
        {
            if (index is >= 0)
            {
                return (_pos_list[index], _value_list[index], _number_list[index]);
            }
            else
            {
                return (Rational.Zero, TimeSignature.Default, -1);
            }
        }

        private static int CalcNumber(TimeSignature value, Rational length) => (int)(length.Numerator * value.Denominator / length.Denominator / value.Numerator);

        public IEnumerable<BarInfo> EachBar(Rational end)
        {
            int index = 0;
            int number = 0;
            var poses = _pos_list;
            var values = _value_list;
            var pos = Rational.Zero;
            var sign = TimeSignature.Default;
            var signR = sign.ToRational();
            while (index < poses.Count)
            {
                var next = poses[index];
                var len = next - pos;
                while (pos < end && len > signR)
                {
                    yield return new(number++, sign, pos, signR);
                    len -= signR;
                    pos += signR;
                }
                if (pos < end && len.IsPositiveThanZero())
                {
                    yield return new(number++, sign, pos, len);
                    pos += len;
                }
                sign = values[index];
                signR = sign.ToRational();
                index++;
            }
            while (pos < end)
            {
                yield return new(number++, sign, pos, signR);
                pos += signR;
            }
        }

        public IEnumerable<BarLineInfo> EachLine(Rational end)
        {
            foreach (var info in EachBar(end))
            {
                var number = info.Number;
                Rational interval = new(1, info.Signature.Denominator);
                var pos = info.Head;
                var limit = pos + info.Length;
                yield return new(number, pos, true);
                for (pos += interval; pos < limit; pos += interval)
                {
                    yield return new(number, pos, false);
                }
            }
        }
    }
}
