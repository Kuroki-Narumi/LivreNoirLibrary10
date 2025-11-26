using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IXYSingleTimeline<TY, TX, TValue> : IXYTimeline<TY, TX, TValue>
        where TX : struct
    {
        /// <summary>
        /// Set the specified value at the specified key and the specified position.
        /// </summary>
        /// <param name="key">the key to set the value.</param>
        /// <param name="position">the position to set the value.</param>
        /// <param name="value">the value to set.</param>
        void Set(TY key, TX position, TValue value);

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key and the specified position, using the given search mode.
        /// </summary>
        /// <param name="key">the key to search for.</param>
        /// <param name="position">The position to search for.</param>
        /// <param name="type">The search mode that determines how the position is matched.</param>
        /// <param name="actualPosition">When this method returns, contains the actual position that matches the search criteria, if the search is successful;
        /// otherwise, the default value for <typeparamref name="TX"/>.</param>
        /// <param name="value">When this method returns, contains the value associated with the found position, if the search is successful;
        /// otherwise, the default value for <typeparamref name="TValue"/>.</param>
        /// <returns><see langword="true"/> if an value is found; otherwise, <see langword="false"/>.</returns>
        bool TryGetValue(TY key, TX position, SearchMode mode, out TX actualPosition, [MaybeNullWhen(false)] out TValue value);

        /// <summary>
        /// Attempts to find the value with the specified key and the nearest to the specified position.
        /// </summary>
        /// <param name="key">the key to search for the nearest value.</param>
        /// <param name="position">The position to search for the nearest value.</param>
        /// <param name="actualPosition">When this method returns, contains the position of the nearest value found, if any; 
        /// otherwise, the default value for the type.</param>
        /// <param name="value">When this method returns, contains the value associated with the nearest value, if found; 
        /// otherwise, the default value for the type.</param>
        /// <returns><see langword="true"/> if a nearest value is found; otherwise, <see langword="false"/>.</returns>
        bool TryGetNearest(TY key, TX position, out TX actualPosition, [MaybeNullWhen(false)] out TValue value);

        /// <summary>
        /// Copies values to the specified destination timeline, starting at the given destination offset.
        /// </summary>
        /// <param name="destination">The timeline to which the values will be copied.</param>
        /// <param name="destOffset">The offset in the destination timeline at which to begin copying the values.</param>
        void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, TX destOffset);

        /// <summary>
        /// Copies a range of timeline values to the specified destination timeline, starting at the given destination offset.
        /// </summary>
        /// <param name="destination">The timeline to which the values will be copied.</param>
        /// <param name="sourceRange">The range of values to copy from the source timeline. Only values within this range will be copied.</param>
        /// <param name="destOffset">The offset in the destination timeline at which to begin copying the values.</param>
        void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, Range<TX> sourceRange, TX destOffset);

        /// <summary>
        /// Copies values within the specified keys to the specified destination timeline, starting at the given destination offset.
        /// </summary>
        /// <param name="destination">The timeline to which the values will be copied.</param>
        /// <param name="keys">an enumerable object that iterate keys to copy.</param>
        /// <param name="destOffset">The offset in the destination timeline at which to begin copying the values.</param>
        void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, IEnumerable<TY> keys, TX destOffset);

        /// <summary>
        /// Copies a range of timeline values within the specified keys to the specified destination timeline, starting at the given destination offset.
        /// </summary>
        /// <param name="destination">The timeline to which the values will be copied.</param>
        /// <param name="keys">an enumerable object that iterate keys to copy.</param>
        /// <param name="sourceRange">The range of values to copy from the source timeline. Only values within this range will be copied.</param>
        /// <param name="destOffset">The offset in the destination timeline at which to begin copying the values.</param>
        void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, IEnumerable<TY> keys, Range<TX> sourceRange, TX destOffset);

        /// <summary>
        /// Copies values at the specified key to the specified destination timeline, starting at the given destination offset.
        /// </summary>
        /// <param name="key">the key to copy.</param>
        /// <param name="destination">The timeline to which the values will be copied.</param>
        /// <param name="destOffset">The offset in the destination timeline at which to begin copying the values.</param>
        void CopyTo(TY key, IXSingleTimeline<TX, TValue> destination, TX destOffset);

        /// <summary>
        /// Copies a range of timeline values at the specified key to the specified destination timeline, starting at the given destination offset.
        /// </summary>
        /// <param name="key">the key to copy.</param>
        /// <param name="destination">The timeline to which the values will be copied.</param>
        /// <param name="sourceRange">The range of values to copy from the source timeline. Only values within this range will be copied.</param>
        /// <param name="destOffset">The offset in the destination timeline at which to begin copying the values.</param>
        void CopyTo(TY key, IXSingleTimeline<TX, TValue> destination, Range<TX> sourceRange, TX destOffset);
    }
}
