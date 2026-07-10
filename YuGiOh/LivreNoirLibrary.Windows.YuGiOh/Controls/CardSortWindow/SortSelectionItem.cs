using LivreNoirLibrary.YuGiOh.Search;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class SortSelectionItem(IVocabData vocab, SortKey key)
    {
        public IVocabData VocabData { get; } = vocab;
        public SortKey Key { get; } = key;

        public static SortSelectionItem[] Items { get; } = CreateItems();
        public static SortSelectionItem None => Items[0];

        private static SortSelectionItem[] CreateItems()
        {
            var v = Vocab.Current;
            return
            [
                new(v.None, SortKey.None),
                new(v.CInfo.Id, SortKey.Id),
                new(v.CInfo.Name, SortKey.Name),
                new(v.CInfo.Ruby, SortKey.Ruby),
                new(v.CInfo.EnName, SortKey.EnName),
                new(v.CInfo.CardType, SortKey.CardType),
                new(v.CInfo.Attribute, SortKey.Attribute),
                new(v.CInfo.MonsterType, SortKey.MonsterType),
                new(v.CInfo.LevelRankLink, SortKey.Level),
                new(v.CInfo.Atk, SortKey.Atk),
                new(v.CInfo.Def, SortKey.Def),
                new(v.CInfo.PendulumScale, SortKey.Scale),
                new(v.SortKey.NameLength, SortKey.NameLength),
                new(v.SortKey.RubyLength, SortKey.RubyLength),
                new(v.SortKey.EnNameLength, SortKey.EnNameLength),
                new(v.SortKey.TextLength, SortKey.TextLength),
                new(v.SortKey.PTextLength, SortKey.PTextLength),
                new(v.SortKey.FirstDateOcg, SortKey.FirstDateOcg),
                new(v.SortKey.LastDateOcg, SortKey.LastDateOcg),
                new(v.SortKey.FirstDateTcg, SortKey.FirstDateTcg),
                new(v.SortKey.LastDateTcg, SortKey.LastDateTcg),
                new(v.CInfo.PackInfo, SortKey.PackCount),
                new(v.SortKey.PackInfoOcg, SortKey.PackCountOcg),
                new(v.SortKey.PackInfoTcg, SortKey.PackCountTcg),
            ];
        }

        private static Dictionary<SortKey, SortSelectionItem>? _index;

        public static SortSelectionItem? GetSelectionItem(SortKey key)
        {
            _index ??= CreateIndex();
            if (_index.TryGetValue(key, out var item))
            {
                return item;
            }
            return null;
        }

        private static Dictionary<SortKey, SortSelectionItem> CreateIndex()
        {
            Dictionary<SortKey, SortSelectionItem> dic = [];
            foreach (var item in Items)
            {
                dic[item.Key] = item;
            }
            return dic;
        }

        public override string ToString() => $"({VocabData.Value}, {Key})";
    }
}
