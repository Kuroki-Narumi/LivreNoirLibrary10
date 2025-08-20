using System;
using System.Drawing;
using System.Text;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteViewModel : ISelectableObject, INoteWrapper
    {
        internal event Action<NoteViewModel, bool>? IsSelectedChanged;

        private string _to_string;
        private bool _selected;
        internal protected string _text;

        public Note Note { get; }
        public int Lane { get; set; }
        public BarPosition Position { get; set; }
        public Rational ActualPosition { get; set; }
        public Rational Length { get; set; }

        public bool HasProblem { get; set; }
        public bool IsConductor { get; set; }

        public string IndexText => _text;
        public string BarText => Position.Bar.GetBarText();
        public string BeatText => Position.Offset.ToString();
        public string LaneText => Lane.GetLaneName();

        public bool IsSelected
        {
            get => _selected;
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    IsSelectedChanged?.Invoke(this, value);
                }
            }
        }

        public NoteViewModel(BarPosition position, Rational actualPos, int lane, string text)
        {
            Note = new(NoteType.Invalid, lane);
            Position = position;
            ActualPosition = actualPos;
            Lane = lane;
            _text = text;
            _to_string = $"{{{position} Lane:{lane} Index:{text}}}";
        }

        public NoteViewModel(BarPosition position, Rational actualPos, Note note, int radix)
        {
            Note = note;
            Position = position;
            ActualPosition = actualPos;
            Lane = note.Lane;

            StringBuilder sb = new();
            sb.Append('{');
            sb.Append(position.ToString());
            sb.Append(' ');
            if (note.IsDecimal())
            {
                IsConductor = true;
                _text = ((decimal)note.Value).ToString();
                if (note.IsTempo())
                {
                    sb.Append($"Bpm:{_text}");
                }
                else if (note.IsScroll())
                {
                    sb.Append($"Scroll:{_text}");
                }
                else if (note.IsSpeed())
                {
                    sb.Append($"Speed:{_text}");
                }
            }
            else if (note.IsRational())
            {
                IsConductor = true;
                _text = note.Value.ToString();
                sb.Append($"Stop:{_text}");
            }
            else
            {
                IsConductor = false;
                if (note.IsSound())
                {
                    if (note.IsMine())
                    {
                        _text = $"{note.Id}%";
                    }
                    else
                    {
                        _text = BmsUtils.ToBased(note.Id, radix);
                    }

                    var valueText =
                        note.IsInvisible() ? $"{_text}(Invisible)" :
                        note.IsMine() ? $"{_text}(Mine)" :
                        note.IsLongEnd() ? $"{_text}(LongEnd)" :
                        _text;
                    sb.Append($"Lane:{note.Lane} Index:{valueText}");
                }
                else if (note.IsIndex(true))
                {
                    _text = BmsUtils.ToBased(note.Id, radix);
                    sb.Append($"Channel:{BmsUtils.GetMetaChannel(note.Lane)} Index:{_text}");
                }
                else
                {
                    _text = note.Id.ToString();
                    sb.Append($"Channel:{BmsUtils.GetMetaChannel(note.Lane)} Value:{_text}");
                }
            }
            sb.Append('}');
            _to_string = sb.ToString();
        }

        public void SetLongEnd(Rational position)
        {
            Length = position - ActualPosition;
            if (Length.IsPositiveThanZero())
            {
                _to_string = $"{{{Position} Lane:{Lane} Index:{_text} Length:{Length}}}";
            }
        }

        public override string ToString() => _to_string;
    }
}
