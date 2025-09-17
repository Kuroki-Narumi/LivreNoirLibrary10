using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class SelectionOptions : IndexesOptionsBase
    {
        private readonly SortedSet<int> _lanes = [];
        private readonly SortedSet<Channel> _channels = [];

        public ConvertTarget Target { get; set => SetValue(ref field, value); } = new();
        public int BarLower { get; set => SetValue(ref field, value); }
        public int BarUpper { get; set => SetValue(ref field, value); } = Constants.MaxBarNumber;
        public bool BarExclusive { get; set => SetValue(ref field, value); }

        public SortedSet<int> Lanes
        {
            get => _lanes;
            set
            {
                _lanes.Clear();
                _lanes.UnionWith(value);
                SendPropertyChanged();
            }
        }

        public SortedSet<Channel> Channels
        {
            get => _channels;
            set
            {
                _channels.Clear();
                _channels.UnionWith(value);
                SendPropertyChanged();
            }
        }

        public bool NoteType_Normal { get; set => SetValue(ref field, value); }
        public bool NoteType_Invisible { get; set => SetValue(ref field, value); }
        public bool NoteType_LongEnd { get; set => SetValue(ref field, value); }
        public bool NoteType_Mine { get; set => SetValue(ref field, value); }

        public SelectionProcessType ProcessType
        {
            get;
            set => SetValue(ref field, value, [nameof(ProcessType_Select), nameof(ProcessType_Delete), nameof(ProcessType_Edit), nameof(ProcessType_Move), nameof(ProcessType_Quantize)]);
        }
        [JsonIgnore]
        public bool ProcessType_Select { get => ProcessType is SelectionProcessType.Select; set => SetProcessType(SelectionProcessType.Select, value); }
        [JsonIgnore]
        public bool ProcessType_Delete { get => ProcessType is SelectionProcessType.Delete; set => SetProcessType(SelectionProcessType.Delete, value); }
        [JsonIgnore]
        public bool ProcessType_Edit { get => ProcessType is SelectionProcessType.Edit; set => SetProcessType(SelectionProcessType.Edit, value); } 
        [JsonIgnore]
        public bool ProcessType_Move { get => ProcessType is SelectionProcessType.Move; set => SetProcessType(SelectionProcessType.Move, value); } 
        [JsonIgnore]
        public bool ProcessType_Quantize { get => ProcessType is SelectionProcessType.Quantize; set => SetProcessType(SelectionProcessType.Quantize, value); }

        public Rational ReplaceValue { get; set => SetValue(ref field, value); }
        public ValueOperationMode ReplaceMode { get; set => SetValue(ref field, value); }

        public Rational MoveValue { get; set => SetValue(ref field, value); }
        public ValueOperationMode MoveMode { get; set => SetValue(ref field, value); } = ValueOperationMode.Add;
        public Rational QuantizeValue { get; set => SetValue(ref field, value); } = new(1, 192);

        private bool _index_skip;
        private bool _lane_skip;
        private bool _type_skip;
        private readonly SortedSet<NoteType> _note_types = [];

        public void Prepare()
        {
            _index_skip = _indexes.Count is 0;
            _lane_skip = (_lanes.Count + _channels.Count) is 0;
            var t = _note_types;
            t.Clear();
            if (NoteType_Normal)
            {
                t.Add(NoteType.Normal);
            }
            if (NoteType_Invisible)
            {
                t.Add(NoteType.Invisible);
            }
            if (NoteType_LongEnd)
            {
                t.Add(NoteType.LongEnd);
            }
            if (NoteType_Mine)
            {
                t.Add(NoteType.Mine);
            }
            _type_skip = t.Count is 0;
        }

        public bool IsMatch(BarPosition position, INote note)
        {
            var bar = position.Bar;
            return
                !(BarExclusive ^ (bar < BarLower || bar > BarUpper)) &&
                (_index_skip || note.IsDef(out var n) && _indexes.Contains(n.Value)) &&
                (_lane_skip || note is IChannelNote c && _channels.Contains(c.Channel) || note is ISoundNote ss && _lanes.Contains(ss.Lane)) &&
                (_type_skip || _note_types.Contains(note is ISoundNote s ? s.Type : NoteType.Normal));
        }

        private void SetProcessType(SelectionProcessType type, bool value)
        {
            if (value)
            {
                ProcessType = type;
            }
        }

        public void RotateProcessType(int delta)
        {
            ProcessType = delta is > 0
                ? ProcessType switch // upward
                {
                    SelectionProcessType.Select => SelectionProcessType.Quantize,
                    SelectionProcessType.Delete => SelectionProcessType.Select,
                    SelectionProcessType.Edit => SelectionProcessType.Delete,
                    SelectionProcessType.Move => SelectionProcessType.Edit,
                    SelectionProcessType.Quantize => SelectionProcessType.Move,
                    _ => SelectionProcessType.Select,
                }
                : ProcessType switch // downward
                {
                    SelectionProcessType.Select => SelectionProcessType.Delete,
                    SelectionProcessType.Delete => SelectionProcessType.Edit,
                    SelectionProcessType.Edit => SelectionProcessType.Move,
                    SelectionProcessType.Move => SelectionProcessType.Quantize,
                    SelectionProcessType.Quantize => SelectionProcessType.Select,
                    _ => SelectionProcessType.Select,
                };
        }

        public string GetReplaceText(int radix) => GetReplaceText(ReplaceMode, ReplaceValue, radix);

        public bool TrySetReplace(string? text, int radix)
        {
            if (TryParseReplaceText(text, radix, out var mode, out var value))
            {
                ReplaceMode = mode;
                ReplaceValue = value;
                return true;
            }
            return false;
        }

        public string GetMoveText() => ValueOperation.GetText(MoveMode, MoveValue);

        public bool TrySetMove(string? text)
        {
            if (ValueOperation.TryParse(text, out var mode, out var value))
            {
                MoveMode = mode;
                MoveValue = value;
                return true;
            }
            return false;
        }

        public static string GetReplaceText(ValueOperationMode mode, Rational value, int radix)
        {
            if (mode is ValueOperationMode.Set)
            {
                return BmsUtils.ToBased((int)value, radix);
            }
            else
            {
                return $"{ValueOperation.GetText(mode)}{value}";
            }
        }

        public static bool TryParseReplaceText(string? text, int radix, out ValueOperationMode mode, out Rational value)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var match = GR_Replace.Match(text);
                var op = match.Groups["op"];
                if (op.Success)
                {
                    mode = ValueOperation.GetMode(op.Value);
                    var val = match.Groups["val"];
                    if (Rational.TryParse(val.Value, out value))
                    {
                        return true;
                    }
                }
                else
                {
                    var id = match.Groups["id"];
                    if (id.Success && BasedNumber.TryParseToInt(id.Value, radix, out var intVal) && intVal is > 0 && intVal < radix * radix)
                    {
                        mode = ValueOperationMode.Set;
                        value = intVal;
                        return true;
                    }
                }
            }
            mode = default;
            value = default;
            return false;
        }

        [GeneratedRegex(@"(?:(?:(?<op>[+\-/*%<>])(?<val>\d+(?:[,./]\d+)?)?)|(?<id>[0-9A-Za-z]+))")]
        private static partial Regex GR_Replace { get; }
    }
}
