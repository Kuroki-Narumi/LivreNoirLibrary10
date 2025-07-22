using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Bms.RawData;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        internal void Dump(BmsTextWriter writer, int radix, BaseData? parent = null)
        {
            ProcessInherit(parent, () => DumpCore(writer, radix));
        }

        internal virtual void DumpCore(BmsTextWriter writer, int radix)
        {
            CreateRawData();
            if (!Headers.IsEmpty() || radix is not Constants.Base_Default)
            {
                Headers.Dump(writer, radix);
            }
            if (Comments.Count > 0)
            {
                foreach (var comment in CollectionsMarshal.AsSpan(Comments))
                {
                    writer.Dump(comment);
                }
            }
            if (!DefLists.IsEmpty())
            {
                DefLists.Dump(writer, radix, false);
            }
        }

        internal void DumpMain(BinaryWriter writer)
        {
            writer.Write(Comments.Count);
            foreach (var comment in CollectionsMarshal.AsSpan(Comments))
            {
                writer.Write(comment);
            }
            Headers.Dump(writer);
            DefLists.Dump(writer);
            Bars.Dump(writer);
            Timeline.Dump(writer);
        }

        internal void LoadMain(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            var comments = Comments;
            comments.Clear();
            for (var i = 0; i < count; i++)
            {
                var comment = reader.ReadString();
                comments.Add(comment);
            }
            Headers.ProcessLoad(reader);
            DefLists.ProcessLoad(reader);
            Bars.ProcessLoad(reader);
            Timeline.ReplaceBy(reader);
        }
    }
}
