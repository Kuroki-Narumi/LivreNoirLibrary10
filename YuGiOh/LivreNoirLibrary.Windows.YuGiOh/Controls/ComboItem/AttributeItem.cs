using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.YuGiOh;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class AttributeItem : ComboItemBase<Attribute>
    {
        public ElementGroup Icon => LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Value);

        public static AttributeItem[] Items { get; }
        public static AttributeItem? GetItem(Attribute value) => _items.GetValueOrDefault(value);

        private AttributeItem(Attribute value, IVocabData? name) : base(value, name) { }
        protected override int GetRow(Attribute value) => (int)(value - 1) % 4;
        protected override int GetColumn(Attribute value) => (int)(value - 1) / 4;

        private static AttributeItem Create(Attribute value) => new(value, SelectVocabData(value));
        private static readonly Dictionary<Attribute, AttributeItem> _items;

        static AttributeItem()
        {
            Items = [.. EnumUtils.Attributes.Select(Create)];
            _items = CreateMap(Items);
        }

        private static VocabData? SelectVocabData(Attribute type) => type switch
        {
            Attribute.Light => Vocab.Current.Attr.Light,
            Attribute.Dark => Vocab.Current.Attr.Dark,
            Attribute.Water => Vocab.Current.Attr.Water,
            Attribute.Fire => Vocab.Current.Attr.Fire,
            Attribute.Earth => Vocab.Current.Attr.Earth,
            Attribute.Wind => Vocab.Current.Attr.Wind,
            Attribute.Divine => Vocab.Current.Attr.Divine,
            _ => null
        };

        private static SolidColorBrush AltBackground { get; } = MediaUtils.GetBrush("#08000080");
        internal static SolidColorBrush? GetBackgroundStatic(int row, int column) => ((row + column) % 2) is 1 ? AltBackground : null;
    }
}
