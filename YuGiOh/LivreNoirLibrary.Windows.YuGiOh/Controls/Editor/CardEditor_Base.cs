using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class CardEditor_Base : FileEditorBase<CardEditorHistoryData>
    {
        [DependencyProperty]
        private ObservableList<Card>? _itemsSource;

        protected virtual void OnItemsSourceChanged(ObservableList<Card>? value)
        {
            this.ClearHistory();
        }

        protected sealed override CardEditorHistoryData GetHistoryData() => new(ItemsSource);
        protected sealed override void ProcessNew() => ItemsSource?.Clear();

        protected sealed override bool ProcessOpen(string path)
        {
            if (ItemsSource is { } items && Json.TryOpen<Card[]>(path, out var cards))
            {
                items.ClearWithoutNotify();
                items.AddRange(cards);
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
