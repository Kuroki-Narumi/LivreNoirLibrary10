using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class LimitVocab : VocabBase
    {
        public VocabData Regulation { get => GetData(); set => SetData(value); }
        public VocabData Unlimited { get => GetData(); set => SetData(value); }
        public VocabData Forbidden { get => GetData(); set => SetData(value); }
        public VocabData Limit1 { get => GetData(); set => SetData(value); }
        public VocabData Limit2 { get => GetData(); set => SetData(value); }
        public VocabData Specified { get => GetData(); set => SetData(value); }
        public VocabData Unusable { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            SetData(nameof(Regulation), "レギュレーション");
            SetData(nameof(Unlimited), LivreNoirLibrary.YuGiOh.Vocab.Unlimited, "3");
            SetData(nameof(Forbidden), LivreNoirLibrary.YuGiOh.Vocab.Forbidden, "0");
            SetData(nameof(Limit1), LivreNoirLibrary.YuGiOh.Vocab.Limit1, "1");
            SetData(nameof(Limit2), LivreNoirLibrary.YuGiOh.Vocab.Limit2, "2");
            SetData(nameof(Specified), LivreNoirLibrary.YuGiOh.Vocab.Specified, "4");
            SetData(nameof(Unusable), LivreNoirLibrary.YuGiOh.Vocab.Unusable);
        }
    }
}
