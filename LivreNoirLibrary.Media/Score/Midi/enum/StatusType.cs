using System;

namespace LivreNoirLibrary.Media.Midi
{
    [Flags]
    public enum StatusType : byte
    {
        None = 0,

        MetaEvent = 0xFF,
        SysEx1 = 0xF0,
        SysEx2 = 0xF7,
        NoteOff = 0x80,
        NoteOn = 0x90,
        PolyphonicKeyPressure = 0xA0,
        ControlChange = 0xB0,
        ProgramChange = 0xC0,
        ChannelPressure = 0xD0,
        PitchBend = 0xE0,

        ChannelMask = 0x0F,
        TypeMask = SysEx1,
    }
}
