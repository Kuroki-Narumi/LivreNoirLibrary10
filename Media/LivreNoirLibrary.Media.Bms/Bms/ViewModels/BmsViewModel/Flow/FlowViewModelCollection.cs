using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class FlowViewModelCollection(IBmsData root) : FlowBranchViewModel(root, null, FlowBranch.Root), IFlowViewModel
    {
        public bool HasContent { get; private set => SetValue(ref field, value); }

        IEnumerable<FlowContainerViewModel> IFlowViewModel.Children =>  Children;

        public void Load(IBmsData root)
        {
            Children.ClearWithoutNotify();
            RefreshChildren(root);
            HasContent = Children.Count is > 0;
            Children.NotifyCollectionReset();
        }

        public void AddContainer(IBmsData root, FlowBranchViewModel parent, FlowType type, string? note, int max, bool isFixed, bool createChildren)
        {
            // crate flow
            var flows = root.GetBranchData(parent.Model).Flows;
            var flow = new FlowContainer
            {
                Note = note,
                Type = type,
                Max = max,
                IsFixed = isFixed
            };
            var index = flows.Count;
            flows.Add(flow);
            if (createChildren)
            {
                flow.EnsureBranches();
            }
            // create view model
            var viewModel = new FlowContainerViewModel(root, parent, index, flow)
            {
                IsExpanded = true,
            };
            parent.Children.Add(viewModel);
            HasContent = true;
        }

        public static void AddBranch(IBmsData root, FlowContainerViewModel parent, string? note, int condition)
        {
            var branch = parent.Model.GetOrAddBranch(condition);
            branch.Note = note;
            parent.Children.Add(new(root, parent, branch));
        }

        public void Delete(IBmsData root, FlowContainerViewModel item)
        {
            var parent = item.Parent!;
            if (root.GetBranchData(parent.Model).Flows.Remove(item.Model))
            {
                item.OnDelete(root);
                parent.Children.Remove(item);
                parent.UpdateChildrenAddress();
                if (parent == this)
                {
                    HasContent = Children.Count > 0;
                }
            }
        }

        public static void Delete(IBmsData root, FlowBranchViewModel item)
        {
            if (item.Parent is { } parent && parent.Model.DeleteBranch(root, item.Model))
            {
                item.OnDelete(root);
                parent.Children.Remove(item);
                parent.UpdateChildrenAddress();
            }
        }
    }
}
