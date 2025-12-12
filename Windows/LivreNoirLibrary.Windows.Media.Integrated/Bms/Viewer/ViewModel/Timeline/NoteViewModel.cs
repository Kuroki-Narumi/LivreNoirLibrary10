using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public sealed class NoteViewModel : SelectableObject
    {
        public static NoteViewModel Dummy { get; } = new();

        private string _valueText = "";
        private string _fullText = "";
        private int _originalLaneIndex;
        private readonly double _originalPosition;
        private Rect _rect;

        public Note? Note { get; }
        public int LaneIndex { get; private set; }
        public BarPosition Position { get; }
        public double HeadPosition { get; }
        public double AbsolutePosition { get; private set; }
        public double Time { get; private set; }
        public double Length { get; private set; }

        public bool HasProblem { get; set; }
        public bool IsVisibleLane { get; set; }
        public double X { get; set; }
        public double Width { get; set; }
        public Color Color { get; set; }
        public Color LongBody { get; set; }

        public string BarText => Position.Bar.GetBarText();
        public string OffsetText => Position.Offset.ToString();
        public string? LaneText => Note?.GetLaneText();
        public string ValueText => _valueText;

        private NoteViewModel()
        {
            _valueText = "(dummy)";
            _fullText = "(dummy)";
        }

        public NoteViewModel(BarPosition position, double headPosition, double absolutePosition, double time, Note note, int radix)
        {
            Position = position;
            HeadPosition = headPosition;
            AbsolutePosition = _originalPosition = absolutePosition;
            Time = time;
            Note = note;
            UpdateValueText(radix);
        }

        public void UpdateValueText(int radix)
        {
            if (Note is not { } note)
            {
                _fullText = $"{{{Position}}}";
                return;
            }
            var ch = note.Channel;
            var value = note.Value;
            var laneText = ch.GetChannelName();
            StringBuilder sb = new($"{{{Position} ");
            var valueText = value.ToString();
            _valueText = valueText;
            if (ch.IsDefValue())
            {
                _valueText = BmsUtils.ToBased((int)note.Value, radix);
            }
            if (note.IsSoundLane())
            {
                switch (note.Type)
                {
                    case NoteType.Invisible:
                        valueText = $"{valueText}(Invisible)";
                        break;
                    case NoteType.LongEnd:
                        valueText = $"{valueText}(Long End)";
                        break;
                    case NoteType.Mine:
                        _valueText = $"{note.Value}%";
                        valueText = $"{valueText}(Mine)";
                        break;
                }
            }
            sb.Append($"{laneText} Value={valueText}");
            if (Length is > 0)
            {
                sb.Append($" Length={Length}");
            }
            sb.Append('}');
            _fullText = sb.ToString();
        }

        public void SetLongEnd(double position)
        {
            Length = position - AbsolutePosition;
            _fullText = $"{{{Position} Value={_valueText} Length={Length}}}";
        }

        /// <returns>true if this note is visible; otherwise, false.</returns>
        public bool UpdateVisualParameters(LaneIndexMap map)
        {
            var visible = false;
            Color noteColor = default;
            Color longColor = default;
            if (map.TryGetLaneInfo(Note, out var index, out var x, out var width, out var info))
            {
                visible = true;
                noteColor = info.NoteColor;
                longColor = info.LongColor;
            }
            IsVisibleLane = visible;
            LaneIndex = _originalLaneIndex = index;
            X = x;
            Width = width;
            Color = noteColor;
            LongBody = longColor;
            _rect = new(x, AbsolutePosition, width, Length);
            return visible;
        }

        public void SetOffsetY(double offsetY)
        {
            AbsolutePosition = _originalPosition + offsetY;
        }

        public void SetOffsetX(LaneIndexMap map, int offset)
        {

        }

        public override string ToString() => _fullText;

        public bool Intersects(in Rect rect) => rect.IntersectsWith(_rect);
    }
}
