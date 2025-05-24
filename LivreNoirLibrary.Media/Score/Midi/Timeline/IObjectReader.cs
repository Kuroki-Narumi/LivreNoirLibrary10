using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Midi
{
    public class IObjectReader
    {
        private readonly List<NoteGroup> _groups = [];

        public IObject Read(BinaryReader reader)
        {
            var type = (ObjectType)reader.ReadByte();
            switch (type)
            {
                case ObjectType.Tempo:
                    return TempoEvent.Load(reader);
                case ObjectType.TimeSignature:
                    return TimeSignatureEvent.Load(reader);
                case ObjectType.Tonality:
                    return TonalityEvent.Load(reader);

                case ObjectType.MetaText:
                    return MetaText.Load(reader);
                case ObjectType.SmpteOffset:
                    return SmpteOffsetEvent.Load(reader);
                case ObjectType.SequencerEvent:
                    return SequencerEvent.Load(reader);

                case ObjectType.ControlChange:
                    return ControlChange.Load(reader);
                case ObjectType.SysEx:
                    return SysEx.Load(reader);
                case ObjectType.KeySwitch:
                    return KeySwitch.Load(reader);

                case ObjectType.Note:
                    return Note.Load(reader);
                case ObjectType.NoteGroup:
                    var g = NoteGroup.Load(reader);
                    _groups.Add(g);
                    return g;
                case ObjectType.NoteGroupIndex:
                    var index = reader.ReadInt32();
                    return _groups[index];

                default:
                    return new DummyObject();
            }
        }
    }
}
