using LivreNoirLibrary.YuGiOh;
using System.Text.Json.Serialization;

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

        public VocabData Delimiter { get => GetData(); set => SetData(value); }

        [JsonIgnore]
        public MergedVocabData Normal_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Effect_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Main_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Fusion_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Ritual_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Synchro_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Xyz_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Link_Monster { get; }
        [JsonIgnore]
        public MergedVocabData Normal_Spell { get; }
        [JsonIgnore]
        public MergedVocabData Field_Spell { get; }
        [JsonIgnore]
        public MergedVocabData Equip_Spell { get; }
        [JsonIgnore]
        public MergedVocabData Continuous_Spell { get; }
        [JsonIgnore]
        public MergedVocabData Quick_Spell { get; }
        [JsonIgnore]
        public MergedVocabData Ritual_Spell { get; }
        [JsonIgnore]
        public MergedVocabData Normal_Trap { get; }
        [JsonIgnore]
        public MergedVocabData Continuous_Trap { get; }
        [JsonIgnore]
        public MergedVocabData Counter_Trap { get; }

        public CardTypeVocab()
        {
            Normal_Monster = new([Normal, Monster], Delimiter);
            Effect_Monster = new([Effect, Monster], Delimiter);
            Main_Monster = new([Main, Monster], Delimiter);
            Fusion_Monster = new([Fusion, Monster], Delimiter);
            Ritual_Monster = new([Ritual, Monster], Delimiter);
            Synchro_Monster = new([Synchro, Monster], Delimiter);
            Xyz_Monster = new([Xyz, Monster], Delimiter);
            Link_Monster = new([Link, Monster], Delimiter);
            Normal_Spell = new([Normal, Spell], Delimiter);
            Field_Spell = new([Field, Spell], Delimiter);
            Equip_Spell = new([Equip, Spell], Delimiter);
            Continuous_Spell = new([Continuous, Spell], Delimiter);
            Quick_Spell = new([Quick, Spell], Delimiter);
            Ritual_Spell = new([Ritual, Spell], Delimiter);
            Normal_Trap = new([Normal, Trap], Delimiter);
            Continuous_Trap = new([Continuous, Trap], Delimiter);
            Counter_Trap = new([Counter, Trap], Delimiter);
        }

        public void LoadDefault()
        {
            foreach (var type in EnumUtils.EnumerateCardTypes(true, true, true))
            {
                SetData(type.ToString(), type.GetName(true));
            }
            
            SetData(nameof(Monster), LivreNoirLibrary.YuGiOh.Vocab.Monster);
            SetData(nameof(Spell), LivreNoirLibrary.YuGiOh.Vocab.Spell);
            SetData(nameof(Trap), LivreNoirLibrary.YuGiOh.Vocab.Trap);

            SetData(nameof(Main), LivreNoirLibrary.YuGiOh.Vocab.Main);
            SetData(nameof(Fusion), LivreNoirLibrary.YuGiOh.Vocab.Fusion);
            SetData(nameof(Ritual), LivreNoirLibrary.YuGiOh.Vocab.Ritual);
            SetData(nameof(Synchro), LivreNoirLibrary.YuGiOh.Vocab.Synchro);
            SetData(nameof(Xyz), LivreNoirLibrary.YuGiOh.Vocab.Xyz);
            SetData(nameof(Link), LivreNoirLibrary.YuGiOh.Vocab.Link);
            SetData(nameof(Token), LivreNoirLibrary.YuGiOh.Vocab.Token);
            SetData(nameof(Normal), LivreNoirLibrary.YuGiOh.Vocab.Normal);
            SetData(nameof(Effect), LivreNoirLibrary.YuGiOh.Vocab.Effect);
            SetData(nameof(Continuous), LivreNoirLibrary.YuGiOh.Vocab.Continuous);
            SetData(nameof(Field), LivreNoirLibrary.YuGiOh.Vocab.Field);
            SetData(nameof(Equip), LivreNoirLibrary.YuGiOh.Vocab.Equip);
            SetData(nameof(Quick), LivreNoirLibrary.YuGiOh.Vocab.Quick);
            SetData(nameof(Counter), LivreNoirLibrary.YuGiOh.Vocab.Counter);

            SetData(nameof(Delimiter), "");
        }
    }
}
