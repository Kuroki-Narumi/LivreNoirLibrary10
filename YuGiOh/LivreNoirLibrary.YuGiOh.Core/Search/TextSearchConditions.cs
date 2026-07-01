using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Text.Convert;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class TextSearchConditions : ObservableObjectBase
    {
        public string? Text { get; set => SetValue(ref field, value, CheckRegex); }

        public TextSearchFlags Flags { get; set => SetValue(ref field, value, _flagProps, OnFlagChanged); } = TextSearchFlags.Default;

        public bool IsValid { get; private set => SetValue(ref field, value); } = true;

        private static readonly string[] _flagProps = 
        [
            nameof(Flag_Name), nameof(Flag_Ruby), nameof(Flag_EnName), nameof(Flag_Text), nameof(Flag_PText), 
            nameof(Flag_IgnoreCase), nameof(Flag_IgnoreTextCase), nameof(Flag_Regex), nameof(Flag_IgnoreSymbols), nameof(Flag_IgnoreTextSymbols),
        ];
        public bool Flag_Name { get => GetFlag(TextSearchFlags.Name); set => SetFlag(TextSearchFlags.Name, value); }
        public bool Flag_Ruby { get => GetFlag(TextSearchFlags.Ruby); set => SetFlag(TextSearchFlags.Ruby, value); }
        public bool Flag_EnName { get => GetFlag(TextSearchFlags.EnName); set => SetFlag(TextSearchFlags.EnName, value); }
        public bool Flag_Text { get => GetFlag(TextSearchFlags.Text); set => SetFlag(TextSearchFlags.Text, value); }
        public bool Flag_PText { get => GetFlag(TextSearchFlags.PText); set => SetFlag(TextSearchFlags.PText, value); }
        public bool Flag_IgnoreCase { get => GetFlag(TextSearchFlags.IgnoreCase); set => SetFlag(TextSearchFlags.IgnoreCase, value); }
        public bool Flag_IgnoreTextCase { get => GetFlag(TextSearchFlags.IgnoreTextCase); set => SetFlag(TextSearchFlags.IgnoreTextCase, value); }
        public bool Flag_Regex { get => GetFlag(TextSearchFlags.Regex); set => SetFlag(TextSearchFlags.Regex, value); }
        public bool Flag_IgnoreSymbols { get => GetFlag(TextSearchFlags.IgnoreSymbols); set => SetFlag(TextSearchFlags.IgnoreSymbols, value); }
        public bool Flag_IgnoreTextSymbols { get => GetFlag(TextSearchFlags.IgnoreTextSymbols); set => SetFlag(TextSearchFlags.IgnoreTextSymbols, value); }

        private void CheckRegex()
        {
            var text = Text;
            if (!string.IsNullOrEmpty(text) && Flag_Regex)
            {
                try
                {
                    _ = new Regex(text);
                }
                catch
                {
                    IsValid = false;
                    return;
                }
            }
            IsValid = true;
        }

        private void OnFlagChanged(TextSearchFlags oldValue, TextSearchFlags newValue)
        {
            if ((oldValue & TextSearchFlags.Regex) != (newValue & TextSearchFlags.Regex))
            {
                CheckRegex();
            }
        }

        private bool GetFlag(TextSearchFlags flag) => (Flags & flag) is not 0;

        private void SetFlag(TextSearchFlags flag, bool value)
        {
            if (value)
            {
                Flags |= flag;
            }
            else
            {
                Flags &= ~flag;
            }
        }

        public void Load(TextSearchConditions source)
        {
            Text = source.Text;
            Flags = source.Flags;
        }

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
            var ignoreTextCase = (flags & TextSearchFlags.IgnoreTextCase) is not 0;
            var regex = (flags & TextSearchFlags.Regex) is not 0;
            var ignoreSymbol = (flags & TextSearchFlags.IgnoreSymbols) is not 0;
            var ignoreTextSymbols = (flags & TextSearchFlags.IgnoreTextSymbols) is not 0;

            // 正規表現が有効な場合は生成して終了
            if (regex && IsValid)
            {
                _regex = new(text.ToHalfRegex(), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                _regexForText = new(text.ToHalfRegex(), ignoreTextCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                _notEffective = false;
                return;
            }

            var converter = _converter = new(ignoreCase, ignoreSymbol);
            var converterForText = _converterForText = new(ignoreTextCase, ignoreTextSymbols);
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

        private static readonly char[] _searchBuffer = new char[512];

        public bool IsMatch(ICard card)
        {
            if (_notEffective)
            {
                return true;
            }
            if (_regex is { } regex)
            {
                var regex2 = _regexForText!;
                var cache = GetTextCache(card);
                return (_name && regex.IsMatch(cache.Name.AsSpan())) ||
                       (_ruby && regex.IsMatch(cache.Ruby.AsSpan())) ||
                       (_enName && regex.IsMatch(cache.EnName.AsSpan())) ||
                       (_text && regex2.IsMatch(cache.Text.AsSpan())) ||
                       (_pText && regex2.IsMatch(cache.PendulumText.AsSpan()));
            }
            else
            {
                var buffer = _searchBuffer.AsSpan();
                var segments1 = _input.AsSpan();
                var segments2 = _inputForText.AsSpan();
                var converter = _converter;
                var converterForText = _converterForText;
                return CheckMatch(_name, card.Name, buffer, segments1, converter) ||
                       CheckMatch(_ruby, card.Ruby, buffer, segments1, converter) ||
                       CheckMatch(_enName, card.EnName, buffer, segments1, converter) ||
                       CheckMatch(_text, card.Text, buffer, segments2, converterForText) ||
                       CheckMatch(_pText, card.PendulumText, buffer, segments2, converterForText);
            }
        }

        private static bool CheckMatch(bool checkFlag, ReadOnlySpan<char> text, Span<char> buffer, ReadOnlySpan<SearchSegment> segments, TextForSearchStringConverter converter)
        {
            if (checkFlag)
            {
                var length = converter.Convert(text, buffer);
                return segments.IsMatch(buffer[..length], StringComparison.Ordinal);
            }
            return false;
        }

        private static readonly Dictionary<ICard, TextCache> _textCaches = [];

        public static void RemoveTextCache(ICard card) => _textCaches.Remove(card);

        private static TextCache GetTextCache(ICard card)
        {
            if (!_textCaches.TryGetValue(card, out var cache))
            {
                cache = new(card);
                _textCaches.Add(card, cache);
            }
            return cache;
        }

        private class TextCache(ICard card)
        {
            public readonly string Name = card.Name.ToHalf();
            public readonly string Ruby = card.Ruby.ToHalf();
            public readonly string EnName = card.EnName.ToHalf();
            public readonly string Text = card.Text.ToHalf();
            public readonly string PendulumText = card.PendulumText.ToHalf();
        }
    }
}
