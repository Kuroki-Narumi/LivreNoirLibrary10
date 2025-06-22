using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel
    {
        private readonly ObservableList<FlowViewModel> _flows = [];
        private readonly FlowAddressList _flowAddress = [];
        private readonly List<BaseData> _dataStack = [];
        private readonly List<IFlowViewModel> _flowStack = [];

        public IList<FlowViewModel> Flows => _flows;
        public bool HasFlow => _flows.Count is > 0;
        public bool IsDescending => _dataStack.Count is > 0;

        private void ClearStack()
        {
            _flowAddress.Clear();
            _dataStack.Clear();
            _flowStack.Clear();
            RefreshFlows(_root);
        }

        internal Dictionary<FlowAddress, FlowHistoryData> CreateFlowHistoryData()
        {
            Dictionary<FlowAddress, FlowHistoryData> data = [];
            foreach (var address in CollectionsMarshal.AsSpan(_flowAddress))
            {
                data.Add(address, new(false, false, true));
            }

            return data;
        }

        private static void GetFlowHistoryData(ObservableList<FlowViewModel> items, Dictionary<FlowAddress, FlowHistoryData> history)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var address = item.Address;
                if (history.TryGetValue(address, out var current))
                {
                    history[address] = current.Update(item.IsExpanded, item.IsSelected);
                }
                else
                {
                    history.Add(address, new(item));
                }
                GetFlowHistoryData(item.Children, history);
            }
        }

        private void ApplyFlowHistoryData(BmsData root, Dictionary<FlowAddress, FlowHistoryData> historyData, Action<IFlowBranchViewModel> descendAction)
        {
            RefreshFlows(root);
            ApplyFlowHistoryData(_flows, historyData, descendAction);
        }

        private static void ApplyFlowHistoryData(ObservableList<FlowViewModel> items, Dictionary<FlowAddress, FlowHistoryData> dic, Action<IFlowBranchViewModel> descendAction)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var address = item.Address;
                if (dic.TryGetValue(address, out var value))
                {
                    item.IsExpanded = value.IsExpanded;
                    item.IsSelected = value.IsSelected;
                    if (value.IsFocused && item.CheckIfBranch(out var branch))
                    {
                        descendAction(branch);
                    }
                }
                ApplyFlowHistoryData(item.Children, dic, descendAction);
            }
        }

        private void RefreshFlows(BmsData root)
        {
            var items = _flows;
            items.ClearWithoutNotify();
            if (root is not null)
            {
                var i = 0;
                foreach (var flow in root.Flows)
                {
                    items.AddWithoutNotify(FlowViewModel.CreateFlow(root, i++, flow, null));
                }
            }
            items.NotifyCollectionReset();
            SendPropertyChanged(nameof(HasFlow));
        }

        private void ForceInherit()
        {
            var list = _dataStack;
            var max = list.Count - 1;
            if (_dataStack.Count is > 0)
            {
                for (var i = 0; i < max; i++)
                {
                    list[i + 1].Inherit(list[i]);
                }
                _currentData!.Inherit(list[^1]);
            }
            else
            {
                _currentData!.Insulate();
            }
        }

        private bool ProcessAscend(ref BaseData current)
        {
            var ds = _dataStack;
            if (ds.Count is > 0)
            {
                var stack = _flowStack;
                current.Insulate();
                current = ds.Pop();
                _flowAddress.Pop();
                stack[^1].IsFocused = false;
                stack.Pop();
                stack.Pop();
                return true;
            }
            return false;
        }

        public void Ascend()
        {
            var current = _currentData!;
            if (ProcessAscend(ref current))
            {
                CurrentData = current;
            }
        }

        public void AutoAscend(ref BaseData current, FlowViewModel item)
        {
            var stack = _flowStack;
            var index = stack.IndexOf(item);
            if (index is >= 0)
            {
                while (stack.Count > index)
                {
                    ProcessAscend(ref current);
                }
            }
        }

        private void ProcessDescend(ref BaseData current, IFlowBranchViewModel branch)
        {
            var parent = branch.ParentVM!;
            if (parent is FlowIfVM @if)
            {
                parent = @if.ParentVM!;
            }
            AutoAscend(ref current, parent);
            parent.IsExpanded = true;
            var next = branch.Data;
            _dataStack.Add(current);
            _flowAddress.Add(branch.Address);
            _flowStack.Add(parent);
            _flowStack.Add(branch);
            branch.IsFocused = true;
            current = next;
        }

        private void Descend(ref BaseData current, IFlowBranchViewModel item)
        {
            List<IFlowBranchViewModel> list = [item];
            var currentVM = item.ParentVM;
            while (currentVM is not null)
            {
                if (currentVM.CheckIfBranch(out var b))
                {
                    list.Add(b);
                }
                currentVM = currentVM.ParentVM;
            }
            list.Reverse();
            foreach (var b in CollectionsMarshal.AsSpan(list))
            {
                ProcessDescend(ref current, b);
            }
        }

        public void Descend(IFlowBranchViewModel item)
        {
            var current = _currentData!;
            Descend(ref current, item);
            CurrentData = current;
        }

        private void RefreshFlowAddress()
        {
            var list = _flows;
            for (var i = 0; i < list.Count; i++)
            {
                list[i].RefreshFlowAddress(i);
            }
        }

        public void DeleteFlow(FlowViewModel item)
        {
            var root = _root;
            var current = _currentData;
            AutoAscend(ref current, item);
            if (item is FlowIfChildVM ifc)
            {
                root.RemoveIfChild(ifc.Parent, ifc.SourceGeneric);
            }
            else if (item.CheckIfBranch(out var branch))
            {
                root.RemoveFlowBranch(branch.ParentContainer.Container, branch.Branch);
            }
            else if (item.CheckIfContainer(out var container))
            {
                root.RemoveFlow(container.ParentBranch?.Branch, container.Container);
            }
            var parentList = item.ParentVM?.Children ?? _flows;
            item.OnDelete();
            parentList.Remove(item);
            RefreshFlowAddress();
            SendPropertyChanged(nameof(HasFlow));
            this.OnEdit(true);
            CurrentData = current;
        }

        private void AddFlowCore(BmsData root, FlowViewModel? target, int index, FlowContainer flow)
        {
            if (target is not null && target.CheckIfContainer(out _))
            {
                target = target.ParentVM;
            }
            var items = target is not null && target.CheckIfBranch(out _) ? target.Children : _flows;
            var vm = FlowViewModel.CreateFlow(root, index, flow, target);
            vm.IsExpanded = true;
            var childIndex = items.FindIndex(item => item is FlowIfChildVM);
            if (childIndex is >= 0)
            {
                items.Insert(childIndex, vm);
            }
            else
            {
                items.Add(vm);
            }
            SendPropertyChanged(nameof(HasFlow));
        }

        public void AddFlowRandom(FlowViewModel? target, string? note, int max, bool isFixed, bool generate)
        {
            var (item, index) = _root.CreateRandom(target?.Source as FlowBranch, max, isFixed, generate);
            item.Note = note;
            AddFlowCore(_root, target, index, item);
            this.OnEdit(true);
        }

        public void AddFlowIf(FlowViewModel? target, string? note, int index)
        {
            if (target is not FlowRandomVM pVM)
            {
                pVM = (target!.ParentVM as FlowRandomVM)!;
            }
            var parent = pVM.SourceGeneric;
            if (_root.TryCreateIf(parent, index, out var @if))
            {
                pVM.Children.Add(new FlowIfVM(_root, parent, @if, pVM));
            }
            @if.Note = note;
            this.OnEdit(true);
        }

        public void AddFlowElse(FlowViewModel? target, string? note)
        {
            if (target is FlowIfVM pVM)
            {
                var parent = pVM.SourceGeneric;
                if (_root.TryCreateElse(parent, out var @else))
                {
                    pVM.Children.Add(new FlowIfChildVM(_root, parent, @else, pVM));
                }
                @else.Note = note;
            }
        }

        public void AddFlowSwitch(FlowViewModel? target, string? note, int max, bool isFixed, bool generate)
        {
            var (item, index) = _root.CreateSwitch(target?.Source as FlowBranch, max, isFixed, generate);
            item.Note = note;
            AddFlowCore(_root, target, index, item);
            this.OnEdit(true);
        }

        public void AddFlowCase(FlowViewModel? target, string? note, int index)
        {
            if (target is not FlowSwitchVM sVM)
            {
                sVM = (target!.ParentVM as FlowSwitchVM)!;
            }
            var parent = sVM.SourceGeneric;
            var children = parent.Branches;
            var pos = children.Length - (children[^1].Index is FlowTexts.DefaultIndex ? 1 : 0);
            if (_root.TryCreateCase(parent, index, out var @case))
            {
                sVM.Children.Insert(pos, new FlowCaseVM(_root, parent, @case, sVM));
            }
            @case.Note = note;
            this.OnEdit(true);
        }

        public void AddFlowDefault(FlowViewModel? target, string? note)
        {
            if (target is not FlowSwitchVM sVM)
            {
                sVM = (target!.ParentVM as FlowSwitchVM)!;
            }
            var parent = sVM.SourceGeneric;
            if (_root.TryCreateDefault(parent, out var @default))
            {
                sVM.Children.Add(new FlowCaseVM(_root, parent, @default, sVM));
            }
            @default.Note = note;
            this.OnEdit(true);
        }
    }
}
