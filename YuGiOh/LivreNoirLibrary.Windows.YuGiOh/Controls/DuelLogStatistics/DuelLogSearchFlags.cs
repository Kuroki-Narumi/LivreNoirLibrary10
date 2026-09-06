using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Vocabulary;
using LivreNoirLibrary.Windows.YuGiOh.Controls.DuelLogStatistics;
using System;
using System.Collections.Generic;
using System.Text;
using MD = LivreNoirLibrary.YuGiOh.MasterDuel;
using DS = LivreNoirLibrary.Windows.YuGiOh.Controls.DuelLogStatistics;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class DuelLogSearchFlags : ObservableObjectBase
    {
        public List<CheckableItem> Rank { get; } = [];
        public List<DS.OrderItem> Order { get; } = [];
        public List<CheckableItem> Result { get; } = [];

        public string AnyText { get; set => SetValue(ref field, value); } = "(any)";
        public int RankCount { get; private set => SetValue(ref field, value); }
        public string? RankText { get; private set => SetValue(ref field, value); }
        public int OrderCount { get; private set => SetValue(ref field, value); }
        public string? OrderText { get; private set => SetValue(ref field, value); }
        public int ResultCount { get; private set => SetValue(ref field, value); }
        public string? ResultText { get; private set => SetValue(ref field, value); }

        private readonly Dictionary<Rank, DS.RankItem> _rankItems = [];
        private readonly Dictionary<Order, DS.OrderItem> _orderItems = [];
        private readonly Dictionary<Result, DS.ResultItem> _resultItems = [];

        public DuelLogSearchFlags()
        {
            BuildRanks(Rank, _rankItems);
            BuildOrders(Order, _orderItems);
            BuildResults(Result, _resultItems);
        }

        private static void BuildRanks(List<CheckableItem> list, Dictionary<Rank, DS.RankItem> items)
        {
            for (var rank = MD.Rank.Room; rank <= MD.Rank.O4; rank++)
            {
                var r = (int)rank % 5;
                var row = rank switch
                {
                    MD.Rank.R1 => 5,
                    MD.Rank.R2 => 6,
                    >= MD.Rank.B5 and <= MD.Rank.M1 => 6 - r,
                    _ => r + 2
                };
                var item = new DS.RankItem(rank)
                {
                    Name = new FixedVocabData(rank.ToString()),
                    Row = row,
                    Column = (int)rank / 5,
                };
                list.Add(item);
                items[rank] = item;
            }

            AddParent("B", 1);
            AddParent("S", 2);
            AddParent("G", 3);
            AddParent("P", 4);
            AddParent("D", 5);
            AddParent("M", 6);

            var parent = new CheckableItem()
            {
                Name = Vocab.Current.Others,
                Column = 7,
            };
            parent.SetChildren(list.AsSpan((int)MD.Rank.Rate, 5));
            list.Add(parent);

            parent = new CheckableItem()
            {
                Name = Vocab.Current.All,
            };
            parent.SetChildren(list.AsSpan(0, (int)MD.Rank.O4 + 1));
            list.Add(parent);

            void AddParent(string header, int column)
            {
                var item = new CheckableItem()
                {
                    Name = new FixedVocabData(header),
                    Column = column,
                };
                item.SetChildren(list.AsSpan(column * 5, 5));
                list.Add(item);
            }
        }

        private static void BuildOrders(List<DS.OrderItem> list, Dictionary<Order, DS.OrderItem> items)
        {
            var wf = Add(MD.Order.First, Vocab.Current.DLog.First_S, 1, 1);
            var lf = Add(MD.Order.CFirst, Vocab.Current.DLog.CFirst_S, 1, 2);
            var ws = Add(MD.Order.CSecond, Vocab.Current.DLog.CSecond_S, 2, 1);
            var ls = Add(MD.Order.Second, Vocab.Current.DLog.Second_S, 2, 2);
            AddParent(Vocab.Current.DLog.CoinWin, 0, 1, wf, ws);
            AddParent(Vocab.Current.DLog.CoinLose, 0, 2, lf, ls);
            AddParent(Vocab.Current.DLog.First, 1, 0, wf, lf);
            AddParent(Vocab.Current.DLog.Second, 2, 0, ws, ls);

            DS.OrderItem Add(Order value, VocabData name, int row, int column)
            {
                var item = new DS.OrderItem(value)
                {
                    Name = name,
                    Row = row,
                    Column = column,
                    IsOptionMarkVisible = true,
                };
                list.Add(item);
                items[value] = item;
                return item;
            }

            void AddParent(VocabData name, int row, int column, params ReadOnlySpan<DS.OrderItem> values)
            {
                var item = new DS.OrderItem(0)
                {
                    Name = name,
                    Row = row,
                    Column = column,
                    IsOptionMarkVisible = false,
                };
                item.SetChildren(values);
                list.Add(item);
            }
        }

        private static void BuildResults(List<CheckableItem> list, Dictionary<Result, DS.ResultItem> items)
        {
            Add(MD.Result.Lose, Vocab.Current.DLog.Lose);
            Add(MD.Result.Win, Vocab.Current.DLog.Win);
            Add(MD.Result.Draw, Vocab.Current.DLog.Draw);
            Add(MD.Result.DiscLose, Vocab.Current.DLog.DiscLose);
            Add(MD.Result.DiscWin, Vocab.Current.DLog.DiscWin);

            void Add(Result value, VocabData name)
            {
                var iv = (int)value;
                var item = new DS.ResultItem(value)
                {
                    Name = name,
                    Row = iv % 3,
                    Column = iv / 3,
                };
                list.Add(item);
                items[value] = item;
            }
        }

        public void Load(DuelLogSearchConditions conditions)
        {
            var ranks = conditions.Ranks;
            foreach (var rank in _rankItems)
            {
                rank.Value.IsChecked = ranks.Contains(rank.Key);
            }
            var orders = conditions.Orders;
            foreach (var order in _orderItems)
            {
                order.Value.IsChecked = orders.Contains(order.Key);
            }
            var results = conditions.Results;
            foreach (var result in _resultItems)
            {
                result.Value.IsChecked = results.Contains(result.Key);
            }
            (RankText, RankCount) = GetText(_rankItems, AnyText);
            (OrderText, OrderCount) = GetText(_orderItems, AnyText);
            (ResultText, ResultCount) = GetText(_resultItems, AnyText);
        }

        public void SaveRanks(DuelLogSearchConditions conditions)
        {
            var ranks = conditions.Ranks;
            ranks.Clear();
            foreach (var rank in _rankItems)
            {
                if (rank.Value.IsChecked is true)
                {
                    ranks.Add(rank.Key);
                }
            }
            (RankText, RankCount) = GetText(_rankItems, AnyText);
        }

        public void SaveOrders(DuelLogSearchConditions conditions)
        {
            var orders = conditions.Orders;
            orders.Clear();
            foreach (var order in _orderItems)
            {
                if (order.Value.IsChecked is true)
                {
                    orders.Add(order.Key);
                }
            }
            (OrderText, OrderCount) = GetText(_orderItems, AnyText);
        }

        public void SaveResults(DuelLogSearchConditions conditions)
        {
            var results = conditions.Results;
            results.Clear();
            foreach (var result in _resultItems)
            {
                if (result.Value.IsChecked is true)
                {
                    results.Add(result.Key);
                }
            }
            (ResultText, ResultCount) = GetText(_resultItems, AnyText);
        }

        private static (string, int) GetText<TKey, TValue>(Dictionary<TKey, TValue> items, string anyText)
            where TKey : struct
            where TValue : CheckableItem
        {
            return DuelLog.GetTagText(EnumerateChecked(items), anyText);
        }

        private static IEnumerable<string?> EnumerateChecked<TKey, TValue>(Dictionary<TKey, TValue> items)
            where TKey : struct
            where TValue : CheckableItem
        {
            foreach (var (_, item) in items)
            {
                if (item.IsChecked is true)
                {
                    yield return item.Name.Value;
                }
            }
        }
    }
}
