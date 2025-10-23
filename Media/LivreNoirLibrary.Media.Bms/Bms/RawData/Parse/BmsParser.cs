using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsParser : IBmsParser
    {
        public ulong DenominatorLimit { get; set; } = Rational.DefaultConvertDenLimit;

        private int _radix;
        private long _lnObj;

        private readonly ParseState _root;
        private readonly StringBuilder _comments = new();
        private ParseState _current;

        private readonly List<ParseState> _states = [];
        private readonly Dictionary<DefType, Dictionary<long, decimal>> _conductor_defs = [];

        private readonly Stack<(ParseState, FlowContainer?)> _stack = [];
        private FlowContainer? _currentFlow;

        public BmsParser()
        {
            BmsData rootData = new();
            _current = _root = new(null, rootData);
        }

        public BmsData Parse(Stream stream)
        {
            IBmsParserExtensions.Parse(this, stream);
            return (_root.Data as BmsData)!;
        }

        void IBmsParser.InitializeParse(int radix, long lnObj)
        {
            _radix = radix;
            _lnObj = lnObj;

            _root.Data.Clear();
            _comments.Clear();
            _stack.Clear();
            _states.Clear();
            _current = _root;
            _currentFlow = null;

            if (lnObj is not 0)
            {
                _root.Data.LnObj = (int)lnObj;
            }
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
            foreach (var state in CollectionsMarshal.AsSpan(_states))
            {
                ResolveConductor(state);
            }
            ResolveConductor(_root);
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
            var headers = _current.Data.Headers;
            if (Enum.TryParse<HeaderType>(key, true, out var type))
            {
                if (type is HeaderType.Base or HeaderType.LnObj)
                {
                    return;
                }
                if (BmsUtils.IsNumberHeader(type))
                {
                    if (double.TryParse(value, out var result))
                    {
                        headers.Set(type, result);
                    }
                }
                else
                {
                    headers.Set(type, value.ToString());
                }
            }
            else
            {
                headers.SubHeaders.Add((key.ToString(), value.ToString()));
            }
        }

        void IBmsParser.AddDef(DefType type, long key, string value) => _current.Data.DefLists.Set(type, (int)key, value);
        void IBmsParser.AddConductorDef(DefType type, long key, decimal value) => _conductor_defs.GetOrAdd(type)[key] = value;

        void IBmsParser.AddBar(int number, Channel channel, ReadOnlySpan<char> value)
        {
            var current = _current;
            if (channel is Channel.Bar)
            {
                if (double.TryParse(value, out var v))
                {
                    current.Data.Bars.Set(number, Rational.ConvertBySBT(v, DenominatorLimit));
                }
            }
            else if (channel.IsConductor())
            {
                current.AddUnProcessedLine(number, channel, value.ToString());
            }
            else
            {
                var tl = current.Data.Timeline;
                Func<long, INote> noteCreator;
                if (channel is Channel.Bpm_Base)
                {
                    noteCreator = v => new ConductorNote(Channel.Bpm, v);
                }
                else if (channel is Channel.Bgm)
                {
                    var lane = current.UpdateBgmLane(number);
                    noteCreator = v => new SoundNote(lane, v, NoteType.Normal);
                }
                else if (BmsUtils.IsSoundChannel(channel))
                {
                    var type = channel.GetNoteType();
                    var lane = channel.GetLane();
                    if (channel.IsLong())
                    {
                        noteCreator = v =>
                        {
                            if (_current.LastLongNotes.Remove(lane))
                            {
                                return new SoundNote(lane, v, NoteType.LongEnd);
                            }
                            else
                            {
                                _current.LastLongNotes.Add(lane);
                                return new SoundNote(lane, v);
                            }
                        };
                    }
                    else if (channel.IsVisible())
                    {
                        noteCreator = v => v == _lnObj ? new SoundNote(lane, 0, NoteType.LongEnd) : new SoundNote(lane, v);
                    }
                    else
                    {
                        noteCreator = v => new SoundNote(lane, v, type);
                    }
                }
                else
                {
                    noteCreator = v => new MetaNote(channel, v);
                }
                var radix = channel.IsHex() ? 16 : _radix;
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
            var defs = _conductor_defs;
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
                foreach (var (number, line) in CollectionsMarshal.AsSpan(list))
                {
                    var span = line.AsSpan();
                    var den = span.Length / 2;
                    for (var i = 0; i < den; i++)
                    {
                        if (BasedNumber.TryParseToLong(span[..2], radix, out var key) && def.TryGetValue(key, out var value))
                        {
                            tl.Add(new(number, i, den), new ConductorNote(channel, value));
                        }
                        span = span[2..];
                    }
                }
            }
        }

        private void StartFlow(FlowType type, int max, bool isFixed)
        {
            FlowContainer flow = new(type, max, isFixed, _current.Data);
            if (_comments.Length is > 0)
            {
                flow.Note = _comments.ToString();
                _comments.Clear();
            }
            _current.Data.Flows.Add(flow);
            _currentFlow = flow;
        }

        private void EndFlow()
        {
            EndBranch();
            _currentFlow = null;
        }

        private void StartBranch(int value)
        {
            if (_currentFlow is null)
            {
                EndBranch();
            }
            _stack.Push((_current, _currentFlow));
            var branch = _currentFlow?.GetOrCreateBranch(value) ?? new(_current.Data, value);
            _current = new(_current, branch);
            if (_currentFlow is not null)
            {
                _states.Add(_current);
            }
            _currentFlow = null;
        }

        private void EndBranch()
        {
            if (_current.Data is FlowBranch && _stack.TryPop(out var item))
            {
                _current = item.Item1;
                _currentFlow = item.Item2;
            }
        }

        void IBmsParser.StartRandom(int value) => StartFlow(FlowType.Random, value, false);
        void IBmsParser.StartSetRandom(int value) => StartFlow(FlowType.Random, value, true);
        void IBmsParser.StartIf(int value) => StartBranch(value);
        void IBmsParser.StartElseIf(int value) => StartBranch(value);
        void IBmsParser.StartElse() => StartBranch(Constants.DefaultCondition);
        void IBmsParser.EndIf() => EndBranch();
        void IBmsParser.EndRandom() => EndFlow();

        void IBmsParser.StartSwitch(int value) => StartFlow(FlowType.Switch, value, false);
        void IBmsParser.StartSetSwitch(int value) => StartFlow(FlowType.Switch, value, true);
        void IBmsParser.StartCase(int value) => StartBranch(value);
        void IBmsParser.StartDefault() => StartBranch(Constants.DefaultCondition);
        void IBmsParser.Skip() => EndBranch();
        void IBmsParser.EndSwitch() => EndFlow();
    }
}
