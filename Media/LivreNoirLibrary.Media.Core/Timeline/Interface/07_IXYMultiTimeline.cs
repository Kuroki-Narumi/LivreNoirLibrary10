using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IXYMultiTimeline<TY, TX, TValue> : IXYTimeline<TY, TX, TValue>
        where TX : struct
    {
        /// <summary>
        /// Attempts to retrieve the value list associated with the specified key and the specified position.
        /// </summary>
        /// <param name="key">the key to serach for.</param>
        /// <param name="position">The position to search for.</param>
        /// <param name="values">When this method returns, the value list associated with the specified key and the found position, if the search is successful;
        /// otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if an value is found; otherwise, <see langword="false"/>.</returns>
        bool TryGetList(TY key, TX position, [MaybeNullWhen(false)] out List<TValue> values);

        /// <summary>
        /// Gets the list of values associated with the specified key and the specified position, or creates and adds a new list if none
        /// exists.
        /// </summary>
        /// <param name="key">The key for which to retrieve the associated list of values.</param>
        /// <param name="position">The position for which to retrieve the associated list of values.</param>
        /// <returns>A list of values associated with the specified key and the specified position. If no list exists, a new empty list is created, added, and returned.</returns>
        List<TValue> GetOrAddList(TY key, TX position);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs.
        /// </summary>
        /// <returns>an enumerable that can be used to iterate.</returns>
        IEnumerable<(TY, TX, List<TValue>)> EnumerateList();

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs within the specified range.
        /// </summary>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TY, TX, List<TValue>)> EnumerateList(Range<TX> range);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs in reverse order.
        /// </summary>
        /// <returns>an enumerable that can be used to iterate.</returns>
        IEnumerable<(TY, TX, List<TValue>)> ReverseEnumerateList();

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs within the specified range in reverse order.
        /// </summary>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TY, TX, List<TValue>)> ReverseEnumerateList(Range<TX> range);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs.
        /// </summary>
        /// <param name="key">the key to iterate.</param>
        /// <returns>an enumerable that can be used to iterate.</returns>
        IEnumerable<(TX, List<TValue>)> EnumerateList(TY key);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs within the specified range.
        /// </summary>
        /// <param name="key">the key to iterate.</param>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TX, List<TValue>)> EnumerateList(TY key, Range<TX> range);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs in reverse order.
        /// </summary>
        /// <param name="key">the key to iterate.</param>
        /// <returns>an enumerable that can be used to iterate.</returns>
        IEnumerable<(TX, List<TValue>)> ReverseEnumerateList(TY key);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs within the specified range in reverse order.
        /// </summary>
        /// <param name="key">the key to iterate.</param>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TX, List<TValue>)> ReverseEnumerateList(TY key, Range<TX> range);

        /// <summary>
        /// Attempts to retrieve the value list associated with the specified key and the specified position, using the given search mode.
        /// </summary>
        /// <param name="key">the key to search for.</param>
        /// <param name="position">The position to search for.</param>
        /// <param name="type">The search mode that determines how the position is matched.</param>
        /// <param name="actualPosition">When this method returns, contains the actual position that matches the search criteria, if the search is successful;
        /// otherwise, the default value for <typeparamref name="TX"/>.</param>
        /// <param name="values">When this method returns, the value list associated with the found position, if the search is successful;
        /// otherwise, the default value for <typeparamref name="TValue"/>.</param>
        /// <returns><see langword="true"/> if an value is found; otherwise, <see langword="false"/>.</returns>
        bool TryGetValue(TY key, TX position, SearchMode mode, out TX actualPosition, [MaybeNullWhen(false)] out List<TValue> values);

        /// <summary>
        /// Attempts to find the value list with the specified key and the nearest to the specified position.
        /// </summary>
        /// <param name="key">the key to search for.</param>
        /// <param name="position">The position to search for the nearest value.</param>
        /// <param name="actualPosition">When this method returns, contains the position of the nearest value found, if any;
        /// otherwise, the default value for the type.</param>
        /// <param name="values">When this method returns, the value list associated with the nearest value, if found; otherwise, the
        /// default value for the type.</param>
        /// <returns><see langword="true"/> if a nearest value is found; otherwise, <see langword="false"/>.</returns>
        bool TryGetNearest(TY key, TX position, out TX actualPosition, [MaybeNullWhen(false)] out List<TValue> values);

        void CopyTo(IXYMultiTimeline<TY, TX, TValue> destination, TX destOffset);
        void CopyTo(IXYMultiTimeline<TY, TX, TValue> destination, Range<TX> sourceRange, TX destOffset);
        void CopyTo<TEnum>(IXYMultiTimeline<TY, TX, TValue> destination, TEnum keys, TX destOffset) where TEnum : IEnumerable<TY>;
        void CopyTo<TEnum>(IXYMultiTimeline<TY, TX, TValue> destination, TEnum keys, Range<TX> sourceRange, TX destOffset) where TEnum : IEnumerable<TY>;
        void CopyTo(TY key, IXMultiTimeline<TX, TValue> destination, TX destOffset);
        void CopyTo(TY key, IXMultiTimeline<TX, TValue> destination, Range<TX> sourceRange, TX destOffset);
    }
}
