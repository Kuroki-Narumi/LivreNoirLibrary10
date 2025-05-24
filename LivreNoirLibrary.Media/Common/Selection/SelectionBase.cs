using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media
{
    public abstract class SelectionBase<T> : RationalMultiTimeline<T>
        where T : ISelectionItem
    {
        public void Add(T item) => Add(item.ActualPosition, item);

        public IEnumerable<T> EachItem()
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

        public void ForEachItem(Action<T> action)
        {
            foreach (var list in CollectionsMarshal.AsSpan(_value_list))
            {
                foreach (var item in CollectionsMarshal.AsSpan(list))
                {
                    action(item);
                }
            }
        }

        public void ReplaceToClone() => ForEachItem(item => item.ReplaceToClone());

        public Rational GetFirstBeat()
        {
            if (IsEmpty())
            {
                return Rational.Zero;
            }
            var item = _value_list[0][0];
            return item.ActualPosition;
        }

        protected void ProcessDump(BinaryWriter writer, Rational offset, ValueWriter<T> valueWriter, string chid)
        {
            writer.WriteChid(chid);
            var c = _pos_list.Count;
            writer.Write(c);
            for (int i = 0; i < c; i++)
            {
                _operator.Write(writer, _pos_list[i] - offset);
                var v = _value_list[i];
                writer.Write(v.Count);
                foreach (var item in CollectionsMarshal.AsSpan(v))
                {
                    valueWriter(writer, item);
                }
            }
        }
    }
}
