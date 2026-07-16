using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IHistory
    {
        /// <summary>
        /// Gets the number of undo operations currently available in the undo stack.
        /// </summary>
        int UndoCount { get; }

        /// <summary>
        /// Gets the number of redo operations currently available in the redo stack.
        /// </summary>
        int RedoCount { get; }

        /// <summary>
        /// Clear this history.
        /// </summary>
        void Clear();

        /// <summary>
        /// Pushes the current state onto the undo stack, allowing it to be restored later.
        /// </summary>
        /// <param name="force">A value indicating whether to force the state to be pushed onto the undo stack, even if it is identical to
        /// the previous state.</param>
        /// <returns><see langword="true"/> if the undo history pushed; otherwise, <see langword="false"/>.</returns>
        bool PushUndo(bool force);

        /// <summary>
        /// Reverts the most recent operation, restoring the state to its previous condition.
        /// </summary>
        /// <remarks>This method undoes the last operation performed. 
        /// If no operations are available to undo, calling this method may have no effect.</remarks>
        /// <returns><see langword="true"/> if the undo operation is successful; otherwise, <see langword="false"/>.</returns>
        bool Undo();

        /// <summary>
        /// Reapplies the most recently undone operation, if available.
        /// </summary>
        /// <remarks>This method restores the state to what it was before the last undo operation.  If
        /// there are no operations to redo, calling this method has no effect.</remarks>
        /// <returns><see langword="true"/> if the redo operation is successful; otherwise, <see langword="false"/>.</returns>
        bool Redo();
    }
}
