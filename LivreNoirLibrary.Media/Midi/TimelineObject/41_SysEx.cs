using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class SysEx(StatusType type, byte[] data) : DataObjectBase(ObjectType.SysEx, data), IObject, ICloneable<SysEx>, IDumpable<SysEx>
    {
        private readonly StatusType _type = type;

        public override string ObjectName => nameof(SysEx);
        public StatusType Type => _type;

        public SysEx(RawData.SysEx source) : this(source.Status, source.Data) { }

        public override void Dump(BinaryWriter stream)
        {
            stream.Write((byte)_type);
            DumpData(stream);
        }

        public static SysEx Load(BinaryReader stream)
        {
            var type = stream.ReadByte();
            var data = LoadData(stream);
            return new((StatusType)type, data);
        }

        public SysEx Clone() => new(_type, [.. _data]);
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new RawData.SysEx(_type is StatusType.SysEx1, _data));
        }

        public int CompareTo(SysEx other)
        {
            return ((ReadOnlySpan<byte>)_data).SequenceCompareTo(other._data);
        }

        public int CompareTo(IObject? other)
        {
            if (other is SysEx se)
            {
                return CompareTo(se);
            }
            return IObject.CompareBase(this, other);
        }
    }
}
