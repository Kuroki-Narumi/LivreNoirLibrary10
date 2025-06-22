using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface IFlowViewModel
    {
        public FlowViewModel? ParentVM { get; }
        public FlowAddress Address { get; }
        public string? Name { get; }
        public string? Note { get; }
        public bool IsFocused { get; set; }
        public ObservableList<FlowViewModel> Children { get; }

        public bool CheckIfContainer([MaybeNullWhen(false)] out IFlowContainerViewModel container);
        public bool CheckIfBranch([MaybeNullWhen(false)] out IFlowBranchViewModel branch);
        public void RefreshFlowAddress(int index);
    }

    public interface IFlowContainerViewModel : IFlowViewModel
    {
        public int Index { get; }
        public FlowContainer Container { get; }
        public int Max { get; }
        public bool IsFixed { get; }
        public IFlowBranchViewModel? ParentBranch { get; }
    }

    public interface IFlowBranchViewModel : IFlowViewModel
    {
        public int Index { get; }
        public FlowBranch Branch { get; }
        public FlowData Data { get; }
        public IFlowContainerViewModel ParentContainer { get; }
    }

    public abstract partial class FlowViewModel : ObservableObjectBase, IFlowViewModel
    {
        protected readonly BmsData _root;
        protected FlowViewModel? _parentVM;
        protected int _index;
        protected FlowAddress _address;
        [ObservableProperty]
        protected string? _note;
        [ObservableProperty]
        private bool _isFocused;
        [ObservableProperty]
        private bool _isExpanded;
        [ObservableProperty]
        private bool _isSelected;

        public FlowViewModel(BmsData root, string? note, FlowViewModel? parentVM, int index)
        {
            _root = root;
            _note = note;
            _parentVM = parentVM;
            _index = index;
            _address = GetAddress(index);
        }

        public FlowViewModel? ParentVM => _parentVM;
        public abstract FlowItem Source { get; }

        public FlowAddress Address => _address;
        public abstract string? Name { get; }
        public ObservableList<FlowViewModel> Children { get; } = [];
        public virtual bool IsDecendable => false;

        private void OnNoteChanged(string? value)
        {
            Source.Note = value;
        }

        public abstract bool CheckIfContainer([MaybeNullWhen(false)] out IFlowContainerViewModel container);
        public abstract bool CheckIfBranch([MaybeNullWhen(false)] out IFlowBranchViewModel branch);

        public virtual void OnDelete()
        {
            _parentVM = null;
            foreach (var child in Children)
            {
                child.OnDelete();
            }
        }

        public void RefreshFlowAddress(int index)
        {
            _index = index;
            _address = GetAddress(index);
            var list = Children;
            for (var i = 0; i < list.Count; i++)
            {
                var child = list[i];
                if (child.CheckIfBranch(out var branch))
                {
                    branch.RefreshFlowAddress(branch.Index);
                }
                else if (child.CheckIfContainer(out var container))
                {
                    container.RefreshFlowAddress(i);
                }
            }
        }

        protected virtual FlowAddress GetAddress(int index) => _parentVM?._address.Append(index) ?? new(index);

        public bool TryGetBranch(int index, [MaybeNullWhen(false)] out IFlowBranchViewModel branch)
        {
            var list = Children;
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].CheckIfBranch(out branch) && branch.Index == index)
                {
                    return true;
                }
            }
            branch = null;
            return false;
        }

        public static FlowViewModel CreateFlow(BmsData root, int index, FlowContainer source, FlowViewModel? parentVM)
        {
            return source switch
            {
                FlowRandom f => new FlowRandomVM(root, index, f, parentVM),
                FlowSwitch f => new FlowSwitchVM(root, index, f, parentVM),
                _ => throw new NotImplementedException()
            };
        }

        public static FlowViewModel CreateBranch(BmsData root, FlowContainer parent, FlowBranch branch, FlowViewModel parentVM)
        {
            return branch switch
            {
                FlowIf f => new FlowIfVM(root, (parent as FlowRandom)!, f, parentVM),
                FlowCase f => new FlowCaseVM(root, (parent as FlowSwitch)!, f, parentVM),
                _ => throw new NotImplementedException()
            };
        }
    }

    public abstract class FlowVMBase<T> : FlowViewModel
        where T : FlowItem
    {
        protected T? _source;

        public T SourceGeneric => _source!;
        public override FlowItem Source => _source!;
        public sealed override string? Name => _source?.DumpHeader;

        public FlowVMBase(BmsData root, T source, FlowViewModel? parentVM, int index) : base(root, source.Note, parentVM, index)
        {
            _source = source;
            RefreshChildren();
        }

        protected abstract void RefreshChildren();

        public override void OnDelete()
        {
            _source = null;
            base.OnDelete();
        }
    }

    public abstract class FlowContainerVMBase<T>(BmsData root, int index, T source, FlowViewModel? parentVM) : FlowVMBase<T>(root, source, parentVM, index), IFlowContainerViewModel
        where T : FlowContainer
    {
        protected int _max = source.Max;
        protected bool _fixed = source.IsFixed;
        FlowContainer IFlowContainerViewModel.Container => SourceGeneric;
        IFlowBranchViewModel? IFlowContainerViewModel.ParentBranch => _parentVM as IFlowBranchViewModel;

        public int Index => _index;
        public int Max
        {
            get => _max;
            set
            {
                if (SetProperty(ref _max, value))
                {
                    SourceGeneric.Max = value;
                    SendPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsFixed
        {
            get => _fixed;
            set
            {
                if (SetProperty(ref _fixed, value))
                {
                    SourceGeneric.IsFixed = value;
                    SendPropertyChanged(nameof(Name));
                }
            }
        }

        public sealed override bool CheckIfContainer([MaybeNullWhen(false)] out IFlowContainerViewModel container)
        {
            container = this;
            return true;
        }

        public sealed override bool CheckIfBranch([MaybeNullWhen(false)] out IFlowBranchViewModel branch)
        {
            branch = null;
            return false;
        }

        protected sealed override void RefreshChildren()
        {
            var root = _root;
            var source = SourceGeneric;
            foreach (var branch in source.Branches)
            {
                Children.AddWithoutNotify(CreateBranch(root, source, branch, this));
            }
        }
    }

    public class FlowRandomVM(BmsData root, int index, FlowRandom item, FlowViewModel? parent) : FlowContainerVMBase<FlowRandom>(root, index, item, parent) { }
    public class FlowSwitchVM(BmsData root, int index, FlowSwitch item, FlowViewModel? parent) : FlowContainerVMBase<FlowSwitch>(root, index, item, parent) { }

    public abstract class FlowChildVMBase<TParent, TChild>(BmsData root, TParent parent, TChild branch, FlowViewModel parentVM) : FlowVMBase<TChild>(root, branch, parentVM, branch.Index), IFlowBranchViewModel
        where TParent : FlowItem
        where TChild : FlowBranch
    {
        protected TParent? _parent = parent;

        public TParent Parent => _parent!;
        FlowBranch IFlowBranchViewModel.Branch => SourceGeneric;
        IFlowContainerViewModel IFlowBranchViewModel.ParentContainer => (_parentVM as IFlowContainerViewModel)!;
        public FlowData Data => _root.GetFlowDataOrNull(SourceGeneric.DataId)!;
        public override bool IsDecendable => true;

        public int Index
        {
            get => _index;
            set
            {
                if (SetProperty(ref _index, value))
                {
                    SourceGeneric.Index = value;
                    RefreshFlowAddress(value);
                    SendPropertyChanged(nameof(Name));
                }
            }
        }

        public sealed override bool CheckIfContainer([MaybeNullWhen(false)] out IFlowContainerViewModel container)
        {
            container = null;
            return false;
        }

        public sealed override bool CheckIfBranch([MaybeNullWhen(false)] out IFlowBranchViewModel branch)
        {
            branch = this;
            return true;
        }

        protected override void RefreshChildren()
        {
            var root = _root;
            var list = Children;
            var source = SourceGeneric;
            var i = 0;
            foreach (var flow in source.Flows)
            {
                list.AddWithoutNotify(CreateFlow(root, i++, flow, this));
            }
        }

        public override void OnDelete()
        {
            _parent = null;
            base.OnDelete();
        }
    }

    public class FlowIfVM(BmsData root, FlowRandom parent, FlowIf branch, FlowViewModel parentVM) : FlowChildVMBase<FlowRandom, FlowIf>(root, parent, branch, parentVM)
    {
        protected override void RefreshChildren()
        {
            base.RefreshChildren();
            var root = _root;
            var list = Children;
            var source = SourceGeneric;
            foreach (var elif in source.ElseIfs)
            {
                list.AddWithoutNotify(new FlowIfChildVM(root, source, elif, this));
            }
            if (source.Else is FlowIfChild els)
            {
                list.AddWithoutNotify(new FlowIfChildVM(root, source, els, this));
            }
        }
    }

    public class FlowIfChildVM(BmsData root, FlowIf parent, FlowIfChild branch, FlowViewModel parentVM) : FlowChildVMBase<FlowIf, FlowIfChild>(root, parent, branch, parentVM)
    {
        protected override FlowAddress GetAddress(int index) => _parentVM?.ParentVM?.Address.Append(index) ?? new(index);
    }

    public class FlowCaseVM(BmsData root, FlowSwitch parent, FlowCase branch, FlowViewModel parentVM) : FlowChildVMBase<FlowSwitch, FlowCase>(root, parent, branch, parentVM)
    {
        private bool _skip = branch.Skip;

        public bool Skip
        {
            get => _skip;
            set
            {
                if (SetProperty(ref _skip, value))
                {
                    SourceGeneric.Skip = value;
                    SendPropertyChanged(nameof(SkipText));
                }
            }
        }

        public string SkipText => _source!.Skip ? "" : "(no skip)";
    }
}
