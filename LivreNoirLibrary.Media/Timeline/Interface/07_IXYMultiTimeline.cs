using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public interface IXYMultiTimeline<TY, TX, TValue> : IXYTimeline<TY, TX, TValue>
        where TX : struct
    {
        public void Add(TY key, TX position, TValue value);
        public void Add(TY key, TX position, IEnumerable<TValue> values);
        public void Add(TY key, IEnumerable<(TX, TValue)> values);
        public void Add(IEnumerable<(TY, TX, TValue)> values);
        public bool Remove(TY key, TX position, TValue value);
        public int Remove(TY key, TX position, IEnumerable<TValue> values);
        public int Remove(TY key, IEnumerable<(TX, TValue)> values);
        public int Remove(IEnumerable<(TY, TX, TValue)> values);

        public void RemoveIf(Predicate<TY, TX, TValue> predicate);
        public void RemoveIf(Predicate<TY, TX, TValue> predicate, Range<TX> range);
        public void RemoveIf(TY key, Predicate<TX, TValue> predicate);
        public void RemoveIf(TY key, Predicate<TX, TValue> predicate, Range<TX> range);

        public void MoveIf(Predicate<TY, TX, TValue> predicate, Func<TX, TX> converter);
        public void MoveIf(Predicate<TY, TX, TValue> predicate, Func<TX, TX> converter, Range<TX> range);
        public void MoveIf(TY key, Predicate<TValue> predicate, Func<TX, TX> converter);
        public void MoveIf(TY key, Predicate<TValue> predicate, Func<TX, TX> converter, Range<TX> range);

        public bool TryGet(TY key, TX position, SearchMode type, [MaybeNullWhen(false)] out List<TValue> values);
        public bool Find(Predicate<TY, TX, TValue> predicate, [MaybeNullWhen(false)] out TValue value);
        public bool Find(Predicate<TY, TX, TValue> predicate, Range<TX> range, [MaybeNullWhen(false)] out TValue value);
        public bool Find(TY key, Predicate<TX, TValue> predicate, [MaybeNullWhen(false)] out TValue value);
        public bool Find(TY key, Predicate<TX, TValue> predicate, Range<TX> range, [MaybeNullWhen(false)] out TValue value);
        public bool Find(TY key, TX position, Predicate<TValue> predicate, [MaybeNullWhen(false)] out TValue value);
        public List<(TY, TX, TValue)> FindAll(Predicate<TY, TX, TValue> predicate);
        public List<(TY, TX, TValue)> FindAll(Predicate<TY, TX, TValue> predicate, Range<TX> range);
        public List<(TX, TValue)> FindAll(TY key, Predicate<TValue> predicate);
        public List<(TX, TValue)> FindAll(TY key, Predicate<TValue> predicate, Range<TX> range);
        public List<TValue> FindAll(TY key, TX position, Predicate<TValue> predicate);

        public void CopyTo<T>(T destination)
            where T : IXYMultiTimeline<TY, TX, TValue>;
        public void CopyTo<T>(T destination, TX destOffset)
            where T : IXYMultiTimeline<TY, TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange)
            where T : IXYMultiTimeline<TY, TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange, TX destOffset)
            where T : IXYMultiTimeline<TY, TX, TValue>;

        public void CopyTo<T, TEnum>(T destination, TEnum keys)
            where T : IXYMultiTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;
        public void CopyTo<T, TEnum>(T destination, TEnum keys, TX destOffset)
            where T : IXYMultiTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<TX> srcRange)
            where T : IXYMultiTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<TX> srcRange, TX destOffset)
            where T : IXYMultiTimeline<TY, TX, TValue>
            where TEnum : IEnumerable<TY>;

        public void CopyTo<T>(TY key, T destination)
            where T : IXMultiTimeline<TX, TValue>;
        public void CopyTo<T>(TY key, T destination, TX destOffset)
            where T : IXMultiTimeline<TX, TValue>;
        public void CopyTo<T>(TY key, T destination, Range<TX> srcRange)
            where T : IXMultiTimeline<TX, TValue>;
        public void CopyTo<T>(TY key, T destination, Range<TX> srcRange, TX destOffset)
            where T : IXMultiTimeline<TX, TValue>;
    }
}
