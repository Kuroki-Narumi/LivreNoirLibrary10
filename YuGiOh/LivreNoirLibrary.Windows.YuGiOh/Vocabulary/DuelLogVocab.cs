using V = LivreNoirLibrary.YuGiOh.Vocab;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class DuelLogVocab : VocabBase
    {
        public VocabData DateTime { get => GetData(); set => SetData(value); }

        public VocabData Order { get => GetData(); set => SetData(value); }
        public VocabData First { get => GetData(); set => SetData(value); }
        public VocabData Second { get => GetData(); set => SetData(value); }
        public VocabData CoinWin { get => GetData(); set => SetData(value); }
        public VocabData CoinLose { get => GetData(); set => SetData(value); }
        public VocabData WinFirst { get => GetData(); set => SetData(value); }
        public VocabData LoseFirst { get => GetData(); set => SetData(value); }
        public VocabData LoseSecond { get => GetData(); set => SetData(value); }
        public VocabData WinSecond { get => GetData(); set => SetData(value); }
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
        public VocabData Filter { get => GetData(); set => SetData(value); }
        public VocabData FilterClear { get => GetData(); set => SetData(value); }

        public VocabData Confirm_TagReplace { get => GetData(); set => SetData(value); }
        public VocabData Confirm_TagUpdate { get => GetData(); set => SetData(value); }

        public VocabData RefreshStatistics { get => GetData(); set => SetData(value); }
        public VocabData Tab_Tag { get => GetData(); set => SetData(value); }
        public VocabData Tab_TagSingle { get => GetData(); set => SetData(value); }
        public VocabData Tab_InitialHand { get => GetData(); set => SetData(value); }
        public VocabData Tab_TotalHand { get => GetData(); set => SetData(value); }

        public VocabData Header_Percent { get => GetData(); set => SetData(value); }
        public VocabData Header_Tag { get => GetData(); set => SetData(value); }
        public VocabData Header_Card { get => GetData(); set => SetData(value); }
        public VocabData Header_Total { get => GetData(); set => SetData(value); }
        public VocabData Header_Win { get => GetData(); set => SetData(value); }
        public VocabData Header_Lose { get => GetData(); set => SetData(value); }
        public VocabData Header_Draw { get => GetData(); set => SetData(value); }
        public VocabData Header_DiscWin { get => GetData(); set => SetData(value); }
        public VocabData Header_DiscLose { get => GetData(); set => SetData(value); }
        public VocabData Header_WinLike { get => GetData(); set => SetData(value); }
        public VocabData Header_First { get => GetData(); set => SetData(value); }
        public VocabData Header_Second { get => GetData(); set => SetData(value); }
        public VocabData Header_CFirst { get => GetData(); set => SetData(value); }
        public VocabData Header_CSecond { get => GetData(); set => SetData(value); }
        public VocabData Header_FirstWin { get => GetData(); set => SetData(value); }
        public VocabData Header_SecondWin { get => GetData(); set => SetData(value); }
        public VocabData Header_CFirstWin { get => GetData(); set => SetData(value); }
        public VocabData Header_CSecondWin { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            SetData(nameof(DateTime), "日時");

            SetData(nameof(Order), V.Order);
            SetData(nameof(First), V.First);
            SetData(nameof(Second), V.Second);
            SetData(nameof(CoinWin), V.CoinWin);
            SetData(nameof(CoinLose), V.CoinLose);
            SetData(nameof(WinFirst), V.First_Full);
            SetData(nameof(LoseFirst), V.CFirst_Full);
            SetData(nameof(LoseSecond), V.Second_Full);
            SetData(nameof(WinSecond), V.CSecond_Full);
            SetData(nameof(First_S), V.WinFirst);
            SetData(nameof(CFirst_S), V.LoseFirst);
            SetData(nameof(Second_S), V.LoseSecond);
            SetData(nameof(CSecond_S), V.WinSecond);

            SetData(nameof(Rank), V.Rank);

            SetData(nameof(Result), V.Result);
            SetData(nameof(Win), V.Win);
            SetData(nameof(Lose), V.Lose);
            SetData(nameof(DiscWin), V.DiscWin);
            SetData(nameof(DiscLose), V.DiscLose);
            SetData(nameof(Draw), V.Draw);

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
            SetData(nameof(Filter), "チェック付き項目のみ表示");
            SetData(nameof(FilterClear), "絞り込みを解除");

            SetData(nameof(Confirm_TagReplace), "変更しようとしている名前({0})のタグは既に存在します。\n置き換えますか？");
            SetData(nameof(Confirm_TagUpdate), "既存ログデータのタグ情報も更新しますか？\n注:変更されたタグ情報は「元に戻す」で戻せません。");

            SetData(nameof(RefreshStatistics), "統計データを生成する");
            SetData(nameof(Tab_Tag), "タグ(完全一致)");
            SetData(nameof(Tab_TagSingle), "タグ(個別)");
            SetData(nameof(Tab_InitialHand), "カード(初手札)");
            SetData(nameof(Tab_TotalHand), "カード(全手札)");

            SetData(nameof(Header_Percent), "%");
            SetData(nameof(Header_Tag), "タグ");
            SetData(nameof(Header_Card), "カード");
            SetData(nameof(Header_Total), "総数");
            SetData(nameof(Header_Win), "勝");
            SetData(nameof(Header_Lose), "負");
            SetData(nameof(Header_Draw), "分");
            SetData(nameof(Header_DiscWin), "切勝");
            SetData(nameof(Header_DiscLose), "切負");
            SetData(nameof(Header_WinLike), "勝+切");
            SetData(nameof(Header_First), "先");
            SetData(nameof(Header_Second), "後");
            SetData(nameof(Header_CFirst), "C先");
            SetData(nameof(Header_CSecond), "C後");
            SetData(nameof(Header_FirstWin), "先勝");
            SetData(nameof(Header_SecondWin), "後勝");
            SetData(nameof(Header_CFirstWin), "C先勝");
            SetData(nameof(Header_CSecondWin), "C後勝");
        }
    }
}
