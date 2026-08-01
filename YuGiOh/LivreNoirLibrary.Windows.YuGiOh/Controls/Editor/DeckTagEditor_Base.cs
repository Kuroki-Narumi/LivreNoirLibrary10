using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.MasterDuel;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class DeckTagEditor_Base : FileEditorBase<DeckTagHistoryData>
    {
        [DependencyProperty]
        private DeckTagCollection? _itemsSource;

        protected virtual void OnItemsSourceChanged(DeckTagCollection? value)
        {
            this.ClearHistory();
        }

        protected sealed override DeckTagHistoryData GetHistoryData() => new(ItemsSource);
        protected sealed override void ProcessNew() => ItemsSource?.Clear();

        protected sealed override bool ProcessOpen(string path)
        {
            if (ItemsSource is { } items && Json.TryOpen<DeckTag[]>(path, out var source))
            {
                DeckTagHistoryData.LoadData(items, source);
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
