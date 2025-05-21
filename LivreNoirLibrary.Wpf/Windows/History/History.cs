using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows
{
    public class History<TOwner, TData>(TOwner owner) : IHistory
        where TOwner : IHistoryOwner<TOwner, TData>
    {
        private readonly TOwner _owner = owner;
        private readonly Stack<TData> _undo = new();
        private readonly Stack<TData> _redo = new();
        private TData? _temporal_data;

        public int UndoCount => _undo.Count;
        public int RedoCount => _redo.Count;

        public void Clear()
        {
            _undo.Clear();
            _redo.Clear();
        }

        public void BeforeEdit()
        {
            _temporal_data = _owner.GetHistoryData();
        }

        public void AfterEdit()
        {
            if (_temporal_data is not null)
            {
                PushUndo(_temporal_data);
            }
        }

        public void PushUndo(TData data)
        {
            _redo.Clear();
            _undo.Push(data);
            _temporal_data = default;
        }

        private void ProcessDo(Stack<TData> from, Stack<TData> to)
        {
            to.Push(_owner.GetHistoryData());
            var data = from.Pop();
            _owner.ApplyHistoryData(data);
        }

        public void Undo() => ProcessDo(_undo, _redo);
        public void Redo() => ProcessDo(_redo, _undo);
    }
}
