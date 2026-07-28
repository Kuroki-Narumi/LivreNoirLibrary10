using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract class StatusSelector(int step) : CardSelector
    {
        public int Step { get; set; } = step;

        public override bool SkipEmpty => true;

        protected int GetKey(int value) => value >= 0 ? value / Step : value;

        public override IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            var key = -1;
            yield return new(key, LivreNoirLibrary.YuGiOh.Vocab.Unknown);
            for (var i = 0; i <= 5000; i += Step)
            {
                yield return new(++key, $"{i}~", $"{i}\n~");
            }
        }
    }

    public sealed class AtkSelector(int step = 100) : StatusSelector(step)
    {
        public override int GetKey(Card card) => GetKey(card.AtkIndexD);
        public override IVocabData Name => Vocab.Current.CInfo.Atk;
    }

    public sealed class DefSelector(int step = 100) : StatusSelector(step)
    {
        public override int GetKey(Card card) => GetKey(card.DefIndexD);
        public override IVocabData Name => Vocab.Current.CInfo.Def;
    }
}
