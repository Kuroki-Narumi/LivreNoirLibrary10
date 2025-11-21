using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class FlowBranchViewModel(IBmsData root, FlowContainerViewModel? parent, FlowBranch model) :
        FlowViewModel<FlowBranch, FlowContainerViewModel>(root, parent, GetAddress(parent, model), model)
    {
        private static FlowAddress GetAddress(FlowContainerViewModel? parent, FlowBranch model) => parent?.Address.Append(model.Condition) ?? FlowAddress.Empty;

        public sealed override string? Name => Parent!.Model.GetBranchHeader(Condition);

        public int Condition { get; set => SetValue(ref field, value, [nameof(Name)], OnConditionChanged); } = model.Condition;

        private void OnConditionChanged(int oldValue, int newValue)
        {
            Model.Condition = newValue;
            UpdateAddress(GetAddress(Parent, Model));
        }

        protected sealed override void RefreshChildren(IBmsData root)
        {
            var list = Children;
            var i = 0;
            foreach (var flow in root.GetBranchData(Model).Flows.AsSpan())
            {
                list.AddWithoutNotify(new(root, this, i++, flow));
            }
        }

        internal sealed override void UpdateChildrenAddress()
        {
            var i = 0;
            foreach (var flow in Children.AsSpan())
            {
                flow.UpdateAddress(Address.Append(i++));
            }
        }

        internal sealed override void OnDelete(IBmsData root)
        {
            root.InsulateBranch(Model);
            base.OnDelete(root);
        }
    }
}