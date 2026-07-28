using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.IO;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardEditorHistoryData(ICardCollection? data) : CompressionHistoryData<ICardCollection>(data), IHistoryData<CardEditorHistoryData>
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

        protected override void Dump(Stream stream, ICardCollection source)
        {
            Json.Dump(stream, source, false);
        }

        protected override void Load(Stream stream, ICardCollection target, object? state)
        {
            var source = Json.Load<Card[]>(stream);
            target.Clear();
            target.AddRange(source);
        }
    }
}
