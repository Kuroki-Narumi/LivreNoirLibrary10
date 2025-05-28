using System;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows
{
    public interface IHistory
    {
        public int UndoCount { get; }
        public int RedoCount { get; }
        public void Initialize();
        public void PushUndo();
        public void Undo();
        public void Redo();
    }
}
