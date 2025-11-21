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
        /// <param name="historyData">A history data that stored to the undo stack.</param>
        /// <returns><see langword="true"/> if the current state changed from given <paramref name="historyData"/>.</returns>
        bool NeedsUpdateHistory(T historyData);

        /// <summary>
        /// Apply the operations from history.
        /// </summary>
        /// <param name="historyData">A history data that stored to the undo stack.</param>
        void ApplyHistory(T historyData);
    }

    public static class IHistoryOwnerExtensions
    {
        /// <summary>
        /// Marks the object as edited and records the current state in the undo history.
        /// </summary>
        /// <param name="obj">The object that owns the history to which the edit operation applies.</param>
        /// <param name="force">A value indicating whether to force the operation to record the current state in the undo history,
        /// even if no changes have been detected.</param>
        public static void OnEdit(this IHistoryOwner obj, bool force = false) => obj.History.PushUndo(force);
    }
}
