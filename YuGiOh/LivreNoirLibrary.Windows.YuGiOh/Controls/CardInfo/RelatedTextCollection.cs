using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class RelatedTextCollection : ObservableObjectBase, ISafeEnumerable<string>, IObservableCollection
    {
        internal readonly HashSet<string> _set = [];

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count => _set.Count;

        public void Clear()
        {
            _set.Clear();
            this.NotifyCollectionReset();
        }

        public void Load(Card card)
        {
            card.CreateRelatedText(_set);
            this.NotifyCollectionReset();
        }

        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        IEnumerator<string> IEnumerable<string>.GetEnumerator() => _set.GetEnumerator();
    }
}
