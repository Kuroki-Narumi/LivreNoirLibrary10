using LivreNoirLibrary.YuGiOh;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public sealed class MonsterTypeItem : AltBackgroundComboItem<MonsterType>
    {
        public static MonsterTypeItem[] Items { get; }
        public static MonsterTypeItem? GetItem(MonsterType value) => _items.GetValueOrDefault(value);

        private MonsterTypeItem(MonsterType value, IVocabData? name) : base(value, name) { }
        protected override int GetRow(MonsterType value) => (int)(value - 1) % 9;
        protected override int GetColumn(MonsterType value) => (int)(value - 1) / 9;

        private static MonsterTypeItem Create(MonsterType value) => new(value, SelectVocabData(value));
        private static readonly Dictionary<MonsterType, MonsterTypeItem> _items;

        static MonsterTypeItem()
        {
            Items = [.. EnumUtils.MonsterTypes.Select(Create)];
            _items = CreateMap(Items);
        }

        private static VocabData? SelectVocabData(MonsterType type) => type switch
        {
            MonsterType.Spellcaster => Vocab.Current.MType.Spellcaster,
            MonsterType.Dragon => Vocab.Current.MType.Dragon,
            MonsterType.Zombie => Vocab.Current.MType.Zombie,
            MonsterType.Warrior => Vocab.Current.MType.Warrior,
            MonsterType.BeastWarrior => Vocab.Current.MType.BeastWarrior,
            MonsterType.Beast => Vocab.Current.MType.Beast,
            MonsterType.WingedBeast => Vocab.Current.MType.WingedBeast,
            MonsterType.Machine => Vocab.Current.MType.Machine,
            MonsterType.Fiend => Vocab.Current.MType.Fiend,
            MonsterType.Fairy => Vocab.Current.MType.Fairy,
            MonsterType.Insect => Vocab.Current.MType.Insect,
            MonsterType.Dinosaur => Vocab.Current.MType.Dinosaur,
            MonsterType.Reptile => Vocab.Current.MType.Reptile,
            MonsterType.Fish => Vocab.Current.MType.Fish,
            MonsterType.SeaSerpent => Vocab.Current.MType.SeaSerpent,
            MonsterType.Aqua => Vocab.Current.MType.Aqua,
            MonsterType.Pyro => Vocab.Current.MType.Pyro,
            MonsterType.Thunder => Vocab.Current.MType.Thunder,
            MonsterType.Rock => Vocab.Current.MType.Rock,
            MonsterType.Plant => Vocab.Current.MType.Plant,
            MonsterType.Psychic => Vocab.Current.MType.Psychic,
            MonsterType.Wyrm => Vocab.Current.MType.Wyrm,
            MonsterType.Cyberse => Vocab.Current.MType.Cyberse,
            MonsterType.Illusion => Vocab.Current.MType.Illusion,
            MonsterType.DivineBeast => Vocab.Current.MType.DivineBeast,
            MonsterType.CreatorGod => Vocab.Current.MType.CreatorGod,
            _ => null
        };
    }
}
