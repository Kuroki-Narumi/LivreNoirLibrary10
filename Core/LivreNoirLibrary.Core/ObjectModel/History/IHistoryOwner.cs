using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IHistoryOwner
    {
        /// <summary>
        /// Gets the history of operations or events associated with the current instance.
        /// </summary>
        IHistory History { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of history data that stored to the undo stack.</typeparam>
    public interface IHistoryOwner<T> : IHistoryOwner
    {
        /// <summary>
        /// Gets current state as <typeparamref name="T"/> for the undo history.
        /// </summary>
        /// <returns>A history data that stored to the undo stack.</returns>
        T GetHistoryData();

        /// <summary>
        /// Determine if the history needs to be updated.
        /// </summary>
        /// <param name="previous">A history data that stored to the undo stack.</param>
        /// <param name="current">A history data to compare.</param>
        /// <returns>A <see cref="bool"/> value indicating whether if <paramref name="previous"/> is equals to <paramref name="current"/>.</returns>
        bool HistoryEquals(T previous, T current);

        /// <summary>
        /// Ensure state before pushing <paramref name="historyData"/> to the undo history.
        /// </summary>
        /// <param name="historyData">A history data that stored to the undo stack.</param>
        void EnsureHistoryData(T historyData) { }

        /// <summary>
        /// Apply the operations from history.
        /// </summary>
        /// <param name="historyData">A history data that stored to the undo stack.</param>
        void ApplyHistory(T historyData);
    }
}
