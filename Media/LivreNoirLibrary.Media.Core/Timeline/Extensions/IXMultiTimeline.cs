using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static partial class TimelineExtensions
    {
        extension<TX, TValue>(IXMultiTimeline<TX, TValue> obj) where TX : struct
        {
            /// <summary>
            /// Adds a new value to the end of the value list at the specified position.
            /// </summary>
            /// <param name="position">the position to add value.</param>
            /// <param name="value">the value to add.</param>
            public void Add(TX position, TValue value) => obj.GetOrAddList(position).Add(value);

            /// <summary>
            /// Inserts a new value to the beginning of the value list at the specified position.
            /// </summary>
            /// <param name="position">the position to add value.</param>
            /// <param name="value">the value to add.</param>
            public void AddToFront(TX position, TValue value) => obj.GetOrAddList(position).Insert(0, value);

            /// <summary>
            /// Adds new values to the value list at the specified position.
            /// </summary>
            /// <param name="position">the position to add value.</param>
            /// <param name="values">the values to add.</param>
            public void AddRange(TX position, IEnumerable<TValue> values) => obj.GetOrAddList(position).AddRange(values);

            /// <summary>
            /// Adds position and value pairs given as an enumerable object.
            /// </summary>
            /// <param name="values">the position and value pairs to add.</param>
            public void AddRange(IEnumerable<(TX, TValue)> values)
            {
                foreach (var (pos, value) in values)
                {
                    obj.Add(pos, value);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void RemoveEmptyList(TX position, List<TValue> list)
            {
                if (list.Count is 0)
                {
                    obj.RemoveAt(position);
                }
            }

            /// <summary>
            /// Removes the value in the value list at the specified position.
            /// </summary>
            /// <param name="position">the position to remove value.</param>
            /// <param name="value">the value to remove.</param>
            /// <returns><see langword="true"/> if the value was successfully removed; otherwise, <see langword="false"/>.</returns>
            public bool Remove(TX position, TValue value)
            {
                if (obj.TryGetList(position, out var list) && list.Remove(value))
                {
                    obj.RemoveEmptyList(position, list);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Removes the values in the value list at the specified position.
            /// </summary>
            /// <param name="position">the position to remove value.</param>
            /// <param name="values">the values to remove.</param>
            /// <returns>number of removed values.</returns>
            public int Remove(TX position, IEnumerable<TValue> values)
            {
                if (obj.TryGetList(position, out var list))
                {
                    var count = 0;
                    foreach (var value in values)
                    {
                        if (list.Remove(value))
                        {
                            count++;
                        }
                    }
                    obj.RemoveEmptyList(position, list);
                    return count;
                }
                return 0;
            }

            /// <summary>
            /// Removes position and value pairs given as an enumerable object.
            /// </summary>
            /// <param name="values">the position and value pairs to remove.</param>
            /// <returns>number of removed values.</returns>
            public int Remove(IEnumerable<(TX, TValue)> values)
            {
                var count = 0;
                foreach (var (position, value) in values)
                {
                    if (obj.Remove(position, value))
                    {
                        count++;
                    }
                }
                return count;
            }

            /// <summary>
            /// Removes all the values at the specified position that match the conditions defined by the specified predicate.
            /// </summary>
            /// <param name="position">the position to remove value.</param>
            /// <param name="selector">the predicate that defines the conditions to remove.</param>
            /// <returns>number of removed values.</returns>
            public int RemoveAll(TX position, Predicate<TValue> selector)
            {
                if (obj.TryGetList(position, out var list))
                {
                    var count = list.RemoveAll(selector);
                    obj.RemoveEmptyList(position, list);
                    return count;
                }
                return 0;
            }

            private int RemoveAll(IEnumerable<(TX, List<TValue>)> enumer, Predicate<TX, TValue> selector)
            {
                using var o = ObjectPool.RentHashSet<TX>(out var removeList);
                var count = 0;
                foreach (var (position, list) in enumer)
                {
                    count += list.RemoveAll(value => selector(position, value));
                    if (list.Count is 0)
                    {
                        removeList.Add(position);
                    }
                }
                foreach (var position in removeList)
                {
                    obj.RemoveAt(position);
                }
                return count;
            }

            /// <summary>
            /// Removes all the values that match the conditions defined by the specified predicate.
            /// </summary>
            /// <param name="selector">the predicate that defines the conditions to remove.</param>
            /// <returns>number of removed values.</returns>
            public int RemoveAll(Predicate<TX, TValue> selector) => RemoveAll(obj, obj.EnumerateList(), selector);

            /// <summary>
            /// Removes all the values that match the conditions defined by the specified predicate within the specified range.
            /// </summary>
            /// <param name="selector">the predicate that defines the conditions to remove.</param>
            /// <param name="range">The range of positions to remove.</param>
            /// <returns>number of removed values.</returns>
            public int RemoveAll(Predicate<TX, TValue> selector, Range<TX> range) => RemoveAll(obj, obj.EnumerateList(range), selector);

            /// <summary>
            /// Moves all the values that match the conditions defined by the specified predicate.
            /// </summary>
            /// <param name="from">the position to move value.</param>
            /// <param name="to">the destination for move.</param>
            /// <param name="selector">the predicate that defines the conditions to move.</param>
            public void MoveAll(TX from, TX to, Predicate<TValue> selector)
            {
                if (!EqualityComparer<TX>.Default.Equals(from, to) && obj.TryGetList(from, out var list))
                {
                    using var o = ObjectPool.RentList<TValue>(out var moveList);
                    list.RemoveAll(value =>
                    {
                        if (selector(value))
                        {
                            moveList.Add(value);
                            return true;
                        }
                        return false;
                    });
                    obj.RemoveEmptyList(from, list);
                    obj.AddRange(to, moveList);
                }
            }

            private void MoveAll(IEnumerable<(TX, List<TValue>)> enumer, Predicate<TX, TValue> selector, Func<TX, TX> converter)
            {
                using var o1 = ObjectPool.RentDictionary<TX, List<TValue>>(out var moveListList);
                using var o2 = ObjectPool.RentHashSet<TX>(out var removeList);
                foreach (var (position, list) in enumer)
                {
                    var newPosition = converter(position);
                    if (EqualityComparer<TX>.Default.Equals(position, newPosition))
                    {
                        continue;
                    }
                    var moveList = moveListList.GetOrAdd(newPosition);
                    list.RemoveAll(value =>
                    {
                        if (selector(position, value))
                        {
                            moveList.Add(value);
                            return true;
                        }
                        return false;
                    });
                    if (list.Count is 0)
                    {
                        removeList.Add(position);
                    }
                }
                foreach (var (position, list) in moveListList)
                {
                    obj.AddRange(position, list);
                    removeList.Remove(position);
                }
                foreach (var position in removeList)
                {
                    obj.RemoveAt(position);
                }
            }

            /// <summary>
            /// Moves all the values that match the conditions defined by the specified predicate according to the specified converter.
            /// </summary>
            /// <param name="selector">the predicate that defines the conditions to move.</param>
            /// <param name="converter">A function that takes the original position and returns the destination.</param>
            public void MoveAll(Predicate<TX, TValue> selector, Func<TX, TX> converter) => MoveAll(obj, obj.EnumerateList(), selector, converter);

            /// <summary>
            /// Moves all the values that match the conditions defined by the specified predicate within the specified range according to the specified converter.
            /// </summary>
            /// <param name="selector">the predicate that defines the conditions to move.</param>
            /// <param name="converter">A function that takes the original position and returns the destination.</param>
            /// <param name="range">The range of positions to move.</param>
            public void MoveAll(Predicate<TX, TValue> selector, Func<TX, TX> converter, Range<TX> range) => MoveAll(obj, obj.EnumerateList(range), selector, converter);

            /// <summary>
            /// Attempts to find the first value at the specified position that match the condition defined by the specified predicate.
            /// </summary>
            /// <param name="position">the position to find value.</param>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="value">When this method returns, contains the value associated with the nearest value, if found; 
            /// otherwise, the default value for the type.</param>
            /// <returns><see langword="true"/> if a value is found; otherwise, <see langword="false"/>.</returns>
            public bool Find(TX position, Predicate<TValue> predicate, [MaybeNullWhen(false)] out TValue value)
            {
                if (obj.TryGetList(position, out var list))
                {
                    value = list.Find(predicate);
                    return value is not null;
                }
                value = default;
                return false;
            }

            /// <inheritdoc cref=" IXMultiTimeline{TX, TValue}.CopyTo(IXMultiTimeline{TX, TValue}, TX)"/>
            public void CopyTo(IXMultiTimeline<TX, TValue> destination) => obj.CopyTo(destination, Range<TX>.All, default);

            /// <inheritdoc cref=" IXMultiTimeline{TX, TValue}.CopyTo(IXMultiTimeline{TX, TValue}, Range{TX}, TX)"/>
            public void CopyTo(IXMultiTimeline<TX, TValue> destination, Range<TX> sourceRange) => obj.CopyTo(destination, sourceRange, default);
        }
    }
}