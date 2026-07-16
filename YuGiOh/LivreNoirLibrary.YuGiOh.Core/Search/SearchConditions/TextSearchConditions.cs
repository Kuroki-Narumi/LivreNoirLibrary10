using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class TextSearchConditions
    {
        public string? Text { get; set; }

        public TextSearchFlags Flags { get; set; } = TextSearchFlags.Default;

        private bool _notEffective;
        private bool _name;
        private bool _ruby;
        private bool _enName;
        private bool _text;
        private bool _pText;
        private Regex? _regex;
        private Regex? _regexForText;
        private TextForSearchStringConverter _converter;
        private TextForSearchStringConverter _converterForText;
        private readonly List<SearchSegment> _input = [];
        private readonly List<SearchSegment> _inputForText = [];

        public void Prepare()
        {
            var input1 = _input;
            var input2 = _inputForText;
            input1.Clear();
            input2.Clear();
            _regex = null;
            _regexForText = null;
            _notEffective = true;

            var text = Text.AsSpan();
            var flags = Flags;
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
            var converter = _converter = new(ignoreCase, ignoreSymbols);
            var converterForText = _converterForText = new(ignoreTextCase, ignoreTextSymbols);

            // 正規表現が有効な場合は生成して終了
            if (regex)
            {
                try
                {
                    _regex = new(text.ToHalfRegex(ignoreCase), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    _regexForText = new(text.ToHalfRegex(ignoreTextCase), ignoreTextCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    _notEffective = false;
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

            _notEffective = input1.Count is 0;
        }

        public bool IsMatch(Card card)
        {
            if (_notEffective)
            {
                return true;
            }
            var buffer = StringBuffer.Get();
            var converter1 = _converter;
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
                var segments1 = _input.AsSpan();
                var segments2 = _inputForText.AsSpan();
                return SearchUtils.IsMatch(_name, card.Name, buffer, segments1, converter1) ||
                       SearchUtils.IsMatch(_ruby, card.Ruby, buffer, segments1, converter1) ||
                       SearchUtils.IsMatch(_enName, card.EnName, buffer, segments1, converter1) ||
                       SearchUtils.IsMatch(_text, card.Text, buffer, segments2, converter2) ||
                       SearchUtils.IsMatch(_pText, card.PendulumText, buffer, segments2, converter2);
            }
        }
    }
}
