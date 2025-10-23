using System;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class ConductorNote(Channel channel, decimal value) : IConductorNote, INote<ConductorNote>
    {
        public Channel Channel { get; set; } = channel;
        public decimal Value { get; set; } = value;

        public override string ToString() => $"{{Channel={Channel}, Value={Value}}}";
        public string GetValueText(int radix) => $"{Value}";

        public void CopyFrom(ConductorNote source)
        {
            Channel = source.Channel;
            Value = source.Value;
        }

        void INote.CopyFrom(INote source)
        {
            if (source is ConductorNote note)
            {
                CopyFrom(note);
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((short)Channel);
            writer.Write(Value);
        }

        public static ConductorNote Load(BinaryReader reader)
        {
            var ch = (Channel)reader.ReadInt16();
            var value = reader.ReadDecimal();
            return new(ch, value);
        }

        public ConductorNote Clone() => new(Channel, Value);
        INote INote.Clone() => Clone();
    }
}
