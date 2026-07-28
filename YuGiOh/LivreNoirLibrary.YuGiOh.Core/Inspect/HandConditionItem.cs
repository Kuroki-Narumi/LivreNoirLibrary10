using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandConditionItem : ObservableObjectBase
    {
        public SortedCardList Cards { get; } = [];
        public bool IsFirst { get; internal set => SetValue(ref field, value); }

        private readonly HashSet<int> _ids = [];

        public static bool IsEffective(HandConditionItem item) => item.Cards.Count > 0;

        public void Load(List<int> ids, ICardProvider? provider)
        {
            var list = Cards;
            list.ClearWithoutNotify();
            list.AddRange(ids, provider);
        }

        public void Load(List<string> ids, ICardProvider? provider)
        {
            var list = Cards;
            list.ClearWithoutNotify();
            list.AddRange(ids, provider);
        }

        public HandConditionItem Clone()
        {
            HandConditionItem result = new()
            {
                IsFirst = IsFirst,
            };
            result.Cards.AddRange(Cards);
            return result;
        }

        public void Prepare()
        {
            _ids.Clear();
            _ids.UnionWith(Cards.Select(IId.GetId));
        }

        public bool IsMatch(List<int> mutableList)
        {
            var ids = _ids;
            for (var i = 0; i < mutableList.Count; i++)
            {
                if (ids.Contains(mutableList[i]))
                {
                    mutableList.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
