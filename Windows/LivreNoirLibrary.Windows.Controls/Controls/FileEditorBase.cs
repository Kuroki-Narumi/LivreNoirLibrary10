using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class FileEditorBase<T> : UserControl, IHistoryOwner<T>, IDataNew, IDataOpen, IDataSave
        where T : IHistoryData<T>
    {
        private readonly History<T> _history;

        IHistory IHistoryOwner.History => _history;

        protected abstract IListView[] ListViews { get; }

        public virtual string OpenFilter => Filters.Json;
        public virtual string SaveFilter => Filters.Json;
        public virtual Regex DropFilter => ExtRegs.Json;

        protected virtual bool CanNew => true;
        protected virtual bool CanOpen => true;
        protected virtual bool CanSave => true;
        protected virtual FileDialogOptions? OpenDialogOptions => null;
        protected virtual FileDialogOptions? SaveDialogOptions => null;
        protected virtual string? SaveFilePath => null;

        bool IDataNew.CanNew => CanNew;
        bool IDataOpen.CanOpen => CanOpen;
        FileDialogOptions? IDataOpen.OpenDialogOptions => OpenDialogOptions;
        bool IDataSave.CanSave => CanSave;
        FileDialogOptions? IDataSave.SaveDialogOptions => SaveDialogOptions;
        string? IDataSave.SaveFilePath => SaveFilePath;

        public FileEditorBase()
        {
            Initialize();
            _history = new(this);
            AllowDrop = true;
            this.RegisterHistoryCommands();
            this.RegisterNewCommand();
            this.RegisterOpenCommand();
            this.RegisterSaveCommand();
        }

        protected abstract void Initialize();
        protected abstract T GetHistoryData();
        protected abstract void ApplyHistory(T historyData);

        T IHistoryOwner<T>.GetHistoryData() => GetHistoryData();
        void IHistoryOwner<T>.ApplyHistory(T historyData) => ApplyHistory(historyData);
        bool IHistoryOwner<T>.HistoryEquals(T previous, T current) => previous.EqualsAll(current);

        protected void BeforeEdit()
        {
            _history.LastData.StoreSelection(ListViews);
        }

        void IHistoryOwner<T>.EnsureHistoryData(T data)
        {
            if (!data.IsSelectionStored)
            {
                data.StoreSelection(ListViews);
            }
        }

        void IDataNew.ExecuteNewData()
        {
            BeforeEdit();
            ProcessNew();
            this.OnEdit();
        }

        public void ExecuteOpenData(string path)
        {
            BeforeEdit();
            if (ProcessOpen(path))
            {
                this.OnEdit();
            }
        }

        void IDataSave.ExecuteSaveData(string path) => ProcessSave(path);

        protected abstract void ProcessNew();
        protected abstract bool ProcessOpen(string path);
        protected abstract void ProcessSave(string path);

        protected override void OnDragOver(DragEventArgs e)
        {
            if (!e.Handled)
            {
                e.ApplyEffect(DropFilter);
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (!e.Handled && e.TryGetAvailable(DropFilter, out var path))
            {
                e.Handled = true;
                ExecuteOpenData(path);
            }
        }
    }
}
