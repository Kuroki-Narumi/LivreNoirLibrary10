using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteRectBase : SelectableObject
    {
        public bool IsVisibleLane { get; protected set; } = true;
        public double AbsolutePosition { get; protected set; }
        public double NoteLength { get; protected set; }

        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Length { get; protected set; }
        public Rect Rect { get; private set; }

        public void UpdateHorizontal(double x, double width)
        {
            X = x.RoundToInt();
            Width = width.RoundToInt();
            UpdateRect();
        }

        public void UpdateVertical(int headHeight, double bottom, double scaleY)
        {
            var len = NoteLength;
            var visualLength =  len is 0 ? 0 : (len * scaleY).RoundToInt();
            var height = headHeight + visualLength;
            Y = (bottom - AbsolutePosition * scaleY).RoundToInt() - height;
            Height = height;
            Length = visualLength;
            UpdateRect();
        }

        public void UpdateRect() => Rect = new(X, Y, Width, Height);

        public virtual bool Contains(in Point point) => IsVisibleLane && Rect.Contains(point);
        public virtual bool Intersects(in Rect rect) => IsVisibleLane && Rect.IntersectsWith(rect);

        protected void DrawHead(DrawingContext ctx, int y, int headHeight, Color color) => ctx.DrawNoteRect(X + 1, Y, Width - 1, headHeight, color);
        protected void DrawSelectedHead(DrawingContext ctx, int y, int headHeight, Color color) => ctx.DrawSelectedNoteRect(X + 1, Y, Width - 1, headHeight, color);
        protected void DrawText(DrawingContext ctx, int y, string? text) => ctx.DrawCachedText(X + 2, y, text);
        protected void DrawText(DrawingContext ctx, int y, string? text, Color color) => ctx.DrawCachedText(X + 2, y, text, color);
    }
}
