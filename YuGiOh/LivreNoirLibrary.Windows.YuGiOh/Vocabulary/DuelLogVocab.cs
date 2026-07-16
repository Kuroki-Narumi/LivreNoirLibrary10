using V = LivreNoirLibrary.YuGiOh.Vocab;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class DuelLogVocab : VocabBase
    {
        public VocabData DateTime { get => GetData(); set => SetData(value); }

        public VocabData Order { get => GetData(); set => SetData(value); }
        public VocabData First { get => GetData(); set => SetData(value); }
        public VocabData CFirst { get => GetData(); set => SetData(value); }
        public VocabData Second { get => GetData(); set => SetData(value); }
        public VocabData CSecond { get => GetData(); set => SetData(value); }
        public VocabData First_S { get => GetData(); set => SetData(value); }
        public VocabData CFirst_S { get => GetData(); set => SetData(value); }
        public VocabData Second_S { get => GetData(); set => SetData(value); }
        public VocabData CSecond_S { get => GetData(); set => SetData(value); }

        public VocabData Rank { get => GetData(); set => SetData(value); }

        public VocabData Result { get => GetData(); set => SetData(value); }
        public VocabData Win { get => GetData(); set => SetData(value); }
        public VocabData Lose { get => GetData(); set => SetData(value); }
        public VocabData DiscWin { get => GetData(); set => SetData(value); }
        public VocabData DiscLose { get => GetData(); set => SetData(value); }
        public VocabData Draw { get => GetData(); set => SetData(value); }

        public VocabData Turn { get => GetData(); set => SetData(value); }
        public VocabData Turn_S { get => GetData(); set => SetData(value); }

        public VocabData User { get => GetData(); set => SetData(value); }
        public VocabData Opponent { get => GetData(); set => SetData(value); }
        public VocabData Note { get => GetData(); set => SetData(value); }
        public VocabData InitialHand { get => GetData(); set => SetData(value); }
        public VocabData AdditionalHand { get => GetData(); set => SetData(value); }
        public VocabData Deck { get => GetData(); set => SetData(value); }
        public VocabData AllCards { get => GetData(); set => SetData(value); }

        public VocabData Name { get => GetData(); set => SetData(value); }
        public VocabData SearchHint { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            SetData(nameof(DateTime), "日時");

            SetData(nameof(Order), V.Order);
            SetData(nameof(First), V.First_Full);
            SetData(nameof(CFirst), V.CFirst_Full);
            SetData(nameof(Second), V.Second_Full);
            SetData(nameof(CSecond), V.CSecond_Full);
            SetData(nameof(First), V.WinFirst);
            SetData(nameof(CFirst), V.LoseFirst);
            SetData(nameof(Second), V.LoseSecond);
            SetData(nameof(CSecond), V.WinSecond);

            SetData(nameof(Rank), V.Rank);

            SetData(nameof(Result), V.Result);
            SetData(nameof(Win), V.Win);
            SetData(nameof(Lose), V.Lose);
            SetData(nameof(DiscWin), V.DiscWin);
            SetData(nameof(DiscLose), V.DiscLose);

            SetData(nameof(Turn), "ターン");
            SetData(nameof(Turn_S), "T");
            SetData(nameof(User), "デッキ");
            SetData(nameof(Opponent), "相手");
            SetData(nameof(Note), "備考");
            SetData(nameof(InitialHand), "初期手札");
            SetData(nameof(AdditionalHand), "追加手札");
            SetData(nameof(Deck), "デッキ");
            SetData(nameof(AllCards), "全カード");

            SetData(nameof(Name), "名前");
            SetData(nameof(SearchHint), "検索用文字列");
        }
    }
}
