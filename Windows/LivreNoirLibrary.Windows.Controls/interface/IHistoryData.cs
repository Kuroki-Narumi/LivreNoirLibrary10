using LivreNoirLibrary.Windows.Controls;
using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IHistoryData<TSelf> where TSelf : IHistoryData<TSelf>
    {
        bool IsSelectionStored { get; }

        void StoreSelection(ReadOnlySpan<IListView> listViews);

        bool EqualsAll(TSelf other);

        void RestoreSelection(ReadOnlySpan<IListView> listViews);
    }
}
