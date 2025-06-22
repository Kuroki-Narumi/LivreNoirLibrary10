using System;
using System.Windows.Input;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IHistory
    {
        public int UndoCount { get; }
        public int RedoCount { get; }
        public void Initialize();
        public void PushUndo(bool force);
        public void Undo();
        public void Redo();
    }
}
