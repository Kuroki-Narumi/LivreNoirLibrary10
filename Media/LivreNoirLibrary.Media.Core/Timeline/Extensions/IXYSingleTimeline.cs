using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public static partial class TimelineExtensions
    {
        extension<TY, TX, TValue>(IXYSingleTimeline<TY, TX, TValue> obj) where TX : struct
        {
            /// <summary>
            /// Gets the current value of this <see cref="IXSingleTimeline{TX, TValue}"/>.
            /// </summary>
            /// <param name="key">the key to get value.</param>
            /// <param name="position">the position to get value.</param>
            /// <returns>the value at or just before the specified position, if any; otherwise, the default value for <typeparamref name="TValue"/>.</returns>
            public TValue? Get(TY key, TX position) => obj.TryGetValue(key, position, SearchMode.PreviousOrEqual, out _, out var value) ? value : default;

            /// <summary>
            /// Gets the current value of this <see cref="IXSingleTimeline{TX, TValue}"/> with the fallback value.
            /// </summary>
            /// <param name="key">the key to get value.</param>
            /// <param name="position">the position to get value.</param>
            /// <param name="ifNone">the fallback value if this <see cref="IXSingleTimeline{TX, TValue}"/> does not have a value at or before the specified position.</param>
            /// <returns>the value at or just before the specified position, if any; otherwise, the value specified by <paramref name="ifNone"/>.</returns>
            public TValue Get(TY key, TX position, TValue ifNone) => obj.TryGetValue(key, position, SearchMode.PreviousOrEqual, out _, out var value) ? value : ifNone;

            /// <inheritdoc cref=" IXYSingleTimeline{TY, TX, TValue}.CopyTo(IXYSingleTimeline{TY, TX, TValue}, TX)"/>
            public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination) => obj.CopyTo(destination, default);

            /// <inheritdoc cref=" IXYSingleTimeline{TY, TX, TValue}.CopyTo(IXYSingleTimeline{TY, TX, TValue}, Range{TX}, TX)"/>
            public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, Range<TX> sourceRange) => obj.CopyTo(destination, sourceRange, default);

            /// <inheritdoc cref=" IXYSingleTimeline{TY, TX, TValue}.CopyTo(IXYSingleTimeline{TY, TX, TValue}, IEnumerable{TY}, TX)"/>
            public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, IEnumerable<TY> keys) => obj.CopyTo(destination, keys, default);

            /// <inheritdoc cref=" IXYSingleTimeline{TY, TX, TValue}.CopyTo(IXYSingleTimeline{TY, TX, TValue}, IEnumerable{TY}, Range{TX}, TX)"/>
            public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, Range<TX> sourceRange, IEnumerable<TY> keys) => obj.CopyTo(destination, keys, sourceRange, default);

            /// <inheritdoc cref=" IXYSingleTimeline{TY, TX, TValue}.CopyTo(TY, IXSingleTimeline{TX, TValue}, TX)"/>
            public void CopyTo(TY key, IXSingleTimeline<TX, TValue> destination) => obj.CopyTo(key, destination, default);

            /// <inheritdoc cref=" IXYSingleTimeline{TY, TX, TValue}.CopyTo(TY, IXSingleTimeline{TX, TValue}, Range{TX}, TX)"/>
            public void CopyTo(TY key, IXSingleTimeline<TX, TValue> destination, Range<TX> sourceRange) => obj.CopyTo(key, destination, sourceRange, default);
        }
    }
}