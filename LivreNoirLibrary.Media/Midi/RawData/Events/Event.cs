using System.IO;
using LivreNoirLibrary.Text;
using System.Text.Json;
using System;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public abstract class Event : IJsonWriter
    {
        public abstract StatusType Status { get; }

        internal static Event Load(StatusType status, BinaryReader reader)
        {
            return status switch
            {
                StatusType.MetaEvent => MetaEvent.Load(reader),
                StatusType.SysEx1 or StatusType.SysEx2 => SysEx.Load(status, reader),
                _ => ChannelEvent.Load(status, reader),
            };
        }

        internal void Dump(BinaryWriter writer)
        {
            writer.Write((byte)Status);
            DumpContents(writer);
        }

        protected virtual void DumpContents(BinaryWriter writer) { }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
        }

        public static byte GetMax15(int value) => (byte)Math.Clamp(value, 0, 15);
        public static byte GetMax127(int value, int min = 0) => (byte)Math.Clamp(value, min, 127);

        public static byte[] ReadWithSize(BinaryReader reader)
        {
            var length = reader.Read7BitEncodedIntBigEndian();
            return reader.ReadBytes(length);
        }

        public static void WriteWithSize(BinaryWriter writer, byte[] data)
        {
            writer.Write7BitEncodedIntBigEndian(data.Length);
            writer.Write(data);
        }
    }
}
