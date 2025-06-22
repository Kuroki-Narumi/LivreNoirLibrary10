using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public sealed class SelectedRect : RectBase
    {
        public NoteRect Reference { get; }
        public int InitialLane { get; }
        public bool IsConductor => Reference.ViewModel.IsConductor;

        private readonly Rational _initial_position;
        private readonly bool _invisible;
        private readonly bool _key;

        public SelectedRect(NoteRect reference) : base(reference.Lane, reference.Position, reference.ActualPosition)
        {
            var note = reference.Note;
            Reference = reference;
            InitialLane = note.Lane;
            _initial_position = reference.ActualPosition;
            _text = reference._text;
            _invisible = note.IsInvisible();
            _key = note.IsKey();
            Length = reference.Length;
        }

        public void SetOffsetX(int newLane, double x, int width)
        {
            Lane = newLane;
            SetHorizontal(x, width);
        }

        public void SetOffsetY(Rational offset, double bottom, double scaleY)
        {
            ActualPosition = _initial_position + offset;
            SetVertical(bottom, scaleY);
        }

        public void Render(DrawingContext ctx, Color color, Color longBody)
        {
            var w = _width;
            if (w is <= 0)
            {
                return;
            }
            if (_invisible)
            {
                ctx.PushOpacity(0.5);
            }
            var x = _x;
            var y = _y;
            var length = _length;
            if (_key && length is > HeadHeight)
            {
                Rect rect = new(x + 2, y + HeadHeight, w - 4, length - HeadHeight);
                ctx.DrawRectangle(MediaUtils.GetBrush(longBody), null, rect);
            }
            y += length;
            RenderSelectedHead(ctx, y, color);
            RenderText(ctx, y, _text);
            if (_invisible)
            {
                ctx.Pop();
            }
        }
    }
}
