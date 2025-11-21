using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface ITimeline<TX> : IPositionRange<TX>
        where TX : struct
    {
        /// <summary>
        /// The number of items contained in this <see cref="ITimeline{TX}"/>.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Indicates whether this <see cref="ITimeline{TX}"/> is empty.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if this <see cref="ITimeline{TX}"/> contains no items; otherwise, <see langword="false"/>.
        /// </returns>
        bool IsEmpty => Count is 0;

        /// <summary>
        /// Removes all items from this <see cref="ITimeline{TX}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the span of positions where the value exists in this <see cref="ITimeline{TX}"/>.
        /// </summary>
        ReadOnlySpan<TX> GetPositions();

        /// <summary>
        /// Gets the span of positions where the value exists in this <see cref="ITimeline{TX}"/>.
        /// </summary>
        /// <param name="range">The range of positions to get the span.</param>
        ReadOnlySpan<TX> GetPositions(Range<TX> range);

        /// <summary>
        /// Removes the item at the specified position.
        /// </summary>
        /// <param name="position">The position of the item to remove.</param>
        /// <returns>
        /// <see langword="true"/> if the item was successfully removed; otherwise, <see langword="false"/>.
        /// </returns>
        bool RemoveAt(TX position);

        /// <summary>
        /// Removes items within the specified range from this <see cref="ITimeline{TX}"/>.
        /// </summary>
        /// <param name="range">The range of positions to remove.</param>
        void RemoveRange(Range<TX> range);

        /// <summary>
        /// Moves all items according to the specified converter.
        /// </summary>
        /// <param name="converter">A function that takes the original position and returns the destination.</param>
        void Move(Func<TX, TX> converter);

        /// <summary>
        /// Moves items within the specified range according to the specified converter.
        /// </summary>
        /// <param name="converter">A function that takes the original position and returns the destination.</param>
        /// <param name="range">The range of positions to move.</param>
        void Move(Func<TX, TX> converter, Range<TX> range);
    }
}
