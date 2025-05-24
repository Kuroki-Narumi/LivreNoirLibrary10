using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BmsData
    {
        public bool TryGetFlowData(int id, [MaybeNullWhen(false)] out FlowData data)
        {
            if ((uint)id < (uint)_flow_data.Count && !_flow_free_index.Contains(id))
            {
                data = _flow_data[id];
                return true;
            }
            data = null;
            return false;
        }

        public FlowData? GetFlowDataOrNull(int id) => TryGetFlowData(id, out var data) ? data : null;

        public void ClearFlow()
        {
            _flows.Clear();
            _flow_data.Clear();
            _flow_free_index.Clear();
        }

        private (TContainer Flow, int Index) CreateFlowCore<TContainer, TBranch>(
            FlowBranch? parent, Func<int, bool, TContainer> creator, int max, bool isFixed, bool createBranch, Func<TContainer, int, TBranch> childCreator)
            where TContainer : FlowContainer
        {
            var item = creator(max, isFixed);
            if (createBranch)
            {
                for (var i = 1; i <= max; i++)
                {
                    childCreator(item, i);
                }
            }
            var index = parent is not null ? parent.AddFlow(item) : FlowItem.AddFlow(_flows, item);
            return (item, index);
        }

        public (FlowRandom Flow, int Index) CreateRandom(FlowBranch? parent, int max, bool isFixed, bool createBranch = false) => 
            CreateFlowCore(parent, FlowRandom.Create, max, isFixed, createBranch, CreateIf);
        public (FlowSwitch Flow, int Index) CreateSwitch(FlowBranch? parent, int max, bool isFixed, bool createBranch = false) => 
            CreateFlowCore(parent, FlowSwitch.Create, max, isFixed, createBranch, CreateCase);

        private TBranch CreateBranchCore<TBranch>(Func<int, int, TBranch> creator, int index)
            where TBranch : FlowBranch
        {
            int dataId;
            var set = _flow_free_index;
            if (set.Count is > 0)
            {
                dataId = set.Min;
                set.Remove(dataId);
            }
            else
            {
                dataId = _flow_data.Count;
                _flow_data.Add(new(this));
            }
            var item = creator(index, dataId);
            return item;
        }

        public FlowIf CreateIf(FlowRandom parent, int index)
        {
            TryCreateIf(parent, index, out var branch);
            return branch;
        }

        public bool TryCreateIf(FlowRandom parent, int index, out FlowIf branch)
        {
            if (parent.GetBranch(index) is FlowIf b)
            {
                branch = b;
                return false;
            }
            else
            {
                branch = CreateBranchCore(FlowIf.Create, index);
                parent.AddChild(branch);
                return true;
            }
        }

        public FlowIfChild CreateElseIf(FlowIf parent, int index)
        {
            TryCreateElseIf(parent, index, out var branch);
            return branch;
        }

        public bool TryCreateElseIf(FlowIf parent, int index, out FlowIfChild branch)
        {
            if (parent.TryGetElse(index, out var b))
            {
                branch = b;
                return false;
            }
            else
            {
                branch = CreateBranchCore(FlowIfChild.Create, index);
                parent.AddElseIf(branch);
                return true;
            }
        }

        public FlowIfChild CreateElse(FlowIf parent)
        {
            TryCreateElse(parent, out var branch);
            return branch;
        }

        public bool TryCreateElse(FlowIf parent, out FlowIfChild branch)
        {
            if (parent.Else is FlowIfChild b)
            {
                branch = b;
                return false;
            }
            else
            {
                branch = CreateBranchCore(FlowIfChild.Create, -parent.Index);
                parent.Else = branch;
                return true;
            }
        }

        public FlowCase CreateCase(FlowSwitch parent, int index)
        {
            TryCreateCase(parent, index, out var branch);
            return branch;
        }

        public bool TryCreateCase(FlowSwitch parent, int index, out FlowCase branch)
        {
            if (parent.GetBranch(index) is FlowCase b)
            {
                branch = b;
                return false;
            }
            else
            {
                branch = CreateBranchCore(FlowCase.Create, index);
                parent.AddChild(branch);
                return true;
            }
        }

        public FlowCase CreateDefault(FlowSwitch parent)
        {
            TryCreateDefault(parent, out var branch);
            return branch;
        }

        public bool TryCreateDefault(FlowSwitch parent, out FlowCase branch) => TryCreateCase(parent, FlowTexts.DefaultIndex, out branch);

        public bool RemoveFlow(FlowBranch? parent, FlowContainer flow)
        {
            if (parent is not null ? parent.RemoveFlow(flow) : _flows.Remove(flow))
            {
                RemoveFlowData(flow);
                return true;
            }
            return false;
        }

        public bool RemoveFlowBranch(FlowContainer parent, FlowBranch branch)
        {
            if (parent.RemoveChild(branch))
            {
                RemoveFlowData(branch);
                return true;
            }
            return false;
        }

        public bool RemoveIfChild(FlowIf parent, FlowIfChild child)
        {
            bool flag;
            if (parent.Else == child)
            {
                parent.Else = null;
                flag = true;
            }
            else
            {
                flag = parent.RemoveElseIf(child);
            }
            if (flag)
            {
                RemoveFlowData(child);
            }
            return flag;
        }

        private void RemoveFlowData(FlowContainer flow)
        {
            foreach (var child in flow.Branches)
            {
                RemoveFlowData(child);
            }
        }

        private void RemoveFlowData(FlowBranch branch)
        {
            var id = branch.DataId;
            _flow_data[branch.DataId].Clear();
            _flow_free_index.Add(id);
            foreach (var f in branch.Flows)
            {
                RemoveFlowData(f);
            }
            if (branch is FlowIf fif)
            {
                foreach (var fc in fif.ElseIfs)
                {
                    RemoveFlowData(fc);
                }
                if (fif.Else is FlowIfChild el)
                {
                    RemoveFlowData(el);
                }
            }
        }

        public BmsData GetRandom(RandomProvider random, FlowAddressList fixedAddress)
        {
            List<FlowData> randomList = [];
            FlowItem.GetRandom(this, _flows, random, FlowAddress.Empty, fixedAddress, randomList);
            BmsData result = new(){ ChartType = ChartType };
            result.Merge(this);
            foreach (var data in CollectionsMarshal.AsSpan(randomList))
            {
                result.Merge(data);
            }
            return result;
        }

        public IEnumerable<BaseData> EachData()
        {
            yield return this;
            foreach (var data in FlowItem.EachData(this, _flows))
            {
                yield return data;
            }
        }

        public BmsData GetPartialRandom(FlowAddressList fixedAddress)
        {
            if (_flows.Count is 0)
            {
                return this;
            }
            var data = Clone();
            FlowItem.SetRandom(data._flows, FlowAddress.Empty, fixedAddress);
            return data;
        }
    }
}
