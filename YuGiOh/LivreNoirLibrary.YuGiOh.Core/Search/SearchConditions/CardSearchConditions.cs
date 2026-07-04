using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class CardSearchConditions
    {
        [JsonPropertyName(JsonPropertyNames.Search_CardTypes)]
        public HashSet<CardType> CardTypes { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_Limits)]
        public HashSet<int> Limits { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_Attributes)]
        public HashSet<Attribute> Attributes { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_MonsterTypes)]
        public HashSet<MonsterType> MonsterTypes { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_StatusFlags)]
        public StatusFlags StatusFlags { get; set; } = 0;

        [JsonPropertyName(JsonPropertyNames.Search_Abilities)]
        public Ability Abilities { get; set; } = 0;

        [JsonPropertyName(JsonPropertyNames.Search_AbilitiesExcept)]
        public Ability AbilitiesExcept { get; set; } = 0;

        [JsonPropertyName(JsonPropertyNames.Search_Levels)]
        public HashSet<int> Levels { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_Atk)]
        public NumberRange Atk { get; set; } = new(-1, 5000, false, false);

        [JsonPropertyName(JsonPropertyNames.Search_Def)]
        public NumberRange Def { get; set; } = new(-1, 5000, false, false);

        [JsonPropertyName(JsonPropertyNames.Search_Scale)]
        public HashSet<int> PendulumScales { get; set; } = [];
        
        [JsonPropertyName(JsonPropertyNames.Search_LinkMarkers)]
        public LinkDirection LinkMarkers { get; set; } = 0;

        [JsonPropertyName(JsonPropertyNames.Search_StatusExpression)]
        public string StatusExpression { get; set; } = "";

        [JsonPropertyName(JsonPropertyNames.Search_OcgState)]
        public LocaleState OcgState { get; set; }

        [JsonPropertyName(JsonPropertyNames.Search_TcgState)]
        public LocaleState TcgState { get; set; }

        [JsonPropertyName(JsonPropertyNames.Search_FirstDate)]
        public DateRange FirstDate { get; set; } = new();

        [JsonPropertyName(JsonPropertyNames.Search_LastDate)]
        public DateRange LastDate { get; set; } = new();

        [JsonPropertyName(JsonPropertyNames.Search_DateLocale)]
        public LocaleType DateLocale { get; set; } = 0;

        [JsonPropertyName(JsonPropertyNames.Search_TextLength)]
        public NumberRange TextLength { get; set; } = new(0, 999, false, false);

        [JsonPropertyName(JsonPropertyNames.Search_PTextLength)]
        public NumberRange PTextLength { get; set; } = new(0, 999, false, false);

        [JsonIgnore]
        public string SearchText { get; set; } = "";

        [JsonPropertyName(JsonPropertyNames.Search_TextFlags)]
        public TextSearchFlags TextFlags { get; set; } = TextSearchFlags.Default;

        private bool _req_monster;
        private bool _req_normal;
        private bool _req_effect;
        private bool _req_def;
        private bool _req_link;
        private bool _req_pen;
        private bool _abiPerf;
        private bool _markerPerf;
        private bool _exprEnabled;
        private bool _req_date;

        private readonly StatusExpression _expr = new();
        private readonly TextSearchConditions _textConds = new();

        public void Prepare()
        {
            var stFlag = StatusFlags;
            var normal = (stFlag & StatusFlags.Normal) is not 0;
            var effect = (stFlag & StatusFlags.Effect) is not 0;
            _req_normal = normal && !effect;
            _req_effect = !normal && effect;
            _abiPerf = (stFlag & StatusFlags.AbilityPerf) is not 0;
            _markerPerf = (stFlag & StatusFlags.LinkMarkerPerf) is not 0;

            var def = Def.IsEnabled;
            var link = _req_link = LinkMarkers is not 0;
            var pen = PendulumScales.Count is > 0 || PTextLength.IsEnabled;
            var monster = def || link || pen ||
                Attributes.Count is > 0 ||
                MonsterTypes.Count is > 0 ||
                normal || effect || Abilities is not 0 || AbilitiesExcept is not 0 ||
                Levels.Count is > 0 ||
                Atk.IsEnabled;
            if ((stFlag & StatusFlags.StatusExpression) is not 0)
            {
                _expr.Expression = StatusExpression;
                _exprEnabled = _expr.IsEffective;
                var (m, d, p) = _expr.CheckRequirements();
                monster |= m;
                def |= d;
                pen |= p;
            }
            _req_monster = monster;
            _req_def = def;
            _req_pen = pen;

            _req_date = FirstDate.IsEnabled || LastDate.IsEnabled;

            _textConds.Text = SearchText;
            _textConds.Flags = TextFlags;
            _textConds.Prepare();
        }

        public bool IsMatch(ICard card)
        {
            // 種類
            if (SearchCondition.NotMatch(CardTypes, card.CardType)) return false;
            // リミット
            if (SearchCondition.NotMatch(Limits, Regulation.Instance.Get(card))) return false;
            // ロケール
            if (SearchCondition.NotMatch(card.IsOcgReleased(), OcgState)) return false;
            if (SearchCondition.NotMatch(card.IsTcgReleased(), TcgState)) return false;

            // ペンデュラム
            if (_req_pen)
            {
                // ペンデュラムモンスター以外は無条件で偽
                if (!card.IsPendulum()) return false;
                // Pスケール
                if (SearchCondition.NotMatch(PendulumScales, card.PendulumScale)) return false;
                // P効果の文字数
                if (SearchCondition.NotMatch(PTextLength, Vocab.GetTextLength(card.PendulumText))) return false;
            }

            // モンスターステータス
            if (_req_monster)
            {
                // モンスター以外は無条件で偽
                if (!card.IsMonster()) return false;
                // リンクモンスターの場合
                if (card.IsLink())
                {
                    // 守備力を必要とする場合は偽
                    if (_req_def) return false;
                    // リンクマーカー
                    if (_req_link)
                    {
                        var reqMarker = LinkMarkers;
                        var markerPerf = _markerPerf;
                        var marker = card.GetLinkDirections() & LinkMarkers;
                        if ((markerPerf && marker != reqMarker) || (!markerPerf && marker is 0)) return false;
                    }
                }
                // それ以外でリンクモンスターを要求する場合は偽
                else if (_req_link)
                {
                    return false;
                }
                // 属性
                if (SearchCondition.NotMatch(Attributes, card.Attribute)) return false;
                // 種族
                if (SearchCondition.NotMatch(MonsterTypes, card.MonsterType)) return false;
                // 効果を持たないモンスター
                if (_req_normal && card.HasEffect) return false;
                // 効果モンスター
                if (_req_effect && !card.HasEffect) return false;
                // 能力
                var abilities = Abilities;
                var cabi = card.Ability;
                if (abilities is not 0)
                {
                    var abi = abilities & cabi;
                    var abiPerf = _abiPerf;
                    if ((abiPerf && abi != abilities) || (!abiPerf && abi is 0)) return false;
                }
                // 能力(除外)
                if ((AbilitiesExcept & cabi) is not 0) return false;
                // レベル
                if (SearchCondition.NotMatch(Levels, card.Level)) return false;
                // 攻撃力
                if (SearchCondition.NotMatch(Atk, card.Atk)) return false;
                // 守備力
                if (SearchCondition.NotMatch(Def, card.Def)) return false;
                // 条件式
                if (_exprEnabled && !_expr.IsMatch(card)) return false;
            }

            // テキスト長さ
            if (SearchCondition.NotMatch(TextLength, Vocab.GetTextLength(card.Text))) return false;

            // 発売日
            if (_req_date)
            {
                var list = card.PackInfo;
                // 収録パック情報が存在しない場合は偽
                if (list.Count is 0) return false;
                var (first, last) = list.GetDate(DateLocale);
                // 初登場
                if (SearchCondition.NotMatch(FirstDate, first)) return false;
                // 最終収録
                if (SearchCondition.NotMatch(LastDate, last)) return false;
            }

            // テキスト内容
            return _textConds.IsMatch(card);
        }
    }
}
