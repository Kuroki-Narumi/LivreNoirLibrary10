
namespace LivreNoirLibrary.Media.Midi
{
    public enum MetaType : byte
    {
        SeqNumber = 0x00,
        Text = 0x01,
        Copyright = 0x02,
        Title = 0x03,
        Instrument = 0x04,
        Lyrics = 0x05,
        Marker = 0x06,
        Cue = 0x07,
        Program = 0x08,
        Device = 0x09,
        ChannelPrefix = 0x20,
        Port = 0x21,
        TrackEnd = 0x2f,
        Tempo = 0x51,
        SmpteOffset = 0x54,
        TimeSignature = 0x58,
        Tonality = 0x59,
        SequencerEvent = 0x7F,

        Invalid = 0xFF,
    }

    public static class MetaTypeExtension
    {
        public static bool IsMetaText(this MetaType type) => type is >= MetaType.Text and <= MetaType.Device;
    }
}
