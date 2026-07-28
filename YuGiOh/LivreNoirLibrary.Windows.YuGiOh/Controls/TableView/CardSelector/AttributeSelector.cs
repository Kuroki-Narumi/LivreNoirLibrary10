using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class AttributeSelector(bool includesSpecial = true) : CardSelector
    {
        public bool IncludesSpecial { get; set; } = includesSpecial;

        public override IVocabData Name => Vocab.Current.CInfo.Attribute;

        public override bool SkipEmpty => false;

        public override int GetKey(Card card) => (int)card.AttributeIndex;

        public override IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            foreach (var value in EnumUtils.EnumerateAttributes(IncludesSpecial))
            {
                yield return new((int)value, value.GetName(), value.GetShortName());
            }
        }
    }
}
