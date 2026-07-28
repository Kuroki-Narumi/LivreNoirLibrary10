using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class CardTypeSelector(bool monster = true, bool spell = true, bool trap = true) : CardSelector
    {
        public bool IncludesMonsters { get; set; } = monster;
        public bool IncludesSpells { get; set; } = spell;
        public bool IncludesTraps { get; set; } = trap;

        public override IVocabData Name => Vocab.Current.CInfo.CardType;

        public override bool SkipEmpty => true;

        public override int GetKey(Card card) => (int)card.CardType;

        public override IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            foreach (var value in EnumUtils.EnumerateCardTypes(IncludesMonsters, IncludesSpells, IncludesTraps))
            {
                var name = value.GetName();
                yield return new((int)value, name, Converters.VerticalStringConverter.Convert(name));
            }
        }
    }
}
