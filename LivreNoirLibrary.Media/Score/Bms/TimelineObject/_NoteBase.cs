using System;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class NoteBase(NoteType type, int lane) : IJsonWriter
    {
        protected NoteType _type = type;
        protected short _lane = (short)lane;

        public NoteType Type { get => _type; set => _type = value; }
        public int Lane { get => _lane; set => _lane = (short)value; }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", _type.ToString());
            if (BmsUtils.IsMetaLane(_lane))
            {
                writer.WriteString("channel", BmsUtils.GetMetaChannel(_lane).ToString());
            }
            else
            {
                writer.WriteNumber("lane", _lane);
            }
            WriteContent(writer);
            writer.WriteEndObject();
        }

        protected virtual void WriteContent(Utf8JsonWriter writer) { }
    }
}
