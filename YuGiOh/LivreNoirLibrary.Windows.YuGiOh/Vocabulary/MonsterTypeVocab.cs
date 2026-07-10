using LivreNoirLibrary.YuGiOh;
using System;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class MonsterTypeVocab : VocabBase
    {
        public VocabData Spellcaster { get => GetData(); set => SetData(value); }
        public VocabData Dragon { get => GetData(); set => SetData(value); }
        public VocabData Zombie { get => GetData(); set => SetData(value); }
        public VocabData Warrior { get => GetData(); set => SetData(value); }
        public VocabData BeastWarrior { get => GetData(); set => SetData(value); }
        public VocabData Beast { get => GetData(); set => SetData(value); }
        public VocabData WingedBeast { get => GetData(); set => SetData(value); }
        public VocabData Machine { get => GetData(); set => SetData(value); }
        public VocabData Fiend { get => GetData(); set => SetData(value); }
        public VocabData Fairy { get => GetData(); set => SetData(value); }
        public VocabData Insect { get => GetData(); set => SetData(value); }
        public VocabData Dinosaur { get => GetData(); set => SetData(value); }
        public VocabData Reptile { get => GetData(); set => SetData(value); }
        public VocabData Fish { get => GetData(); set => SetData(value); }
        public VocabData SeaSerpent { get => GetData(); set => SetData(value); }
        public VocabData Aqua { get => GetData(); set => SetData(value); }
        public VocabData Pyro { get => GetData(); set => SetData(value); }
        public VocabData Thunder { get => GetData(); set => SetData(value); }
        public VocabData Rock { get => GetData(); set => SetData(value); }
        public VocabData Plant { get => GetData(); set => SetData(value); }
        public VocabData Psychic { get => GetData(); set => SetData(value); }
        public VocabData Wyrm { get => GetData(); set => SetData(value); }
        public VocabData Cyberse { get => GetData(); set => SetData(value); }
        public VocabData Illusion { get => GetData(); set => SetData(value); }
        public VocabData DivineBeast { get => GetData(); set => SetData(value); }
        public VocabData CreatorGod { get => GetData(); set => SetData(value); }

        public VocabData Spellcaster_S { get => GetData(); set => SetData(value); }
        public VocabData Dragon_S { get => GetData(); set => SetData(value); }
        public VocabData Zombie_S { get => GetData(); set => SetData(value); }
        public VocabData Warrior_S { get => GetData(); set => SetData(value); }
        public VocabData BeastWarrior_S { get => GetData(); set => SetData(value); }
        public VocabData Beast_S { get => GetData(); set => SetData(value); }
        public VocabData WingedBeast_S { get => GetData(); set => SetData(value); }
        public VocabData Machine_S { get => GetData(); set => SetData(value); }
        public VocabData Fiend_S { get => GetData(); set => SetData(value); }
        public VocabData Fairy_S { get => GetData(); set => SetData(value); }
        public VocabData Insect_S { get => GetData(); set => SetData(value); }
        public VocabData Dinosaur_S { get => GetData(); set => SetData(value); }
        public VocabData Reptile_S { get => GetData(); set => SetData(value); }
        public VocabData Fish_S { get => GetData(); set => SetData(value); }
        public VocabData SeaSerpent_S { get => GetData(); set => SetData(value); }
        public VocabData Aqua_S { get => GetData(); set => SetData(value); }
        public VocabData Pyro_S { get => GetData(); set => SetData(value); }
        public VocabData Thunder_S { get => GetData(); set => SetData(value); }
        public VocabData Rock_S { get => GetData(); set => SetData(value); }
        public VocabData Plant_S { get => GetData(); set => SetData(value); }
        public VocabData Psychic_S { get => GetData(); set => SetData(value); }
        public VocabData Wyrm_S { get => GetData(); set => SetData(value); }
        public VocabData Cyberse_S { get => GetData(); set => SetData(value); }
        public VocabData Illusion_S { get => GetData(); set => SetData(value); }
        public VocabData DivineBeast_S { get => GetData(); set => SetData(value); }
        public VocabData CreatorGod_S { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            foreach (var type in EnumUtils.EnumerateMonsterTypes(true))
            {
                SetData(type.ToString(), type.GetName());
                SetData($"{type}_S", type.GetShortName());
            }
        }
    }
}
