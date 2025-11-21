using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public partial class BmsViewModel : ObservableObjectBase, IHistoryOwner<BmsHistoryData>, IBmsViewModel
    {
        private readonly History<BmsHistoryData> _history;
        private readonly MemoryStream _historyStream = new(32768);

        private Selection _selection = [];
        private readonly List<IBmsDataUnit> _inheritanceList = [];
        private readonly List<FlowAddress> _flowAddressList = [];

        private readonly InheritedTimeline _timeline = new();
        private bool _isTimeCounterReady;
        private readonly TimeCounter _timeCounter = new();

        public BmsData Root { get; set => SetValue(ref field, value ?? new(), OnRootChanged); }
        IBmsData IBmsViewModel.Root => Root;
        public IBmsDataUnit CurrentData => _inheritanceList[^1];
        public IListEnumerable<BarPosition, Note> CurrentTimeline => _timeline;
        public ITimeCounter TimeCounter => GetTimeCounter();
        public DoubleBarLengthCache BarLengthCache { get; } = new();
        public FlowViewModelCollection FlowViewModel { get; }

        IHistory IHistoryOwner.History => _history;

        public BmsViewModel()
        {
            var root = new BmsData();
            Root = root;
            FlowViewModel = new(root);
            _history = new(this);
        }

        private void OnRootChanged(IBmsData oldValue, IBmsData newValue)
        {
            _history?.Clear();
            this.OnConductorChanged(0);
            _inheritanceList.Clear();
            _inheritanceList.Add(newValue.Root);
            _timeline.ParentTimeline.Clear();
            _flowAddressList.Clear();
            FlowViewModel?.Load(newValue);
        }

        IEnumerable<IBmsDataUnit> IBmsViewModel.EnumerateParents()
        {
            var list = _inheritanceList;
            var count = list.Count;
            for (var i = 1; i < count; i++)
            {
                yield return list[i];
            }
        }

        IEnumerable<IBmsDataUnit> IBmsViewModel.ReverseEnumerateParents()
        {
            var list = _inheritanceList;
            for (var i = list.Count - 1; i > 0; i--)
            {
                yield return list[i];
            }
        }

        public void InvalidateTimeCounter()
        {
            _isTimeCounterReady = false;
        }

        protected ITimeCounter GetTimeCounter()
        {
            if (!_isTimeCounterReady)
            {
                _timeCounter.Load(this);
                _isTimeCounterReady = true;
            }
            return _timeCounter;
        }

        public BmsHistoryData GetHistoryData()
        {
            var ms = _historyStream;
            ms.SetLength(0);
            Root.WriteHistoryData(ms);
            BmsHistoryData data = new(ms.ToArray(), _selection);
            // TODO: flow history
            return data;
        }

        public void ApplyHistory(BmsHistoryData historyData)
        {
            using (MemoryStream ms = new(historyData.MainData))
            {
                Root.ReadHistoryData(ms);
            }
            // TODO: flow history
            historyData.Selection.Restore(_selection, CurrentData.Timeline);
            this.OnConductorChanged(0);
        }

        public bool NeedsUpdateHistory(BmsHistoryData historyData) => true;
        public void OnModified() => _history.PushUndo();
    }
}
