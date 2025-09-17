using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IXYTimeline<TY, TX, TValue> : ITimeline<TX, TValue>, IEnumerable<(TY, TX, TValue)>
        where TX : struct
    {
        public ReadOnlySpan<TX> GetPositions(TY key);
        public ReadOnlySpan<TX> GetPositions(TY key, Range<TX> range);
        public ReadOnlySpan<TY> GetKeyList();

        public IEnumerable<(TY, TX, TValue)> Range(Range<TX> range);
        public IEnumerable<(TX, TValue)> Range(TY key);
        public IEnumerable<(TX, TValue)> Range(TY key, Range<TX> range);

        public bool RemoveKey(TY key);
        public bool RemoveAt(TY key, TX position);
        public void RemoveRange(TY key, Range<TX> range);

        public void Move(TY key, TX from, TX to);
        public void Move(TY key, Func<TX, TX> converter);
        public void Move(TY key, Func<TX, TX> converter, Range<TX> range);
        public void InsertSpace(TY key, TX offset, TX length);
        public void DeleteSpace(TY key, TX offset, TX length);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
