using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsParser(IBmsData root) : IBmsParser
    {
        private int _radix;
        private long _lnObj;

        private readonly IBmsData _root = root;
        private readonly StringBuilder _comments = new();
        private ParseState _current = null!;

        private readonly List<ParseState> _states = [];
        private readonly Dictionary<DefType, Dictionary<long, double>> _conductorDefs = [];

        private readonly Stack<(ParseState, IFlowContainer?)> _stack = [];
        private IFlowContainer? _currentFlow;
        private bool _insideBranch;

        void IBmsParser.InitializeParse(int radix, long lnObj)
        {
            _radix = radix;
            _lnObj = lnObj;

            _root.Clear();
            _root.LnObj = (int)lnObj;
            _comments.Clear();
            _stack.Clear();
            _states.Clear();
            _current = new(_root.Root);
            _states.Add(_current);
            _currentFlow = null;
            _insideBranch = false;
        }

        void IBmsParser.OnLineProcessed(int lineNumber)
        {
            var comments = _comments;
            if (comments.Length is > 0)
            {
                _current.Comments.Add(comments.ToString());
                comments.Clear();
            }
        }

        void IBmsParser.FinalizeParse()
        {
            foreach (var state in _states.AsSpan())
            {
                ResolveConductor(state);
            }
        }

        void IBmsParser.AddComment(ReadOnlySpan<char> line)
        {
            var s = line.ToString();
            if (string.IsNullOrWhiteSpace(s))
            {
                _comments.AppendLine(s);
            }
        }

        void IBmsParser.AddHeader(ReadOnlySpan<char> key, ReadOnlySpan<char> value)
        {
            var data = _current.Data;
            if (Enum.TryParse<HeaderType>(key, true, out var type))
            {
                if (type is HeaderType.Base or HeaderType.LnObj)
                {
                    return;
                }
                data.MainHeaders[type] = value.ToString();
            }
            else
            {
                data.SubHeaders.Add(new(key.ToString(), value.ToString()));
            }
        }

        void IBmsParser.AddDef(DefType type, long key, string value) => _current.Data.DefLists.Set(type, (int)key, value);
        void IBmsParser.AddConductorDef(DefType type, long key, double value) => _conductorDefs.GetOrAdd(type)[key] = value;

        void IBmsParser.AddBar(int number, Channel channel, ReadOnlySpan<char> value)
        {
            var current = _current;
            if (channel is Channel.Bar)
            {
                if (double.TryParse(value, out var v))
                {
                    current.Data.BarDefs.Set(number, v);
                }
            }
            else if (channel.IsConductor())
            {
                current.AddUnProcessedLine(number, channel, value.ToString());
            }
            else
            {
                var tl = current.Data.Timeline;
                Func<long, Note> noteCreator;
                if (channel is Channel.Bpm_Base)
                {
                    noteCreator = v => new(Channel.Bpm, v);
                }
                else if (channel is Channel.Bgm)
                {
                    channel = current.UpdateBgmLane(number);
                    noteCreator = v => new(channel, v);
                }
                else if (channel.IsWavDef())
                {
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
                        },
                        NoteType.Normal => v => v == _lnObj ? new(lane, NoteType.LongEnd, 0) : new(lane, v),
                        _ => v => new(lane, type, v),
                    };
                }
                else
                {
                    noteCreator = v => new(channel, v);
                }
                var radix = channel.IsHexValue() ? 16 : _radix;
                var den = value.Length / 2;
                for (var i = 0; i < den; i++)
                {
                    if (BasedNumber.TryParseToLong(value[..2], radix, out var v) && v is not 0)
                    {
                        tl.Add(new(number, i, den), noteCreator(v));
                    }
                    value = value[2..];
                }
            }
        }

        private void ResolveConductor(ParseState state)
        {
            state.Data.Note = string.Join(Environment.NewLine, state.Comments);
            var radix = _radix;
            var tl = state.Data.Timeline;
            var defs = _conductorDefs;
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
                        if (BasedNumber.TryParseToLong(span[..2], radix, out var key) && def.TryGetValue(key, out var value))
                        {
                            tl.Add(new(number, i, den), new Note(channel, value));
                        }
                        span = span[2..];
                    }
                }
            }
        }

        private void ApplyComment(INoteObject obj)
        {
            if (_comments.Length is > 0)
            {
                obj.Note = _comments.ToString();
                _comments.Clear();
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
            var data = _current.Data;
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
                _stack.Push((_current, flow));
                var branch = flow.GetOrAddBranch(value);
                var data = _root.GetBranchData(branch);
                ApplyComment(branch);
                _current = new(data);
                _states.Add(_current);
                _currentFlow = null;
                _insideBranch = true;
            }
        }

        private void EndBranch()
        {
            if (_insideBranch && _stack.TryPop(out var item))
            {
                (_current, _currentFlow) = item;
            }
        }

        void IBmsParser.StartRandom(int value) => StartFlow(FlowType.Random, value, false);
        void IBmsParser.StartSetRandom(int value) => StartFlow(FlowType.Random, value, true);
        void IBmsParser.StartIf(int value) => StartBranch(value);
        void IBmsParser.StartElseIf(int value) => StartBranch(value);
        void IBmsParser.StartElse() => StartBranch(BmsConstants.DefaultCondition);
        void IBmsParser.EndIf() => EndBranch();
        void IBmsParser.EndRandom() => EndFlow();

        void IBmsParser.StartSwitch(int value) => StartFlow(FlowType.Switch, value, false);
        void IBmsParser.StartSetSwitch(int value) => StartFlow(FlowType.Switch, value, true);
        void IBmsParser.StartCase(int value) => StartBranch(value);
        void IBmsParser.StartDefault() => StartBranch(BmsConstants.DefaultCondition);
        void IBmsParser.Skip() => EndBranch();
        void IBmsParser.EndSwitch() => EndFlow();
    }
}
