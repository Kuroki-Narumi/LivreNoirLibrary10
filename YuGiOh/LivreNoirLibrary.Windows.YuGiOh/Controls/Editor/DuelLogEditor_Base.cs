using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class DuelLogEditor_Base : FileEditorBase<DuelLogHistoryData>
    {
        [DependencyProperty]
        private ICollection<DuelLog>? _itemsSource;

        protected virtual void OnItemsSourceChanged(ICollection<DuelLog>? value)
        {
            this.ClearHistory();
        }

        protected sealed override DuelLogHistoryData GetHistoryData() => new(ItemsSource);
        protected sealed override void ProcessNew() => ItemsSource?.Clear();

        protected sealed override bool ProcessOpen(string path)
        {
            if (ItemsSource is { } items && Json.TryOpen<DuelLog[]>(path, out var source))
            {
                DuelLogHistoryData.LoadData(items, source);
                return true;
            }
            return false;
        }

        protected sealed override void ProcessSave(string path)
        {
            if (ItemsSource is { } items)
            {
                Json.Save(path, items, true);
            }
        }
    }
}
