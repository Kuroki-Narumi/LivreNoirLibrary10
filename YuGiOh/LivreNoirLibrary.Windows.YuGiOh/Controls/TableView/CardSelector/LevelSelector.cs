using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class LevelSelector(int min = 0, int max = 13) : CardSelector
    {
        public int Minimum { get; set; } = min;
        public int Maximum { get; set; } = max;

        public override IVocabData Name => Vocab.Current.CInfo.Level;

        public override bool SkipEmpty => true;

        public override int GetKey(Card card) => card.LevelIndex;

        public override IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            for (var i = Minimum; i <= Maximum; i++)
            {
                yield return new(i, $"★{i}");
            }
        }
    }
}
