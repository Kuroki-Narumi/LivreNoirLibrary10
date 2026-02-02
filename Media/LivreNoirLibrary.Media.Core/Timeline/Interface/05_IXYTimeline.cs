using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IXYTimeline<TY, TX, TValue> : ITimeline<TX>, IEnumerable<(TY, TX, TValue)>
        where TX : struct
    {
        ReadOnlySpan<TX> GetPositions(TY key);
        ReadOnlySpan<TX> GetPositions(TY key, Range<TX> range);
        ReadOnlySpan<TY> GetKeyList();

        IEnumerable<(TY, TX, TValue)> Range(Range<TX> range);
        IEnumerable<(TX, TValue)> Range(TY key);
        IEnumerable<(TX, TValue)> Range(TY key, Range<TX> range);

        bool RemoveKey(TY key);
        bool RemoveAt(TY key, TX position);
        void RemoveRange(TY key, Range<TX> range);

        void Move(TY key, Func<TX, TX> converter);
        void Move(TY key, Func<TX, TX> converter, Range<TX> range);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
