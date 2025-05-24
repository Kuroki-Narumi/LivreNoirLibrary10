using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    internal partial class BmsTextReader
    {
        private readonly BmsData _root = new();
        private int _radix = Constants.Base_Default;
        private readonly string[] _lines;
        private readonly Dictionary<Command, Action<Match>> _actions;

        private readonly Stack<BaseData?> _data_stack = new();
        private readonly Stack<FlowContainer?> _flow_stack = new();
        private readonly Stack<FlowBranch?> _branch_stack = new();
        private BaseData? _current_data;
        private FlowContainer? _current_flow;
        private FlowBranch? _current_branch;

        private readonly StringBuilder _comment_builder = new();

        private static string[] ReadLines(Stream stream, Action<string>? actionForAllText = null)
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
            actionForAllText?.Invoke(text);
            return text.SplitLines();
        }

        public BmsTextReader(Stream stream)
        {
            _lines = ReadLines(stream, CheckRadix);
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
        }

        public BmsData Parse()
        {
            ParseInternal();
            return _root;
        }

        private void CheckRadix(string text)
        {
            var match = GR_Base.Match(text);
            if (match.Success)
            {
                _root.Base = _radix = ParseInt(match, 1);
            }
        }
        
        private static bool IsEmpty(string str) => GR_Empty.IsMatch(str);
        private static int ParseInt(Match match, int index) => int.TryParse(match.Groups[index].Value, out int value) ? value : 0;

        private void ParseInternal()
        {
            // initialize
            _data_stack.Clear();
            _flow_stack.Clear();
            _branch_stack.Clear();
            _current_data = _root;
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
            _root.ExtractRawData(null);
        }

        private void StartFlow(FlowContainer flow)
        {
            ForceEndFlow();

            _data_stack.Push(_current_data);
            _flow_stack.Push(_current_flow);
            _branch_stack.Push(_current_branch);

            _current_data = null;
            _current_flow = flow;
            _current_branch = null;

            FlushComment(s => flow.Note = s);
        }

        private void FlushComment(Action<string> action)
        {
            if (_comment_builder.Length != 0)
            {
                action(_comment_builder.ToString().Trim());
            }
            _comment_builder.Clear();
        }

        private void EndFlow()
        {
            if (_current_flow is not null)
            {
                _current_data = _data_stack.Pop();
                _current_flow = _flow_stack.Pop();
                _current_branch = _branch_stack.Pop();
            }
        }

        private void ForceEndFlow()
        {
            if (_current_data is null)
            {
                EndFlow();
            }
        }

        private void ApplyComment()
        {
            FlushComment(s => (_current_data ?? _root).Comments.Add(s));
        }

        private void AddComment(string text)
        {
            _comment_builder.AppendLine(text.Trim());
        }

        private void AddHeader(Match match)
        {
            ForceEndFlow();
            _current_data?.Headers.Add(match.Groups[1].Value, match.Groups[2].Value.Trim());
        }

        private void AddDef(DefType type, Match match)
        {
            ForceEndFlow();
            var index = BmsUtils.ToInt(match.Groups[1].Value, _radix);
            _current_data?.DefLists.Set(type, index, match.Groups[2].Value.Trim());
        }

        private void AddBar(Match match)
        {
            ForceEndFlow();
            _current_data?.SetRawData(ParseInt(match, 1), BmsUtils.ToChannel(match.Groups[2].Value), match.Groups[3].Value);
        }

        private void UpdateBranch(FlowBranch branch)
        {
            _current_branch = branch;
            _current_data = _root.GetFlowDataOrNull(branch.DataId);
            FlushComment(s => branch.Note = s);
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

        private void UpdateIfChild(FlowIfChild child)
        {
            _current_data = _root.GetFlowDataOrNull(child.DataId);
            FlushComment(s => child.Note = s);
        }

        public void Parse_ElseIf(Match match)
        {
            if (_current_branch is FlowIf b)
            {
                UpdateIfChild(_root.CreateElseIf(b, ParseInt(match, 1)));
            }
        }

        public void Parse_Else(Match match)
        {
            if (_current_branch is FlowIf b)
            {
                UpdateIfChild(_root.CreateElse(b));
            }
        }

        public void Parse_EndIf(Match match)
        {
            if (_current_branch is FlowIf)
            {
                _current_branch = null;
                _current_data = null;
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
                _current_data = null;
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
        public void Parse_Header(Match match) => AddHeader(match);

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
