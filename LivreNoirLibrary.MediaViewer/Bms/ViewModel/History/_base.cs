using System;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel : IHistoryOwner<HistoryData>
    {
        private static readonly DummyData _dummy = new();
        private readonly History<HistoryData> _history;

        IHistory IHistoryOwner.History => _history;

        HistoryData IHistoryOwner<HistoryData>.GetHistoryData() => new(this);

        bool IHistoryOwner<HistoryData>.NeedsUpdateHistory(HistoryData historyData) => false;

        void IHistoryOwner<HistoryData>.ApplyHistory(HistoryData historyData)
        {
            var root = _root;
            _currentData = _dummy;
            historyData.LoadRoot(root);
            BaseData current = root;
            ApplyFlowHistoryData(root, historyData._flow, branch => Descend(ref current, branch));
            CurrentData = current;
            RestoreSelection(historyData._selection.Clone());
        }
    }
}
