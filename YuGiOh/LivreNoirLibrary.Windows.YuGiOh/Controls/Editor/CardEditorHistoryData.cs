using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.IO;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardEditorHistoryData(ICardCollection? data) : CompressionHistoryData<ICardCollection>(data), IHistoryData<CardEditorHistoryData>
    {
        private int _selectedIndex = -1;

        public bool IsSelectionStored { get; private set; }

        public bool EqualsAll(CardEditorHistoryData other) => base.EqualsAll(other);

        public void StoreSelection(ReadOnlySpan<ListBox> listViews)
        {
            if (listViews.Length > 0)
            {
                _selectedIndex = listViews[0].SelectedIndex;
                IsSelectionStored = true;
            }
        }

        public void RestoreSelection(ReadOnlySpan<ListBox> listViews)
        {
            if (listViews.Length > 0)
            {
                var lv = listViews[0];
                if (lv.SelectedIndex == _selectedIndex)
                {
                    lv.SelectedIndex = -1;
                }
                lv.SelectedIndex = _selectedIndex;
            }
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
