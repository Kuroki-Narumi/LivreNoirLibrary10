using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Media.Wave.Chunks;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    internal partial class BmsTextReader
    {
        private class ParseState(BaseData data)
        {
            public readonly BaseData Data = data;
            public readonly RawData RawData = [];
        }

        private readonly BmsData _root;
        private readonly List<ParseState> _states = [];
        private readonly int _radix = Constants.Base_Default;
        private readonly string[] _lines;
        private readonly Dictionary<Command, Action<Match>> _actions;

        private readonly Stack<ParseState?> _data_stack = [];
        private readonly Stack<FlowContainer?> _flow_stack = [];
        private readonly Stack<FlowBranch?> _branch_stack = [];
        private ParseState? _current;
        private FlowContainer? _current_flow;
        private FlowBranch? _current_branch;

        private readonly StringBuilder _comment_builder = new();

        public BmsTextReader(Stream stream)
        {
            string? text;
            var pos = stream.Position;
            try
            {
                using StreamReader reader = new(stream, Constants.Utf8Encoding, true, -1, true);
                text = reader.ReadToEnd();
            }
            catch (DecoderFallbackException)
            {
                stream.Position = pos;
                using StreamReader reader = new(stream, Constants.DefaultEncoding, false, -1, true);
                text = reader.ReadToEnd();
            }
            var match = GR_Base.Match(text);
            if (match.Success)
            {
                _radix = ParseInt(match, 1);
            }
            _lines = text.SplitLines();
            _actions = new()
                {
                    // #RANDOM flow
                    { Command.Random, Parse_Random },
                    { Command.SetRandom, Parse_SetRandom },
                    { Command.EndRandom, Parse_EndRandom },
                    { Command.If, Parse_If },
                    { Command.ElseIf, Parse_ElseIf },
                    { Command.Else, Parse_Else },
                    { Command.EndIf, Parse_EndIf },
                    // #SWITCH flow
                    { Command.Switch, Parse_Switch },
                    { Command.SetSwitch, Parse_SetSwitch },
                    { Command.EndSwitch, Parse_EndSwitch },
                    { Command.Case, Parse_Case },
                    { Command.Skip, Parse_Skip },
                    { Command.Default, Parse_Default },
                    // Definitions
                    { Command.WavDef, Parse_WavDef },
                    { Command.BmpDef, Parse_BmpDef },
                    { Command.BgaDef, Parse_BgaDef },
                    { Command.BpmDef, Parse_BpmDef },
                    { Command.StopDef, Parse_StopDef },
                    { Command.TextDef, Parse_TextDef },
                    { Command.ExWavDef, Parse_ExWavDef },
                    { Command.ExBmpDef, Parse_ExBmpDef },
                    { Command.AtBgaDef, Parse_AtBgaDef },
                    { Command.ArgbDef, Parse_ArgbDef },
                    { Command.SwBgaDef, Parse_SwBgaDef },
                    { Command.ExRankDef, Parse_ExRankDef },
                    { Command.OptionDef, Parse_OptionDef },
                    { Command.ScrollDef, Parse_ScrollDef },
                    { Command.SpeedDef, Parse_SpeedDef },
                    // Channel commands
                    { Command.Bar, Parse_Bar },
                    { Command.Channel, Parse_Channel },
                    // Header info
                    { Command.Base, Parse_Base },
                    { Command.Header, Parse_Header },
                };
            _root = new();
        }

        public BmsData Parse()
        {
            // initialize
            _data_stack.Clear();
            _flow_stack.Clear();
            _branch_stack.Clear();
            _current = AddState(_root);
            _current_flow = null;
            _current_branch = null;
            // interpret
            foreach (var line in _lines)
            {
                bool matched = false;
                foreach (var (command, regex) in CommandRegex)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        if (_actions.TryGetValue(command, out var action))
                        {
                            action(match);
                        }
                        matched = true;
                        ApplyComment();
                        break;
                    }
                }
                if (!matched && !SeparatorComments.Is(line) && !IsEmpty(line))
                {
                    AddComment(line);
                }
            }
            while (_current_flow is not null)
            {
                EndFlow();
            }
            // extract
            foreach (var state in CollectionsMarshal.AsSpan(_states))
            {
                ExtractRawData(state);
            }
            foreach (var state in CollectionsMarshal.AsSpan(_states))
            {
                var defList = state.Data.DefLists;
                defList.Remove(DefType.Bpm);
                defList.Remove(DefType.Stop);
                defList.Remove(DefType.Scroll);
                defList.Remove(DefType.Speed);
            }
            return _root;
        }

        private ParseState AddState(BaseData data)
        {
            ParseState state = new(data);
            _states.Add(state);
            return state;
        }

        private delegate void AddProc(BarPosition p, int id);
        private void ExtractRawData(ParseState state)
        {
            var data = state.Data;
            var bars = data.Bars;
            var defList = data.DefLists;
            var timeline = data.Timeline;

            HashSet<int> lastNote_ln = []; // LN-channel
            var lnobj = data.Headers.LnObj;

            void ProcessAdd(int number, ChannelData data, AddProc addProc)
            {
                var resolution = data.Length;
                for (var k = 0; k < resolution; k++)
                {
                    if (data[k] is not 0)
                    {
                        addProc(new(number, k, resolution), data[k]);
                    }
                }
            }
            void AddNote(BarPosition pos, NoteType type, int lane, int id)
            {
                timeline.Add(pos, new(type, lane, id));
            }
            void AddMeta(DefType defType, int id, Action<Rational> addProc)
            {
                if (defList.GetInherited(defType, id) is string text && Rational.TryParse(text, out var value))
                {
                    addProc(value);
                }
            }

            foreach (var (number, bar) in state.RawData)
            {
                if (bar.Length is not 1)
                {
                    Rational barLength;
                    try
                    {
                        barLength = Rational.ConvertBySBT(bar.Length * Constants.DefaultBarLength, Constants.BarLengthDenominatorLimit);
                        if (barLength.IsNegativeOrZero())
                        {
                            ExConsole.Write($"#CAUTION# bar length is too small ({number.GetBarText()}: {bar.Length})");
                            barLength = new(1, Constants.BarLengthDenominatorLimit);
                        }
                    }
                    catch (OverflowException)
                    {
                        ExConsole.Write($"#CAUTION# bar length is too large ({number.GetBarText()}: {bar.Length})");
                        barLength = (Rational)Constants.BarLengthDenominatorLimit;
                    }
                    bars.Set(number, barLength);
                }
                var list = bar.Bgms;
                var count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    ProcessAdd(number, list[i], (p, id) => AddNote(p, NoteType.Normal, -i, id));
                }
                // channel data
                list = bar.Channels;
                count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    var line = list[i];
                    var ch = line.Channel;
                    AddProc addProc;
                    switch (ch)
                    {
                        case Channel.Bpm_Base:
                            addProc = (p, id) => timeline.Add(p, Note.Tempo(id));
                            break;
                        case Channel.Bpm:
                            addProc = (p, id) => AddMeta(DefType.Bpm, id, v => timeline.Add(p, Note.Tempo(v)));
                            break;
                        case Channel.Stop:
                            addProc = (p, id) => AddMeta(DefType.Stop, id, v => timeline.Add(p, Note.Stop(BmsUtils.GetStopLength(v))));
                            break;
                        case Channel.Scroll:
                            addProc = (p, id) => AddMeta(DefType.Scroll, id, v => timeline.Add(p, Note.Scroll(v)));
                            break;
                        case Channel.Speed:
                            addProc = (p, id) => AddMeta(DefType.Speed, id, v => timeline.Add(p, Note.Speed(v)));
                            break;
                        default:
                            var type = ch.GetNoteType();
                            var lane = ch.GetLane();
                            if (ch.IsLong())
                            {
                                addProc = (p, v) =>
                                {
                                    if (lastNote_ln.Remove(lane))
                                    {
                                        AddNote(p, NoteType.LongEnd, lane, v);
                                    }
                                    else
                                    {
                                        AddNote(p, NoteType.Normal, lane, v);
                                        lastNote_ln.Add(lane);
                                    }
                                };
                            }
                            else if (ch.IsVisible())
                            {
                                addProc = (p, v) =>
                                {
                                    if (v == lnobj)
                                    {
                                        AddNote(p, NoteType.LongEnd, lane, 0);
                                    }
                                    else
                                    {
                                        AddNote(p, NoteType.Normal, lane, v);
                                    }
                                };
                            }
                            else
                            {
                                addProc = (p, v) => AddNote(p, type, lane, v);
                            }
                            break;
                    }
                    ProcessAdd(number, line, addProc);
                }
            }
        }

        private static bool IsEmpty(string str) => GR_Empty.IsMatch(str);
        private static int ParseInt(Match match, int index) => int.TryParse(match.Groups[index].Value, out int value) ? value : 0;

        private void StartFlow(FlowContainer flow)
        {
            ForceEndFlow();

            _data_stack.Push(_current);
            _flow_stack.Push(_current_flow);
            _branch_stack.Push(_current_branch);

            _current = null;
            _current_flow = flow;
            _current_branch = null;

            FlushComment(flow);
        }

        private void FlushComment(ObjectBase obj)
        {
            if (_comment_builder.Length != 0)
            {
                obj.Note = _comment_builder.ToString().Trim();
            }
            _comment_builder.Clear();
        }

        private void EndFlow()
        {
            if (_current_flow is not null)
            {
                _current = _data_stack.Pop();
                _current_flow = _flow_stack.Pop();
                _current_branch = _branch_stack.Pop();
            }
        }

        private void ForceEndFlow()
        {
            if (_current is null)
            {
                EndFlow();
            }
        }

        private void ApplyComment()
        {
            FlushComment(_current?.Data ?? _root);
        }

        private void AddComment(string text)
        {
            _comment_builder.AppendLine(text.Trim());
        }

        private void AddDef(DefType type, Match match)
        {
            ForceEndFlow();
            var index = BmsUtils.ToInt(match.Groups[1].Value, _radix);
            _current?.Data.DefLists.Set(type, index, match.Groups[2].Value.Trim());
        }

        private void AddBar(Match match)
        {
            ForceEndFlow();
            _current?.RawData.Set(ParseInt(match, 1), BmsUtils.ToChannel(match.Groups[2].Value), match.Groups[3].Value, _radix);
        }

        private void UpdateBranchData(FlowBranch branch)
        {
            var parent = _current?.Data;
            _current = AddState(_root.GetFlowData(branch.DataId));
            if (parent is not null)
            {
                _current.Data.Inherit(parent);
            }
            FlushComment(branch);
        }

        private void UpdateBranch(FlowBranch branch)
        {
            _current_branch = branch;
            UpdateBranchData(branch);
        }

        public void Parse_Random(Match match) => StartFlow(_root.CreateRandom(_current_branch, ParseInt(match, 1), false).Flow);
        public void Parse_SetRandom(Match match) => StartFlow(_root.CreateRandom(_current_branch, ParseInt(match, 1), true).Flow);
        public void Parse_EndRandom(Match match) => EndFlow();

        public void Parse_If(Match match)
        {
            if (_current_flow is not FlowRandom)
            {
                EndFlow();
            }
            if (_current_flow is FlowRandom r)
            {
                UpdateBranch(_root.CreateIf(r, ParseInt(match, 1)));
            }
        }

        public void Parse_ElseIf(Match match)
        {
            if (_current_branch is FlowIf b)
            {
                UpdateBranchData(_root.CreateElseIf(b, ParseInt(match, 1)));
            }
        }

        public void Parse_Else(Match match)
        {
            if (_current_branch is FlowIf b)
            {
                UpdateBranchData(_root.CreateElse(b));
            }
        }

        public void Parse_EndIf(Match match)
        {
            if (_current_branch is FlowIf)
            {
                _current_branch = null;
                _current = null;
            }
        }

        public void Parse_Switch(Match match) => StartFlow(_root.CreateSwitch(_current_branch, ParseInt(match, 1), false).Flow);
        public void Parse_SetSwitch(Match match) => StartFlow(_root.CreateSwitch(_current_branch, ParseInt(match, 1), true).Flow);
        public void Parse_EndSwitch(Match match) => EndFlow();

        public void Parse_Case(Match match)
        {
            if (_current_flow is not FlowSwitch)
            {
                EndFlow();
            }
            if (_current_flow is FlowSwitch s)
            {
                UpdateBranch(_root.CreateCase(s, ParseInt(match, 1)));
            }
        }

        public void Parse_Skip(Match match)
        {
            if (_current_branch is FlowCase b)
            {
                b.Skip = true;
                _current_branch = null;
                _current = null;
            }
        }

        public void Parse_Default(Match match)
        {
            if (_current_flow is not FlowSwitch)
            {
                EndFlow();
            }
            if (_current_flow is FlowSwitch s)
            {
                UpdateBranch(_root.CreateDefault(s));
            }
        }

        public void Parse_WavDef(Match match) => AddDef(DefType.Wav, match);
        public void Parse_BmpDef(Match match) => AddDef(DefType.Bmp, match);
        public void Parse_BgaDef(Match match) => AddDef(DefType.Bga, match);
        public void Parse_AtBgaDef(Match match) => AddDef(DefType.AtBga, match);
        public void Parse_BpmDef(Match match) => AddDef(DefType.Bpm, match);
        public void Parse_StopDef(Match match) => AddDef(DefType.Stop, match);
        public void Parse_ExRankDef(Match match) => AddDef(DefType.ExRank, match);
        public void Parse_ExWavDef(Match match) => AddDef(DefType.ExWav, match);
        public void Parse_ExBmpDef(Match match) => AddDef(DefType.ExBmp, match);
        public void Parse_TextDef(Match match) => AddDef(DefType.Text, match);
        public void Parse_ArgbDef(Match match) => AddDef(DefType.Argb, match);
        public void Parse_SwBgaDef(Match match) => AddDef(DefType.SwBga, match);
        public void Parse_OptionDef(Match match) => AddDef(DefType.ChangeOption, match);
        public void Parse_ScrollDef(Match match) => AddDef(DefType.Scroll, match);
        public void Parse_SpeedDef(Match match) => AddDef(DefType.Speed, match);

        public void Parse_Bar(Match match) => AddBar(match);
        public void Parse_Channel(Match match) => AddBar(match);
        public void Parse_Base(Match match) { }
        public void Parse_Header(Match match)
        {
            ForceEndFlow();
            _current?.Data.Headers.SetAuto(match.Groups[1].Value, match.Groups[2].Value.Trim());
        }

        [GeneratedRegex(@"^\s*$")]
        private static partial Regex GR_Empty { get; }

        [GeneratedRegex(@"^\s*#BASE\s+(\d+)", RegexOptions.IgnoreCase | RegexOptions.Multiline, "ja-JP")]
        private static partial Regex GR_Base { get; }

        [GeneratedRegex(@"^\s*#(?:RANDOM|RONDAM)\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_Random { get; }

        [GeneratedRegex(@"^\s*#SET\s*RANDOM\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_SetRandom { get; }

        [GeneratedRegex(@"^\s*#END\s*RANDOM", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_EndRandom { get; }

        [GeneratedRegex(@"^\s*#IF\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_If { get; }

        [GeneratedRegex(@"^\s*#EL(?:SE)?\s*IF\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_ElseIf { get; }

        [GeneratedRegex(@"^\s*#ELSE", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_Else { get; }

        [GeneratedRegex(@"^\s*#END\s*IF", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_EndIf { get; }
        [GeneratedRegex(@"^\s*#SWITCH\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_Switch { get; }

        [GeneratedRegex(@"^\s*#SET\s*SWITCH\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_SetSwitch { get; }

        [GeneratedRegex(@"^\s*#END\s*SW(?:ITCH)?", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_EndSwitch { get; }

        [GeneratedRegex(@"^\s*#CASE\s*(\d+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_Case { get; }

        [GeneratedRegex(@"^\s*#SKIP", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_Skip { get; }

        [GeneratedRegex(@"^\s*#DEF(?:AULT)?(?!EXRANK)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_Default { get; }

        [GeneratedRegex(@"^\s*#WAV([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_WavDef { get; }

        [GeneratedRegex(@"^\s*#BMP([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_BmpDef { get; }

        [GeneratedRegex(@"^\s*#BGA([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_BgaDef { get; }

        [GeneratedRegex(@"^\s*#(?:EX)?BPM([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_BpmDef { get; }

        [GeneratedRegex(@"^\s*#STOP([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_StopDef { get; }

        [GeneratedRegex(@"^\s*#TEXT([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_TextDef { get; }

        [GeneratedRegex(@"^\s*#EXWAV([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_ExWavDef { get; }

        [GeneratedRegex(@"^\s*#EXBMP([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_ExBmpDef { get; }

        [GeneratedRegex(@"^\s*#@BGA([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_AtBgaDef { get; }

        [GeneratedRegex(@"^\s*#ARGB([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_ArgbDef { get; }

        [GeneratedRegex(@"^\s*#SWBGA([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_SwBgaDef { get; }

        [GeneratedRegex(@"^\s*#EXRANK([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_ExRankDef { get; }

        [GeneratedRegex(@"^\s*#CHANGEOPTION([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_OptionDef { get; }

        [GeneratedRegex(@"^\s*#SCROLL([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_ScrollDef { get; }

        [GeneratedRegex(@"^\s*#SPEED([0-9a-zA-Z]{2})[:\s](.+)", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex GR_SpeedDef { get; }

        [GeneratedRegex(@"^\s*#(\d\d\d)(02)[:\s](\d+(\.\d+)?)")]
        private static partial Regex GR_Bar { get; }

        [GeneratedRegex(@"^\s*#(\d\d\d)([0-9a-zA-Z]{2})[:\s]([0-9a-zA-Z]+)")]
        private static partial Regex GR_Channel { get; }

        [GeneratedRegex(@"^\s*#(\S+?)\s(.*)")]
        private static partial Regex GR_Header { get; }

        private static readonly (Command, Regex)[] CommandRegex =
        [
            // #RANDOM flow
            ( Command.Random, GR_Random ),
            ( Command.SetRandom, GR_SetRandom ),
            ( Command.EndRandom, GR_EndRandom ),
            ( Command.If, GR_If ),
            ( Command.ElseIf, GR_ElseIf ),
            ( Command.Else, GR_Else ),
            ( Command.EndIf, GR_EndIf ),
            // #SWITCH flow
            ( Command.Switch, GR_Switch ),
            ( Command.SetSwitch, GR_SetSwitch ),
            ( Command.EndSwitch, GR_EndSwitch ),
            ( Command.Case, GR_Case ),
            ( Command.Skip, GR_Skip ),
            ( Command.Default, GR_Default ),
            // Definitions
            ( Command.WavDef, GR_WavDef ),
            ( Command.BmpDef, GR_BmpDef ),
            ( Command.AtBgaDef, GR_AtBgaDef ),
            ( Command.BgaDef, GR_BgaDef ),
            ( Command.BpmDef, GR_BpmDef ),
            ( Command.StopDef, GR_StopDef ),
            ( Command.TextDef, GR_TextDef ),
            ( Command.ExWavDef, GR_ExWavDef ),
            ( Command.ExBmpDef, GR_ExBmpDef ),
            ( Command.ArgbDef, GR_ArgbDef ),
            ( Command.SwBgaDef, GR_SwBgaDef ),
            ( Command.ExRankDef, GR_ExRankDef ),
            ( Command.OptionDef, GR_OptionDef ),
            ( Command.ScrollDef, GR_ScrollDef ),
            ( Command.SpeedDef, GR_SpeedDef ),
            // Channel commands
            ( Command.Bar, GR_Bar ),
            ( Command.Channel, GR_Channel ),
            // Header info
            ( Command.Base, GR_Base ),
            ( Command.Header, GR_Header ),
        ];
    }
}
