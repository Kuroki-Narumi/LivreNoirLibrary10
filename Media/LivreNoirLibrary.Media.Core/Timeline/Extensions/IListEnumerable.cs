using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media
{
    public static partial class TimelineExtensions
    {
        extension<TX, TValue>(IListEnumerable<TX, TValue> obj) where TX : struct
        {
            public IEnumerable<(TX, TValue)> EnumerateValues()
            {
                foreach (var (pos, list) in obj.EnumerateList())
                {
                    var count = list.Count;
                    for (var i = 0; i < count; i++)
                    {
                        yield return (pos, list[i]);
                    }
                }
            }

            public IEnumerable<(TX, TValue)> EnumerateValues(Range<TX> range)
            {
                foreach (var (pos, list) in obj.EnumerateList(range))
                {
                    var count = list.Count;
                    for (var i = 0; i < count; i++)
                    {
                        yield return (pos, list[i]);
                    }
                }
            }

            private static bool Find(IEnumerable<(TX, List<TValue>)> enumer, Predicate<TX, TValue> predicate, out TX position, [MaybeNullWhen(false)] out TValue value)
            {
                foreach (var (pos, list) in enumer)
                {
                    if (list.Find(value => predicate(pos, value)) is { } v)
                    {
                        value = v;
                        position = pos;
                        return true;
                    }
                }
                value = default;
                position = default;
                return false;
            }

            /// <summary>
            /// Attempts to find the first value that match the condition defined by the specified predicate.
            /// </summary>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="position">When this method returns, contains the position of the nearest value found, if any; 
            /// otherwise, the default value for the type.</param>
            /// <param name="value">When this method returns, contains the value associated with the nearest value, if found; 
            /// otherwise, the default value for the type.</param>
            /// <returns><see langword="true"/> if a value is found; otherwise, <see langword="false"/>.</returns>
            public bool Find(Predicate<TX, TValue> predicate, out TX position, [MaybeNullWhen(false)] out TValue value)
                => Find(obj.EnumerateList(), predicate, out position, out value);

            /// <summary>
            /// Attempts to find the first value that match the condition defined by the specified predicate within the specified range.
            /// </summary>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="range">The range of positions to find.</param>
            /// <param name="position">When this method returns, contains the position of the nearest value found, if any; 
            /// otherwise, the default value for the type.</param>
            /// <param name="value">When this method returns, contains the value associated with the nearest value, if found; 
            /// otherwise, the default value for the type.</param>
            /// <returns><see langword="true"/> if a value is found; otherwise, <see langword="false"/>.</returns>
            public bool Find(Predicate<TX, TValue> predicate, Range<TX> range, out TX position, [MaybeNullWhen(false)] out TValue value)
                => Find(obj.EnumerateList(range), predicate, out position, out value);

            private static bool FindLast(IEnumerable<(TX, List<TValue>)> enumer, Predicate<TX, TValue> predicate, out TX position, [MaybeNullWhen(false)] out TValue value)
            {
                foreach (var (pos, list) in enumer)
                {
                    if (list.FindLast(value => predicate(pos, value)) is { } v)
                    {
                        value = v;
                        position = pos;
                        return true;
                    }
                }
                value = default;
                position = default;
                return false;
            }

            /// <summary>
            /// Attempts to find the last value that match the condition defined by the specified predicate.
            /// </summary>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="position">When this method returns, contains the position of the nearest value found, if any; 
            /// otherwise, the default value for the type.</param>
            /// <param name="value">When this method returns, contains the value associated with the nearest value, if found; 
            /// otherwise, the default value for the type.</param>
            /// <returns><see langword="true"/> if a value is found; otherwise, <see langword="false"/>.</returns>
            public bool FindLast(Predicate<TX, TValue> predicate, out TX position, [MaybeNullWhen(false)] out TValue value)
                => FindLast(obj.ReverseEnumerateList(), predicate, out position, out value);

            /// <summary>
            /// Attempts to find the last value that match the condition defined by the specified predicate within the specified range.
            /// </summary>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="range">The range of positions to find.</param>
            /// <param name="position">When this method returns, contains the position of the nearest value found, if any; 
            /// otherwise, the default value for the type.</param>
            /// <param name="value">When this method returns, contains the value associated with the nearest value, if found; 
            /// otherwise, the default value for the type.</param>
            /// <returns><see langword="true"/> if a value is found; otherwise, <see langword="false"/>.</returns>
            public bool FindLast(Predicate<TX, TValue> predicate, Range<TX> range, out TX position, [MaybeNullWhen(false)] out TValue value)
                => FindLast(obj.ReverseEnumerateList(range), predicate, out position, out value);

            /// <summary>
            /// Find all values that match the condition defined by the specified predicate.
            /// </summary>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="list"></param>
            /// <returns></returns>
            public List<(TX, TValue)> FindAll(Predicate<TX, TValue> predicate, List<(TX, TValue)>? list = null)
            {
                list ??= [];
                foreach (var (position, l) in obj.EnumerateList())
                {
                    foreach (var value in l.AsSpan())
                    {
                        if (predicate(position, value))
                        {
                            list.Add((position, value));
                        }
                    }
                }
                return list;
            }

            /// <summary>
            /// Find all values that match the condition defined by the specified predicate within the specified range.
            /// </summary>
            /// <param name="predicate">the predicate that defines the conditions to match.</param>
            /// <param name="list"></param>
            /// <returns></returns>
            public List<(TX, TValue)> FindAll(Predicate<TX, TValue> predicate, Range<TX> range, List<(TX, TValue)>? list = null)
            {
                list ??= [];
                foreach (var (position, l) in obj.EnumerateList(range))
                {
                    foreach (var value in l.AsSpan())
                    {
                        if (predicate(position, value))
                        {
                            list.Add((position, value));
                        }
                    }
                }
                return list;
            }
        }
    }
}
