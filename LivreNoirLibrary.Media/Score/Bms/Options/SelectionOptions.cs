using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class SelectionOptions : IndexesOptionsBase
    {
        [ObservableProperty]
        private ConvertTarget _target = new();
        [ObservableProperty]
        private int _barLower = 0;
        [ObservableProperty]
        private int _barUpper = Constants.MaxBarNumber;
        [ObservableProperty]
        private bool _barExclusive = false;

        private readonly SortedSet<int> _lanes = [];
        [ObservableProperty]
        private bool _noteType_Normal;
        [ObservableProperty]
        private bool _noteType_Invisible;
        [ObservableProperty]
        private bool _noteType_LongEnd;
        [ObservableProperty]
        private bool _noteType_Mine;
        [ObservableProperty(Related = [nameof(ProcessType_Select), nameof(ProcessType_Delete), nameof(ProcessType_Edit), nameof(ProcessType_Move), nameof(ProcessType_Quantize)])]
        private SelectionProcessType _processType;
        [ObservableProperty]
        private Rational _replaceValue;
        [ObservableProperty]
        private ValueOperationMode _replaceMode;
        [ObservableProperty]
        private Rational _moveValue;
        [ObservableProperty]
        private ValueOperationMode _moveMode = ValueOperationMode.Add;
        [ObservableProperty]
        private Rational _quantizeValue = new(1, 192);

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

        [JsonIgnore]
        public bool ProcessType_Select { get => _processType is SelectionProcessType.Select; set => SetProcessType(SelectionProcessType.Select, value); }
        [JsonIgnore]
        public bool ProcessType_Delete { get => _processType is SelectionProcessType.Delete; set => SetProcessType(SelectionProcessType.Delete, value); }
        [JsonIgnore]
        public bool ProcessType_Edit { get => _processType is SelectionProcessType.Edit; set => SetProcessType(SelectionProcessType.Edit, value); } 
        [JsonIgnore]
        public bool ProcessType_Move { get => _processType is SelectionProcessType.Move; set => SetProcessType(SelectionProcessType.Move, value); } 
        [JsonIgnore]
        public bool ProcessType_Quantize { get => _processType is SelectionProcessType.Quantize; set => SetProcessType(SelectionProcessType.Quantize, value); }

        private bool _includeLongEnd;
        private bool _index_enabled;
        private bool _lane_enabled;
        private bool _type_enabled;
        private readonly SortedSet<NoteType> _note_types = [];

        public void Prepare(bool includeLongEnd)
        {
            _includeLongEnd = includeLongEnd;
            _index_enabled = _indexes.Count is > 0;
            _lane_enabled = _lanes.Count is > 0;
            var t = _note_types;
            t.Clear();
            if (_noteType_Normal)
            {
                t.Add(NoteType.Normal);
                t.Add(NoteType.Decimal);
                t.Add(NoteType.Rational);
            }
            if (_noteType_Invisible)
            {
                t.Add(NoteType.Invisible);
            }
            if (_noteType_LongEnd)
            {
                t.Add(NoteType.LongEnd);
            }
            if (_noteType_Mine)
            {
                t.Add(NoteType.Mine);
            }
            _type_enabled = t.Count is > 0;
        }

        public bool IsMatch(BarPosition position, Note note)
        {
            var bar = position.Bar;
            if (_barExclusive ^ (bar < _barLower || bar > _barUpper))
            {
                return false;
            }
            if (_index_enabled && note.IsIndex(_includeLongEnd) && !_indexes.Contains(note.Id))
            {
                return false;
            }
            if (_lane_enabled && !_lanes.Contains(note.Lane))
            {
                return false;
            }
            if (_type_enabled && !_note_types.Contains(note.Type))
            {
                return false;
            }
            return true;
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
                ? _processType switch // upward
                {
                    SelectionProcessType.Select => SelectionProcessType.Quantize,
                    SelectionProcessType.Delete => SelectionProcessType.Select,
                    SelectionProcessType.Edit => SelectionProcessType.Delete,
                    SelectionProcessType.Move => SelectionProcessType.Edit,
                    SelectionProcessType.Quantize => SelectionProcessType.Move,
                    _ => SelectionProcessType.Select,
                }
                : _processType switch // downward
                {
                    SelectionProcessType.Select => SelectionProcessType.Delete,
                    SelectionProcessType.Delete => SelectionProcessType.Edit,
                    SelectionProcessType.Edit => SelectionProcessType.Move,
                    SelectionProcessType.Move => SelectionProcessType.Quantize,
                    SelectionProcessType.Quantize => SelectionProcessType.Select,
                    _ => SelectionProcessType.Select,
                };
        }

        public string GetReplaceText(int radix) => GetReplaceText( _replaceMode, _replaceValue, radix);

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

        public string GetMoveText() => ValueOperation.GetText(_moveMode, _moveValue);

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
                    if (id.Success && BasedIndex.TryParseToInt(id.Value, radix, out var intVal) && intVal is > 0 && intVal < radix * radix)
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
