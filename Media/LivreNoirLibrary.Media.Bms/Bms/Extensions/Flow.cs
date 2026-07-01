using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension (IBmsDataUnit data)
        {
            public bool ContainsFlow => data.Flows.Count is > 0;
        }

        extension (IBmsData root)
        {
            public (FlowBranch Branch, IBmsDataUnit Data) GetOrCreateBranch(FlowContainer container, int condition)
            {
                var branch = container.GetOrAddBranch(condition);
                return (branch, root.GetBranchData(branch));
            }

            public BranchDataEnumerator EnumerateAllData() => new(root, root.Root, true);
            public BranchDataEnumerator EnumerateChildren(IBmsDataUnit? start, bool containsSelf = true) => new(root, start ?? root.Root, containsSelf);

            public void DetermineRandom(IBmsDataUnit combineTarget, RandomProvider provider) => DetermineRandom(root, root.Root, combineTarget, provider);
            public void DetermineRandom(IBmsDataUnit start, IBmsDataUnit combineTarget, RandomProvider provider)
            {
                combineTarget.Clear();
                var stack = new Stack<(FlowAddress, IBmsDataUnit, int)>();
                stack.Push((FlowAddress.Empty, start, -1));
                while (stack.TryPop(out var state))
                {
                    var (address, data, flowIndex) = state;
                    if (flowIndex is -1)
                    {
                        combineTarget.Merge(data);
                        flowIndex++;
                    }
                    var flows = data.Flows;
                    for (; flowIndex < flows.Count; flowIndex++)
                    {
                        var newAddress = address.Append(flowIndex + 1);
                        var flow = flows[flowIndex];
                        var condition = flow.IsFixed ? flow.Max : provider(newAddress, flow.Max, flow.Note);
                        if (flow.GetBranch(condition) is { } branch)
                        {
                            stack.Push((address, data, flowIndex + 1));
                            stack.Push((newAddress.Append(condition), root.GetBranchData(branch), -1));
                            break;
                        }
                    }
                }
            }
        }
    }

    public struct BranchDataEnumerator
    {
        private readonly IBmsData _root;
        private readonly Stack<(FlowAddress Address, IBmsDataUnit Data, int ContainerIndex, int BranchIndex)> _stack;
        private FlowAddress _currentAddress;
        private IBmsDataUnit _currentData;

        public BranchDataEnumerator(IBmsData root, IBmsDataUnit start, bool containsSelf)
        {
            _root = root;
            _currentAddress = FlowAddress.Empty;
            _stack = [];
            _stack.Push(new(_currentAddress, start, 0, containsSelf ? -1 : 0));
            _currentData = null!;
        }

        public readonly (FlowAddress, IBmsDataUnit) Current => (_currentAddress, _currentData);

        public bool MoveNext()
        {
            var root = _root;
            var stack = _stack;
            while (stack.TryPop(out var state))
            {
                var (address, data, containerIndex, branchIndex) = state;
                // self
                if (branchIndex is -1)
                {
                    _currentAddress = address;
                    _currentData = data;
                    stack.Push(new(address, data, containerIndex, 0)); // Push back updated state
                    return true;
                }
                // children
                var flows = data.Flows;
                var flowCount = flows.Count;
                while (containerIndex < flowCount)
                {
                    var newAddress = address.Append(containerIndex + 1);
                    var flow = flows[containerIndex];
                    var branches = flow.Branches;
                    var branch = (branches.Count - branchIndex) switch
                    {
                        > 0 => branches[branchIndex],
                        0 => flow.DefaultBranch,
                        _ => null
                    };
                    if (branch is not null)
                    {
                        branchIndex++;
                        stack.Push(new(address, data, containerIndex, branchIndex)); // Push back updated state
                        stack.Push(new(newAddress.Append(branch.Condition), root.GetBranchData(branch), 0, -1)); // Push new child state
                        break;
                    }
                    else
                    {
                        containerIndex++;
                        branchIndex = 0;
                    }
                }
            }
            return false;
        }

        public readonly BranchDataEnumerator GetEnumerator() => this;
    }
}