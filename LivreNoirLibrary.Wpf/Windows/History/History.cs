using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows
{
    public class History<T> : IHistory
    {
        private readonly IHistoryOwner<T> _owner;
        private readonly Stack<T> _undo = new();
        private readonly Stack<T> _redo = new();
        private T _last_data;

        public int UndoCount => _undo.Count;
        public int RedoCount => _redo.Count;

        public History(IHistoryOwner<T> owner)
        {
            _owner = owner;
            _last_data = _owner.GetHistoryData();
        }

        public void Initialize()
        {
            _undo.Clear();
            _redo.Clear();
            _last_data = _owner.GetHistoryData();
        }

        public void PushUndo()
        {
            if (_owner.NeedsUpdateHistory(_last_data))
            {
                _redo.Clear();
                _undo.Push(_last_data);
                _last_data = _owner.GetHistoryData();
            }
        }

        private void ProcessDo(Stack<T> from, Stack<T> to)
        {
            to.Push(_owner.GetHistoryData());
            var data = from.Pop();
            _owner.ApplyHistory(data);
            _last_data = data;
        }

        public void Undo() => ProcessDo(_undo, _redo);
        public void Redo() => ProcessDo(_redo, _undo);
    }
}
