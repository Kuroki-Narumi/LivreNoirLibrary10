using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class SelectionHistory
    {
        private Dictionary<Rational, List<HistoryNote>> _list = [];

        public SelectionHistory(Selection selection)
        {
            var list = _list;
            foreach (var item in selection)
            {
                list.Add(item.AbsolutePosition, HistoryNote.Create(item.Note));
            }
        }

        public void Restore(IBmsData data, Selection selection)
        {
            var buffer = ObjectPool.Rent<List<HistoryNote>>();
            try
            {
                var timeline = data.Timeline;
                foreach (var (hPos, list) in _list)
                {
                    buffer.Clear();
                    buffer.AddRange(list);
                    var barPosition = data.GetBarPosition(hPos);
                    if (timeline.TryGet(barPosition, SearchMode.Equal, out _, out var sList))
                    {
                        foreach (var note in CollectionsMarshal.AsSpan(sList))
                        {
                            var index = buffer.FindIndex(n => n.Equals(note));
                            if (index is >= 0)
                            {
                                buffer.RemoveAt(index);
                                selection.Add(data.GetHead(barPosition), hPos, note);
                            }
                        }
                    }
                }
            }
            finally
            {
                ObjectPool.Return(buffer);
            }
        }
    }
}
