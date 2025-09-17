using System;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class MetaNote(Channel channel, short value) : IMetaNote, INote<MetaNote>
    {
        private short _value = value;

        public Channel Channel { get; set; } = channel;
        public int Value { get => _value; set => _value = (short)value; }

        public MetaNote(Channel channel, long value) : this(channel, (short)value) { }

        public override string ToString() => $"{{Channel={Channel}, {(Channel.IsDefChannel() ? "Index" : "Value")}={_value}}}";
        public string GetValueText(int radix) => Channel.IsDefChannel() ? BmsUtils.ToBased(_value, radix) : _value.ToString();

        public void CopyFrom(MetaNote source)
        {
            Channel = source.Channel;
            _value = source._value;
        }

        void INote.CopyFrom(INote source)
        {
            if (source is MetaNote note)
            {
                CopyFrom(note);
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((short)Channel);
            writer.Write(_value);
        }

        public static MetaNote Load(BinaryReader reader)
        {
            var ch = (Channel)reader.ReadInt16();
            var value = reader.ReadInt16();
            return new(ch, value);
        }

        public MetaNote Clone() => new(Channel, _value);
        INote INote.Clone() => Clone();
    }
}
