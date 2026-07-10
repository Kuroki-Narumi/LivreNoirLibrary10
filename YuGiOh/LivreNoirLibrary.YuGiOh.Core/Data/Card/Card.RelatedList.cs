using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card
    {
        private bool _needCreateRelatedText = true;
        private readonly HashSet<string> _realtedTexts = [];

        public IEnumerable<string> RelatedList
        {
            get
            {
                if (_needCreateRelatedText)
                {
                    CreateRelatedText();
                }
                return _realtedTexts;
            }
        }

        public void ClearRelatedText()
        {
            _needCreateRelatedText = true;
            _realtedTexts.Clear();
        }

        private enum PrefixType { None, Effect, Attribute, Other }
        private readonly record struct RelatedTextState(int Start, PrefixType CurrentType, PrefixType PreviousType);
        private static readonly ThreadLocal<Stack<RelatedTextState>> _rangeStack = new(() => new());

        private void CreateRelatedText()
        {
            _needCreateRelatedText = false;
            var set = _realtedTexts;
            set.Clear();
            set.Add(Name);
            var stack = _rangeStack.Value!;
            CreateRelatedText(set, Text, stack);
            CreateRelatedText(set, PendulumText, stack);
        }


        [GeneratedRegex(@"(?:(?:(?:そ|ー|ド|手)の|した)効果は|持つ、|効果は、)「$")]
        private static partial Regex Regex_EffectExpr { get; }

        [GeneratedRegex(@"属性(?:は|を)「$")]
        private static partial Regex Regex_AttrExpr { get; }

        private static void CreateRelatedText(HashSet<string> set, ReadOnlySpan<char> text, Stack<RelatedTextState> stack)
        {
            stack.Clear();
            var start = -1;
            var type = PrefixType.None;
            var prev = type;
            var regex_effect = Regex_EffectExpr;
            var regex_attr = Regex_AttrExpr;
            for (var i = 0; i < text.Length; i ++)
            {
                switch (text[i])
                {
                    case '「':
                        // 入れ子への対応
                        if (start is not -1)
                        {
                            stack.Push(new(start, type, prev));
                        }
                        start = i + 1;
                        var span = text[..start];
                            // テキストの最初 : その他(典型的には融合素材の指定)
                        type = i is 0 ? PrefixType.Other
                            // 直前が'」' : 以前のタイプを継続
                            : text[i - 1] is '」' ? prev
                            // 属性変更
                            : (regex_attr.IsMatch(span) ? PrefixType.Attribute
                            // 効果の置換
                            : regex_effect.IsMatch(span) ? PrefixType.Effect
                            : PrefixType.Other);
                        break;
                    case '」':
                        if (i > start)
                        {
                            span = text[start..i];
                            if (type is not (PrefixType.Effect or PrefixType.Attribute))
                            {
                                set.Add(new(span));
                            }
                        }
                        if (stack.TryPop(out var state))
                        {
                            (start, type, prev) = state;
                        }
                        else
                        {
                            prev = type;
                            type = PrefixType.None;
                            start = -1;
                        }
                        break;
                }
            }
        }
    }
}
