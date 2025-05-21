using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface ITimeline<TX, TValue>
        where TX : struct
    {
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ITimeline{TX, TValue}"/>
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ITimeline{TX, TValue}"/>.
        /// </returns>
        public int Count { get; }

        /// <summary>
        /// Gets whether the <see cref="ITimeline{TX, TValue}"/> is empty.
        /// </summary>
        /// <returns>
        /// <see cref="bool">true</see> if the <see cref="ITimeline{TX, TValue}"/> is empty; otherwise, <see cref="bool">false</see>.
        /// </returns>
        public bool IsEmpty();

        /// <summary>
        /// Gets the position of the first element.
        /// </summary>
        /// <returns>
        /// the position of the first element if the <see cref="ITimeline{TX, TValue}"/> is not empty; otherwise, the default value of <typeparamref name="TX"/>.
        /// </returns>
        public TX FirstPosition { get; }

        /// <summary>
        /// Gets the position of the last element.
        /// </summary>
        /// <returns>
        /// the position of the last element if the <see cref="ITimeline{TX, TValue}"/> is not empty; otherwise, the default value of <typeparamref name="TX"/>.
        /// </returns>
        public TX LastPosition { get; }

        /// <summary>
        /// Gets the array of positions in the <see cref="ITimeline{TX, TValue}"/>
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<TX> GetPositions();
        public ReadOnlySpan<TX> GetPositions(Range<TX> range);

        /// <summary>
        /// Removes all items from the <see cref="ITimeline{TX, TValue}"/>.
        /// </summary>
        public void Clear();
        public bool RemoveAt(TX position);
        public void RemoveRange(Range<TX> range);

        public void Move(TX from, TX to);
        public void Move(Func<TX, TX> converter);
        public void Move(Func<TX, TX> converter, Range<TX> range);
        public void InsertSpace(TX offset, TX length);
        public void DeleteSpace(TX offset, TX length);
    }
}
