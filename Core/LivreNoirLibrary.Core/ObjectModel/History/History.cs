using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.ObjectModel
{
    public class History<T> : IHistory, IClear
    {
        private readonly IHistoryOwner<T> _owner;
        private readonly Stack<T> _undo = new();
        private readonly Stack<T> _redo = new();
        private T _last_data;

        public int UndoCount => _undo.Count;
        public int RedoCount => _redo.Count;
        public T LastData => _last_data;

        public History(IHistoryOwner<T> owner)
        {
            _owner = owner;
            _last_data = _owner.GetHistoryData();
        }

        public void Clear()
        {
            _undo.Clear();
            _redo.Clear();
            _last_data = _owner.GetHistoryData();
        }

        public bool PushUndo(bool force = false)
        {
            var data = _last_data;
            var newData = _owner.GetHistoryData();
            if (force || !_owner.HistoryEquals(data, newData))
            {
                _redo.Clear();
                _owner.EnsureHistoryData(data);
                _undo.Push(data);
                _last_data = newData;
                return true;
            }
            return false;
        }

        private bool ProcessDo(Stack<T> from, Stack<T> to)
        {
            if (from.TryPop(out var data))
            {
                var newData = _owner.GetHistoryData();
                _owner.EnsureHistoryData(newData);
                to.Push(newData);
                _owner.ApplyHistory(data);
                _last_data = data;
                return true;
            }
            return false;
        }

        public bool Undo() => ProcessDo(_undo, _redo);
        public bool Redo() => ProcessDo(_redo, _undo);
    }
}
