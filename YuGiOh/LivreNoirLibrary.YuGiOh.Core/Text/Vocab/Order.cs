using System;
using System.Collections.Generic;
using LivreNoirLibrary.YuGiOh.MasterDuel;

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

        private static readonly Dictionary<Order, string> _order2name = new()
        {
            { MasterDuel.Order.First, WinFirst },
            { MasterDuel.Order.CFirst, LoseFirst },
            { MasterDuel.Order.Second, LoseSecond },
            { MasterDuel.Order.CSecond, WinSecond },
        };

        private static readonly Dictionary<Order, string> _order2name_full = new()
        {
            { MasterDuel.Order.First, First_Full },
            { MasterDuel.Order.CFirst, CFirst_Full },
            { MasterDuel.Order.Second, Second_Full },
            { MasterDuel.Order.CSecond, CSecond_Full },
        };

        private static readonly Dictionary<string, Order>.AlternateLookup<ReadOnlySpan<char>> _name2order = CreateName2Order();
        private static Dictionary<string, Order>.AlternateLookup<ReadOnlySpan<char>> CreateName2Order()
        {
            var dic = CreateInvertedDictionary(_order2name);
            foreach (var (k, v) in _order2name_full)
            {
                dic[v] = k;
            }
            return dic;
        }

        public static string GetName(this Order value) => GetEnumName(value, _order2name);
        public static string GetFullName(this Order value) => GetEnumName(value, _order2name_full);
        public static Order GetOrder(ReadOnlySpan<char> name) => GetEnumValue(name, _name2order);
    }
}
