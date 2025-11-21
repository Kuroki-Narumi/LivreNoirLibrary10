using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsTextReader
    {
        public string RawText { get; }
        public int Radix { get; }
        public long LnObj { get; }

        public BmsTextReader(Stream stream)
        {
            string text;
            var pos = stream.Position;
            try
            {
                using StreamReader reader = new(stream, BmsConstants.Utf8Encoding, true, -1, true);
                text = reader.ReadToEnd();
            }
            catch (DecoderFallbackException)
            {
                stream.Position = pos;
                using StreamReader reader = new(stream, BmsConstants.DefaultEncoding, false, -1, true);
                text = reader.ReadToEnd();
            }
            RawText = text;
            // #BASE と #LNOBJ は一度だけ適用する
            var radix = BmsConstants.Base_Default;
            ReadOnlySpan<char> span_lnobj = [];
            foreach (var line in text.EnumerateLines())
            {
                var span = line.TrimStart();
                if (Regex_Radix.IsMatch(span))
                {
                    // "#BASE" より後ろの部分
                    span = span[5..].Trim();
                    if (int.TryParse(span, out var value))
                    {
                        radix = value;
                    }
                }
                else if (Regex_LnObj.IsMatch(span))
                {
                    // "LNOBJ"より後ろの部分
                    span_lnobj = span[6..].Trim();
                }
            }
            Radix = radix;
            LnObj = BasedNumber.TryParseToLong(span_lnobj, radix, out var lnobj) ? lnobj : 0;
        }

        public void Parse(IBmsParser parser)
        {
            (string, Action<int>)[] flowActions = [
                    (Tags.Random, parser.StartRandom),
                    ("#RONDAM", parser.StartRandom), // for typo
                    (Tags.SetRandom, parser.StartSetRandom),
                    ("#SET RANDOM", parser.StartSetRandom), // fallback
                    (Tags.Switch, parser.StartSwitch),
                    (Tags.SetSwitch, parser.StartSetSwitch),
                    ("#SET SWITCH", parser.StartSetSwitch), // fallback
                    (Tags.If, parser.StartIf),
                    (Tags.ElseIf, parser.StartElseIf),
                    ("#ELIF", parser.StartElseIf), // fallback
                    ("#ELSE IF", parser.StartElseIf), // fallback
                    (Tags.Case, parser.StartCase),
                ];
            var radix = Radix;
            parser.InitializeParse(radix, LnObj);

            var lineNumber = 0;
            foreach (var line in RawText.EnumerateLines())
            {
                try
                {
                    var span = line.TrimStart();
                    if (span.Length is > 0)
                    {
                        // コマンド行
                        if (span[0] is '#')
                        {
                            // Process Random
                            foreach (var (expr, action) in flowActions)
                            {
                                if (TryGetFlow(line, expr, out var value))
                                {
                                    action(value);
                                    goto AfterProcess;
                                }
                            }
                            if (Regex_EndRandom.IsMatch(line))
                            {
                                parser.EndRandom();
                            }
                            else if (Regex_EndSwitch.IsMatch(line))
                            {
                                parser.EndSwitch();
                            }
                            else if (Regex_Else.IsMatch(line))
                            {
                                parser.StartElse();
                            }
                            else if (Regex_EndIf.IsMatch(line))
                            {
                                parser.EndIf();
                            }
                            else if (Regex_Default.IsMatch(line))
                            {
                                parser.StartDefault();
                            }
                            else if (Regex_Skip.IsMatch(line))
                            {
                                parser.Skip();
                            }
                            else
                            {
                                goto ProcessDef;
                            }
                            goto AfterProcess;
                        ProcessDef:
                            foreach (var (tag, type) in DefTags)
                            {
                                if (TryGetDef(line, tag, radix, out var key, out var value))
                                {
                                    if (type is >= DefType.Bpm and <= DefType.Speed)
                                    {
                                        parser.AddConductorDef(type, key, double.Parse(value));
                                    }
                                    else
                                    {
                                        parser.AddDef(type, key, value.ToString());
                                    }
                                    goto AfterProcess;
                                }
                            }
                            // Process Channel
                            if (TryGetChannel(line, out var number, out var channel, out var valueSpan))
                            {
                                parser.AddBar(number, channel, valueSpan);
                            }
                            // Process Header
                            else if (TryGetHeader(line, out var key, out var value))
                            {
                                parser.AddHeader(key, value);
                            }
                            else
                            {
                                parser.OnLineUnprocessed(line);
                            }
                        AfterProcess:
                            parser.OnLineProcessed(lineNumber);
                        }
                        // フィールド区切りではない
                        else if (!FieldSeparators.IsMatch(span))
                        {
                            parser.AddComment(line);
                        }
                    }
                    lineNumber++;
                }
                catch (Exception e)
                {
                    if (e is BmsParseException)
                    {
                        throw;
                    }
                    else
                    {
                        throw new BmsParseException(lineNumber, line.ToString(), e);
                    }
                }
            }
            parser.FinalizeParse();
        }

        [GeneratedRegex(@"^#BASE\s+(\d+)", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_Radix { get; }
        [GeneratedRegex(@"^#LNOBJ\s+(\w+)", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_LnObj { get; }
        [GeneratedRegex(@"^#\d{3}[0-9a-zA-Z]{2}.")]
        private static partial Regex Regex_Channel { get; }

        [GeneratedRegex(@"^#END\s*RANDOM\s*$", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_EndRandom { get; }
        [GeneratedRegex(@"^#END\s*SW(?:ITCH)?\s*$", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_EndSwitch { get; }
        [GeneratedRegex(@"^#ELSE\s*$", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_Else { get; }
        [GeneratedRegex(@"^#END\s*IF\s*$", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_EndIf { get; }
        [GeneratedRegex(@"^#DEF(?:AULT)?\s*$", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_Default { get; }
        [GeneratedRegex(@"^#SKIP\s*$", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_Skip { get; }

        private static readonly (string, DefType)[] DefTags = [
            (Tags.Wav, DefType.Wav),
            (Tags.Bmp, DefType.Bmp),
            (Tags.Bga, DefType.Bga),
            (Tags.Bpm, DefType.Bpm),
            (Tags.ExBpm, DefType.Bpm), // fallback
            (Tags.Stop, DefType.Stop),
            (Tags.Text,  DefType.Text),
            (Tags.ExWav, DefType.ExWav),
            (Tags.ExBmp, DefType.Bmp),
            (Tags.AtBga, DefType.AtBga),
            (Tags.Argb, DefType.Argb),
            (Tags.SwBga, DefType.SwBga),
            (Tags.ExRank, DefType.ExRank),
            (Tags.ChangeOption, DefType.ChangeOption),
            (Tags.Scroll, DefType.Scroll),
            (Tags.Speed, DefType.Speed),
            ];

        private static bool IsDelimiter(char c) => char.GetUnicodeCategory(c) is UnicodeCategory.SpaceSeparator or UnicodeCategory.OtherPunctuation;

        private static bool TryGetFlow(ReadOnlySpan<char> line, ReadOnlySpan<char> flowExpression, out int value)
        {
            if (line.StartsWith(flowExpression, StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(line[flowExpression.Length..].Trim(), out value))
                {
                    return true;
                }
            }
            value = default;
            return false;
        }

        private static bool TryGetDef(ReadOnlySpan<char> line, ReadOnlySpan<char> defExpression, int radix, out long key, out ReadOnlySpan<char> valueSpan)
        {
            key = default;
            valueSpan = default;
            var defLength = defExpression.Length;
            // 定義プレフィクス + インデックス2桁
            if (line.Length < defLength + 2)
            {
                return false;
            }
            if (line.StartsWith(defExpression, StringComparison.InvariantCultureIgnoreCase) && 
                // インデックスは2桁で固定
                BasedNumber.TryParseToLong(line.Slice(defLength, 2), radix, out key))
            {
                var rest = line[(defLength + 2)..];
                // 1文字の区切り文字を挟んだ残りが内容(空文字列になる可能性もある)
                // フォールバックのため、インデックスの直後が区切り文字でない(例:'a')場合、それは値に含める。
                valueSpan = rest.Length is 0 ? [] : (IsDelimiter(rest[0]) ? rest[1..] : rest);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool TryGetChannel(ReadOnlySpan<char> line, out int number, out Channel channel, out ReadOnlySpan<char> valueSpan)
        {
            number = default;
            channel = default;
            valueSpan = default;
            if (Regex_Channel.IsMatch(line))
            {
                // この時点で長さ7以上が保証される('#' + 小節番号3桁 + チャンネル2桁 + 値1桁以上)
                // 正規表現から使われる文字種も限定されているので、以下の2行では例外は発生しない。
                number = int.Parse(line.Slice(1, 3));
                channel = BmsUtils.ToChannel(line.Slice(4, 2));
                // 1文字の区切り文字を挟んだ残りが内容(空文字列になる可能性もある)
                // フォールバックのため、チャンネルの直後が区切り文字でない(例:'a')場合、それは値に含める。
                valueSpan = line[(IsDelimiter(line[6]) ? 7 : 6)..];
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool TryGetHeader(ReadOnlySpan<char> line, out ReadOnlySpan<char> keySpan, out ReadOnlySpan<char> valueSpan)
        {
            keySpan = valueSpan = default;
            // "#" + キー1文字以上 + (空白 + 0文字以上の値)?
            if (line.Length is >= 2 && line[0] is '#' && !IsDelimiter(line[1]))
            {
                var index = 2;
                // 空白文字を探す
                for (; index < line.Length && !IsDelimiter(line[index]); index++) ;
                // この時点で index == (line.Length または 最初に出現した空白の位置)
                keySpan = line[1..index];
                // 空白の次の位置
                index++;
                if (index < line.Length)
                {
                    valueSpan = line[index..];
                }
                return true;
            }
            return false;
        }
    }
}
