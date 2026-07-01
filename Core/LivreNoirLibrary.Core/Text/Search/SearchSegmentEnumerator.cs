using System;

namespace LivreNoirLibrary.Text
{
    public ref struct SearchSegmentEnumerator(ReadOnlySpan<char> input, char require, char reject)
    {
        readonly ReadOnlySpan<char> _input = input;
        readonly char _require = require;
        readonly char _reject = reject;

        int _index;
        RefSearchSegment _current;

        public readonly RefSearchSegment Current => _current;

        public bool MoveNext()
        {
            var input = _input;
            var len = input.Length;
            var start = _index;
            var require = _require;
            var reject = _reject;
            for (var i = start; i < len;)
            {
                // 空白を飛ばす
                for (; i < len && char.IsWhiteSpace(input[i]); i++) ;
                start = i;
                // 空白文字を探す
                for (; i < len && !char.IsWhiteSpace(input[i]); i++) ;
                // 間に1文字以上あった
                var length = i - start;
                if (length is > 0)
                {
                    var span = input.Slice(start, length);
                    SearchSegmentFlag flag = 0;
                    // 必要性プレフィクス
                    if (length is >= 2)
                    {
                        if (input[start] == require)
                        {
                            flag = SearchSegmentFlag.Required;
                            span = span[1..];
                        }
                        else if (input[start] == reject)
                        {
                            flag = SearchSegmentFlag.Rejected;
                            span = span[1..];
                        }
                    }
                    _current = new(span, flag);
                    _index = i;
                    return true;
                }
            }
            _current = default;
            _index = start;
            return false;
        }

        public readonly SearchSegmentEnumerator GetEnumerator() => this;
    }
}
