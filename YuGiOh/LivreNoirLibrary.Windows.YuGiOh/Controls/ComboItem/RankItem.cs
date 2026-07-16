using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.MasterDuel;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class RankItem(Rank value) : Windows.ComboItemBase<Rank>(value)
    {
        public string Name { get; } = value.GetName();
        protected override int GetRow(Rank value) => (int)value % 5;
        protected override int GetColumn(Rank value) => (int)value / 5;
        protected override Brush? GetBackground(int row, int column) => AttributeItem.GetBackgroundStatic(row, column);

        public static RankItem[] Items { get; }
        private static readonly Dictionary<Rank, RankItem> _items;
        public static RankItem? GetItem(Rank value) => _items.TryGetValue(value, out var item) ? item : null;

        static RankItem()
        {
            Items = [.. EnumUtils.Ranks.Select(v => new RankItem(v))];
            _items = CreateMap(Items);
        }
    }
}
