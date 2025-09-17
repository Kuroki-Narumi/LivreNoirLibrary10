using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowBranch : BaseData
    {
        public int Condition { get; set; }

        public FlowBranch(BaseData parent, int condition)
        {
            Parent = parent;
            Condition = condition;
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(Condition);
            DumpMain(writer);
        }

        public static FlowBranch Load(BinaryReader reader, BaseData parent)
        {
            var condition = reader.ReadInt32();
            FlowBranch result = new(parent, condition);
            result.LoadMain(reader);
            return result;
        }
    }
}
