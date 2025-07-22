using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class Note : INote, ICloneable<Note>, IDumpable<Note>
    {
        private byte _nn;
        private byte _vel;
        private Rational _length;

        ObjectType IObject.ObjectType => ObjectType.Note;
        public string ObjectName => $"{KeyNames.Get(_nn)}({_nn})";
        public string ContentString => $"Vel:{_vel} Len:{_length}";
        public int Number { get => _nn; set => _nn = Event.GetMax127(value); }
        public int Velocity { get => _vel; set => _vel = Event.GetMax127(value); }
        public Rational Length { get => _length; set => _length = value.IsNegative() ? Rational.Zero : value; }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_nn);
            writer.Write(_vel);
            writer.Write(_length);
        }

        public static Note Load(BinaryReader reader)
        {
            var nn = reader.ReadByte();
            var vel = reader.ReadByte();
            var length = reader.ReadRational();
            return new() { _nn = nn, _vel = vel, _length = length };
        }

        public Note Clone() => new() { _nn = _nn, _vel = _vel, _length = _length };
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational position, long ticksPerWholeNote)
        {
            NoteOn on = new(channel, _nn, _vel);
            NoteOff off = new(channel, _nn);
            timeline.Add(tick, on);
            tick = IObject.GetTick(position + _length, ticksPerWholeNote);
            timeline.Add(tick, off);
        }

        Rational[] INote.GetMarkersArray(Rational offset) => [offset];

        IEnumerable<(Rational, Note)> INote.EachNote(Rational position)
        {
            yield return (position, this);
        }

        public void QuantizeVelocity(int q) => Velocity = INote.GetQuantized(_vel, q);
        public void QuantizeLength(Rational q) => Length = INote.GetQuantized(_length, q);

        public bool ContentEquals(INote other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other switch
            {
                Note n => ContentEquals(n),
                NoteGroup ng => ng.ContentEquals(this),
                _ => false,
            };
        }

        public bool ContentEquals(Note other) => _nn == other._nn && _vel == other._vel && _length == other._length;

        public int CompareTo(Note other)
        {
            var c = _nn - other._nn;
            if (c is not 0)
            {
                return c;
            }
            c = _vel - other._vel;
            if (c is not 0)
            {
                return c;
            }
            return _length.CompareTo(other._length);
        }

        public int CompareTo(IObject? other)
        {
            if (other is Note note)
            {
                return CompareTo(note);
            }
            return IObject.CompareBase(this, other);
        }

        public SortKey GetSortKey(SortKeyType key1, SortKeyType key2, SortKeyType key3, int index) => new(GetSortKey(key1), GetSortKey(key2), GetSortKey(key3), index);

        public double GetSortKey(SortKeyType type) => type switch
        {
            SortKeyType.NN => _nn,
            SortKeyType.NNI => -_nn,
            SortKeyType.Vel => _vel,
            SortKeyType.VelI => -_vel,
            SortKeyType.Gate => (double)_length,
            SortKeyType.GateI => -(double)_length,
            _ => 0,
        };

        public string GetMarkerName(string format)
        {
            return SliceUtils.ReplaceAutoSuffix(format,
                    () => KeyNames.Get(_nn),
                    _nn.ToString,
                    _vel.ToString,
                    () => INote.GetLengthText(_length)
                );
        }


        internal bool MatchedNumber(SortedSet<int> set) => set.Contains(_nn);
        bool INote.MatchesNumber(SortedSet<int> set) => MatchedNumber(set);

        internal Note GetEdited(Rational lenQ, Func<Rational, Rational>? lenFunc, int velQ, Func<double, double>? velFunc, Func<double, double>? nnFunc)
        {
            var nn = INote.GetByteEdit(_nn, 0, nnFunc);
            var vel = INote.GetByteEdit(_vel, velQ, velFunc, 1);
            var len = INote.GetEdit(_length, lenQ, lenFunc);
            return new Note() { _nn = nn, _vel = vel, _length = len };
        }
        INote INote.GetEdited(Rational lenQ, Func<Rational, Rational>? lenFunc, int velQ, Func<double, double>? velFunc, Func<double, double>? nnFunc)
            => GetEdited(lenQ, lenFunc, velQ, velFunc, nnFunc);
    }
}
