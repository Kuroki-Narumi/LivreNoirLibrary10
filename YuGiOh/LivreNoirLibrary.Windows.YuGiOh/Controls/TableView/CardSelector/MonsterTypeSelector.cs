using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class MonsterTypeSelector(bool includesSpecial = true) : CardSelector
    {
        public bool IncludesSpecial { get; set; } = includesSpecial;

        public override IVocabData Name => Vocab.Current.CInfo.MonsterType;

        public override bool SkipEmpty => false;

        public override int GetKey(Card card) => (int)card.MonsterTypeIndex;

        public override IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            foreach (var value in EnumUtils.EnumerateMonsterTypes(IncludesSpecial))
            {
                var name = value.GetName();
                yield return new((int)value, name, Converters.VerticalStringConverter.Convert(name));
            }
        }
    }
}
