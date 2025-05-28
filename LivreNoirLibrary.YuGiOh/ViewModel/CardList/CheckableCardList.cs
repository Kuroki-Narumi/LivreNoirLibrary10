using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class CheckableCardList : SortedCardWrapperList<CheckableCard>
    {
        public CheckableCardList() { }
        public CheckableCardList(CheckableCardList list) => Load(list);
        public CheckableCardList(IEnumerable<Card> list) => Load(list);

        public override void Add(Card card) => Add(new CheckableCard(card));
        public override bool Remove(Card card) => RemoveKey(card.Id);

        public override void Load(IEnumerable<Card> source)
        {
            ClearWithoutNotify();
            foreach (var card in source)
            {
                AddWithoutNotify(new CheckableCard(card));
            }
            NotifyCollectionReset();
        }

        public void Load(DeckCardList source)
        {
            ClearWithoutNotify();
            foreach (var (card, _) in source.EnumCardsWithCount())
            {
                AddWithoutNotify(new CheckableCard(card));
            }
            NotifyCollectionReset();
        }

        public void Update(IEnumerable<Card> source)
        {
            Dictionary<int, CheckableCard> current = [];
            var c = _key_list.Count;
            for (var i = 0; i < c; i++)
            {
                current.Add(_key_list[i], _list[i]);
            }
            foreach (var card in source)
            {
                if (!current.Remove(card.Id))
                {
                    AddWithoutNotify(new CheckableCard(card));
                }
            }
            foreach (var (_, card) in current)
            {
                RemoveWithoutNotify(card);
            }
            NotifyCollectionReset();
        }

        public List<Card> GetCheckedList()
        {
            List<Card> result = [];
            foreach (var item in CollectionsMarshal.AsSpan(_list))
            {
                if (item.IsChecked)
                {
                    result.Add(item.Card);
                }
            }
            return result;
        }

        public HashSet<int> GetCheckedIdList()
        {
            HashSet<int> result = [];
            foreach (var item in CollectionsMarshal.AsSpan(_list))
            {
                if (item.IsChecked)
                {
                    result.Add(item.Id);
                }
            }
            return result;
        }
    }
}
