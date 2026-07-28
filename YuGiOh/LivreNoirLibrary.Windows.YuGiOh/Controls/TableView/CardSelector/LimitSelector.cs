using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class LimitSelector(bool unusable = true) : CardSelector
    {
        public bool IncludesUnusable { get; set; } = unusable;

        public override IVocabData Name => Vocab.Current.Limit.Regulation;

        public override bool SkipEmpty => true;

        public override int GetKey(Card card) => card.ActualLimitCount;

        public override IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            for (var i = 0; i <= 3; i++)
            {
                var name = LivreNoirLibrary.YuGiOh.Vocab.GetLimitText(i, true);
                yield return new(i, name, Converters.VerticalStringConverter.Convert(name));
            }
            if (IncludesUnusable)
            {
                var name = LivreNoirLibrary.YuGiOh.Vocab.GetLimitText(-1);
                yield return new(-1, name, Converters.VerticalStringConverter.Convert(name));
            }
        }
    }
}
