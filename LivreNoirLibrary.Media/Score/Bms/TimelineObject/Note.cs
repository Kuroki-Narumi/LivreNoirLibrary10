using System;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class Note(NoteType type, int lane) : NoteBase(type, lane), INote, IDumpable<Note>, ICloneable<Note>, IComparable<Note>
    {
        private Rational _value;

        public Rational Value { get => _value; set => _value = value; }
        public decimal Decimal => (decimal)_value;
        public int Id { get => (int)_value; set => _value = new(value); }

        public Note(NoteType type, short lane, Rational value) : this(type, lane)
        {
            _value = value;
        }

        public Note(NoteType type, int lane, int id) : this(type, lane)
        {
            Id = id;
        }

        public static Note Tempo(Rational value) => new(NoteType.Decimal, BmsUtils.TempoLane, value);
        public static Note Tempo(decimal value) => Tempo((Rational)value);
        public static Note Stop(Rational value) => new(NoteType.Rational, BmsUtils.StopLane, value);
        public static Note Scroll(Rational value) => new(NoteType.Decimal, BmsUtils.ScrollLane, value);
        public static Note Scroll(decimal value) => Scroll((Rational)value);
        public static Note Speed(Rational value) => new(NoteType.Decimal, BmsUtils.SpeedLane, value);
        public static Note Speed(decimal value) => Speed((Rational)value);

        public bool IsNonZero() => !_value.IsZero();

        public void Replace(Note source)
        {
            _type = source._type;
            _lane = source._lane;
            _value = source._value;
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((short)_type);
            writer.Write(_lane);
            writer.Write(_value);
        }

        public static Note Load(BinaryReader reader)
        {
            var type = (NoteType)reader.ReadInt16();
            var lane = reader.ReadInt16();
            var value = reader.ReadRational();
            return new(type, lane, value);
        }

        public string GetValueText(int radix) =>
            this.IsDecimal() ? ((decimal)_value).ToString()
            : this.IsRational() ? _value.ToString()
            : this.IsIndex(true) ? BmsUtils.ToBased(Id, radix)
            : Id.ToString();

        public int CompareTo(Note? other)
        {
            if (other is not null)
            {
                var c = _lane.CompareTo(other._lane);
                if (c is not 0)
                {
                    return c;
                }
                return _value.CompareTo(other._value);
            }
            return 1;
        }

        protected override void WriteContent(Utf8JsonWriter writer)
        {
            if (this.IsDecimal())
            {
                writer.WriteNumber("value", Decimal);
            }
            else if (this.IsRational())
            {
                writer.WriteString("value", _value.ToString());
            }
            else if (this.IsIndex(true))
            {
                writer.WriteNumber("id", Id);
            }
            else
            {
                writer.WriteNumber("value", Id);
            }
        }

        public override string ToString() => $"(Type:{_type}, Lane:{_lane}, {GetValueString()})";

        private string GetValueString() =>
            this.IsDecimal() ? $"Value:{Decimal}"
            : this.IsRational() ? $"Value:{_value}"
            : this.IsIndex(true) ? $"Id:{Id}" 
            : $"Value:{Id}";

        public bool IsSame(Note other) => _type == other._type && _lane == other._lane && _value == other._value;

        public Note Clone() => new(_type, _lane, _value);
        public StructNote ToStruct() => new(_type, _lane, _value);
        public bool Equals(StructNote note) => note.Equals(_type, _lane, _value);
    }
}
