using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class CardTypeVocab : VocabBase
    {
        public VocabData Monster { get => GetData(); set => SetData(value); }
        public VocabData Spell { get => GetData(); set => SetData(value); }
        public VocabData Trap { get => GetData(); set => SetData(value); }

        public VocabData Main { get => GetData(); set => SetData(value); }
        public VocabData Fusion { get => GetData(); set => SetData(value); }
        public VocabData Ritual { get => GetData(); set => SetData(value); }
        public VocabData Synchro { get => GetData(); set => SetData(value); }
        public VocabData Xyz { get => GetData(); set => SetData(value); }
        public VocabData Link { get => GetData(); set => SetData(value); }
        public VocabData Token { get => GetData(); set => SetData(value); }

        public VocabData Normal { get => GetData(); set => SetData(value); }
        public VocabData Effect { get => GetData(); set => SetData(value); }
        public VocabData Continuous { get => GetData(); set => SetData(value); }
        public VocabData Field { get => GetData(); set => SetData(value); }
        public VocabData Equip { get => GetData(); set => SetData(value); }
        public VocabData Quick { get => GetData(); set => SetData(value); }
        public VocabData Counter { get => GetData(); set => SetData(value); }

        public VocabData Main_Monster { get => GetData(); set => SetData(value); }
        public VocabData Fusion_Monster { get => GetData(); set => SetData(value); }
        public VocabData Ritual_Monster { get => GetData(); set => SetData(value); }
        public VocabData Synchro_Monster { get => GetData(); set => SetData(value); }
        public VocabData Xyz_Monster { get => GetData(); set => SetData(value); }
        public VocabData Link_Monster { get => GetData(); set => SetData(value); }
        public VocabData Normal_Spell { get => GetData(); set => SetData(value); }
        public VocabData Field_Spell { get => GetData(); set => SetData(value); }
        public VocabData Equip_Spell { get => GetData(); set => SetData(value); }
        public VocabData Continuous_Spell { get => GetData(); set => SetData(value); }
        public VocabData Quick_Spell { get => GetData(); set => SetData(value); }
        public VocabData Ritual_Spell { get => GetData(); set => SetData(value); }
        public VocabData Normal_Trap { get => GetData(); set => SetData(value); }
        public VocabData Continuous_Trap { get => GetData(); set => SetData(value); }
        public VocabData Counter_Trap { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            var dic = _dictionary;
            foreach (var type in EnumUtils.EnumerateCardTypes(true, true, true))
            {
                dic[$"{type}"] = type.GetName();
            }
            
            dic[nameof(Monster)] = LivreNoirLibrary.YuGiOh.Vocab.Monster;
            dic[nameof(Spell)] = LivreNoirLibrary.YuGiOh.Vocab.Spell;
            dic[nameof(Trap)] = LivreNoirLibrary.YuGiOh.Vocab.Trap;

            dic[nameof(Main)] = LivreNoirLibrary.YuGiOh.Vocab.Main;
            dic[nameof(Fusion)] = LivreNoirLibrary.YuGiOh.Vocab.Fusion;
            dic[nameof(Ritual)] = LivreNoirLibrary.YuGiOh.Vocab.Ritual;
            dic[nameof(Synchro)] = LivreNoirLibrary.YuGiOh.Vocab.Synchro;
            dic[nameof(Xyz)] = LivreNoirLibrary.YuGiOh.Vocab.Xyz;
            dic[nameof(Link)] = LivreNoirLibrary.YuGiOh.Vocab.Link;
            dic[nameof(Token)] = LivreNoirLibrary.YuGiOh.Vocab.Token;
            dic[nameof(Normal)] = LivreNoirLibrary.YuGiOh.Vocab.Normal;
            dic[nameof(Effect)] = LivreNoirLibrary.YuGiOh.Vocab.Effect;
            dic[nameof(Continuous)] = LivreNoirLibrary.YuGiOh.Vocab.Continuous;
            dic[nameof(Field)] = LivreNoirLibrary.YuGiOh.Vocab.Field;
            dic[nameof(Equip)] = LivreNoirLibrary.YuGiOh.Vocab.Equip;
            dic[nameof(Quick)] = LivreNoirLibrary.YuGiOh.Vocab.Quick;
            dic[nameof(Counter)] = LivreNoirLibrary.YuGiOh.Vocab.Counter;
        }
    }
}
