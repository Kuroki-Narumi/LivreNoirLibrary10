using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            public (FlowBranch Branch, IBmsDataUnit Data) GetOrCreateBranch(IFlowContainer container, int condition)
            {
                var branch = container.GetOrAddBranch(condition);
                return (branch, root.GetBranchData(branch));
            }

            public BranchDataEnumerator EnumerateAllData() => new(root, root.Root, true);
            public BranchDataEnumerator EnumerateChildren(IBmsDataUnit? start, bool containsSelf = true) => new(root, start ?? root.Root, containsSelf);

            public void DeterminateRandom(IBmsDataUnit combineTarget, RandomProvider provider) => DeterminateRandom(root, root.Root, combineTarget, provider);
            public void DeterminateRandom(IBmsDataUnit start, IBmsDataUnit combineTarget, RandomProvider provider)
            {
                combineTarget.Clear();
                var stack = ObjectPool.Rent<Stack<(IBmsDataUnit, int)>>();
                try
                {
                    stack.Push((start, -1));
                    while (stack.TryPop(out var state))
                    {
                        var (data, flowIndex) = state;
                        if (flowIndex is -1)
                        {
                            combineTarget.Merge(data);
                            flowIndex++;
                        }
                        var flows = data.Flows;
                        for (; flowIndex < flows.Count; flowIndex++)
                        {
                            var flow = flows[flowIndex];
                            var condition = flow.IsFixed ? flow.Max : provider(flow.Max, flow.Note);
                            if (flow.GetBranch(condition) is { } branch)
                            {
                                stack.Push((data, flowIndex));
                                stack.Push((root.GetBranchData(branch), -1));
                                break;
                            }
                        }
                    }
                }
                finally
                {
                    ObjectPool.Return(stack);
                }
            }
        }
    }

    public struct BranchDataEnumerator
    {
        private readonly IBmsData _root;
        private readonly Stack<(IBmsDataUnit Data, int ContainerIndex, int BranchIndex)> _stack;
        private IBmsDataUnit _current;

        public BranchDataEnumerator(IBmsData root, IBmsDataUnit start, bool containsSelf)
        {
            _root = root;
            _stack = [];
            _stack.Push(new(start, 0, containsSelf ? -1 : 0));
            _current = null!;
        }

        public readonly IBmsDataUnit Current => _current;

        public bool MoveNext()
        {
            var root = _root;
            var stack = _stack;
            while (stack.TryPop(out var state))
            {
                var (data, containerIndex, branchIndex) = state;
                // self
                if (branchIndex is -1)
                {
                    _current = data;
                    stack.Push(new(data, containerIndex, 0)); // Push back updated state
                    return true;
                }
                // children
                var flows = data.Flows;
                var flowCount = flows.Count;
                while (containerIndex < flowCount)
                {
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
                        stack.Push(new(data, containerIndex, branchIndex)); // Push back updated state
                        stack.Push(new(root.GetBranchData(branch), 0, -1)); // Push new child state
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