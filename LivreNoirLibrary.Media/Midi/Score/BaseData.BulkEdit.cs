using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Midi
{
    public abstract partial class MidiData<TTrack>
    {
        public bool BulkEdit<T>(T trackIndexes, BulkEditOptions options)
            where T : ICollection<int>
        {
            var flag = false;
            foreach (var (i, t) in EachTrack())
            {
                if (trackIndexes.Contains(i) && t.BulkEdit(options, null, out _))
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}
