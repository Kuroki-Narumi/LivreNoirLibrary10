using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class SmpteOffsetEvent(int hour, int minute, int second, int frame, int subframe) : IMetaObject, ICloneable<SmpteOffsetEvent>, IDumpable<SmpteOffsetEvent>
    {
        private byte _hour = (byte)hour;
        private byte _minute = (byte)minute;
        private byte _second = (byte)second;
        private byte _frame = (byte)frame;
        private byte _sub_frame = (byte)subframe;

        public MetaType Type => MetaType.SmpteOffset;
        public ObjectType ObjectType => ObjectType.SmpteOffset;
        public string ObjectName => nameof(SmpteOffsetEvent);
        public string ContentString => $"{_hour:2D}:{_minute:2D}:{_second:2D}:{_frame:2D}:{_sub_frame:2D}";

        public int Hour { get => _hour; set => _hour = (byte)value; }
        public int Minute { get => _minute; set => _minute = (byte)value; }
        public int Second { get => _second; set => _second = (byte)value; }
        public int Frame { get => _frame; set => _frame = (byte)value; }
        public int Subframe { get => _sub_frame; set => _sub_frame = (byte)value; }

        public SmpteOffsetEvent(SmpteOffset source) : this(source.Hour, source.Minute, source.Second, source.Frame, source.Subframe) { }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_hour);
            writer.Write(_minute);
            writer.Write(_second);
            writer.Write(_frame);
            writer.Write(_sub_frame);
        }

        public static SmpteOffsetEvent Load(BinaryReader reader)
        {
            var hour = reader.ReadByte();
            var minute = reader.ReadByte();
            var second = reader.ReadByte();
            var frame = reader.ReadByte();
            var subframe = reader.ReadByte();
            return new(hour, minute, second, frame, subframe);
        }

        public SmpteOffsetEvent Clone() => new(_hour, _minute, _second, _frame, _sub_frame);
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new SmpteOffset(_hour, _minute, _second, _frame, _sub_frame));
        }

        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
    }
}
