using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class RadarChart : TextViewerBase
    {
        private WriteableBitmap? _fill;

        public RadarChart()
        {
            _captions.CollectionChanged += (s, e) => InvalidateVisual();
            _values.CollectionChanged += (s, e) => ReserveRefresh();
            ReserveRefresh();
        }

        protected int _count;
        protected List<PointInfo> _points = [];
        protected Int32Rect _value_rect;
        protected bool _round;
        protected Func<Point, Point> _get_p = p => p;

        protected override void Refresh()
        {
            if (IsFilled)
            {
                CreateBitmap();
            }
            else
            {
                _fill = null;
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            /* Refresh Properties */
            // 項目数
            var count = _values.Count;
            // 項目名の足りない分を補完
            while (_captions.Count < count)
            {
                _captions.AddWithoutNotify(null);
            }
            // レーダー半径
            var radius = Radius.Validate(DefaultRadius);
            // 描画オフセット
            var ox = OffsetX.Validate(0) + radius;
            var oy = OffsetY.Validate(0) + radius;
            // 点座標の補正
            _round = UseLayoutRounding;
            _get_p = _round ?
                p => new(Math.Round(p.X) + 0.5, Math.Round(p.Y) + 0.5) :
                p => p;

            /* Refresh Points */
            _points.Clear();
            // 回転角度
            var angle = Angle.Validate(DefaultAngle);
            // 回転方向
            var isCcw = SweepDirection is SweepDirection.Counterclockwise;
            // 目盛り分割数
            var step = Step;
            // 項目名オフセット
            _fontSize = FontSize;
            var caption_ox = HorizontalCaptionOffset.Validate(_fontSize);
            var caption_oy = VerticalCaptionOffset.Validate(_fontSize / 2);
            for (var i = 0; i < count; i++)
            {
                // 基本位置
                var th = Math.PI * 2.0 * (angle + (isCcw ? 1.0 : -1.0) * i / count);
                var (sin, cos) = Math.SinCos(th);
                sin = -sin;
                var x = radius * cos;
                var y = radius * sin;
                // 分割線座標のリスト
                List<Point> list = [];
                for (int j = 1; j < step; j++)
                {
                    list.Add(new(ox + x * j / step, oy + y * j / step));
                }
                _points.Add(new()
                {
                    X = x,
                    Y = y,
                    FramePoint = new(ox + x, oy + y),
                    Steps = [.. list],
                    CaptionX = ox + x + cos * caption_ox,
                    CaptionY = oy + y + sin * caption_oy,
                });
            }
            if (count > 0)
            {
                _points.Add(_points[0]);
            }

            /* Refresh Value Potins */
            // レーダー最大値と最小値指定の処理
            var minD = MinValue;
            var maxD = MaxValue;
            var autoMin = !double.IsFinite(minD);
            var autoMax = !double.IsFinite(maxD);
            for (int i = 0; i < count; i++)
            {
                var v = _values[i];
                if (autoMin && v < minD)
                {
                    minD = v;
                }
                if (autoMax && v > maxD)
                {
                    maxD = v;
                }
            }
            var min = (float)minD;
            var max = (float)maxD;
            // グラデーション基準点
            var midD = MidValue.Validate((maxD + minD) / 2);
            var mid = (float)midD;
            var gradient = (max != min) && IsGradient;
            var den_max = max - mid;
            var den_min = mid - min;
            var den_mm = max - min;
            if (den_mm == 0)
            {
                den_mm = max;
            }
            Func<float, HsvColor> GetColor;
            var maxColor = HsvColor.FromColor(MaxColor);
            var minColor = HsvColor.FromColor(MinColor);
            var medColor = HsvColor.FromColor(MidColor);
            if (gradient)
            {
                GetColor = v =>
                {
                    if ((max >= min) ? (v > mid) : (v < mid))
                    {
                        if (den_max != 0)
                        {
                            var dif = (v - mid) / den_max;
                            return HsvColor.GetBlended(medColor, maxColor, dif);
                        }
                        else
                        {
                            return maxColor;
                        }
                    }
                    else if ((max >= min) ? (v < mid) : (v > mid))
                    {
                        if (den_min != 0)
                        {
                            var dif = (v - min) / den_min;
                            return HsvColor.GetBlended(minColor, medColor, dif);
                        }
                        else
                        {
                            return minColor;
                        }
                    }
                    else
                    {
                        return medColor;
                    }
                };
            }
            else
            {
                GetColor = v => medColor;
            }
            double minX = 0, maxX = 0, minY = 0, maxY = 0;
            for (var i = 0; i < count; i++)
            {
                var v = (float)_values[i];
                var point = _points[i];
                var ratio = (v - min) / den_mm;
                var x = (ratio * _points[i].X).RoundToInt();
                var y = (ratio * _points[i].Y).RoundToInt();
                point.ValueX = x;
                point.ValueY = y;
                point.ValueColor = GetColor(v);
                if (x > maxX) { maxX = x; }
                if (x < minX) { minX = x; }
                if (y > maxY) { maxY = y; }
                if (y < minY) { minY = y; }
            }
            _value_rect = new((int)minX, (int)minY, (int)Math.Ceiling(maxX - minX), (int)Math.Ceiling(maxY - minY));

            base.OnRender(dc);

            /* Draw Frame */
            var pen_frame = MediaUtils.GetPen(FrameBrush, FrameThickness);
            var pen_step = MediaUtils.GetPen(StepBrush, StepThickness);
            var center = new Point(ox, oy);
            for (var i = 1; i <= count; i++)
            {
                var now = _points[i];
                var pre = _points[i - 1];
                if (pen_step is not null)
                {
                    var len = now.Steps.Length;
                    for (int j = 0; j < len; j++)
                    {
                        dc.DrawLine(pen_step, _get_p(pre.Steps[j]), _get_p(now.Steps[j]));
                    }
                }
                if (pen_frame is not null)
                {
                    var nowP = _get_p(now.FramePoint);
                    dc.DrawLine(pen_frame, _get_p(center), nowP);
                    dc.DrawLine(pen_frame, _get_p(pre.FramePoint), nowP);
                }
            }
            /* Draw Fill */
            if (_fill is not null)
            {
                dc.PushOpacity(FillOpacity);
                dc.DrawImage(_fill, new(ox + _value_rect.X, oy + _value_rect.Y, _value_rect.Width, _value_rect.Height));
                dc.Pop();
            }
            /* Draw Title */
            var title = Title;
            if (title is ImageSource source)
            {
                DrawCaption_Image(dc, ox, oy, source, false);
            }
            else
            {
                DrawCaption_Text(dc, ox, oy, title?.ToString(), 1);
            }
            /* Draw Values */
            var lineTh = LineThickness;
            for (var i = 1; i <= count; i++)
            {
                var now = _points[i];
                var pre = _points[i - 1];
                var p0 = _get_p(new(ox + pre.ValueX, oy + pre.ValueY));
                var p1 = _get_p(new(ox + now.ValueX, oy + now.ValueY));
                var brush = HsvColor.CreateGradientBrush(pre.ValueColor, now.ValueColor);
                (brush.StartPoint, brush.EndPoint) = GetGradientPoint(p0, p1);
                var pen = MediaUtils.GetPen(brush, lineTh);
                if (pen is not null)
                {
                    dc.DrawLine(pen, p0, p1);
                }
            }
            /* Draw Captions */
            var valueVisible = IsValueVisible;
            for (var i = 0; i < count; i++)
            {
                var caption = _captions[i];
                var point = _points[i];
                if (caption is ImageSource capSource)
                {
                    DrawCaption_Image(dc, point.CaptionX, point.CaptionY, capSource, valueVisible);
                }
                else
                {
                    DrawCaption_Text(dc, point.CaptionX, point.CaptionY, caption?.ToString(), valueVisible ? 2 : 1);
                }
            }
            if (IsValueVisible)
            {
                for (int i = 0; i < count; i++)
                {
                    var value = _values[i];
                    var text = string.Format(ValueFormat ?? "{0}", value);
                    DrawCaption_Text(dc, _points[i].CaptionX, _points[i].CaptionY, text, _captions[i] is not null ? 0 : 1);
                }
            }
        }

        protected void DrawCaption_Text(DrawingContext dc, double cx, double cy, string? text, int lineOffset)
        {
            if (_round)
            {
                cx = Math.Round(cx);
                cy = Math.Round(cy);
            }
            RenderText(dc, cx, cy, text, va: (VerticalAlignment)lineOffset);
        }

        protected void DrawCaption_Image(DrawingContext dc, double cx, double cy, ImageSource source, bool valueVisible)
        {
            var x = cx - source.Width / 2;
            var y = cy - source.Height / 2;
            if (valueVisible)
            {
                y -= _fontSize / 2;
            }
            if (_round)
            {
                x = Math.Round(x);
                y = Math.Round(y);
            }
            Rect rect = new(x, y, source.Width, source.Height);
            dc.DrawImage(source, rect);
        }

        protected void CreateBitmap()
        {
            var x = _value_rect.X;
            var y = _value_rect.Y;
            var w = _value_rect.Width;
            var h = _value_rect.Height;
            if (w <= 0 || h <= 0)
            {
                _fill = null;
                return;
            }
            if (_fill is null || (w != _fill.PixelWidth || h != _fill.PixelHeight))
            {
                _fill = Bitmap.Create(w, h);
            }
            else
            {
                _fill.Clear();
            }
            for (int i = 1; i <= _count; i++)
            {
                var now = _points[i];
                var pre = _points[i - 1];
                _fill.FillTriangle(-x, -y, pre.ValueX - x, pre.ValueY - y, now.ValueX - x, now.ValueY - y, pre.ValueColor, now.ValueColor);
            }
        }

        protected static (Point Start, Point End) GetGradientPoint(Point point0, Point point1)
        {
            var (x0, y0) = (point0.X, point0.Y);
            var (x1, y1) = (point1.X, point1.Y);
            var dx = Math.Abs(x0 - x1);
            var dy = Math.Abs(y0 - y1);
            // 横長
            if (dx >= dy)
            {
                // 左から右
                if (x0 <= x1)
                {
                    (x0, x1) = (0, 1);
                }
                // 右から左
                else
                {
                    (x0, x1) = (1, 0);
                }
                var dif = dy / dx / 2.0;
                // 上から下
                if (y0 <= y1)
                {
                    (y0, y1) = (0.5 - dif, 0.5 + dif);
                }
                // 下から上
                else
                {
                    (y0, y1) = (0.5 + dif, 0.5 - dif);
                }
            }
            // 縦長
            else
            {
                var dif = dx / dy / 2.0;
                // 左から右
                if (x0 <= x1)
                {
                    (x0, x1) = (0.5 - dif, 0.5 + dif);
                }
                // 右から左
                else
                {
                    (x0, x1) = (0.5 + dif, 0.5 - dif);
                }
                // 上から下
                if (y0 <= y1)
                {
                    (y0, y1) = (0, 1);
                }
                // 下から上
                else
                {
                    (y0, y1) = (1, 0);
                }
            }
            return (new(x0, y0), new(x1, y1));
        }

        protected class PointInfo
        {
            public double X { get; init; }
            public double Y { get; init; }

            public Point FramePoint { get; init; }

            public double CaptionX { get; init; }
            public double CaptionY { get; init; }

            public Point[] Steps { get; init; } = [new(0, 0)];

            public int ValueX { get; set; }
            public int ValueY { get; set; }

            public HsvColor ValueColor { get; set; }
        }
    }
}
