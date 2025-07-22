using System;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public class Selection() : SelectionBase<SelectionItem>, IDumpable<Selection>
    {
        public const string Chid = "LNMdSl";

        public void Add(Rational position, IObject obj) => Add(position, new SelectionItem(position, obj));
        public void Remove(Rational position, IObject obj) => RemoveIf(position, item => ReferenceEquals(item.Object, obj));

        public Selection ShallowCopy()
        {
            Selection selection = [];
            ForEachItem(item => selection.Add(item.Position, item));
            return selection;
        }

        public bool Contains(Rational position, IObject obj)
        {
            if (TryGet(position, out var list))
            {
                foreach (var item in CollectionsMarshal.AsSpan(list))
                {
                    if (ReferenceEquals(item.Object, obj))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Dump(BinaryWriter writer)
        {
            IObjectWriter w = new();
            ProcessDump(writer, GetFirstBeat(), (writer, item) => w.Write(writer, item.Object), Chid);
        }

        public static Selection Load(BinaryReader reader)
        {
            IObjectReader r = new();
            Selection selection = [];
            selection.ProcessLoad(reader, (reader, pos) => new(pos, r.Read(reader)), Chid);
            return selection;
        }
    }
}
