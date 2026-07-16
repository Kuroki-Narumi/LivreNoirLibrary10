using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.IO;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardEditorHistoryData(ObservableList<Card>? data) : CompressionHistoryData<ObservableList<Card>>(data), IHistoryData<CardEditorHistoryData>
    {
        public int SelectedIndex { get; set; } = -1;

        public bool IsSelectionStored { get; private set; }

        public bool EqualsAll(CardEditorHistoryData other) => base.EqualsAll(other);

        public void StoreSelection(ReadOnlySpan<IListView> listViews)
        {
            SelectedIndex = listViews[0].SelectedIndex;
            IsSelectionStored = true;
        }

        public void RestoreSelection(ReadOnlySpan<IListView> listViews)
        {
            listViews[0].SelectedIndex = SelectedIndex;
        }

        protected override void Dump(Stream stream, ObservableList<Card> source)
        {
            Json.Dump(stream, source, false);
        }

        protected override void Load(Stream stream, ObservableList<Card> target)
        {
            var source = Json.Load<Card[]>(stream);
            target.ClearWithoutNotify();
            target.AddRange(source);
        }
    }
}
