using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class SysEx : Event
    {
        private readonly StatusType _type;
        public override StatusType Status => _type;

        public byte[] Data { get; set; }

        private SysEx(StatusType type, byte[] data)
        {
            _type = type;
            Data = data;
        }

        public SysEx(bool isF0, byte[] data) : this(isF0 ? StatusType.SysEx1 : StatusType.SysEx2, data) { }

        internal static new SysEx Load(StatusType type, BinaryReader reader) => new(type, ReadWithSize(reader));
        protected override void DumpContents(BinaryWriter writer) => WriteWithSize(writer, Data);

        public override string ToString() => $"{nameof(SysEx)}{{{nameof(Status)}={_type}, {nameof(Data)}={BitConverter.ToString(Data)}}}";
    }
}
