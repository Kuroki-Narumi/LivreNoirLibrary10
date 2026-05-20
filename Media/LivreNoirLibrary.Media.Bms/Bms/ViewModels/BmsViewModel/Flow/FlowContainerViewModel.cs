using System;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public sealed class FlowContainerViewModel(IBmsData root, FlowBranchViewModel parent, int index, FlowContainer model) : 
        FlowViewModel<FlowContainer, FlowBranchViewModel>(root, parent, parent.Address.Append(index), model)
    {
        public override string? Name => Model.BmsHeader;

        public int Max { get; set => SetValue(ref field, value, [nameof(Name)], OnMaxChaned); } = model.Max;
        public bool IsFixed { get; set => SetValue(ref field, value, [nameof(Name)], OnIsFixedChanged); } = model.IsFixed;

        private void OnMaxChaned(int oldValue, int newValue)
        {
            Model.Max = newValue;
        }

        private void OnIsFixedChanged(bool oldValue, bool newValue)
        {
            Model.IsFixed = newValue;
        }

        protected override void RefreshChildren(IBmsData root)
        {
            var list = Children;
            foreach (var branch in Model.EnumerateBranches())
            {
                list.AddWithoutNotify(new(root, this, branch));
            }
        }

        internal override void UpdateChildrenAddress()
        {
            foreach (var branch in Children.AsSpan())
            {
                branch.UpdateAddress(Address.Append(branch.Condition));
            }
        }
    }
}
