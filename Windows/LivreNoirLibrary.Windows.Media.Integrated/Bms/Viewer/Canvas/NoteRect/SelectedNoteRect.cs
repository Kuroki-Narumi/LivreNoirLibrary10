using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class SelectedNoteRect : NoteRectBase
    {
        public NoteRect Source { get; }

        private readonly bool _isInvisible;
        private readonly double _initialPosition;

        public SelectedNoteRect(NoteRect source)
        {
            Source = source;
            AbsolutePosition = _initialPosition = source.AbsolutePosition;
            NoteLength = source.NoteLength;
            X = source.X;
            Y = source.Y;
            Width = source.Width;
            Height = source.Height;
            Length = source.Length;
            _isInvisible = source.IsInvisibleNote;
        }

        public void SetOffsetY(double offset, int headHeight, double bottom, double scaleY)
        {
            AbsolutePosition = _initialPosition + offset;
            UpdateVertical(headHeight, bottom, scaleY);
        }

        public void Render(DrawingContext ctx, INoteRectContainer provider)
        {
            var x = X;
            var y = Y;
            var w = Width;
            var length = Length;
            var isInvisible = _isInvisible;
            if (isInvisible)
            {
                ctx.PushOpacity(0.5);
            }
            var headHeight = provider.HeadHeight;
            // ロングボディ
            if (length > headHeight)
            {
                Rect rect = new(x + 3, y + headHeight, w - 5, length - headHeight);
                ctx.DrawRectangle(MediaUtils.GetBrush(provider.SelectedLongColor), null, rect);
            }
            y += length;
            // 本体
            DrawSelectedHead(ctx, y, headHeight, provider.SelectedColor);
            if (isInvisible)
            {
                ctx.Pop();
            }
        }
    }
}
