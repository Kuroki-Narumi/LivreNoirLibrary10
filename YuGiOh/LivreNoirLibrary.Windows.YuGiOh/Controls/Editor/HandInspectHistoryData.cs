using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Inspect;
using LivreNoirLibrary.YuGiOh.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class HandInspectHistoryData(HandConditionsCollection? conds) : CompressionHistoryData<HandConditionsCollection>(conds), IHistoryData<HandInspectHistoryData>
    {
        public int SelectedIndex { get; set; } = -1;

        public bool IsSelectionStored { get; private set; }

        public bool EqualsAll(HandInspectHistoryData other) => base.EqualsAll(other);

        public void StoreSelection(ReadOnlySpan<IListView> listViews)
        {
            SelectedIndex = listViews[0].SelectedIndex;
            IsSelectionStored = true;
        }

        public void RestoreSelection(ReadOnlySpan<IListView> listViews)
        {
            listViews[0].SelectedIndex = SelectedIndex;
        }

        protected override void Dump(Stream stream, HandConditionsCollection source)
        {
            Json.Dump(stream, source, false);
        }

        protected override void Load(Stream stream, HandConditionsCollection target, object? state)
        {
            var source = Json.Load<List<HandInspectConditions<int>>>(stream);
            target.Clear();
            if (state is ICardProvider provider)
            {
                target.Load(source, provider);
            }
        }
    }
}
