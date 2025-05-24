using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi
{
    public abstract class DataObjectBase(ObjectType type, byte[] data)
    {
        private readonly ObjectType _obj_type = type;
        protected byte[] _data = data;

        public ObjectType ObjectType => _obj_type;
        public abstract string ObjectName { get; }
        public string ContentString => BitConverter.ToString(_data);

        public byte[] Data { get => _data; }

        public virtual void Dump(BinaryWriter writer) => DumpData(writer);
        protected void DumpData(BinaryWriter writer) => writer.WriteWithSize(_data);
        protected static byte[] LoadData(BinaryReader reader) => reader.ReadWithSize();

        public bool StartsWith(ReadOnlySpan<byte> span)
        {
            return span.Length is > 0 && span.SequenceEqual(new ReadOnlySpan<byte>(_data, 0, span.Length));
        }
    }
}
