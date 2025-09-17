using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IHistory
    {
        /// <summary>
        /// Gets the number of undo operations currently available in the undo stack.
        /// </summary>
        public int UndoCount { get; }

        /// <summary>
        /// Gets the number of redo operations currently available in the redo stack.
        /// </summary>
        public int RedoCount { get; }

        /// <summary>
        /// Clear this history.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Pushes the current state onto the undo stack, allowing it to be restored later.
        /// </summary>
        /// <param name="force">A value indicating whether to force the state to be pushed onto the undo stack, even if it is identical to
        /// the previous state.</param>
        public void PushUndo(bool force);

        /// <summary>
        /// Reverts the most recent operation, restoring the state to its previous condition.
        /// </summary>
        /// <remarks>This method undoes the last operation performed. If no operations are available to
        /// undo,  calling this method may have no effect. Ensure that the operation history is managed appropriately 
        /// to avoid unexpected behavior.</remarks>
        public void Undo();

        /// <summary>
        /// Reapplies the most recently undone operation, if available.
        /// </summary>
        /// <remarks>This method restores the state to what it was before the last undo operation.  If
        /// there are no operations to redo, calling this method has no effect.</remarks>
        public void Redo();
    }
}
