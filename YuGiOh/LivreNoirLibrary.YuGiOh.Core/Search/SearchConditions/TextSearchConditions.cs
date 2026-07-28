using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class TextSearchConditions
    {
        [JsonIgnore]
        public string? SearchText { get; set; }

        [JsonPropertyName(JsonPropertyNames.Search_TextFlags)]
        public TextSearchFlags TextFlags { get; set; } = TextSearchFlags.Default;

        protected bool _textNotEffective;
        protected Regex? _regex;
        protected TextForSearchStringConverter _textConverter;
        protected readonly List<SearchSegment> _inputText = [];

        public virtual void PrepareText()
        {
            var input = _inputText;
            input.Clear();
            _regex = null;
            var text = SearchText.AsSpan();
            var flags = TextFlags;
            if (!text.IsWhiteSpace())
            {
                var ignoreCase = (flags & TextSearchFlags.IgnoreCase) is not 0;
                var ignoreSymbols = (flags & TextSearchFlags.IgnoreSymbols) is not 0;
                var converter = _textConverter = new(ignoreCase, ignoreSymbols);

                var regex = (flags & TextSearchFlags.UseRegex) is not 0;
                // 正規表現が有効な場合は生成して終了
                if (regex)
                {
                    try
                    {
                        _regex = new(text.ToHalfRegex(ignoreCase), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
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
                    input.Add(new(t1, flag));
                }
            }
            _textNotEffective = input.Count is 0;
        }

        public bool IsTextMatch(ReadOnlySpan<char> text)
        {
            if (_textNotEffective)
            {
                return true;
            }
            var buffer = StringBuffer.Get();
            var converter1 = _textConverter;
            if (_regex is { } regex1)
            {
                return SearchUtils.IsMatch(true, text, buffer, regex1, converter1);
            }
            else
            {
                return SearchUtils.IsMatch(true, text, buffer, _inputText.AsSpan(), converter1);
            }
        }

        public bool IsTextMatch(INamedObject obj) => IsTextMatch(obj.Name);
    }
}
