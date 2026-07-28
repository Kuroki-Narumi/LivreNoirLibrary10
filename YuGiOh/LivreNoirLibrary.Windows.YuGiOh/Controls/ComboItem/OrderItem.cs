using LivreNoirLibrary.YuGiOh.MasterDuel;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class OrderItem : AltBackgroundComboItem<Order>
    {
        public static OrderItem[] Items { get; }
        public static OrderItem? GetItem(Order value) => _items.GetValueOrDefault(value);

        protected override int GetRow(Order value) => value switch
        {
            Order.Second => 1,
            Order.CFirst => 2,
            Order.CSecond => 3,
            _ => 0,
        };

        private OrderItem(Order value, IVocabData name) : base(value, name) { }
        private static readonly Dictionary<Order, OrderItem> _items;

        static OrderItem()
        {
            var v = Vocab.Current.DLog;
            Items = [new(Order.First, v.First), new(Order.Second, v.Second), new(Order.CFirst, v.CFirst), new(Order.CSecond, v.CSecond)];
            _items = CreateMap(Items);
        }
    }
}
