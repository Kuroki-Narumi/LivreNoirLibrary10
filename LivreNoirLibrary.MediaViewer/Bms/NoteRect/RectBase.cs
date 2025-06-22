using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class RectBase : SelectableObject
    {
        public const int HeadHeight = 10;

        internal protected string? _text;
        protected int _x;
        protected int _y;
        protected int _width;
        protected int _height;
        protected int _length;
        protected Rect _rect;

        public int Lane { get; protected set; }
        public BarPosition Position { get; protected set; }
        public Rational ActualPosition { get; protected set; }
        public Rational Length { get; protected set; }
        public Rect Rect => _rect;

        protected RectBase(int lane, BarPosition position, Rational actualPos)
        {
            Lane = lane;
            Position = position;
            ActualPosition = actualPos;
            _height = HeadHeight;
            UpdateRect();
        }

        public void UpdateRect()
        {
            _rect = new(_x, _y, _width, _height);
        }

        public void SetHorizontal(double x, int width)
        {
            _x = x.RoundToInt();
            _width = width;
            UpdateRect();
        }

        public void SetVertical(double bottom, double scaleY)
        {
            if (Length.IsZero())
            {
                _length = 0;
            }
            else
            {
                _length = (Length * scaleY).RoundToInt();
            }
            _height = HeadHeight + _length;
            bottom -= ActualPosition * scaleY;
            _y = bottom.RoundToInt() - _height;
            UpdateRect();
        }

        public virtual bool Contains(in Point point) => _rect.Contains(point);
        public virtual bool Intersects(in Rect rect) => _rect.IntersectsWith(rect);

        protected void RenderHead(DrawingContext ctx, int y, Color color)
        {
            ctx.DrawNoteRect(_x, y, _width, HeadHeight, color);
        }

        protected void RenderSelectedHead(DrawingContext ctx, int y, Color color)
        {
            ctx.DrawSelectedNoteRect(_x, y, _width, HeadHeight, color);
        }

        protected void RenderText(DrawingContext ctx, int y, string? text)
        {
            ctx.DrawNoteText(_x + 2, y, text);
        }
    }
}
