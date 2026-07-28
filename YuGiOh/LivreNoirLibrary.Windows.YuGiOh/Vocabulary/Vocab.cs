using LivreNoirLibrary.Windows.YuGiOh.Vocabulary;
using Microsoft.VisualBasic.Logging;
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
        public PackInfoVocab PInfo { get; } = new();
        public DuelLogVocab DLog { get; } = new();

        public VocabData Close { get => GetData(); set => SetData(value); }
        public VocabData ShowInTaskbar { get => GetData(); set => SetData(value); }
        public VocabData Update { get => GetData(); set => SetData(value); }
        public VocabData Save { get => GetData(); set => SetData(value); }
        public VocabData Open { get => GetData(); set => SetData(value); }
        public VocabData Undo { get => GetData(); set => SetData(value); }
        public VocabData Redo { get => GetData(); set => SetData(value); }
        public VocabData Copy { get => GetData(); set => SetData(value); }
        public VocabData Cut { get => GetData(); set => SetData(value); }
        public VocabData Paste { get => GetData(); set => SetData(value); }
        public VocabData Merge { get => GetData(); set => SetData(value); }
        public VocabData Reload { get => GetData(); set => SetData(value); }
        public VocabData Import { get => GetData(); set => SetData(value); }

        public VocabData Add { get => GetData(); set => SetData(value); }
        public VocabData Overwrite { get => GetData(); set => SetData(value); }
        public VocabData MoveUp { get => GetData(); set => SetData(value); }
        public VocabData MoveDown { get => GetData(); set => SetData(value); }
        public VocabData Delete { get => GetData(); set => SetData(value); }
        public VocabData Duplicate { get => GetData(); set => SetData(value); }

        public VocabData OfficialDatabase { get => GetData(); set => SetData(value); }
        public VocabData TcgDatabase { get => GetData(); set => SetData(value); }
        public VocabData Detach { get => GetData(); set => SetData(value); }

        public VocabData Apply { get => GetData(); set => SetData(value); }
        public VocabData Clear { get => GetData(); set => SetData(value); }
        public VocabData AllClear { get => GetData(); set => SetData(value); }
        public VocabData Sort { get => GetData(); set => SetData(value); }
        public VocabData Search { get => GetData(); set => SetData(value); }
        public VocabData Preset { get => GetData(); set => SetData(value); }
        public VocabData DefaultPreset { get => GetData(); set => SetData(value); }
        public VocabData Desc_DefaultPreset { get => GetData(); set => SetData(value); }
        public VocabData SetToDefault { get => GetData(); set => SetData(value); }
        public VocabData SaveAsImage { get => GetData(); set => SetData(value); }
        public VocabData CopyImage { get => GetData(); set => SetData(value); }

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
        public VocabData Tab_PackList { get => GetData(); set => SetData(value); }
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
        public VocabData Message_NoUpdate { get => GetData(); set => SetData(value); }
        public VocabData Message_UpdateComplete { get => GetData(); set => SetData(value); }
        public VocabData OnlineManual { get => GetData(); set => SetData(value); }
        public VocabData CheckUpdate { get => GetData(); set => SetData(value); }
        public VocabData CardUpdate { get => GetData(); set => SetData(value); }
        public VocabData Desc_CardUpdate { get => GetData(); set => SetData(value); }
        public VocabData Message_CardUpdateComplete { get => GetData(); set => SetData(value); }

        public VocabData Desc_RegulationOcg { get => GetData(); set => SetData(value); }
        public VocabData Desc_RegulationTcg { get => GetData(); set => SetData(value); }

        public VocabData MatchType_Any { get => GetData(); set => SetData(value); }
        public VocabData MatchType_Any_Desc { get => GetData(); set => SetData(value); }
        public VocabData MatchType_All { get => GetData(); set => SetData(value); }
        public VocabData MatchType_All_Desc { get => GetData(); set => SetData(value); }
        public VocabData MatchType_Minimum { get => GetData(); set => SetData(value); }
        public VocabData MatchType_Minimum_Desc { get => GetData(); set => SetData(value); }
        public VocabData MatchType_Perfect { get => GetData(); set => SetData(value); }
        public VocabData MatchType_Perfect_Desc { get => GetData(); set => SetData(value); }

        public VocabData Table_Vertical { get => GetData(); set => SetData(value); }
        public VocabData Table_Horizontal { get => GetData(); set => SetData(value); }
        public VocabData Table_Total { get => GetData(); set => SetData(value); }
        public VocabData Table_Zero { get => GetData(); set => SetData(value); }
        public VocabData Table_Style { get => GetData(); set => SetData(value); }
        public VocabData Table_Style_Count { get => GetData(); set => SetData(value); }
        public VocabData Table_Style_RatioV { get => GetData(); set => SetData(value); }
        public VocabData Table_Style_RatioH { get => GetData(); set => SetData(value); }
        public VocabData Table_Digits { get => GetData(); set => SetData(value); }
        public VocabData Table_Decimal { get => GetData(); set => SetData(value); }
        public VocabData Table_Entire { get => GetData(); set => SetData(value); }
        public VocabData Table_Detail { get => GetData(); set => SetData(value); }

        public VocabData Token_Referer { get => GetData(); set => SetData(value); }
        public VocabData Token_NegativeReferer { get => GetData(); set => SetData(value); }
        public VocabData Token_Generator { get => GetData(); set => SetData(value); }
        public VocabData Token_Unselect { get => GetData(); set => SetData(value); }
        public VocabData Token_Header_Generator { get => GetData(); set => SetData(value); }
        public VocabData Token_Header_Referer { get => GetData(); set => SetData(value); }
        public VocabData Token_Text { get => GetData(); set => SetData(value); }

        public VocabData Matched { get => GetData(); set => SetData(value); }
        public VocabData Settings { get => GetData(); set => SetData(value); }
        public VocabData Match_Flags { get => GetData(); set => SetData(value); }
        public VocabData Match_Count { get => GetData(); set => SetData(value); }
        public VocabData Match_Equal { get => GetData(); set => SetData(value); }
        public VocabData Match_Greater { get => GetData(); set => SetData(value); }
        public VocabData Match_ExceptSelf { get => GetData(); set => SetData(value); }
        public VocabData Match_Candidate { get => GetData(); set => SetData(value); }
        public VocabData Match_Target { get => GetData(); set => SetData(value); }
        public VocabData Match_All { get => GetData(); set => SetData(value); }
        public VocabData Match_MainDeck { get => GetData(); set => SetData(value); }
        public VocabData Match_Sculptor { get => GetData(); set => SetData(value); }
        public VocabData Match_SmallWorld { get => GetData(); set => SetData(value); }
        public VocabData Match_Nightmell { get => GetData(); set => SetData(value); }
        public VocabData Match_Hedgehog { get => GetData(); set => SetData(value); }

        public VocabData LessOrEqual { get => GetData(); set => SetData(value); }

        public VocabData Numbers_Material { get => GetData(); set => SetData(value); }
        public VocabData Numbers_Filter { get => GetData(); set => SetData(value); }

        public VocabData Deck_Main { get => GetData(); set => SetData(value); }
        public VocabData Deck_Extra { get => GetData(); set => SetData(value); }
        public VocabData Deck_Side { get => GetData(); set => SetData(value); }
        public VocabData Deck_Unique { get => GetData(); set => SetData(value); }
        public VocabData Deck_TotalCount { get => GetData(); set => SetData(value); }
        public VocabData Deck_Import { get => GetData(); set => SetData(value); }
        public VocabData Deck_ImportJS { get => GetData(); set => SetData(value); }
        public VocabData Deck_Export { get => GetData(); set => SetData(value); }
        public VocabData Deck_Desc_Import { get => GetData(); set => SetData(value); }
        public VocabData Deck_Desc_ImportJS { get => GetData(); set => SetData(value); }
        public VocabData Deck_Desc_Export { get => GetData(); set => SetData(value); }
        public VocabData Deck_AddOne { get => GetData(); set => SetData(value); }
        public VocabData Deck_AddMax { get => GetData(); set => SetData(value); }
        public VocabData Deck_RemoveOne { get => GetData(); set => SetData(value); }
        public VocabData Deck_RemoveAll { get => GetData(); set => SetData(value); }
        public VocabData Deck_Count { get => GetData(); set => SetData(value); }
        public VocabData Deck_LineBreak { get => GetData(); set => SetData(value); }
        public VocabData Deck_Order { get => GetData(); set => SetData(value); }
        public VocabData Deck_NumberFirst { get => GetData(); set => SetData(value); }
        public VocabData Deck_NameFirst { get => GetData(); set => SetData(value); }
        public VocabData Deck_Separator { get => GetData(); set => SetData(value); }
        public VocabData Deck_WithBracket { get => GetData(); set => SetData(value); }
        public VocabData Deck_ContentHeight { get => GetData(); set => SetData(value); }
        public VocabData Deck_NameWidth { get => GetData(); set => SetData(value); }

        public VocabData Graph_Directed { get => GetData(); set => SetData(value); }
        public VocabData Graph_EdgeName { get => GetData(); set => SetData(value); }
        public VocabData Graph_Radius { get => GetData(); set => SetData(value); }
        public VocabData Graph_FontSize { get => GetData(); set => SetData(value); }
        public VocabData Graph_EdgeFontSize { get => GetData(); set => SetData(value); }
        public VocabData Graph_TextOutline { get => GetData(); set => SetData(value); }
        public VocabData Graph_HorizontalGrid { get => GetData(); set => SetData(value); }
        public VocabData Graph_VerticalGrid { get => GetData(); set => SetData(value); }

        public VocabData AddToDeck { get => GetData(); set => SetData(value); }
        public VocabData RemoveFromDeck { get => GetData(); set => SetData(value); }

        public VocabData Inspect_Name { get => GetData(); set => SetData(value); }
        public VocabData Inspect_GroupId { get => GetData(); set => SetData(value); }
        public VocabData Inspect_Value1 { get => GetData(); set => SetData(value); }
        public VocabData Inspect_Value2 { get => GetData(); set => SetData(value); }
        public VocabData Inspect_Deck { get => GetData(); set => SetData(value); }
        public VocabData Inspect_Hand { get => GetData(); set => SetData(value); }
        public VocabData Inspect_And { get => GetData(); set => SetData(value); }
        public VocabData Inspect_Or { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Count { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Hand { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Priority { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Other { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Start { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Result { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Header_Name { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Header_Group { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Header_Count { get => GetData(); set => SetData(value); }
        public VocabData InspectTest_Header_Probability { get => GetData(); set => SetData(value); }

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
            PInfo.LoadDefault();
            DLog.LoadDefault();

            SetData(nameof(Close), "閉じる", "X");
            SetData(nameof(ShowInTaskbar), "タスクバーに表示する");
            SetData(nameof(Update), "更新", "U");
            SetData(nameof(Save), "保存", "S");
            SetData(nameof(Open), "開く", "O");
            SetData(nameof(Undo), "元に戻す", "U");
            SetData(nameof(Redo), "やり直し", "R");
            SetData(nameof(Copy), "コピー", "C");
            SetData(nameof(Cut), "切り取り", "X");
            SetData(nameof(Paste), "貼り付け", "V");
            SetData(nameof(Merge), "統合", "M");
            SetData(nameof(Apply), "適用");
            SetData(nameof(Clear), "クリア");
            SetData(nameof(AllClear), "全てクリア", "Q");
            SetData(nameof(Sort), "並べ替え");
            SetData(nameof(Search), "絞り込み");
            SetData(nameof(Reload), "リロード");
            SetData(nameof(Import), "インポート");

            SetData(nameof(Add), "追加", "A");
            SetData(nameof(Overwrite), "上書き", "W");
            SetData(nameof(MoveUp), "上に移動", "U");
            SetData(nameof(MoveDown), "下に移動", "J");
            SetData(nameof(Delete), "削除", "D");
            SetData(nameof(Duplicate), "複製", "D");

            SetData(nameof(OfficialDatabase), "公式DB");
            SetData(nameof(TcgDatabase), "TCG DB");
            SetData(nameof(Detach), "別窓");
            SetData(nameof(Preset), "プリセ");
            SetData(nameof(DefaultPreset), "デフォ");
            SetData(nameof(Desc_DefaultPreset), "各設定項目をデフォルトの状態にします。");
            SetData(nameof(SetToDefault), "デフォルトにする");
            SetData(nameof(SaveAsImage), "画像として保存");
            SetData(nameof(CopyImage), "クリップボードにコピー");

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
            SetData(nameof(Tab_PackList), "パックリスト");
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
            SetData(nameof(Message_NoUpdate), "更新はありません。");
            SetData(nameof(Message_UpdateComplete), "アップデート完了しました。");
            SetData(nameof(OnlineManual), "オンラインマニュアル");
            SetData(nameof(CheckUpdate), "更新の確認");
            SetData(nameof(CardUpdate), "リスト更新");
            SetData(nameof(Desc_CardUpdate), "公式データベースにアクセスして、カードリストとパックリストを最新データに更新します。");
            SetData(nameof(Message_CardUpdateComplete), "{0}件のカード情報を更新しました。");

            SetData(nameof(Desc_RegulationOcg), "OCGの現在のレギュレーションを適用します。");
            SetData(nameof(Desc_RegulationTcg), "TCGの現在のレギュレーションを適用します。");

            SetData(nameof(MatchType_Any), "いずれか");
            SetData(nameof(MatchType_Any_Desc), "選択された項目のうち1つ以上を含む");
            SetData(nameof(MatchType_All), "全て含む");
            SetData(nameof(MatchType_All_Desc), "選択された項目を全て含む");
            SetData(nameof(MatchType_Minimum), "最小一致");
            SetData(nameof(MatchType_Minimum_Desc), "選択されていない項目を含まない");
            SetData(nameof(MatchType_Perfect), "完全一致");
            SetData(nameof(MatchType_Perfect_Desc), "選択された項目を全て含み、選択されていない項目を含まない");

            SetData(nameof(Table_Vertical), "縦");
            SetData(nameof(Table_Horizontal), "横");
            SetData(nameof(Table_Total), "Total列");
            SetData(nameof(Table_Zero), "ゼロ");
            SetData(nameof(Table_Style), "スタイル");
            SetData(nameof(Table_Style_Count), "枚数");
            SetData(nameof(Table_Style_RatioV), "%(縦)");
            SetData(nameof(Table_Style_RatioH), "%(横)");
            SetData(nameof(Table_Digits), "桁数");
            SetData(nameof(Table_Decimal), "小数点以下");
            SetData(nameof(Table_Entire), "全体幅");
            SetData(nameof(Table_Detail), "クリックで詳細");

            SetData(nameof(Token_Referer), "参照するカード");
            SetData(nameof(Token_NegativeReferer), "除外するカード");
            SetData(nameof(Token_Generator), "生成するカード");
            SetData(nameof(Token_Unselect), "選択解除");
            SetData(nameof(Token_Header_Referer), "参照");
            SetData(nameof(Token_Header_Generator), "生成");
            SetData(nameof(Token_Text), "参考テキスト");

            SetData(nameof(Matched), "マッチ");
            SetData(nameof(Settings), "設定");
            SetData(nameof(Match_Flags), "比較項目");
            SetData(nameof(Match_Count), "一致数");
            SetData(nameof(Match_Equal), "のみ");
            SetData(nameof(Match_Greater), "以上");
            SetData(nameof(Match_ExceptSelf), "自身を除外");
            SetData(nameof(Match_Candidate), "リスト候補");
            SetData(nameof(Match_Target), "検索候補");
            SetData(nameof(Match_All), "全て");
            SetData(nameof(Match_MainDeck), "メインデッキ");
            SetData(nameof(Match_Sculptor), "造形家");
            SetData(nameof(Match_SmallWorld), "スモワ");
            SetData(nameof(Match_Nightmell), "悪醒師");
            SetData(nameof(Match_Hedgehog), "針鼠");

            SetData(nameof(LessOrEqual), "以下");

            SetData(nameof(Numbers_Material), "素材候補");
            SetData(nameof(Numbers_Filter), "絞り込み");

            SetData(nameof(Deck_Main), "メインデッキ");
            SetData(nameof(Deck_Extra), "EXデッキ");
            SetData(nameof(Deck_Side), "サイドデッキ");
            SetData(nameof(Deck_Unique), "種");
            SetData(nameof(Deck_TotalCount), "枚");
            SetData(nameof(Deck_Import), "URLから読み込む");
            SetData(nameof(Deck_ImportJS), "JavaScriptで読み込む");
            SetData(nameof(Deck_Export), "デッキ編集用JavaScriptをコピー");
            SetData(nameof(Deck_Desc_Import), "公式データベースのデッキURLを貼り付けてください。");
            SetData(nameof(Deck_Desc_ImportJS), "デッキ情報取得用のJavaScriptをクリップボードに送りました。\n配列オブジェクトをコピーした状態で「インポート」を押してください。");
            SetData(nameof(Deck_Desc_Export), "デッキ編集用のJavaScriptをクリップボードに送りました。");
            SetData(nameof(Deck_AddOne), "1枚追加");
            SetData(nameof(Deck_AddMax), "最大まで追加");
            SetData(nameof(Deck_RemoveOne), "1枚削除");
            SetData(nameof(Deck_RemoveAll), "全て削除");
            SetData(nameof(Deck_Count), "数");
            SetData(nameof(Deck_LineBreak), "クリックで改行を調整");
            SetData(nameof(Deck_Order), "順序");
            SetData(nameof(Deck_NameFirst), "名前→数");
            SetData(nameof(Deck_NumberFirst), "数→名前");
            SetData(nameof(Deck_Separator), "区切り");
            SetData(nameof(Deck_WithBracket), "《》で囲む");
            SetData(nameof(Deck_ContentHeight), "項目高さ");
            SetData(nameof(Deck_NameWidth), "項目幅");

            SetData(nameof(Graph_Directed), "接続先も表示");
            SetData(nameof(Graph_EdgeName), "辺の名前を表示");
            SetData(nameof(Graph_Radius), "半径");
            SetData(nameof(Graph_FontSize), "名前");
            SetData(nameof(Graph_EdgeFontSize), "辺名前");
            SetData(nameof(Graph_TextOutline), "文字枠");
            SetData(nameof(Graph_HorizontalGrid), "横グリッド");
            SetData(nameof(Graph_VerticalGrid), "縦グリッド");

            SetData(nameof(AddToDeck), "デッキに追加");
            SetData(nameof(RemoveFromDeck), "デッキから削除");

            SetData(nameof(Inspect_Name), "表示名");
            SetData(nameof(Inspect_GroupId), "グループ");
            SetData(nameof(Inspect_Value1), "価値1");
            SetData(nameof(Inspect_Value2), "価値2");
            SetData(nameof(Inspect_Deck), "デッキ");
            SetData(nameof(Inspect_Hand), "手札");
            SetData(nameof(Inspect_And), "AND");
            SetData(nameof(Inspect_Or), "OR");
            SetData(nameof(InspectTest_Count), "試行回数");
            SetData(nameof(InspectTest_Hand), "初期手札");
            SetData(nameof(InspectTest_Priority), "初期手札");
            SetData(nameof(InspectTest_Priority), "優先度");
            SetData(nameof(InspectTest_Start), "検証開始");
            SetData(nameof(InspectTest_Result), "結果");
            SetData(nameof(InspectTest_Header_Name), "名前");
            SetData(nameof(InspectTest_Header_Group), "G");
            SetData(nameof(InspectTest_Header_Count), "回数");
            SetData(nameof(InspectTest_Header_Probability), "確率");
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
                PInfo.UpdateVocabData(v.PInfo);
                DLog.UpdateVocabData(v.DLog);
            }
        }
    }
}
