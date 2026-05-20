using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsParser
    {
        public string RawText { get; }
        public int Radix { get; }
        public short LnObj { get; }

        private IBmsData? _root;
        private StringBuilder? _comments;
        private ParseState? _current;

        private List<ParseState>? _states;
        private Dictionary<DefType, Dictionary<short, double>>? _conductorDefs;

        private Stack<(ParseState, FlowContainer?)>? _stack;
        private FlowContainer? _currentFlow;
        private bool _insideBranch;

        public BmsParser(Stream stream)
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
            if (BasedNumber.TryParseToShort(span_lnobj, radix, out var lnobj))
            {
                LnObj = lnobj;
            }
        }

        public void Parse(IBmsData target)
        {
            BeginConstruct(target);

            (string, Action<int>)[] flowActions = [
                    (Tags.Random, StartRandom),
                    ("#RONDAM", StartRandom), // for typo
                    (Tags.SetRandom, StartSetRandom),
                    ("#SET RANDOM", StartSetRandom), // fallback
                    (Tags.Switch, StartSwitch),
                    (Tags.SetSwitch, StartSetSwitch),
                    ("#SET SWITCH", StartSetSwitch), // fallback
                    (Tags.If, StartIf),
                    (Tags.ElseIf, StartElseIf),
                    ("#ELIF", StartElseIf), // fallback
                    ("#ELSE IF", StartElseIf), // fallback
                    (Tags.Case, StartCase),
                ];
            var radix = Radix;

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
                                EndRandom();
                            }
                            else if (Regex_EndSwitch.IsMatch(line))
                            {
                                EndSwitch();
                            }
                            else if (Regex_Else.IsMatch(line))
                            {
                                StartElse();
                            }
                            else if (Regex_EndIf.IsMatch(line))
                            {
                                EndIf();
                            }
                            else if (Regex_Default.IsMatch(line))
                            {
                                StartDefault();
                            }
                            else if (Regex_Skip.IsMatch(line))
                            {
                                Skip();
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
                                        AddConductorDef(type, key, double.Parse(value));
                                    }
                                    else
                                    {
                                        AddDef(type, key, value);
                                    }
                                    goto AfterProcess;
                                }
                            }
                            // Process Channel
                            if (TryGetChannel(line, out var number, out var channel, out var valueSpan))
                            {
                                if (channel is Channel.Bar)
                                {
                                    SetBarLength(number, double.Parse(valueSpan));
                                }
                                else if (channel.IsConductor())
                                {
                                    AddConductorLine(number, channel, valueSpan);
                                }
                                else
                                {
                                    AddNormalLine(number, channel, valueSpan);
                                }
                            }
                            // Process Header
                            else if (TryGetHeader(line, out var key, out var value))
                            {
                                AddHeader(key, value);
                            }
                            else
                            {
                                AddComment(line);
                            }
                            AfterProcess:
                            OnLineProcessed(lineNumber);
                        }
                        // フィールド区切りではない
                        else if (!FieldSeparators.IsMatch(span))
                        {
                            AddComment(line);
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
            EndConstruct();
        }

        private void BeginConstruct(IBmsData target)
        {
            _root = target;
            target.Clear();
            target.LnObj = LnObj;
            _comments = new();
            _stack = [];
            _states = [];
            _conductorDefs = [];
            _current = new(target.Root);
            _states.Add(_current);
        }

        private void OnLineProcessed(int lineNumber)
        {
            var comments = _comments!;
            if (comments.Length is > 0)
            {
                _current!.Comments.Add(comments.ToString());
                comments.Clear();
            }
        }

        private void EndConstruct()
        {
            foreach (var state in _states.AsSpan())
            {
                ResolveConductor(state);
            }
        }

        private void AddComment(ReadOnlySpan<char> line)
        {
            if (line.Length is > 0 && !line.IsWhiteSpace())
            {
                _comments!.AppendLine(new(line));
            }
        }

        private void AddHeader(ReadOnlySpan<char> key, ReadOnlySpan<char> value)
        {
            var data = _current!.Data;
            if (Enum.TryParse<HeaderType>(key, true, out var type))
            {
                if (type is HeaderType.Base or HeaderType.LnObj)
                {
                    return;
                }
                data.MainHeaders[type] = new(value);
            }
            else
            {
                data.SubHeaders.Add(new(new(key), new(value)));
            }
        }

        private void AddDef(DefType type, short key, ReadOnlySpan<char> value) => _current!.Data.DefLists.Set(type, key, new(value));
        private void AddConductorDef(DefType type, short key, double value) => _conductorDefs!.GetOrAdd(type)[key] = value;
        private void SetBarLength(int number, double value) => _current!.Data.BarDefs.Set(number, value);

        private void AddConductorLine(int number, Channel channel, ReadOnlySpan<char> value) => _current!.UnProcessedLines.GetOrAdd(channel).Add((number, new(value)));

        private void ResolveConductor(ParseState state)
        {
            state.Data.Note = string.Join(Environment.NewLine, state.Comments);
            var radix = Radix;
            var tl = state.Data.Timeline;
            var defs = _conductorDefs!;
            foreach (var (channel, list) in state.UnProcessedLines)
            {
                var defType = channel switch
                {
                    Channel.Bpm => DefType.Bpm,
                    Channel.Stop => DefType.Stop,
                    Channel.Scroll => DefType.Scroll,
                    Channel.Speed => DefType.Speed,
                    _ => DefType.None,
                };
                if (!defs.TryGetValue(defType, out var def))
                {
                    continue;
                }
                foreach (var (number, line) in list.AsSpan())
                {
                    var span = line.AsSpan();
                    var den = span.Length / 2;
                    for (var i = 0; i < den; i++)
                    {
                        if (BasedNumber.TryParseToShort(span[..2], radix, out var key) && def.TryGetValue(key, out var value))
                        {
                            tl.Add(new(number + (double)i / den), new Note(channel, value));
                        }
                        span = span[2..];
                    }
                }
            }
        }

        private void AddNormalLine(int number, Channel channel, ReadOnlySpan<char> value)
        {
            var tl = _current!.Data.Timeline;
            Func<short, Note> noteCreator;
            if (channel is Channel.Bpm_Base)
            {
                noteCreator = v => new(Channel.Bpm, v);
            }
            else if (channel is Channel.Bgm)
            {
                var counts = _current.BgmLaneCounts;
                if (!counts.TryGetValue(number, out var count))
                {
                    count = 0;
                }
                counts[number] = count + 1;
                channel = Channel.Bgm_Start + (short)count;
                noteCreator = v => new(channel, v);
            }
            else if (channel.IsSoundLane())
            {
                var lnObj = LnObj;
                var (lane, type) = channel.Split();
                noteCreator = type switch
                {
                    NoteType.LongEnd => v =>
                    {
                        if (_current.LastLongNotes.Remove(lane))
                        {
                            return new(lane, NoteType.LongEnd, v);
                        }
                        else
                        {
                            _current.LastLongNotes.Add(lane);
                            return new(lane, v);
                        }
                    }
                    ,
                    NoteType.Normal => v => v == lnObj ? new(lane, NoteType.LongEnd, 0) : new(lane, v),
                    _ => v => new(lane, type, v),
                };
            }
            else
            {
                noteCreator = v => new(channel, v);
            }
            var radix = channel.IsHexValue() ? 16 : Radix;
            var den = value.Length / 2;
            for (var i = 0; i < den; i++)
            {
                if (BasedNumber.TryParseToShort(value[..2], radix, out var v) && v is not 0)
                {
                    tl.Add(new(number + (double)i / den), noteCreator(v));
                }
                value = value[2..];
            }
        }

        private void ApplyComment(INoteObject obj)
        {
            var comments = _comments!;
            if (comments.Length is > 0)
            {
                obj.Note = comments.ToString();
                comments.Clear();
            }
        }

        private void StartFlow(FlowType type, int max, bool isFixed)
        {
            // 想定される構造
            // 01 #RANDOM 3
            // 02   #IF 1
            // 03     (contents)
            // 04   #ENDIF
            // 06 #RANDOM 3
            // - 05 #ENDRANDOMが省略されている
            //   - 現在のフローを終了する
            if (_currentFlow is not null)
            {
                EndFlow();
            }
            var data = _current!.Data;
            var flow = new FlowContainer
            {
                Type = type,
                Max = max,
                IsFixed = isFixed
            };
            ApplyComment(flow);
            data.Flows.Add(flow);
            _currentFlow = flow;
            _insideBranch = false;
        }

        private void EndFlow()
        {
            // 想定される構造
            // 06 #RANDOM 3
            // 07  #IF 2
            // 08    (contents)
            // 10 #ENDRANDOM
            // - 09 #ENDIFが省略されている
            //   - 現在のブランチでフローが開始していないにも関わらず、フローを終了しようとしている
            if (_currentFlow is null)
            {
                EndBranch();
            }
            _currentFlow = null;
        }

        private void StartBranch(int value)
        {
            // 想定される構造
            // 01 #RANDOM 3
            // 02   #IF 1
            // 03     (contents)
            // 04   #IF 2
            // 05     (contents)
            // 07   #ENDIF
            // 08 #ENDRANDOM
            // - 04 フローの外でブランチが開始している
            //   - ブランチの終了タグが省略されているとみなして、現在のブランチを終了
            if (_insideBranch)
            {
                EndBranch();
            }
            if (_currentFlow is { } flow)
            {
                _stack!.Push((_current!, flow));
                var branch = flow.GetOrAddBranch(value);
                var data = _root!.GetBranchData(branch);
                ApplyComment(branch);
                _current = new(data);
                _states!.Add(_current);
                _currentFlow = null;
                _insideBranch = true;
            }
        }

        private void EndBranch()
        {
            if (_insideBranch && _stack!.TryPop(out var item))
            {
                (_current, _currentFlow) = item;
            }
        }

        private void StartRandom(int value) => StartFlow(FlowType.Random, value, false);
        private void StartSetRandom(int value) => StartFlow(FlowType.Random, value, true);
        private void StartIf(int value) => StartBranch(value);
        private void StartElseIf(int value) => StartBranch(value);
        private void StartElse() => StartBranch(BmsConstants.DefaultCondition);
        private void EndIf() => EndBranch();
        private void EndRandom() => EndFlow();

        private void StartSwitch(int value) => StartFlow(FlowType.Switch, value, false);
        private void StartSetSwitch(int value) => StartFlow(FlowType.Switch, value, true);
        private void StartCase(int value) => StartBranch(value);
        private void StartDefault() => StartBranch(BmsConstants.DefaultCondition);
        private void Skip() => EndBranch();
        private void EndSwitch() => EndFlow();

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

        private static bool TryGetDef(ReadOnlySpan<char> line, ReadOnlySpan<char> defExpression, int radix, out short key, out ReadOnlySpan<char> valueSpan)
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
                BasedNumber.TryParseToShort(line.Slice(defLength, 2), radix, out key))
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
