using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class AbilityVocab : VocabBase
    {
        public VocabData Toon { get => GetData(); set => SetData(value); }
        public VocabData Gemini { get => GetData(); set => SetData(value); }
        public VocabData Union { get => GetData(); set => SetData(value); }
        public VocabData Spirit { get => GetData(); set => SetData(value); }
        public VocabData Tuner { get => GetData(); set => SetData(value); }
        public VocabData Flip { get => GetData(); set => SetData(value); }
        public VocabData Pendulum { get => GetData(); set => SetData(value); }
        public VocabData SpecialSummon { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            var dic = _dictionary;
            foreach (var abi in EnumUtils.EnumerateAbilities(false))
            {
                dic[$"{abi}"] = abi.GetSingleName();
            }
        }
    }
}
