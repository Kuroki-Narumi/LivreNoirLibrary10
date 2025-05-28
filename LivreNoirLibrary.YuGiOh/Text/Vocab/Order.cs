using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.YuGiOh.DuelLog;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Order = "手番";
        public const string Coin = "コイン";

        public const string First = "先攻";
        public const string Second = "後攻";
        public const string CoinWin = "コイン表";
        public const string CoinLose = "コイン裏";

        public const string WinFirst = "表/先";
        public const string LoseFirst = "裏/先";
        public const string LoseSecond = "裏/後";
        public const string WinSecond = "表/後";

        public const string First_Full = $"{CoinWin} / {First}";
        public const string CFirst_Full = $"{CoinLose} / {First}";
        public const string Second_Full = $"{CoinLose} / {Second}";
        public const string CSecond_Full = $"{CoinWin} / {Second}";

        public static string GetName(Order value) => GetEnumName(value, _order2name);
        public static string GetFullName(Order value) => GetEnumName(value, _order2name_full);
        public static Order GetOrder(string? name) => GetEnumValue(name, _name2order);

        private static readonly Dictionary<Order, string> _order2name = new()
        {
            { DuelLog.Order.First, WinFirst },
            { DuelLog.Order.CFirst, LoseFirst },
            { DuelLog.Order.Second, LoseSecond },
            { DuelLog.Order.CSecond, WinSecond },
        };

        private static readonly Dictionary<Order, string> _order2name_full = new()
        {
            { DuelLog.Order.First, First_Full },
            { DuelLog.Order.CFirst, CFirst_Full },
            { DuelLog.Order.Second, Second_Full },
            { DuelLog.Order.CSecond, CSecond_Full },
        };

        private static readonly Dictionary<string, Order> _name2order = CreateName2Order();

        private static Dictionary<string, Order> CreateName2Order()
        {
            var dic = _order2name.Invert();
            foreach (var (k, v) in _order2name_full)
            {
                dic.Add(v, k);
            }
            void Add(Order order)
            {
                dic.Add(order.ToString(), order);
            }
            Add(DuelLog.Order.First);
            Add(DuelLog.Order.CFirst);
            Add(DuelLog.Order.Second);
            Add(DuelLog.Order.CSecond);
            return dic;
        }
    }
}
