using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LivreNoirLibrary.YuGiOh.MasterDuel.ViewModel
{
    public class CheckableDeckTagCollection : CheckableItemCollection<string, CheckableDeckTag>
    {
        protected override CheckableDeckTag CreateItem() => new();
        protected override string GetKey(CheckableDeckTag item) => item.Name ?? "";

        public void LoadFlags(IEnumerable<string> tags)
        {
            using var o = ObjectPool.RentHashSet<string>(out var set);
            set.UnionWith(tags);
            foreach (var item in _items.AsSpan())
            {
                item.IsChecked = set.Contains(item.Name ?? "");
            }
        }

        public void AttachSource<T>(T source)
            where T : IEnumerable<DeckTag>, INotifyCollectionChanged
        {
            RefreshSource(source);
            source.CollectionChanged += Source_OnCollectionChanged;
        }

        public void DetachSource(INotifyCollectionChanged source)
        {
            source.CollectionChanged -= Source_OnCollectionChanged;
        }

        private void Source_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshSource(sender as IEnumerable<DeckTag>);
        }

        private void RefreshSource(IEnumerable<DeckTag>? source)
        {
            if (source is null)
            {
                Clear();
                return;
            }
            RefreshItems(source, InitializeItem);
        }

        private static void InitializeItem(CheckableDeckTag item, DeckTag source) => item.Update(source);
    }
}
