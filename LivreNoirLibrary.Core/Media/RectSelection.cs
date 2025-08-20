using System;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static class RectSelection
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ProcessSnap(ref double value, double minimum, double maximum, int division, double threshold)
        {
            var den = maximum - minimum;
            if (division is > 0 && den is > 0)
            {
                var snapped = Math.Round((value - minimum) * division / den) / division * den + minimum;
                if (Math.Abs(value - snapped) <= threshold)
                {
                    value = snapped;
                    return true;
                }
            }
            return false;
        }

        private static bool CheckMoving(ref RectSelectionInfo info, double dx, double dy)
        {
            if (!info.IsMoving)
            {
                if (Math.Abs(dx) < info.MoveThreshold && Math.Abs(dy) < info.MoveThreshold)
                {
                    return false; // Not moving enough to trigger resize
                }
                info.IsMoving = true;
            }
            return true;
        }

        public static void Move(ref RectSelectionInfo info, double x, double y, bool isDirectionFixed, bool isSnapEnabled)
        {
            var (initX, initY) = info.InitialPosition;
            var (minX, maxX, minY, maxY) = info.MoveLimits;
            var dx = Math.Clamp(x - initX, minX, maxX);
            var dy = Math.Clamp(y - initY, minY, maxY);
            if (!CheckMoving(ref info, dx, dy))
            {
                return;
            }
            // 各値のロード
            var (limitLeft, limitTop, limitRight, limitBottom) = info.VertexLimits;
            var (initLeft, initTop, initRight, initBottom) = info.InitialVertexes;
            var iw = initRight - initLeft;
            var ih = initBottom - initTop;
            var lr = limitRight - iw;
            var lb = limitBottom - ih;
            double left, top;
            // スナップ
            var snapDiv = info.SnapDivision;
            if (isSnapEnabled && snapDiv is > 0)
            {
                var snapTh = info.SnapThreshold;
                left = initLeft + dx;
                top = initTop + dy;
                ProcessSnap(ref left, limitLeft, lr, snapDiv, snapTh);
                ProcessSnap(ref top, limitTop, lb, snapDiv, snapTh);
                dx = left - initLeft;
                dy = top - initTop;
            }
            // 移動方向の制限
            if (isDirectionFixed)
            {
                var tan = Math.Abs(dy / dx);
                if (tan < 0.5) // 横方向移動
                {
                    dy = 0;
                }
                else if (tan < 1) // 斜め移動/横を参照
                {
                    dy = Math.Clamp(Math.Abs(dx) * Math.Sign(dy), minY, maxY);
                    dx = Math.Abs(dy) * Math.Sign(dx);
                }
                else if (tan < 2) // 斜め移動/縦を参照
                {
                    dx = Math.Clamp(Math.Abs(dy) * Math.Sign(dx), minX, maxX);
                    dy = Math.Abs(dx) * Math.Sign(dy);
                }
                else // 縦方向移動
                {
                    dx = 0;
                }
            }
            // 最終結果
            left = initLeft + dx;
            top = initTop + dy;
            info.Set(left, top, left + iw, top + ih);
        }

        public static void Resize(ref RectSelectionInfo info, double x, double y, bool isRatioFixed, bool isSnapEnabled)
        {
            var (initX, initY) = info.InitialPosition;
            var (minX, maxX, minY, maxY) = info.MoveLimits;
            var dx = Math.Clamp(x - initX, minX, maxX);
            var dy = Math.Clamp(y - initY, minY, maxY);
            if (!CheckMoving(ref info, dx, dy))
            {
                return;
            }
            // スナップ
            var snapDiv = info.SnapDivision;
            if (isSnapEnabled && snapDiv is > 0)
            {
                var snapTh = info.SnapThreshold;
                ProcessSnap(ref dx, minX, maxX, snapDiv, snapTh);
                ProcessSnap(ref dy, minY, maxY, snapDiv, snapTh);
            }
            // 各値のロード
            var (limitLeft, limitTop, limitRight, limitBottom) = info.VertexLimits;
            var (initLeft, initTop, initRight, initBottom) = info.InitialVertexes;
            var iw = initRight - initLeft;
            var ih = initBottom - initTop;
            var dir = info.Direction;
            double left, top, right, bottom;
            // 変化の基本値
            var moveLeft = dir is 1 or 4 or 7;
            var moveRight = dir is 3 or 6 or 9;
            var moveTop = dir is 7 or 8 or 9;
            var moveBottom = dir is 1 or 2 or 3;
            double nw, nh, lw, lh;
            if (moveRight)
            {
                nw = iw + dx;
                lw = nw is >= 0 ? limitRight - initLeft : initLeft - limitLeft;
            }
            else if (moveLeft)
            {
                nw = iw - dx;
                lw = nw is >= 0 ? initRight - limitLeft : limitRight - initRight;
            }
            else
            {
                nw = 0;
                lw = limitRight - limitLeft;
            }
            if (moveBottom)
            {
                nh = ih + dy;
                lh = nh is >= 0 ? limitBottom - initTop : initTop - limitTop;
            }
            else if (moveTop)
            {
                nh = ih - dy;
                lh = nh is >= 0 ? initBottom - limitTop : limitBottom - initBottom;
            }
            else
            {
                nh = 0;
                lh = limitBottom - limitTop;
            }
            // 縦横比の固定
            var (rw, rh) = isRatioFixed ? info.ReferenceSize : (0, 0);
            if (rw is > 0 && rh is > 0)
            {
                var xSign = 1;
                var ySign = 1;
                if (nw is < 0)
                {
                    xSign = -1;
                    nw = -nw;
                }
                if (nh is < 0)
                {
                    ySign = -1;
                    nh = -nh;
                }
                var sx = nw / rw;
                var sy = nh / rh;
                double actualWidth, actualHeight;
                if (sx >= sy)
                {
                    actualWidth = nw;
                    actualHeight = rh * sx;
                    if (actualHeight > lh)
                    {
                        actualHeight = lh;
                        actualWidth = rw * actualHeight / rh;
                    }
                }
                else
                {
                    actualWidth = rw * sy;
                    actualHeight = nh;
                    if (actualWidth > lw)
                    {
                        actualWidth = lw;
                        actualHeight = rh * actualWidth / rw;
                    }
                }
                actualWidth *= xSign;
                actualHeight *= ySign;
                if (actualWidth != nw || actualHeight != nh)
                {
                    nw = actualWidth;
                    nh = actualHeight;
                }
            }
            // 最終結果
            if (moveLeft)
            {
                left = initRight - nw;
                right = initRight;
            }
            else if (nw > limitRight - initLeft)
            {
                left = limitRight - nw;
                right = limitRight;
            }
            else
            {
                left = initLeft;
                right = nw is 0 ? initRight : initLeft + nw;
            }
            if (moveTop)
            {
                top = initBottom - nh;
                bottom = initBottom;
            }
            else if (nh > limitBottom - initTop)
            {
                top = limitBottom - nh;
                bottom = limitBottom;
            }
            else
            {
                top = initTop;
                bottom = nh is 0 ? initBottom : initTop + nh;
            }
            if (left > right)
            {
                (left, right) = (right, left);
            }
            if (top > bottom)
            {
                (top, bottom) = (bottom, top);
            }
            info.Set(left, top, right, bottom);
        }

        public static void Auto(ref RectSelectionInfo info, double x, double y, bool isRatioFixed, bool isSnapEnabled)
        {
            if (info.Direction is 5)
            {
                Move(ref info, x, y, isRatioFixed, isSnapEnabled);
            }
            else
            {
                Resize(ref info, x, y, isRatioFixed, isSnapEnabled);
            }
        }
    }

    public struct RectSelectionInfo
    {
        public readonly (double X, double Y) InitialPosition;
        public readonly (double Left, double Top, double Right, double Bottom) InitialVertexes;
        public readonly (double Left, double Top, double Right, double Bottom) VertexLimits;
        public readonly (double MinX, double MaxX, double MinY, double MaxY) MoveLimits;

        public readonly (int Width, int Height) ReferenceSize;
        public readonly int Direction;
        public readonly double MoveThreshold;
        public readonly int SnapDivision;
        public readonly double SnapThreshold;

        public RectSelectionInfo(
            (double X, double Y) initialPos,
            (double Left, double Top, double Right, double Bottom) initial,
            (double Left, double Top, double Right, double Bottom) limit,
            (int Width, int Height) refSize = default,
            int direction = 0, double moveThreshold = 8, int snapDivision = 2, double snapThreshold = 16, bool initialSnap = false, bool checkArgs = true)
        {
            if (checkArgs)
            {
                if (initial.Left > initial.Right)
                {
                    throw new ArgumentException("initial.Left must be <= initial.Right", nameof(initial));
                }
                if (initial.Top > initial.Bottom)
                {
                    throw new ArgumentException("initial.Top must be <= initial.Bottom", nameof(initial));
                }
                if (limit.Left > limit.Right)
                {
                    throw new ArgumentException("limit.Left mus be <= limit.Right", nameof(limit));
                }
                if (limit.Top > limit.Bottom)
                {
                    throw new ArgumentException("limit.Top mus be <= limit.Bottom", nameof(limit));
                }
                initial.Left = Math.Max(initial.Left, limit.Left);
                initial.Right = Math.Min(initial.Right, limit.Right);
                initial.Top = Math.Max(initial.Top, limit.Top);
                initial.Bottom = Math.Min(initial.Bottom, limit.Bottom);
            }
            initialPos.X = Math.Clamp(initialPos.X, limit.Left, limit.Right);
            initialPos.Y = Math.Clamp(initialPos.Y, limit.Top, limit.Bottom);
            if (direction is 0)
            {
                if (initialSnap)
                {
                    RectSelection.ProcessSnap(ref initialPos.X, limit.Left, limit.Right, snapDivision, snapThreshold);
                    RectSelection.ProcessSnap(ref initialPos.Y, limit.Top, limit.Bottom, snapDivision, snapThreshold);
                }
                direction = 3;
                initial = (initialPos.X, initialPos.Y, initialPos.X, initialPos.Y);
            }
            if (refSize == (0, 0))
            {
                refSize = (1, 1);
            }
            InitialPosition = initialPos;
            InitialVertexes = initial;
            VertexLimits = limit;
            if (direction is 5)
            {
                MoveLimits = (
                    limit.Left - initial.Left,
                    limit.Right - initial.Right,
                    limit.Top - initial.Top,
                    limit.Bottom - initial.Bottom
                    );
            }
            else
            {
                double minX, maxX, minY, maxY;
                if (direction is 3 or 6 or 9) // 右をリサイズ
                {
                    minX = limit.Left - initial.Right;
                    maxX = limit.Right - initial.Right;
                }
                else if (direction is 1 or 4 or 7) // 左をリサイズ
                {
                    minX = limit.Left - initial.Left;
                    maxX = limit.Right - initial.Left;
                }
                else // 横はリサイズしない
                {
                    minX = maxX = 0;
                }
                if (direction is 1 or 2 or 3) // 下をリサイズ
                {
                    minY = limit.Top - initial.Bottom;
                    maxY = limit.Bottom - initial.Bottom;
                }
                else if (direction is 7 or 8 or 9) // 上をリサイズ
                {
                    minY = limit.Top - initial.Top;
                    maxY = limit.Bottom - initial.Top;
                }
                else // 縦はリサイズしない
                {
                    minY = maxY = 0;
                }
                MoveLimits = (minX, maxX, minY, maxY);
            }
            ReferenceSize = refSize;
            Direction = direction;
            MoveThreshold = moveThreshold;
            SnapDivision = snapDivision;
            SnapThreshold = snapThreshold;
            Left = initial.Left;
            Top = initial.Top;
            Right = initial.Right;
            Bottom = initial.Bottom;
        }

        public bool IsMoving { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public readonly double Width => Right - Left;
        public readonly double Height => Bottom - Top;

        public void Set(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public readonly bool IsModified => Left != InitialVertexes.Left || Top != InitialVertexes.Top || Right != InitialVertexes.Right || Bottom != InitialVertexes.Bottom;
    }
}
