using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public interface IXMultiTimeline<TX, TValue> : IXTimeline<TX, TValue>
        where TX : struct
    {
        public IEnumerable<(TX, List<TValue>)> EachList();
        public IEnumerable<(TX, List<TValue>)> EachList(Range<TX> range);

        public void Add(TX position, TValue value);
        public void AddToFront(TX position, TValue value);
        public void Add<TEnumerable>(TX position, TEnumerable values) where TEnumerable : IEnumerable<TValue>;
        public void Add<TEnumerable>(TEnumerable values) where TEnumerable : IEnumerable<(TX, TValue)>;
        public bool Remove(TX position, TValue value);
        public int Remove<TEnumerable>(TX position, TEnumerable values) where TEnumerable : IEnumerable<TValue>;
        public int Remove<TEnumerable>(TEnumerable values) where TEnumerable : IEnumerable<(TX, TValue)>;

        public int RemoveIf(Predicate<TX, TValue> predicate);
        public int RemoveIf(Predicate<TX, TValue> predicate, Range<TX> range);
        public int RemoveIf(TX position, Predicate<TValue> predicate);

        public void MoveIf(Predicate<TX, TValue> predicate, Func<TX, TX> converter);
        public void MoveIf(Predicate<TX, TValue> predicate, Func<TX, TX> converter, Range<TX> range);
        public void MoveIf(TX from, TX to, Predicate<TValue> predicate);

        public bool TryGet(TX position, SearchMode type, out TX actualPosition, [MaybeNullWhen(false)] out List<TValue> values);
        public bool Find(Predicate<TX, TValue> predicate, out TX position, [MaybeNullWhen(false)] out TValue value);
        public bool Find(Predicate<TX, TValue> predicate, Range<TX> range, out TX position, [MaybeNullWhen(false)] out TValue value);
        public bool Find(TX position, Predicate<TValue> predicate, [MaybeNullWhen(false)] out TValue value);
        public List<(TX, TValue)> FindAll(Predicate<TX, TValue> predicate);
        public List<(TX, TValue)> FindAll(Predicate<TX, TValue> predicate, Range<TX> range);
        public List<TValue> FindAll(TX position, Predicate<TValue> predicate);

        public void CopyTo<T>(T destination)
            where T : IXMultiTimeline<TX, TValue>;
        public void CopyTo<T>(T destination, TX destOffset)
            where T : IXMultiTimeline<TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange)
            where T : IXMultiTimeline<TX, TValue>;
        public void CopyTo<T>(T destination, Range<TX> srcRange, TX destOffset)
            where T : IXMultiTimeline<TX, TValue>;
    }
}
