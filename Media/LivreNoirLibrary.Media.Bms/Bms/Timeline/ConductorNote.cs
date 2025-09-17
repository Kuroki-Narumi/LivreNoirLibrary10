using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class ConductorNote(Channel channel, Rational value) : IConductorNote, INote<ConductorNote>
    {
        public Channel Channel { get; set; } = channel;
        public Rational Value { get; set; } = value;
        public double DoubleValue { get => (double)Value; set => Value = Rational.ConvertBySBT(value); }
        public decimal DecimalValue { get => (decimal)Value; set => Value = Rational.ConvertBySBT(value); }

        public override string ToString() => $"{{Channel={Channel}, Value={Value}}}";
        public string GetValueText(int radix) => $"{DecimalValue}";

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
            var value = reader.ReadRational();
            return new(ch, value);
        }

        public ConductorNote Clone() => new(Channel, Value);
        INote INote.Clone() => Clone();
    }
}
