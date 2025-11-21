using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Midi
{
    public class Selection : RationalMultiTimeline<SelectionItem>, ISelection, IDumpable, ILoadable<Selection>
    {
        public const string Chid = "LNMdSl";

        public void Dump(BinaryWriter writer)
        {
            IObjectWriter w = new();
            var offset = GetFirstBeat();
            writer.WriteChid(Chid);
            var c = _pos_list.Count;
            writer.Write(c);
            for (var i = 0; i < c; i++)
            {
                Operator_Rational.Write(writer, _pos_list[i] - offset);
                var v = _value_list[i];
                writer.Write(v.Count);
                foreach (var item in v.AsSpan())
                {
                    w.Write(writer, item.Object);
                }
            }
        }

        public static Selection Load(BinaryReader reader)
        {
            IObjectReader r = new();
            Selection selection = [];
            selection.ProcessLoad(reader, (reader, pos) => new(pos, r.Read(reader)), Chid);
            return selection;
        }

        public void Add(SelectionItem item) => this.Add(item.Position, item);
        public void Add(Rational position, IObject obj) => this.Add(position, new SelectionItem(position, obj));
        public bool Remove(Rational position, IObject obj) => this.RemoveAll(position, item => ReferenceEquals(item.Object, obj)) is > 0;

        public IEnumerable<(Rational, IObject)> EachItem()
        {
            var imax = _value_list.Count;
            for (var i = 0; i < imax; i++)
            {
                var list = _value_list[i];
                var jmax = list.Count;
                for (var j = 0; j < jmax; j++)
                {
                    yield return list[j];
                }
            }
        }

        public void ForEachItem(Action<SelectionItem> action)
        {
            foreach (var list in _value_list.AsSpan())
            {
                foreach (var item in list.AsSpan())
                {
                    action(item);
                }
            }
        }

        public void ReplaceToClone() => ForEachItem(item => item.ReplaceToClone());

        public Rational GetFirstBeat()
        {
            if (IsEmpty)
            {
                return Rational.Zero;
            }
            var item = _value_list[0][0];
            return item.Position;
        }
    }
}
