using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.YuGiOh;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class CardTypeItem : AltBackgroundComboItem<CardType>
    {
        public ElementGroup Icon => LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(Value, true, false);

        public static CardTypeItem[] Items { get; }
        public static CardTypeItem? GetItem(CardType value) => _items.GetValueOrDefault(value);

        private CardTypeItem(CardType value, IVocabData? name) : base(value, name) { }
        protected override int GetRow(CardType value) => value < CardType.Normal_Spell ? (int)value - 1 : (int)value % 16;
        protected override int GetColumn(CardType value) => value < CardType.Normal_Spell ? 0 : (int)value / 16;

        private static CardTypeItem Create(CardType value) => new(value, SelectVocabData(value));
        private static readonly Dictionary<CardType, CardTypeItem> _items;

        static CardTypeItem()
        {
            Items = [.. EnumUtils.CardTypes.Select(Create)];
            _items = CreateMap(Items);
        }

        private static IVocabData? SelectVocabData(CardType type) => type switch
        {
            CardType.Main_Monster => Vocab.Current.CType.Monster,
            CardType.Fusion_Monster => Vocab.Current.CType.Fusion,
            CardType.Ritual_Monster => Vocab.Current.CType.Ritual,
            CardType.Synchro_Monster => Vocab.Current.CType.Synchro,
            CardType.Xyz_Monster => Vocab.Current.CType.Xyz,
            CardType.Link_Monster => Vocab.Current.CType.Link,
            CardType.Normal_Spell => Vocab.Current.CType.Normal_Spell,
            CardType.Field_Spell => Vocab.Current.CType.Field_Spell,
            CardType.Equip_Spell => Vocab.Current.CType.Equip_Spell,
            CardType.Continuous_Spell => Vocab.Current.CType.Continuous_Spell,
            CardType.Quick_Spell => Vocab.Current.CType.Quick_Spell,
            CardType.Ritual_Spell => Vocab.Current.CType.Ritual_Spell,
            CardType.Normal_Trap => Vocab.Current.CType.Normal_Trap,
            CardType.Continuous_Trap => Vocab.Current.CType.Continuous_Trap,
            CardType.Counter_Trap => Vocab.Current.CType.Counter_Trap,
            _ => null
        };
    }
}
