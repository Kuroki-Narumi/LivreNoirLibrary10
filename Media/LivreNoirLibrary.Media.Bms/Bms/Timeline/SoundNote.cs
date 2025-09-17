using System;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class SoundNote(NoteType type, short lane, short value) : ISoundNote, INote<SoundNote>
    {
        private short _lane = lane;
        private short _value = value;

        public NoteType Type { get; set; } = type;
        public int Lane { get => _lane; set => _lane = (short)value; }
        public Channel Channel
        {
            get => BmsUtils.GetChannel(Type, _lane); 
            set
            {
                Type = BmsUtils.GetNoteType(value);
                _lane = BmsUtils.GetLane(value);
            }
        }
        public int Value { get => _value; set => _value = (short)value; }

        public SoundNote(long lane, long value, NoteType type = NoteType.Normal) : this(type, (short)lane, (short)value) { }

        public override string ToString() => $"{{Type={Type}, Lane={_lane}, Index={_value}}}";
        public string GetValueText(int radix) => BmsUtils.ToBased(_value, radix);

        public bool Equals(SoundNote other) => Type == other.Type && _lane == other._lane && _value == other._value;
        public bool Equals(in StructNote note) => note.Equals(Type, _lane, _value);

        public void CopyFrom(SoundNote source)
        {
            Type = source.Type;
            _lane = source._lane;
            _value = source._value;
        }

        void INote.CopyFrom(INote source)
        {
            if (source is SoundNote note)
            {
                CopyFrom(note);
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((sbyte)Type);
            writer.Write(_lane);
            writer.Write(_value);
        }

        public static SoundNote Load(BinaryReader reader)
        {
            var type = (NoteType)reader.ReadSByte();
            var lane = reader.ReadInt16();
            var value = reader.ReadInt16();
            return new(type, lane, value);
        }

        public SoundNote Clone() => new(Type, _lane, _value);
        INote INote.Clone() => Clone();
        public StructNote ToStruct() => new(Type, _lane, _value);
    }
}
