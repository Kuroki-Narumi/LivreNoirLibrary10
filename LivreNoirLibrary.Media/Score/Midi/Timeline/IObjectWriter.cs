using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Midi
{
    public class IObjectWriter
    {
        private readonly Dictionary<NoteGroup, int> _groups = [];

        public void Write(BinaryWriter writer, IObject value)
        {
            if (value is NoteGroup group)
            {
                if (_groups.TryGetValue(group, out var index))
                {
                    writer.Write((byte)ObjectType.NoteGroupIndex);
                    writer.Write(index);
                    return;
                }
                else
                {
                    _groups.Add(group, _groups.Count);
                }
            }
            writer.Write((byte)value.ObjectType);
            value.Dump(writer);
        }
    }
}
