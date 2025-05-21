using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IXSingleTimeline<TX, TValue> : IXTimeline<TX, TValue>
        where TX : struct
    {
        public void Set(TX position, TValue value);
        public TValue Get(TX position, TValue ifnone);
        public TValue Get(TX position, SearchMode type, TValue ifnone);
        public bool TryGet(TX position, SearchMode type, out TX actualPosition, [MaybeNullWhen(false)] out TValue value);

        public void CopyTo<T>(T destination)
            where T : IXSingleTimeline<TX, TValue>;
        public void CopyTo<T>(T destination, TX destOffset)
           where T : IXSingleTimeline<TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange)
            where T : IXSingleTimeline<TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange, TX destOffset)
            where T : IXSingleTimeline<TX, TValue>;
    }
}
