using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class CardInfoVocab : VocabBase
    {
        public VocabData Name { get => GetData(); set => SetData(value); }
        public VocabData Ruby { get => GetData(); set => SetData(value); }
        public VocabData EnName { get => GetData(); set => SetData(value); }
        public VocabData CardType { get => GetData(); set => SetData(value); }
        public VocabData Text { get => GetData(); set => SetData(value); }

        public VocabData Attribute { get => GetData(); set => SetData(value); }
        public VocabData MonsterType { get => GetData(); set => SetData(value); }
        public VocabData Ability { get => GetData(); set => SetData(value); }
        public VocabData Level { get => GetData(); set => SetData(value); }
        public VocabData Rank { get => GetData(); set => SetData(value); }
        public VocabData Link { get => GetData(); set => SetData(value); }
        public VocabData Atk { get => GetData(); set => SetData(value); }
        public VocabData Def { get => GetData(); set => SetData(value); }
        public VocabData PendulumScale { get => GetData(); set => SetData(value); }
        public VocabData PendulumText { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            var dic = _dictionary;
            dic[nameof(Name)] = "カード名";
            dic[nameof(Ruby)] = "読み";
            dic[nameof(EnName)] = "TCG名";
            dic[nameof(CardType)] = "種類";
            dic[nameof(Text)] = "テキスト";
            dic[nameof(Attribute)] = LivreNoirLibrary.YuGiOh.Vocab.Attribute;
            dic[nameof(MonsterType)] = LivreNoirLibrary.YuGiOh.Vocab.MonsterType;
            dic[nameof(Ability)] = LivreNoirLibrary.YuGiOh.Vocab.Ability;
            dic[nameof(Level)] = LivreNoirLibrary.YuGiOh.Vocab.Level;
            dic[nameof(Rank)] = LivreNoirLibrary.YuGiOh.Vocab.Rank;
            dic[nameof(Link)] = LivreNoirLibrary.YuGiOh.Vocab.Link;
            dic[nameof(Atk)] = LivreNoirLibrary.YuGiOh.Vocab.Atk;
            dic[nameof(Def)] = LivreNoirLibrary.YuGiOh.Vocab.Def;
            dic[nameof(PendulumScale)] = LivreNoirLibrary.YuGiOh.Vocab.Scale_Short;
            dic[nameof(PendulumText)] = LivreNoirLibrary.YuGiOh.Vocab.PText;
        }
    }
}
