
namespace LivreNoirLibrary.Media.Midi
{
    public enum ObjectType : byte
    {
        None,

        Tempo,
        TimeSignature,
        Tonality,

        MetaText,
        SmpteOffset,
        SequencerEvent,

        ControlChange,
        SysEx,
        KeySwitch,

        Note,
        NoteGroup,
        NoteGroupIndex,
    }
}
