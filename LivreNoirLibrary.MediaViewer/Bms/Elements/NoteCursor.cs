using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteCursor : FrameworkElement, ICursorInfo
    {
        public const double NoteOpacity = 0.56;
        public static SolidColorBrush TextFill => Brushes.White;
        public static SolidColorBrush TextStroke => Brushes.Black;
        public static readonly Pen? FrameStroke = MediaUtils.GetPen(SelectionColors.Stroke, 1);

        private BarPosition _position;
        private Rational _actualPosition;
        private int _lane;
        private NoteType _noteType;
        private DefType _defType;
        private int _id;

        private int _rectWidth;
        private Color _rectColor;
        private string? _indexText;
        private string? _typeText;

        public BarPosition Position => _position;
        public Rational ActualPosition => _actualPosition;
        public int Lane => _lane;
        public NoteType NoteType => _noteType;
        public DefType DefType => _defType;
        public int Id => _id;

        public bool IsValid => Lane is not int.MaxValue;

        public NoteCursor()
        {
            IsHitTestVisible = false;
            Visibility = Visibility.Collapsed;
        }

        public void Clear()
        {
            _lane = int.MaxValue;
            _indexText = null;
            _typeText = null;
            InvalidateVisual();
        }

        public void Update(double width, BarPosition position, Rational actualPosition, int lane, NoteType noteType, DefType defType, string? typeText, Color color)
        {
            _position = position;
            _actualPosition = actualPosition;
            _lane = lane;
            _defType = BmsUtils.GetDefType(lane);
            _rectWidth = (int)width;
            _rectColor = color;
            _noteType = noteType;
            _defType = defType;
            _typeText = typeText;
            InvalidateVisual();
        }

        public void SetIndex(int index, int radix) => SetIndexText(BmsUtils.ToBased(index, radix), index);

        public void SetIndexText(string? text, int index = 0)
        {
            _id = index;
            _indexText = text;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var w = _rectWidth;
            // ノート外形
            drawingContext.PushOpacity(NoteOpacity);
            drawingContext.DrawNoteRect(0, 0, w, RectBase.HeadHeight, _rectColor);
            drawingContext.Pop();
            // インデックス
            drawingContext.DrawNoteText(2, 0, _indexText);
            // タイプ情報
            if (_typeText is not null)
            {
                RectangularText.Render(drawingContext, w, RectBase.HeadHeight, _typeText, TextFill, TextStroke);
            }
            // 枠
            drawingContext.DrawRectangle(null, FrameStroke, new(0, 0, w, RectBase.HeadHeight));
        }
    }
}
