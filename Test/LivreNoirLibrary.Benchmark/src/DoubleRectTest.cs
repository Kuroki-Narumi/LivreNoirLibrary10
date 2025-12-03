using LivreNoirLibrary.Media;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Drawing;
using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Benchmark
{
    public static class DoubleExtension
    {
        public static int RoundToInt(this double value) => (int)Math.Round(value);
    }

    public class DoubleRectTest
    {

        /// <summary>
        /// <paramref name="sourceOriginalRect"/>で規定された有効範囲から<paramref name="sourceRect"/>で指定された範囲を切り抜き、
        /// <paramref name="destOriginalRect"/>で規定された有効範囲内の<paramref name="destRect"/>で指定された範囲に貼り付けようとした際の、
        /// 実際に参照するべき切り抜き元範囲を<paramref name="actualSourceRect"/>、実際の貼り付け先範囲を<paramref name="actualDestRect"/>へ格納し、
        /// 切り抜き元及び貼り付け先の範囲が全て有効であるかどうかを返します。
        /// </summary>
        /// <param name="sourceOriginalRect">クリップ有効範囲</param>
        /// <param name="sourceRect">クリップ元範囲</param>
        /// <param name="destOriginalRect">貼り付け有効範囲</param>
        /// <param name="destRect">貼り付け先範囲</param>
        /// <param name="actualSourceRect">正規化されたクリップ元範囲</param>
        /// <param name="actualDestRect">正規化された貼り付け先範囲</param>
        /// <returns><see langword="true"/> if both <paramref name="actualSourceRect"/> and <paramref name="actualDestRect"/> are valid; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(
            in DoubleRect sourceOriginalRect, in DoubleRect sourceRect,
            in DoubleRect destOriginalRect, in DoubleRect destRect,
            out Rectangle actualSourceRect, out Rectangle actualDestRect)
        {
            var (sourceOrigX, sourceOrigY, sourceOrigWidth, sourceOrigHeight) = sourceOriginalRect;
            var (sourceX, sourceY, sourceWidth, sourceHeight) = sourceRect;
            var (destOrigX, destOrigY, destOrigWidth, destOrigHeight) = destOriginalRect;
            var (destX, destY, destWidth, destHeight) = destRect;
            // いずれかの領域がゼロである場合は無効
            if (sourceOrigWidth is <= 0 || sourceOrigHeight is <= 0 || sourceWidth is <= 0 || sourceHeight is <= 0 ||
                destOrigWidth is <= 0 || destOrigHeight is <= 0 || destWidth is <= 0 || destHeight is <= 0)
            {
                actualSourceRect = default;
                actualDestRect = default;
                return false;
            }

            // 拡大率
            var scaleX = destWidth / sourceWidth;
            var scaleY = destHeight / sourceHeight;

            // 右下の座標
            var sourceOrigRight = sourceOrigX + sourceOrigWidth;
            var sourceOrigBottom = sourceOrigY + sourceOrigHeight;
            var sourceRight = sourceX + sourceWidth;
            var sourceBottom = sourceY + sourceHeight;
            var destOrigRight = destOrigX + destOrigWidth;
            var destOrigBottom = destOrigY + destOrigHeight;
            var destRight = destX + destWidth;
            var destBottom = destY + destHeight;

            // 実際のクリップ範囲
            // [クリップ元 左上] と [クリップ有効範囲 左上] のうち、より右下にあるほう
            var sourceLeft = Math.Max(sourceX, sourceOrigX);
            var sourceTop = Math.Max(sourceY, sourceOrigY);
            // [クリップ元 右下] と [クリップ有効範囲 右下] のうち、より左上にあるほう
            sourceRight = Math.Min(sourceRight, sourceOrigRight);
            sourceBottom = Math.Min(sourceBottom, sourceOrigBottom);

            // 実際の貼り付け範囲
            // [貼り付け先 左上] と [貼り付け有効範囲 左上] のうち、より右下にあるほう
            var destLeft = Math.Max(destX, destOrigX);
            var destTop = Math.Max(destY, destOrigY);
            // [貼り付け先 右下] と [貼り付け有効範囲 右下] のうち、より左上にあるほう
            destRight = Math.Min(destRight, destOrigRight);
            destBottom = Math.Min(destBottom, destOrigBottom);

            // [クリップ元 範囲] を [貼り付け先 範囲] に合わせる
            sourceLeft = Math.Max(sourceLeft, (destLeft - destX) / scaleX);
            sourceWidth = Math.Min(sourceRight - sourceLeft, (destRight - destLeft) / scaleX);
            sourceTop = Math.Max(sourceTop, (destTop - destY) / scaleY);
            sourceHeight = Math.Min(sourceBottom - sourceTop, (destBottom - destTop) / scaleY);

            // [貼り付け先 範囲] を [クリップ元 範囲] に合わせる
            destLeft = Math.Max(destLeft, (sourceLeft - sourceX) * scaleX);
            destTop = Math.Max(destTop, (sourceTop - sourceY) * scaleY);
            destWidth = sourceWidth * scaleX;
            destHeight = sourceHeight * scaleY;

            actualSourceRect = new(sourceLeft.RoundToInt(), sourceTop.RoundToInt(), sourceWidth.RoundToInt(), sourceHeight.RoundToInt());
            actualDestRect = new(destLeft.RoundToInt(), destTop.RoundToInt(), destWidth.RoundToInt(), destHeight.RoundToInt());

            // 範囲矯正の結果幅や高さが有効でなくなる可能性がある
            return actualSourceRect.Width is > 0 && actualSourceRect.Height is > 0 && actualDestRect.Width is > 0 && actualDestRect.Height is > 0;
        }

        /// <summary>
        /// <paramref name="sourceOriginalRect"/>で規定された有効範囲から<paramref name="sourceRect"/>で指定された範囲を切り抜き、
        /// <paramref name="destOriginalRect"/>で規定された有効範囲内の<paramref name="destRect"/>で指定された範囲に貼り付けようとした際の、
        /// 実際に参照するべき切り抜き元範囲を<paramref name="actualSourceRect"/>、実際の貼り付け先範囲を<paramref name="actualDestRect"/>へ格納し、
        /// 切り抜き元及び貼り付け先の範囲が全て有効であるかどうかを返します。
        /// </summary>
        /// <param name="sourceOriginalRect">クリップ有効範囲</param>
        /// <param name="sourceRect">クリップ元範囲</param>
        /// <param name="destOriginalRect">貼り付け有効範囲</param>
        /// <param name="destRect">貼り付け先範囲</param>
        /// <param name="actualSourceRect">正規化されたクリップ元範囲</param>
        /// <param name="actualDestRect">正規化された貼り付け先範囲</param>
        /// <returns><see langword="true"/> if both <paramref name="actualSourceRect"/> and <paramref name="actualDestRect"/> are valid; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust2(
            in DoubleRect sourceOriginalRect, in DoubleRect sourceRect,
            in DoubleRect destOriginalRect, in DoubleRect destRect,
            out Rectangle actualSourceRect, out Rectangle actualDestRect)
        {
            actualSourceRect = default;
            actualDestRect = default;
            // 各範囲を正規化
            var sourceClip = sourceOriginalRect.Intersect(sourceRect);
            var destClip = destOriginalRect.Intersect(destRect);
            if (sourceClip.IsEmpty || destClip.IsEmpty)
            {
                return false;
            }
            // 拡大率
            var scaleX = destRect.Width / sourceRect.Width;
            var scaleY = destRect.Height / sourceRect.Height;

            // クリップ元の座標系で貼り付け先の範囲を正規化
            var s2dLeft = sourceRect.X + (destClip.X - destRect.X) / scaleX;
            var s2dTop = sourceRect.Y + (destClip.Y - destRect.Y) / scaleY;
            var s2dWidth = destClip.Width / scaleX;
            var s2dHeight = destClip.Height / scaleY;
            sourceClip = sourceClip.Intersect(new(s2dLeft, s2dTop, s2dWidth, s2dHeight));
            if (sourceClip.IsEmpty)
            {
                return false;
            }

            // 貼り付け先の範囲を改めて計算
            destClip = new(
                destRect.X + (sourceClip.X - sourceRect.X) * scaleX,
                destRect.Y + (sourceClip.Y - sourceRect.Y) * scaleY,
                sourceClip.Width * scaleX,
                sourceClip.Height * scaleY
                );

            actualSourceRect = sourceClip.Round();
            actualDestRect = destClip.Round();
            return true;
        }

        public const int Count = 1000;

        private readonly DoubleRect[] _rects = new DoubleRect[Count * 4];
        private readonly bool[] _results1 = new bool[Count];
        private readonly Rectangle[] _results2 = new Rectangle[Count * 2];

        [GlobalSetup]
        public void Setup()
        {
            var random = new XorShift(123456789);
            double A(double f, double o = 0) => random.NextDouble() * f + o;
            for (var i = 0; i < Count; i++)
            {
                _rects[i] = new(0, 0, A(2000, 2000), A(2000, 2000));
                _rects[i + Count] = new(A(100), A(100), A(2000, 2000), A(2000, 2000));
                _rects[i + Count * 2] = new(A(100, -50), A(100, -50), A(1000, 1000), A(1000, 1000));
                _rects[i + Count * 3] = new(A(100, -50), A(100, -50), A(1000, 1000), A(1000, 1000));
            }
        }

        [Benchmark]
        public void Adjust()
        {
            for (var i = 0; i < Count; i++)
            {
                var r1 = _rects[i];
                var r2 = _rects[i + Count];
                var r3 = _rects[i + Count * 2];
                var r4 = _rects[i + Count * 3];
                _results1[i] = Adjust(r1, r2, r3, r4, out var rr1, out var rr2);
                _results2[i] = rr1;
                _results2[i + Count] = rr2;
            }
        }

        [Benchmark]
        public void Adjust2()
        {
            for (var i = 0; i < Count; i++)
            {
                var r1 = _rects[i];
                var r2 = _rects[i + Count];
                var r3 = _rects[i + Count * 2];
                var r4 = _rects[i + Count * 3];
                _results1[i] = Adjust2(r1, r2, r3, r4, out var rr1, out var rr2);
                _results2[i] = rr1;
                _results2[i + Count] = rr2;
            }
        }

        public static void Validate()
        {
            var test1 = new DoubleRectTest();
            var test2 = new DoubleRectTest();
            test1.Setup();
            test2.Setup();
            Console.WriteLine($"initial check: {test1._rects.SequenceEqual(test2._rects)}");
            test1.Adjust();
            test2.Adjust2();
            Console.WriteLine($"result check: {test1._results1.SequenceEqual(test2._results1)}");
            Console.WriteLine($"out check: {test1._results2.SequenceEqual(test2._results2)}");
            Console.WriteLine(test1._results2.AsSpan(0, 10).ToListString());
            Console.WriteLine(test2._results2.AsSpan(0, 10).ToListString());
        }
    }
}
