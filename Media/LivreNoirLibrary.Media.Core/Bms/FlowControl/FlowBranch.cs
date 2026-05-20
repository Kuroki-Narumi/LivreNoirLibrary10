using LivreNoirLibrary.IO;
using System;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class FlowBranch(int condition) : INoteObject, IDumpable, ILoadable<FlowBranch>
    {
        public static FlowBranch Root { get; } = new(0);

        public string? Note { get; set; }
        public int Condition { get; set; } = condition;
        public int DataIndex { get; set; } = -1;

        public void Dump(BinaryWriter writer)
        {
            writer.WriteNullable(Note);
            writer.Write(Condition);
            writer.Write(DataIndex);
        }

        public static FlowBranch Load(BinaryReader reader)
        {
            var note = reader.ReadStringOrNull();
            var condition = reader.ReadInt32();
            var dataIndex = reader.ReadInt32();
            return new(condition) { Note = note, DataIndex = dataIndex };
        }
    }
}
