using System;

namespace LivreNoirLibrary.Media.Bms
{
    public class Note(Channel channel, NoteType type, double value)
    {
        public Channel Channel { get; set; } = channel;
        public NoteType Type { get; set; } = type;
        public double Value { get; set; } = value;

        public Note(Channel channel, double value) : this(channel, NoteType.Normal, value) { }
    }
}
