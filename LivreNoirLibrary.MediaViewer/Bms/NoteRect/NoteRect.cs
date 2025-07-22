using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteRect : RectBase, INoteWrapper
    {
        public NoteViewModel ViewModel { get; }
        public bool IsValid { get; internal set; }
        public Color Color { get; set; }
        public Color LongBody { get; set; }

        public Note Note => ViewModel.Note;

        public string? ValueText
        {
            get
            {
                var note = ViewModel.Note;
                if (note.IsInvisible())
                {
                    return $"{_text}(Invisible)";
                }
                else if (note.IsMine())
                {
                    return $"{_text}(Mine)";
                }
                else if (note.IsLongEnd())
                {
                    return $"{_text}(LongEnd)";
                }
                else
                {
                    return _text;
                }
            }
        }

        public NoteRect(NoteViewModel viewModel) : base(viewModel.Lane, viewModel.Position, viewModel.ActualPosition)
        {
            ViewModel = viewModel;
            _text = viewModel.IndexText;
        }

        public override string ToString() => ViewModel.ToString();

        public override bool Contains(in Point point) => IsValid && base.Contains(point);
        public override bool Intersects(in Rect rect) => IsValid && base.Intersects(rect);

        public void Render(DrawingContext ctx, Color mine, Color longEnd, Color selected, Color selectedLong, Color invalid)
        {
            var note = Note;
            var x = _x;
            var y = _y;
            var w = _width;
            var length = _length;
            var color = note.IsMine() ? mine
                : note.IsLongEnd() ? longEnd
                : Color;
            var invisible = note.IsInvisible();
            if (invisible)
            {
                ctx.PushOpacity(0.5);
            }
            if (note.IsKey() && length is > HeadHeight)
            {
                Rect rect = new(x + 2, y + HeadHeight, w - 4, length - HeadHeight);
                ctx.DrawRectangle(MediaUtils.GetBrush(IsSelected ? selectedLong : LongBody), null, rect);
            }
            y += length;
            if (IsSelected)
            {
                RenderSelectedHead(ctx, y, selected);
            }
            else
            {
                RenderHead(ctx, y, color);
            }
            RenderText(ctx, y, _text);
            if (invisible)
            {
                ctx.Pop();
            }
            if (ViewModel.HasProblem)
            {
                ctx.DrawRectangle(null, MediaUtils.GetPen(invalid, 2), new(x, y, w, HeadHeight));
                DrawIcon(ctx, x - 16, y - 8, Icons.Caution);
            }
        }

        private static void DrawIcon(DrawingContext ctx, double x, double y, Drawing icon)
        {
            Matrix mat = new();
            mat.Scale(0.5, 0.5);
            mat.Translate(x, y);
            ctx.PushTransform(new MatrixTransform(mat));
            ctx.DrawDrawing(icon);
            ctx.Pop();
        }

        public double GetOffset(double y) => y - _y - _length - HeadHeight / 2;
    }
}
