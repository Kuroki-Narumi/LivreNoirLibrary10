using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public abstract class ChannelEvent(int channel) : Event
    {
        public override StatusType Status => Type | (StatusType)Channel;
        public abstract StatusType Type { get; }

        protected byte _channel = GetMax15(channel);

        public int Channel { get => _channel; set => _channel = GetMax15(value); }

        public static new ChannelEvent Load(StatusType type, BinaryReader reader)
        {
            var ch = (int)(type & StatusType.ChannelMask);
            type &= StatusType.TypeMask;
            return type switch
            {
                StatusType.NoteOff => NoteOff.Load(ch, reader),
                StatusType.NoteOn => NoteOn.Load(ch, reader),
                StatusType.PolyphonicKeyPressure => PolyphonicKeyPressure.Load(ch, reader),
                StatusType.ControlChange => ControlChange.Load(ch, reader),
                StatusType.ProgramChange => ProgramChange.Load(ch, reader),
                StatusType.ChannelPressure => ChannelPressure.Load(ch, reader),
                StatusType.PitchBend => PitchBend.Load(ch, reader),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid StatusType value")
            };
        }
    }
}
