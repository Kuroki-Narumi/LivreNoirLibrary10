using LivreNoirLibrary.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LivreNoirLibrary.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CloningCardList : DisposableBase, ICardProvider, ISafeEnumerable<Card>, INotifyPropertyChanged, INotifyCollectionChanged, ICardEnumerable, IIdEnumerable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ICardProvider Source  { get; private set; }

        public CloningCardList(ICardProvider source)
        {
            Source = source;
            if (source is INotifyPropertyChanged pc)
            {
                pc.PropertyChanged += Source_PropertyChanged;
            }
            if (source is INotifyCollectionChanged cc)
            {
                cc.CollectionChanged += Source_CollectionChanged;
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            if (Source is INotifyPropertyChanged pc)
            {
                pc.PropertyChanged -= Source_PropertyChanged;
            }
            if (Source is INotifyCollectionChanged cc)
            {
                cc.CollectionChanged -= Source_CollectionChanged;
            }
            Source = null!;
        }

        private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
        private void Source_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        public IEnumerator<Card> GetEnumerator() => Source.GetEnumerator();
        IEnumerable<Card> ICardEnumerable.CardEnumerable => this;

        Card? ICardProvider.GetOrDefault(int id) => Source?.GetOrDefault(id);
        bool ICardProvider.TryGetByName(string name, [MaybeNullWhen(false)] out Card card)
        {
            if (Source is { } s)
            {
                return s.TryGetByName(name, out card);
            }
            card = null;
            return false;
        }
    }
}
