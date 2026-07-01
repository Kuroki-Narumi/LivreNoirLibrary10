using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        private IBmsData _root = null!;
        private StringBuilder _comments = null!;

        private ParseState _current = null!;

        private List<ParseState> _states = null!;

        public static string ReadRawText(Stream stream)
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
            return text;
        }

        public static bool TryGetBase(ReadOnlySpan<char> line, out int radix)
        {
            if (Regex_Radix.IsMatch(line))
            {
                // "#BASE" より後ろの部分
                line = line[5..].Trim();
                if (int.TryParse(line, out var value))
                {
                    radix = value;
                    return true;
                }
            }
            radix = 0;
            return false;
        }

        public BmsParser(Stream stream)
        {
            var text = ReadRawText(stream);
            RawText = text;
            // #BASE と #LNOBJ は一度だけ適用する
            var radix = BmsConstants.Base_Default;
            ReadOnlySpan<char> span_lnobj = [];
            foreach (var line in text.EnumerateLines())
            {
                var span = line.TrimStart();
                if (TryGetBase(span, out var value))
                {
                    radix = value;
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

            var radix = Radix;
            var flowActions = _flowActions;
            var endFlowActions = _endFlowActions;

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
                                if (TryGetFlow(span, expr, out var value))
                                {
                                    action(this, value);
                                    goto AfterProcess;
                                }
                            }
                            foreach (var (expr, action) in endFlowActions)
                            {
                                if (IsMatch(span, expr))
                                {
                                    action(this);
                                    goto AfterProcess;
                                }
                            }
                            // Process Definitions
                            foreach (var (tag, type) in DefTags)
                            {
                                if (TryGetDef(span, tag, radix, out var key, out var value))
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
                            if (TryGetChannel(span, out var number, out var channel, out var valueSpan))
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
                            else if (TryGetHeader(span, out var key, out var value))
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
                        else if (!FieldSeparators.IsMatch(line))
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
            _current = new(FlowAddress.Empty, target.Root, null);

            _comments ??= new();
            _comments.Clear();

            _states ??= [];
            _states.Clear();
            _states.Add(_current);
        }

        private void OnLineProcessed(int lineNumber)
        {
            var comments = _comments;
            if (comments.Length is > 0)
            {
                _current.Comments.Add(comments.ToString());
                comments.Clear();
            }
        }

        private void EndConstruct()
        {
            foreach (var state in _states.AsSpan())
            {
                state.Data.Note = string.Join(Environment.NewLine, state.Comments);
                var radix = Radix;
                var tl = state.Data.Timeline;
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
                    if (!CanResolveConductor(state, defType))
                    {
                        continue;
                    }
                    foreach (var (number, line) in list.AsSpan())
                    {
                        var span = line.AsSpan();
                        var den = span.Length / 2;
                        for (var i = 0; i < den; i++)
                        {
                            if (BasedNumber.TryParseToShort(span[..2], radix, out var key) && TryGetConductorDef(state, defType, key, out var value))
                            {
                                tl.Add(new(number + (double)i / den), new Note(channel, value));
                            }
                            span = span[2..];
                        }
                    }
                }
            }
        }

        static bool CanResolveConductor(ParseState state, DefType type)
        {
            if (state.ConductorDefs.ContainsKey(type))
            {
                return true;
            }
            if (state.Parent is { } parent)
            {
                return CanResolveConductor(parent, type);
            }
            return false;
        }

        static bool TryGetConductorDef(ParseState state, DefType type, short key, out double value)
        {
            if (state.ConductorDefs.TryGetValue(type, out var dic) && 
                dic.TryGetValue(key, out value))
            {
                return true;
            }
            if (state.Parent is { } parent)
            {
                return TryGetConductorDef(parent, type, key, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        private void AddComment(ReadOnlySpan<char> line)
        {
            if (!line.IsWhiteSpace())
            {
                _comments.AppendLine(new(line));
            }
        }

        private void AddHeader(ReadOnlySpan<char> key, ReadOnlySpan<char> value)
        {
            var data = _current.Data;
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

        private void AddDef(DefType type, short key, ReadOnlySpan<char> value) => _current.Data.DefLists.Set(type, key, new(value));
        private void AddConductorDef(DefType type, short key, double value) => _current.ConductorDefs.GetOrAdd(type)[key] = value;
        private void SetBarLength(int number, double value) => _current.Data.BarDefs.Set(number, value);

        private void AddConductorLine(int number, Channel channel, ReadOnlySpan<char> value) => _current.UnProcessedLines.GetOrAdd(channel).Add((number, new(value)));

        private void AddNormalLine(int number, Channel channel, ReadOnlySpan<char> value)
        {
            var state = _current;
            var tl = state.Data.Timeline;
            Func<short, Note> noteCreator;
            if (channel is Channel.Bpm_Base)
            {
                noteCreator = v => new(Channel.Bpm, v);
            }
            else if (channel is Channel.Bgm)
            {
                var counts = state.BgmLaneCounts;
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
                        if (state.LastLongNotes.Remove(lane))
                        {
                            return new(lane, NoteType.LongEnd, v);
                        }
                        else
                        {
                            state.LastLongNotes.Add(lane);
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
            var comments = _comments;
            if (comments.Length is > 0)
            {
                obj.Note = comments.ToString();
                comments.Clear();
            }
        }

        private static string GetIndented(FlowAddress address) => new(' ', (address.Length - 1) * 2);

        private static void StartFlow(BmsParser parser, FlowType type, int max, bool isFixed)
        {
            // 想定される構造
            // 01 #RANDOM 3
            // 02   #IF 1
            // 03     (contents)
            // 04   #ENDIF
            // 06 #RANDOM 3

            // - 05 #ENDRANDOMが省略されている
            // 現在のフローを強制終了(記述が正確な場合、StartFlowはフローの外で起こるはず)
            parser.TryEndFlow(out _);

            var current = parser._current;
            var data = current.Data;
            var flow = new FlowContainer
            {
                Type = type,
                Max = max,
                IsFixed = isFixed
            };
            parser.ApplyComment(flow);
            data.Flows.Add(flow);
            var address = current.Address.Append(data.Flows.Count);
            current.CurrentFlow = flow;
            current.CurrentFlowAddress = address;
            //Console.WriteLine($"{GetIndented(address)}Start{type} address={address}, max={max}, isFixed={isFixed}");
        }

        private bool TryEndFlow([MaybeNullWhen(false)]out FlowContainer flow)
        {
            var current = _current;
            if (current.CurrentFlow is { } f)
            {
                current.CurrentFlow = null;
                //var address = current.CurrentFlowAddress;
                //Console.WriteLine($"{GetIndented(address)}End{f.Type} address={address}");
                flow = f;
                return true;
            }
            flow = default;
            return false;
        }

        private static void EndFlow(BmsParser parser, FlowType type)
        {
            // 想定される構造
            // 01 #RANDOM 3
            // 02  #IF 2
            // 03    (contents)
            // 04    #SWITCH
            // 07 #ENDRANDOM

            // 正常終了
            if (parser.TryEndFlow(out var flow))
            {
                // - 05 #ENDSWが省略されている
                // 期待した種類ではないフローを終了した
                if (flow.Type != type)
                {
                    // 改めてフロー終了を試みる
                    EndFlow(parser, type);
                }
            }
            // - 06 #ENDIFが省略されている
            // フローの外側でフローを終了しようとしている場合、ブランチの終了を試みる
            else if (parser.TryEndBranch(out _))
            {
                // 改めてフロー終了を試みる
                EndFlow(parser, type);
            }
        }

        private static void StartBranch(BmsParser parser, FlowType type, int value)
        {
            // 想定される構造
            // 01 #RANDOM 3
            // 02   #IF 1
            // 03     (contents)
            // 04     #SWITCH 3
            // 05       #CASE 1
            // 07       #CASE 2
            // 08       #SKIP
            // 10   #IF 2
            // 11     (contents)
            // 12   #ENDIF
            // 13 #ENDRANDOM

            var current = parser._current;
            // フローの内側
            if (current.CurrentFlow is { } flow)
            {
                // フローの種類と始めようとしているブランチの種類が一致(正常な開始)
                if (flow.Type == type)
                {
                    var branch = flow.GetOrAddBranch(value);
                    var data = parser._root.GetBranchData(branch);
                    var address = current.CurrentFlowAddress.Append(value);
                    //Console.WriteLine($"{GetIndented(address)}StartBranch address={address}, cond={branch.Condition}, dataIndex={branch.DataIndex}");
                    parser.ApplyComment(branch);
                    var newState = new ParseState(address, data, current);
                    parser._states.Add(newState);
                    parser._current = newState;
                }
                // フローとは異なる種類のブランチを開始しようとしている場合
                else
                {
                    // 現在のフローを強制終了
                    parser.TryEndFlow(out _);
                    // 改めてブランチ開始を試みる
                    StartBranch(parser, type, value);
                }
            }
            // フローの外側でブランチを開始しようとしている場合、ブランチの終了を試みる
            else if (parser.TryEndBranch(out _))
            {
                // 改めてブランチ開始を試みる
                StartBranch(parser, type, value);
            }
        }

        private bool TryEndBranch(out FlowType type)
        {
            var current = _current;
            if (current.Parent is { } parent)
            {
                //var address = current.Address;
                type = current.ParentFlowType;
                //Console.WriteLine($"{GetIndented(address)}EndBranch of {type} address={address}");
                _current = parent;
                return true;
            }
            type = FlowType.None;
            return false;
        }

        private static void EndBranch(BmsParser parser, FlowType type)
        {
            // 想定される構造
            // 01 #RANDOM 3
            // 02  #IF 2
            // 03    (contents)
            // 04    #SWITCH
            // 05    #CASE 1
            // 08 #ENDIF

            // 正常終了
            if (parser.TryEndBranch(out var actualType))
            {
                // -06 #SKIPが省略されている
                // 期待した種類ではないブランチを終了した
                if (actualType != type)
                {
                    // 改めてブランチ終了を試みる
                    EndBranch(parser, type);
                }
            }
            // -07 #ENDSWが省略されている
            // ブランチの外側でブランチを終了しようとしている場合、フローの終了を試みる
            else if (parser.TryEndFlow(out _))
            {
                // 改めてブランチ終了を試みる
                EndBranch(parser, type);
            }
        }

        private static void StartRandom(BmsParser parser, int value) => StartFlow(parser, FlowType.Random, value, false);
        private static void StartSetRandom(BmsParser parser, int value) => StartFlow(parser, FlowType.Random, value, true);
        private static void StartIf(BmsParser parser, int value) => StartBranch(parser, FlowType.Random, value);
        private static void StartElseIf(BmsParser parser, int value) => StartBranch(parser, FlowType.Random, value);
        private static void StartElse(BmsParser parser) => StartBranch(parser, FlowType.Random, BmsConstants.DefaultCondition);
        private static void EndIf(BmsParser parser) => EndBranch(parser, FlowType.Random);
        private static void EndRandom(BmsParser parser) => EndFlow(parser, FlowType.Random);

        private static void StartSwitch(BmsParser parser, int value) => StartFlow(parser, FlowType.Switch, value, false);
        private static void StartSetSwitch(BmsParser parser, int value) => StartFlow(parser, FlowType.Switch, value, true);
        private static void StartCase(BmsParser parser, int value) => StartBranch(parser, FlowType.Switch, value);
        private static void StartDefault(BmsParser parser) => StartBranch(parser, FlowType.Switch, BmsConstants.DefaultCondition);
        private static void Skip(BmsParser parser) => EndBranch(parser, FlowType.Switch);
        private static void EndSwitch(BmsParser parser) => EndFlow(parser, FlowType.Switch);

        private static readonly (string, Action<BmsParser, int>)[] _flowActions =
        [
            (Tags.Random, StartRandom),
            ("#RONDAM", StartRandom), // for typo
            ("#RANDON", StartRandom), // for typo
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

        private static readonly (string, Action<BmsParser>)[] _endFlowActions =
        [
            (Tags.EndRandom, EndRandom),
            ("#END RANDOM", EndRandom), // fallback
            (Tags.EndSwitch, EndSwitch),
            ("#ENDSWITCH", EndSwitch), // fallback
            ("#END SWITCH", EndSwitch), // fallback
            (Tags.EndIf, EndIf),
            (Tags.Skip, Skip),
            (Tags.Else, StartElse),
            (Tags.Default, StartDefault),
            ("#DEFAULT", StartDefault), // fallback
        ];

        [GeneratedRegex(@"^#BASE\s+(\d+)", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_Radix { get; }

        [GeneratedRegex(@"^#LNOBJ\s+(\w+)", RegexOptions.IgnoreCase)]
        private static partial Regex Regex_LnObj { get; }

        [GeneratedRegex(@"^#\d{3}[0-9a-zA-Z]{2}.")]
        private static partial Regex Regex_Channel { get; }

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

        private static bool IsMatch(ReadOnlySpan<char> line, ReadOnlySpan<char> expression)
            => line.StartsWith(expression, StringComparison.OrdinalIgnoreCase);

        private static bool TryGetFlow(ReadOnlySpan<char> line, ReadOnlySpan<char> flowExpression, out int value)
        {
            if (IsMatch(line, flowExpression))
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
            if (IsMatch(line, defExpression) && 
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
