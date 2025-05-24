using System;
using System.Collections.Generic;
using System.Threading;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class IndexesOptionsBase : ObservableObjectBase
    {
        public const string DefaultIndexText = "ex) 01-0Z 15 19-1C";

        private static readonly Lock _index_lock = new();
        private static readonly SortedSet<int> _index_cache = [];

        protected readonly SortedSet<int> _indexes = [];

        public SortedSet<int> Indexes
        {
            get => _indexes;
            set
            {
                _indexes.Clear();
                _indexes.UnionWith(value);
                SendPropertyChanged();
            }
        }

        public string GetIndexText(int radix) => BasedIndex.GetIndexListText(_indexes, radix);

        public bool TrySetIndex(string? text, int radix)
        {
            lock (_index_lock)
            {
                if (BasedIndex.TryParseIndexText(text, _index_cache, radix, radix * radix))
                {
                    Indexes = _index_cache;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
