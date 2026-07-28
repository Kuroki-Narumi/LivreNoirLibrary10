using LivreNoirLibrary.YuGiOh.Search;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class MatchTypeItem : AltBackgroundComboItem<MatchType>
    {
        public static MatchTypeItem[] Items { get; }
        public static MatchTypeItem? GetItem(MatchType value) => _items.GetValueOrDefault(value);

        public IVocabData Description { get; }

        private MatchTypeItem(MatchType value, IVocabData name, IVocabData desc) : base(value, name)
        {
            Description = desc;
        }

        private static readonly Dictionary<MatchType, MatchTypeItem> _items;

        static MatchTypeItem()
        {
            var v = Vocab.Current;
            Items = [
                new(MatchType.Any, v.MatchType_Any, v.MatchType_Any_Desc),
                new(MatchType.All, v.MatchType_All, v.MatchType_All_Desc),
                new(MatchType.Minimum, v.MatchType_Minimum, v.MatchType_Minimum_Desc),
                new(MatchType.Perfect, v.MatchType_Perfect, v.MatchType_Perfect_Desc),
            ];
            _items = CreateMap(Items);
        }
    }
}
