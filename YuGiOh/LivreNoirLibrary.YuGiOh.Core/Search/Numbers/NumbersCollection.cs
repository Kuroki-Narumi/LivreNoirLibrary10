using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class NumbersCollection
    {
        private readonly ObservableList<NumbersCard> _candidates = [];
        private readonly ObjectCache<NumbersCard> _candidateCache = new(() => new());

        private readonly Dictionary<int, List<NumbersKey>> _keyMap = [];
        private readonly CollectionCache<NumbersKey, List<NumbersKey>> _keyCache = new(() => []);

        private readonly List<int> _materialKeys = [];
        private readonly List<List<Card>> _materialLists = [];
        private readonly CollectionCache<Card, List<Card>> _materialCache = new(() => []);

        public IEnumerable<int> Numbers => _materialKeys;

        public ObservableList<NumbersCard> Candidates => _candidates;

        public IEnumerable<NumbersKey> GetKeys(NumbersCard? card)
        {
            if (card is not null && _keyMap.TryGetValue(card.Number, out var list))
            {
                return list;
            }
            return [];
        }

        public (IEnumerable<Card>?, IEnumerable<Card>?, IEnumerable<Card>?, IEnumerable<Card>?) GetMaterials(NumbersKey? key)
        {
            if (key is null)
            {
                return (null, null, null, null);
            }
            var keys = _materialKeys;
            var lists = _materialLists;
            SortedList.TryGetValue(keys, lists, key.Value1, out _, out var list1);
            SortedList.TryGetValue(keys, lists, key.Value2, out _, out var list2);
            SortedList.TryGetValue(keys, lists, key.Value3, out _, out var list3);
            SortedList.TryGetValue(keys, lists, key.Value4, out _, out var list4);
            return (list1, list2, list3, list4);
        }

        public void RefreshCandidates(ICardEnumerable source, ProgressReporter? p = null, CancellationToken c = default)
        {
            p?.ReportInitial("Refresh Numbers...");

            var candidates = _candidates;
            var candidateCache = _candidateCache;
            var keyMap = _keyMap;
            var keyCache = _keyCache;
            var materialKeys = _materialKeys;
            var materialLists = _materialLists;
            var materialCache = _materialCache;
            candidates.Clear();
            candidateCache.Clear();
            keyMap.Clear();
            keyCache.Clear();
            materialKeys.Clear();
            materialLists.Clear();
            materialCache.Clear();

            // 合成元または合成先となりうる「No.」モンスターを数値ごとにグループ化
            p?.Report("creating numbers list...");
            var regex = Regex_Numbers;
            foreach (var card in source.CardEnumerable)
            {
                c.ThrowIfCancellationRequested();
                var name = card.Name.AsSpan();
                foreach (var range in regex.EnumerateMatches(name))
                {
                    if (int.TryParse(name.Slice(range.Index, range.Length), out var number) && number < 1000)
                    {
                        var list = SortedList.GetOrAdd(materialKeys, materialLists, number, MaterialFactory, out _);
                        list.Add(card);
                    }
                    break;
                }
            }
            c.ThrowIfCancellationRequested();

            // 有効な「No.」値にできる合成元を全て列挙
            var min = materialKeys[0];
            var max = materialKeys[^1];
            var iMax = max / 4;
            for (var i = min; i <= iMax; i++)
            {
                p?.ReportFraction(i, max);
                var jMax = (max - i) / 3;
                for (var j = i + 1; j <= jMax; j++)
                {
                    var kMax = (max - i - j) / 2;
                    for (var k = j + 1; k <= kMax; k++)
                    {
                        var lMax = (max - i - j - k);
                        for (var l = k + 1; l <= lMax; l++)
                        {
                            var key = i + j + k + l;
                            var list = keyMap.GetOrAdd(i + j + k + l, KeyFactory);
                            list.Add(new(i, j, k, l));
                        }
                    }
                    c.ThrowIfCancellationRequested();
                }
            }
            c.ThrowIfCancellationRequested();

            // 合成先となりうる「No.」モンスターの一覧を作成
            p?.Report("creating candidates...");
            foreach (var (number, _) in keyMap)
            {
                if (SortedList.TryGetValue(materialKeys, materialLists, number, out _, out var list))
                {
                    foreach (var card in list.AsSpan())
                    {
                        var item = candidateCache.GetNext();
                        item.Number = number;
                        item.ThisCard = card;
                        candidates.AddWithoutNotify(item);
                    }
                }
            }
            candidates.NotifyCollectionReset();
            c.ThrowIfCancellationRequested();
        }

        private List<Card> MaterialFactory(int _) => _materialCache.GetNext();
        private List<NumbersKey> KeyFactory(int _) => _keyCache.GetNext();

        [GeneratedRegex(@"(?<=^.?No\.)\d+", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_Numbers { get; }
    }
}
