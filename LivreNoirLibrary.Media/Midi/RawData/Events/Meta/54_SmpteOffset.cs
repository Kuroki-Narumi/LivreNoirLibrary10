using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class SmpteOffset(int hour, int minute, int second, int frame, int subframe) : MetaEvent(MetaType.SmpteOffset)
    {
        private const byte DataLength = sizeof(byte) * 5;

        private byte _hour = (byte)hour;
        private byte _minute = (byte)minute;
        private byte _second = (byte)second;
        private byte _frame = (byte)frame;
        private byte _sub_frame = (byte)subframe;

        public int Hour { get => _hour; set => _hour = (byte)value; }
        public int Minute { get => _minute; set => _minute = (byte)value; }
        public int Second { get => _second; set => _second = (byte)value; }
        public int Frame { get => _frame; set => _frame = (byte)value; }
        public int Subframe { get => _sub_frame; set => _sub_frame = (byte)value; }

        internal static new SmpteOffset Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            var hour = reader.ReadByte();
            var minute = reader.ReadByte();
            var second = reader.ReadByte();
            var frame = reader.ReadByte();
            var subFrame = reader.ReadByte();
            return new(hour, minute, second, frame, subFrame);
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.Write(_hour);
            writer.Write(_minute);
            writer.Write(_second);
            writer.Write(_frame);
            writer.Write(_sub_frame);
        }

        public override string ToString() => $"{nameof(SmpteOffset)}{{{_hour}:{_minute}:{_second}:{_frame}:{_sub_frame}}}";
    }
}
