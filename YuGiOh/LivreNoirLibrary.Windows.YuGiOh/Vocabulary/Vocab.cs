using LivreNoirLibrary.Windows.YuGiOh.Vocabulary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public partial class Vocab : VocabBase, IVocabulary<Vocab>
    {
        public static ICollection<LanguageData> Languages { get; } = new ObservableCollection<LanguageData>();
        public static Vocab Current { get; } = new();
        public static Vocab Default { get; } = CreateDefault();

        static Vocab()
        {
            SetupInstance<Vocab>();
        }

        private static Vocab CreateDefault()
        {
            Vocab vocab = new();
            vocab.LoadDefault();
            return vocab;
        }

        public CardInfoVocab CInfo { get; } = new();
        public CardTypeVocab CType { get; } = new();
        public AttributeVocab Attr { get; } = new();
        public MonsterTypeVocab MType { get; } = new();
        public AbilityVocab Abi { get; } = new();
        public LimitVocab Limit { get; } = new();

        public VocabData Close { get => GetData(); set => SetData(value); }
        public VocabData Update { get => GetData(); set => SetData(value); }
        public VocabData Save { get => GetData(); set => SetData(value); }
        public VocabData Open { get => GetData(); set => SetData(value); }
        public VocabData OfficialDatabase { get => GetData(); set => SetData(value); }
        public VocabData TcgDatabase { get => GetData(); set => SetData(value); }
        public VocabData Detach { get => GetData(); set => SetData(value); }

        public VocabData Apply { get => GetData(); set => SetData(value); }
        public VocabData Clear { get => GetData(); set => SetData(value); }
        public VocabData Sort { get => GetData(); set => SetData(value); }
        public VocabData Search { get => GetData(); set => SetData(value); }
        public VocabData Preset { get => GetData(); set => SetData(value); }
        public VocabData SetToDefault { get => GetData(); set => SetData(value); }

        public VocabData Add { get => GetData(); set => SetData(value); }
        public VocabData Overwrite { get => GetData(); set => SetData(value); }
        public VocabData MoveUp { get => GetData(); set => SetData(value); }
        public VocabData MoveDown { get => GetData(); set => SetData(value); }
        public VocabData Delete { get => GetData(); set => SetData(value); }

        public VocabData Search_CardType { get => GetData(); set => SetData(value); }
        public VocabData Search_Status { get => GetData(); set => SetData(value); }
        public VocabData Search_Other { get => GetData(); set => SetData(value); }
        public VocabData Search_Exist { get => GetData(); set => SetData(value); }
        public VocabData Search_NotExist { get => GetData(); set => SetData(value); }
        public VocabData Search_Or { get => GetData(); set => SetData(value); }
        public VocabData Search_And { get => GetData(); set => SetData(value); }
        public VocabData Search_Except { get => GetData(); set => SetData(value); }
        public VocabData Search_Expression { get => GetData(); set => SetData(value); }
        public VocabData Search_TextLength { get => GetData(); set => SetData(value); }
        public VocabData Search_PTextLength { get => GetData(); set => SetData(value); }
        public VocabData Search_Text { get => GetData(); set => SetData(value); }
        public VocabData Search_IgnoreCase { get => GetData(); set => SetData(value); }
        public VocabData Search_IgnoreSymbols { get => GetData(); set => SetData(value); }
        public VocabData Search_UseRegex { get => GetData(); set => SetData(value); }

        public VocabData Locale { get => GetData(); set => SetData(value); }
        public VocabData Ocg { get => GetData(); set => SetData(value); }
        public VocabData Tcg { get => GetData(); set => SetData(value); }
        public VocabData Locale_Any { get => GetData(); set => SetData(value); }
        public VocabData Locale_OcgExists { get => GetData(); set => SetData(value); }
        public VocabData Locale_OnlyOcg { get => GetData(); set => SetData(value); }
        public VocabData Locale_TcgExists { get => GetData(); set => SetData(value); }
        public VocabData Locale_OnlyTcg { get => GetData(); set => SetData(value); }
        public VocabData Locale_Both { get => GetData(); set => SetData(value); }
        public VocabData PublishDate { get => GetData(); set => SetData(value); }
        public VocabData Publish_First { get => GetData(); set => SetData(value); }
        public VocabData Publish_Latest { get => GetData(); set => SetData(value); }

        public VocabData None { get => GetData(); set => SetData(value); }
        public VocabData CharCount { get => GetData(); set => SetData(value); }
        public VocabData Ascending { get => GetData(); set => SetData(value); }
        public VocabData Descending { get => GetData(); set => SetData(value); }
        public VocabData Details { get => GetData(); set => SetData(value); }

        public VocabData Tab_Database { get => GetData(); set => SetData(value); }
        public VocabData Tab_Statistics { get => GetData(); set => SetData(value); }
        public VocabData Tab_Deck { get => GetData(); set => SetData(value); }
        public VocabData Tab_DuelLog { get => GetData(); set => SetData(value); }

        public VocabData Tab_CardList { get => GetData(); set => SetData(value); }
        public VocabData Tab_Pack { get => GetData(); set => SetData(value); }
        public VocabData Tab_Regulation { get => GetData(); set => SetData(value); }
        public VocabData Tab_OriginalCards { get => GetData(); set => SetData(value); }

        public VocabData Tab_TableView { get => GetData(); set => SetData(value); }
        public VocabData Tab_Token { get => GetData(); set => SetData(value); }
        public VocabData Tab_TrapMonster { get => GetData(); set => SetData(value); }
        public VocabData Tab_StatusMatch { get => GetData(); set => SetData(value); }
        public VocabData Tab_Hedgehog { get => GetData(); set => SetData(value); }
        public VocabData Tab_Numbers { get => GetData(); set => SetData(value); }
        public VocabData Tab_DeckEdit { get => GetData(); set => SetData(value); }
        public VocabData Tab_CardTable { get => GetData(); set => SetData(value); }
        public VocabData Tab_SwSearch { get => GetData(); set => SetData(value); }
        public VocabData Tab_SwInDeck { get => GetData(); set => SetData(value); }
        public VocabData Tab_TestConds { get => GetData(); set => SetData(value); }
        public VocabData Tab_TestExec { get => GetData(); set => SetData(value); }
        public VocabData Tab_LogManage { get => GetData(); set => SetData(value); }
        public VocabData Tab_TagManage { get => GetData(); set => SetData(value); }

        public VocabData Message_UpdateAvailable { get => GetData(); set => SetData(value); }
        public VocabData Message_UpdateNone { get => GetData(); set => SetData(value); }
        public VocabData Message_UpdateComplete { get => GetData(); set => SetData(value); }
        public VocabData OnlineManual { get => GetData(); set => SetData(value); }
        public VocabData CheckUpdate { get => GetData(); set => SetData(value); }

        [JsonIgnore]
        public SortKeyVocab SortKey { get; }

        public Vocab()
        {
            SortKey = new(this);
        }

        public void LoadDefault()
        {
            CInfo.LoadDefault();
            CType.LoadDefault();
            Attr.LoadDefault();
            MType.LoadDefault();
            Abi.LoadDefault();
            Limit.LoadDefault();

            SetData(nameof(Close), "閉じる", "X");
            SetData(nameof(Update), "更新", "U");
            SetData(nameof(Save), "保存", "S");
            SetData(nameof(Open), "開く", "O");
            SetData(nameof(OfficialDatabase), "公式DB");
            SetData(nameof(TcgDatabase), "TCG DB");
            SetData(nameof(Detach), "別窓");
            SetData(nameof(Apply), "適用");
            SetData(nameof(Clear), "クリア");
            SetData(nameof(Sort), "並べ替え");
            SetData(nameof(Search), "絞り込み");
            SetData(nameof(Preset), "プリセ");
            SetData(nameof(SetToDefault), "デフォルトにする");

            SetData(nameof(Add), "追加", "A");
            SetData(nameof(Overwrite), "上書き", "W");
            SetData(nameof(MoveUp), "上に移動", "U");
            SetData(nameof(MoveDown), "下に移動", "J");
            SetData(nameof(Delete), "削除", "D");

            SetData(nameof(Search_CardType), "カードタイプ");
            SetData(nameof(Search_Status), "ステータス");
            SetData(nameof(Search_Other), "その他");
            SetData(nameof(Search_Exist), "あり");
            SetData(nameof(Search_NotExist), "なし");
            SetData(nameof(Search_Or), "OR");
            SetData(nameof(Search_And), "AND");
            SetData(nameof(Search_Except), "除外");
            SetData(nameof(Search_Expression), "条件式");
            SetData(nameof(Search_TextLength), "テキスト文字数");
            SetData(nameof(Search_PTextLength), "P効果文字数");
            SetData(nameof(Search_Text), "文字列検索");
            SetData(nameof(Search_IgnoreCase), "caseを無視");
            SetData(nameof(Search_IgnoreSymbols), "記号を無視");
            SetData(nameof(Search_UseRegex), "正規表現");

            SetData(nameof(Locale), "ロケール");
            SetData(nameof(Ocg), "OCG");
            SetData(nameof(Tcg), "TCG");
            SetData(nameof(Locale_Any), "全て");
            SetData(nameof(Locale_OcgExists), "OCGあり");
            SetData(nameof(Locale_OnlyOcg), "OCGのみ");
            SetData(nameof(Locale_TcgExists), "TCGあり");
            SetData(nameof(Locale_OnlyTcg), "TCGのみ");
            SetData(nameof(Locale_Both), "共通");
            SetData(nameof(PublishDate), "発売日");
            SetData(nameof(Publish_First), "初登場");
            SetData(nameof(Publish_Latest), "最新収録");

            SetData(nameof(None), "なし");
            SetData(nameof(CharCount), "文字数");
            SetData(nameof(Ascending), "昇順");
            SetData(nameof(Descending), "降順");
            SetData(nameof(Details), "詳細");

            SetData(nameof(Tab_Database), "データベース");
            SetData(nameof(Tab_Statistics), "統計");
            SetData(nameof(Tab_Deck), "デッキ");
            SetData(nameof(Tab_DuelLog), "デュエルログ");
            SetData(nameof(Tab_CardList), "カードリスト");
            SetData(nameof(Tab_Pack), "パックリスト");
            SetData(nameof(Tab_Regulation), "レギュレーション");
            SetData(nameof(Tab_OriginalCards), "オリカ");
            SetData(nameof(Tab_TableView), "統計表");
            SetData(nameof(Tab_Token), "トークン");
            SetData(nameof(Tab_TrapMonster), "罠モンスター");
            SetData(nameof(Tab_StatusMatch), "ステータスマッチング");
            SetData(nameof(Tab_Hedgehog), "ヘッジホッグ");
            SetData(nameof(Tab_Numbers), "エヴァイユ");
            SetData(nameof(Tab_DeckEdit), "デッキ編集");
            SetData(nameof(Tab_CardTable), "カード表");
            SetData(nameof(Tab_SwSearch), "スモワ:検索");
            SetData(nameof(Tab_SwInDeck), "スモワ:デッキ内");
            SetData(nameof(Tab_TestConds), "手札検証:設定");
            SetData(nameof(Tab_TestExec), "手札検証:実行");
            SetData(nameof(Tab_LogManage), "ログ管理");
            SetData(nameof(Tab_TagManage), "タグ管理");

            SetData(nameof(Message_UpdateAvailable), "新しいバージョン({0})が見つかりました。\n更新しますか？");
            SetData(nameof(Message_UpdateNone), "更新はありません。");
            SetData(nameof(Message_UpdateComplete), "アップデート完了しました。");
            SetData(nameof(OnlineManual), "オンラインマニュアル");
            SetData(nameof(CheckUpdate), "更新の確認");
        }

        public override void UpdateVocabData<T>(T? source) where T : class
        {
            base.UpdateVocabData(source);
            if (source is Vocab v)
            {
                CInfo.UpdateVocabData(v.CInfo);
                CType.UpdateVocabData(v.CType);
                Attr.UpdateVocabData(v.Attr);
                MType.UpdateVocabData(v.MType);
                Abi.UpdateVocabData(v.Abi);
                Limit.UpdateVocabData(v.Limit);
            }
        }
    }
}
