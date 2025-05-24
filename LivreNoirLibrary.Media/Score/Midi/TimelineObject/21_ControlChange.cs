using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class ControlChange(CCType type, int value, int? ext = null) : IObject, ICloneable<ControlChange>, IDumpable<ControlChange>, IEquatable<ControlChange>
    {
        public const int ExtDisabled = RawData.ControlChange.ExtDisabled;

        private CCType _type = type;
        private int _value = value;
        private int _ext = ext ?? ExtDisabled;

        ObjectType IObject.ObjectType => ObjectType.ControlChange;
        public string ObjectName => Enum.IsDefined(_type) ? _type.ToString() : $"CC:{(int)_type}";
        public string ContentString => _ext is ExtDisabled ? $"{_value}" : $"{_value};{_ext}";
        public CCType Type { get => _type; set => _type = value; }
        public int Value { get => _value; set => _value = value; }
        public int Ext { get => _ext; set => _ext = value; }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((short)_type);
            writer.Write(_value);
            writer.Write(_ext);
        }

        public static ControlChange Load(BinaryReader reader)
        {
            var type = (CCType)reader.ReadInt16();
            var value = reader.ReadInt32();
            var ext = reader.ReadInt32();
            return new(type, value, ext);
        }

        public ControlChange Clone() => new(_type, _value, _ext);
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            var type = _type;
            var value = _value;
            var ext = _ext;
            switch (type)
            {
                case CCType.PolyphonicKeyPressure:
                    timeline.Add(tick, new PolyphonicKeyPressure(channel, value, ext));
                    break;
                case CCType.ProgramChange:
                    if (ext is not ExtDisabled)
                    {
                        timeline.Add(tick, new RawData.ControlChange(channel, CCType.BankSelect, (ext >> 8) & 0x7F));
                        timeline.Add(tick, new RawData.ControlChange(channel, CCType.BankSelect_LSB, ext & 0x7F));
                    }
                    timeline.Add(tick, new ProgramChange(channel, value));
                    break;
                case CCType.ChannelPressure:
                    timeline.Add(tick, new ChannelPressure(channel, value));
                    break;
                case CCType.PitchBend:
                    timeline.Add(tick, new PitchBend(channel, value));
                    break;
                case CCType.SequenceNumber:
                    timeline.Add(tick, new SequenceNumberEvent(value));
                    break;
                case CCType.ChannelPrefix:
                    timeline.Add(tick, new ChannelPrefix(value));
                    break;
                case CCType.Port:
                    timeline.Add(tick, new Port(value));
                    break;
                case CCType.RPN:
                    timeline.Add(tick, new RawData.ControlChange(channel, CCType.RPN_MSB, (ext >> 8) & 0x7F));
                    timeline.Add(tick, new RawData.ControlChange(channel, CCType.RPN_LSB, ext & 0x7F));
                    if (ext is not 0x7F7F)
                    {
                        if (RawData.ControlChange.NeedsDataMSB(ext))
                        {
                            timeline.Add(tick, new RawData.ControlChange(channel, CCType.DataEntry, (value >> 8) & 0x7F));
                            timeline.Add(tick, new RawData.ControlChange(channel, CCType.DataEntry_LSB, value & 0x7F));
                        }
                        else
                        {
                            timeline.Add(tick, new RawData.ControlChange(channel, CCType.DataEntry, value));
                        }
                    }
                    break;
                case CCType.NRPN:
                    timeline.Add(tick, new RawData.ControlChange(channel, CCType.NRPN_MSB, (ext >> 8) & 0x7F));
                    timeline.Add(tick, new RawData.ControlChange(channel, CCType.NRPN_LSB, ext & 0x7F));
                    if (ext is not 0x7F7F)
                    {
                        timeline.Add(tick, new RawData.ControlChange(channel, CCType.DataEntry, value));
                    }
                    break;
                case CCType.KeySwitch:
                    // do nothing
                    break;
                default:
                    timeline.Add(tick, new RawData.ControlChange(channel, type, value, ext));
                    break;
            }
        }

        public bool Equals(IObject other) => Equals(other as ControlChange);

        public bool Equals(ControlChange? other) => other is not null && _type == other._type && _value == other._value && _ext == other._ext;
        public override bool Equals(object? obj) => obj is ControlChange cc && Equals(cc);
        public override int GetHashCode() => HashCode.Combine(_type, _value, _ext);

        public int CompareTo(ControlChange other)
        {
            var c = ((short)_type).CompareTo((short)other._type);
            if (c is not 0)
            {
                return c;
            }
            c = _value - other._value;
            if (c is not 0)
            {
                return c;
            }
            return _ext - other._ext;
        }

        public int CompareTo(IObject? other)
        {
            if (other is ControlChange cc)
            {
                return CompareTo(cc);
            }
            return IObject.CompareBase(this, other);
        }
    }
}
