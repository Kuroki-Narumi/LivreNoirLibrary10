using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IXYSingleTimeline<TY, TX, TValue> : IXYTimeline<TY, TX, TValue>
        where TX : struct
    {
        public void Set(TY key, TX position, TValue value);
        public TValue Get(TY key, TX position, TValue ifnone);
        public TValue Get(TY key, TX position, SearchMode type, TValue ifnone);
        public bool TryGet(TY key, TX position, SearchMode type, out TX actualPosition, [MaybeNullWhen(false)] out TValue value);

        public void CopyTo<T>(T destination)
            where T : IXYSingleTimeline<TY, TX, TValue>;
        public void CopyTo<T>(T destination, TX destOffset)
            where T : IXYSingleTimeline<TY, TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange)
            where T : IXYSingleTimeline<TY, TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange, TX destOffset)
            where T : IXYSingleTimeline<TY, TX, TValue>;

        public void CopyTo<T, TEnum>(T destination, TEnum keys)
            where T : IXYSingleTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;
        public void CopyTo<T, TEnum>(T destination, TEnum keys, TX destOffset)
            where T : IXYSingleTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<TX> srcRange)
            where T : IXYSingleTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<TX> srcRange, TX destOffset)
            where T : IXYSingleTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;

        public void CopyTo<T>(TY key, T destination)
            where T : IXSingleTimeline<TX, TValue>;
        public void CopyTo<T>(TY key, T destination, TX destOffset)
            where T : IXSingleTimeline<TX, TValue>;
        public void CopyTo<T>(TY key, T destination, Range<TX> srcRange)
            where T : IXSingleTimeline<TX, TValue>;
        public void CopyTo<T>(TY key, T destination, Range<TX> srcRange, TX destOffset)
            where T : IXSingleTimeline<TX, TValue>;
    }
}
