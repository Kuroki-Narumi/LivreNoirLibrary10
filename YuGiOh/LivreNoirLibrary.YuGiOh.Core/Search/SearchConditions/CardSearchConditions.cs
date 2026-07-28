using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class CardSearchConditions : TextSearchConditions
    {
        public static CardSearchConditions Default { get; } = new();
        public static CardSearchConditions Usable { get; } = new() { Limits = [LimitCount.Forbidden, LimitCount.Limit1, LimitCount.Limit2, LimitCount.Unlimited, LimitCount.Specified] };

        [JsonPropertyName(JsonPropertyNames.Search_CardTypes)]
        public HashSet<CardType> CardTypes { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_Limits)]
        public HashSet<int> Limits { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_Attributes)]
        public HashSet<Attribute> Attributes { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_MonsterTypes)]
        public HashSet<MonsterType> MonsterTypes { get; set; } = [];

        [JsonPropertyName(JsonPropertyNames.Search_StatusFlags)]
        public StatusFlags StatusFlags { get; set; } = StatusFlags.Default;

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

        public CardSearchConditions() { }
        public CardSearchConditions(CardSearchConditions source)
        {
            Copy(source, this, true);
        }

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

        private bool _name;
        private bool _ruby;
        private bool _enName;
        private bool _text;
        private bool _pText;
        private Regex? _regexForText;
        private TextForSearchStringConverter _converterForText;
        private readonly List<SearchSegment> _inputForText = [];

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

            PrepareText();
        }

        public override void PrepareText()
        {
            var input1 = _inputText;
            var input2 = _inputForText;
            input1.Clear();
            input2.Clear();
            _regex = null;
            _regexForText = null;
            _textNotEffective = true;

            var text = SearchText.AsSpan();
            var flags = TextFlags;
            if (text.IsWhiteSpace() || (flags & TextSearchFlags.CheckText) is 0)
            {
                return;
            }

            // フラグの整理
            _name = (flags & TextSearchFlags.Name) is not 0;
            _ruby = (flags & TextSearchFlags.Ruby) is not 0;
            _enName = (flags & TextSearchFlags.EnName) is not 0;
            _text = (flags & TextSearchFlags.Text) is not 0;
            _pText = (flags & TextSearchFlags.PText) is not 0;
            var ignoreCase = (flags & TextSearchFlags.IgnoreCase) is not 0;
            var ignoreTextCase = (flags & TextSearchFlags.TextIgnoreCase) is not 0;
            var regex = (flags & TextSearchFlags.UseRegex) is not 0;
            var ignoreSymbols = (flags & TextSearchFlags.IgnoreSymbols) is not 0;
            var ignoreTextSymbols = (flags & TextSearchFlags.TextIgnoreSymbols) is not 0;
            var converter = _textConverter = new(ignoreCase, ignoreSymbols);
            var converterForText = _converterForText = new(ignoreTextCase, ignoreTextSymbols);

            // 正規表現が有効な場合は生成して終了
            if (regex)
            {
                try
                {
                    _regex = new(text.ToHalfRegex(ignoreCase), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    _regexForText = new(text.ToHalfRegex(ignoreTextCase), ignoreTextCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    _textNotEffective = false;
                    return;
                }
                catch
                {
                    // 正規表現の生成に失敗した場合は通常検索に切り替える
                }
            }

            // 検索語の整理
            foreach (var (segment, flag) in text.EnumerateSearchSegments())
            {
                var t1 = converter.Convert(segment);
                input1.Add(new(t1, flag));
                var t2 = converterForText.Convert(segment);
                input2.Add(new(t2, flag));
            }

            _textNotEffective = input1.Count is 0;
        }

        public bool IsMatch(object? obj, ICardProvider? provider)
        {
            if (Card.TryGetCard(obj, provider, out var card))
            {
                return IsMatch(card);
            }
            return false;
        }

        public bool IsMatch(Card card)
        {
            // 種類
            if (SearchUtils.NotMatch(CardTypes, card.CardType)) return false;
            // リミット
            if (SearchUtils.NotMatch(Limits, card.ActualLimitCount)) return false;
            // ロケール
            if (SearchUtils.NotMatch(card.IsOcgReleased(), OcgState)) return false;
            if (SearchUtils.NotMatch(card.IsTcgReleased(), TcgState)) return false;

            // ペンデュラム
            if (_req_pen)
            {
                // ペンデュラムモンスター以外は無条件で偽
                if (!card.IsPendulum()) return false;
                // Pスケール
                if (SearchUtils.NotMatch(PendulumScales, card.PendulumScale)) return false;
                // P効果の文字数
                if (SearchUtils.NotMatch(PTextLength, Vocab.GetTextLength(card.PendulumText))) return false;
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
                if (SearchUtils.NotMatch(Attributes, card.Attribute)) return false;
                // 種族
                if (SearchUtils.NotMatch(MonsterTypes, card.MonsterType)) return false;
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
                if (SearchUtils.NotMatch(Levels, card.Level)) return false;
                // 攻撃力
                if (SearchUtils.NotMatch(Atk, card.Atk)) return false;
                // 守備力
                if (SearchUtils.NotMatch(Def, card.Def)) return false;
                // 条件式
                if (_exprEnabled && !_expr.IsMatch(card)) return false;
            }

            // テキスト長さ
            if (SearchUtils.NotMatch(TextLength, Vocab.GetTextLength(card.Text))) return false;

            // 発売日
            if (_req_date)
            {
                var list = card.PackInfo;
                // 収録パック情報が存在しない場合は偽
                if (list.Count is 0) return false;
                var (first, last) = list.GetDate(DateLocale);
                // 初登場
                if (SearchUtils.NotMatch(FirstDate, first)) return false;
                // 最終収録
                if (SearchUtils.NotMatch(LastDate, last)) return false;
            }

            // テキスト内容
            return IsTextMatch(card);
        }

        public bool IsTextMatch(Card card)
        {
            if (_textNotEffective)
            {
                return true;
            }
            var buffer = StringBuffer.Get();
            var converter1 = _textConverter;
            var converter2 = _converterForText;
            if (_regex is { } regex1)
            {
                var regex2 = _regexForText!;
                return SearchUtils.IsMatch(_name, card.Name, buffer, regex1, converter1) ||
                       SearchUtils.IsMatch(_ruby, card.Ruby, buffer, regex1, converter1) ||
                       SearchUtils.IsMatch(_enName, card.EnName, buffer, regex1, converter1) ||
                       SearchUtils.IsMatch(_text, card.Text, buffer, regex2, converter2) ||
                       SearchUtils.IsMatch(_pText, card.PendulumText, buffer, regex2, converter2);
            }
            else
            {
                var segments1 = _inputText.AsSpan();
                var segments2 = _inputForText.AsSpan();
                return SearchUtils.IsMatch(_name, card.Name, buffer, segments1, converter1) ||
                       SearchUtils.IsMatch(_ruby, card.Ruby, buffer, segments1, converter1) ||
                       SearchUtils.IsMatch(_enName, card.EnName, buffer, segments1, converter1) ||
                       SearchUtils.IsMatch(_text, card.Text, buffer, segments2, converter2) ||
                       SearchUtils.IsMatch(_pText, card.PendulumText, buffer, segments2, converter2);
            }
        }

        public static void Copy(CardSearchConditions from, CardSearchConditions to, bool copyText)
        {
            SearchUtils.CopyHashSet(from.CardTypes, to.CardTypes);
            SearchUtils.CopyHashSet(from.Limits, to.Limits);
            SearchUtils.CopyHashSet(from.Attributes, to.Attributes);
            SearchUtils.CopyHashSet(from.MonsterTypes, to.MonsterTypes);
            to.StatusFlags = from.StatusFlags;
            to.Abilities = from.Abilities;
            to.AbilitiesExcept = from.AbilitiesExcept;
            SearchUtils.CopyHashSet(from.Levels, to.Levels);
            to.Atk.CopyFrom(from.Atk);
            to.Def.CopyFrom(from.Def);
            SearchUtils.CopyHashSet(from.PendulumScales, to.PendulumScales);
            to.LinkMarkers = from.LinkMarkers;
            to.StatusExpression = from.StatusExpression;
            to.OcgState = from.OcgState;
            to.TcgState = from.TcgState;
            to.FirstDate.CopyFrom(from.FirstDate);
            to.LastDate.CopyFrom(from.LastDate);
            to.DateLocale = from.DateLocale;
            to.TextLength.CopyFrom(from.TextLength);
            to.PTextLength.CopyFrom(from.PTextLength);
            if (copyText)
            {
                to.SearchText = from.SearchText;
            }
            to.TextFlags = from.TextFlags;
        }
    }
}
