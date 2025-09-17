using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IXTimeline<TX, TValue> : ITimeline<TX, TValue>, IEnumerable<(TX, TValue)>
        where TX : struct
    {
        public IEnumerable<(TX, TValue)> Range(Range<TX> range);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
