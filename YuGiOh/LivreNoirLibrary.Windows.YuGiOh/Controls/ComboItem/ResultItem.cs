using LivreNoirLibrary.YuGiOh.MasterDuel;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class ResultItem : AltBackgroundComboItem<Result>
    {
        public static ResultItem[] Items { get; }
        public static ResultItem? GetItem(Result value) => _items.GetValueOrDefault(value);

        protected override int GetRow(Result value) => value switch
        {
            Result.Win => 1,
            Result.DiscLose => 2,
            Result.DiscWin => 3,
            Result.Draw => 4,
            _ => 0,
        };

        private ResultItem(Result value, IVocabData name) : base(value, name) { }
        private static readonly Dictionary<Result, ResultItem> _items;

        static ResultItem()
        {
            var v = Vocab.Current.DLog;
            Items = [new(Result.Lose, v.Lose), new(Result.Win, v.Win), new(Result.DiscLose, v.DiscLose), new(Result.DiscWin, v.DiscWin), new(Result.Draw, v.Draw)];
            _items = CreateMap(Items);
        }
    }
}
