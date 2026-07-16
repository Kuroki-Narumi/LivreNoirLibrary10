using LivreNoirLibrary.YuGiOh;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class CardInfoVocab : VocabBase
    {
        public VocabData Id { get => GetData(); set => SetData(value); }
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
        public VocabData Atk_S { get => GetData(); set => SetData(value); }
        public VocabData Def_S { get => GetData(); set => SetData(value); }
        public VocabData LinkMarker { get => GetData(); set => SetData(value); }
        public VocabData PendulumScale { get => GetData(); set => SetData(value); }
        public VocabData PendulumText { get => GetData(); set => SetData(value); }

        public VocabData PackInfo { get => GetData(); set => SetData(value); }
        public VocabData RelatedList { get => GetData(); set => SetData(value); }

        public VocabData LevelSymbol { get => GetData(); set => SetData(value); }

        [JsonIgnore]
        public MergedVocabData LevelRankLink { get; }

        public CardInfoVocab()
        {
            LevelRankLink = new([Level, Rank, Link], new VocabData() { Value = "/" });
        }

        public void LoadDefault()
        {
            SetData(nameof(Id), "ID");
            SetData(nameof(Name), "カード名");
            SetData(nameof(Ruby), "読み");
            SetData(nameof(EnName), "TCG名");
            SetData(nameof(CardType), "種類");
            SetData(nameof(Text), "テキスト");
            SetData(nameof(Attribute), LivreNoirLibrary.YuGiOh.Vocab.Attribute);
            SetData(nameof(MonsterType), LivreNoirLibrary.YuGiOh.Vocab.MonsterType);
            SetData(nameof(Ability), LivreNoirLibrary.YuGiOh.Vocab.Ability);
            SetData(nameof(Level), LivreNoirLibrary.YuGiOh.Vocab.Level);
            SetData(nameof(Rank), LivreNoirLibrary.YuGiOh.Vocab.Rank);
            SetData(nameof(Link), LivreNoirLibrary.YuGiOh.Vocab.Link);
            SetData(nameof(Atk), LivreNoirLibrary.YuGiOh.Vocab.Atk);
            SetData(nameof(Def), LivreNoirLibrary.YuGiOh.Vocab.Def);
            SetData(nameof(Atk_S), "ATK");
            SetData(nameof(Def_S), "DEF");
            SetData(nameof(LinkMarker), LivreNoirLibrary.YuGiOh.Vocab.Marker);
            SetData(nameof(PendulumScale), LivreNoirLibrary.YuGiOh.Vocab.Scale_Short);
            SetData(nameof(PendulumText), LivreNoirLibrary.YuGiOh.Vocab.PText);

            SetData(nameof(PackInfo), "収録シリーズ");
            SetData(nameof(RelatedList), "関連ワード");

            SetData(nameof(LevelSymbol), "★");
        }
    }
}
