using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class HedgehogItemCollection : IClear
    {
        private readonly Dictionary<HedgehogKey, HedgehogItem> _items = [];
        private readonly ObjectCache<HedgehogItem> _cache = new(() => new());
        private readonly Dictionary<HedgehogKey, (int, int)> _countCache = [];

        public int LevelLimit { get; set; } = 3;

        public void Clear()
        {
            _items.Clear();
            _cache.Clear();
        }

        public void Refresh(ICardEnumerable source)
        {
            Clear();
            var levelLimit = LevelLimit;
            var items = _items;
            var countCache = _countCache;
            foreach (var card in source.CardEnumerable)
            {
                if (card.IsMainMonster() && card.Level <= levelLimit)
                {
                    HedgehogKey key = new(card);
                    var item = items.GetOrAdd(key, GetNextItem);
                    (card.HasEffect ? item.EffectMonsters : item.NormalMonsters).Add(card);
                    countCache[key] = (item.NormalCount, item.EffectCount);
                }
            }
            foreach (var (key, (n, e)) in countCache)
            {
                if (n * e is 0)
                {
                    items.Remove(key);
                }
            }
            countCache.Clear();
        }

        private HedgehogItem GetNextItem(HedgehogKey key)
        {
            var item = _cache.GetNext();
            item.Key = key;
            return item;
        }

        public IEnumerable<HedgehogItem> Items
        {
            get
            {
                foreach (var (_, item) in _items)
                {
                    yield return item;
                }
            }
        }
    }
}
