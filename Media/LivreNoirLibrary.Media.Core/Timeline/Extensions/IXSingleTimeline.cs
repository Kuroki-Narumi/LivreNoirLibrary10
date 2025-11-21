using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public static partial class TimelineExtensions
    {
        extension<TX, TValue>(IXSingleTimeline<TX, TValue> obj) where TX : struct
        {
            /// <summary>
            /// Gets the current value of this <see cref="IXSingleTimeline{TX, TValue}"/>.
            /// </summary>
            /// <param name="position">the position to get value.</param>
            /// <returns>the value at or just before the specified position, if any; otherwise, the default value for <typeparamref name="TValue"/>.</returns>
            public TValue? Get(TX position) => obj.TryGetValue(position, SearchMode.PreviousOrEqual, out _, out var value) ? value : default;

            /// <summary>
            /// Gets the current value of this <see cref="IXSingleTimeline{TX, TValue}"/> with the fallback value.
            /// </summary>
            /// <param name="position">the position to get value.</param>
            /// <param name="ifNone">the fallback value if this <see cref="IXSingleTimeline{TX, TValue}"/> does not have a value at or before the specified position.</param>
            /// <returns>the value at or just before the specified position, if any; otherwise, the value specified by <paramref name="ifNone"/>.</returns>
            public TValue Get(TX position, TValue ifNone) => obj.TryGetValue(position, SearchMode.PreviousOrEqual, out _, out var value) ? value : ifNone;

            /// <inheritdoc cref=" IXSingleTimeline{TX, TValue}.CopyTo(IXSingleTimeline{TX, TValue}, TX)"/>
            public void CopyTo(IXSingleTimeline<TX, TValue> destination) => obj.CopyTo(destination, default);

            /// <inheritdoc cref=" IXSingleTimeline{TX, TValue}.CopyTo(IXSingleTimeline{TX, TValue}, Range{TX}, TX)"/>
            public void CopyTo(IXSingleTimeline<TX, TValue> destination, Range<TX> sourceRange) => obj.CopyTo(destination, sourceRange, default);
        }
    }
}