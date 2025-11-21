using System;

namespace LivreNoirLibrary.Media
{
    public interface IPositionRange<TX>
    {
        /// <summary>
        /// The position of the first item.
        /// </summary>
        /// <returns>
        /// The position of the first item. If this <see cref="ITimeline{TX}"/> is empty, return the default value of <typeparamref name="TX"/>.
        /// </returns>
        TX FirstPosition { get; }

        /// <summary>
        /// The position of the last item.
        /// </summary>
        /// <returns>
        /// The position of the last item. If this <see cref="ITimeline{TX}"/> is empty, return the default value of <typeparamref name="TX"/>.
        /// </returns>
        TX LastPosition { get; }
    }
}
