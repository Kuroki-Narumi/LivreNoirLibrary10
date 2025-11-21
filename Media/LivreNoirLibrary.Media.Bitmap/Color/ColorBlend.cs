using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    public interface IColorBlend
    {
        Vector<float> Blend(Vector<float> back, Vector<float> front);
    }

    public static unsafe partial class ColorBlend
    {
        public static readonly Vector<float> FloatFactor = Vector.Create(ColorUtils.FloatFactor);
        public static readonly Vector<float> InvertFactor = Vector.Create(ColorUtils.InvertFactor);
        public static readonly Vector<float> Epsilon = Vector.Create(ColorUtils.Epsilon);
        public static readonly Vector<float> RoundOffset = Vector.Create(ColorUtils.RoundOffset);
        public static readonly Vector<int> AlphaSelector = CreateAlphaSelector();
        private unsafe static Vector<int> CreateAlphaSelector()
        {
            var count = Vector<float>.Count;
            var span = (stackalloc float[count]);
            for (var i = 0; i < count; i++)
            {
                span[i] = (i % 4) is ColorUtils.ColorIndex_A ? 1 : 0;
            }
            return Vector.Equals(Vector<float>.One, Vector.Create(span));
        }

        internal static void BlendCore<T>(uint* pointer, int bitmapWidth, int width, int height, LnColor color, T blend)
            where T : IColorBlend
        {
            width *= 4;
            Parallel.For(0, height, y =>
            {
                // byteの各要素をfloatへ変換するために、ポインタをbyte*に変換
                var backP = (byte*)(pointer + y * bitmapWidth);
                // 定数ベクトル
                var count = Vector<float>.Count;
                var floatFactor = FloatFactor;
                var invertFactor = InvertFactor;
                var epsilon = Epsilon;
                var roundOffset = RoundOffset;
                var one = Vector<float>.One;
                var condition = AlphaSelector;
                // float変換用の作業バッファ
                var backBuffer = (stackalloc float[count]);
                var backAlphaBuffer = (stackalloc float[count]);
                // 前景ベクトル
                var c = color;
                var frontP = (byte*)&c;
                for (var i = 0; i < count; i++)
                {
                    backBuffer[i] = frontP[i % 4];
                    backAlphaBuffer[i] = frontP[0];
                }
                // 正規化してベクトル化
                var frontVector = Vector.Create(backBuffer) * invertFactor;
                var frontAlphaVector = Vector.Create(backAlphaBuffer) * invertFactor;

                // 切りの良い部分
                for (; width >= count; width -= count)
                {
                    Process(backBuffer, backAlphaBuffer, count);
                }
                // 余り
                if (width is > 0)
                {
                    Process(backBuffer, backAlphaBuffer, width);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Process(Span<float> backBuffer, Span<float> backAlphaBuffer, int count)
                {
                    // byteの値をそのままバッファへ格納
                    for (var i = 0; i < count; i++)
                    {
                        backBuffer[i] = backP[i];
                        // alpha値は4の倍数ごとに同じ値になる
                        backAlphaBuffer[i] = backP[i / 4 * 4 + ColorUtils.ColorIndex_A];
                    }
                    // 正規化してベクトル化
                    var backVector = Vector.Create(backBuffer) * invertFactor;
                    var backAlphaVector = Vector.Create(backAlphaBuffer) * invertFactor;

                    // Fb, Ff := Porter Duff 演算の定数 (ここでは Fb = 1 - front.A, Ff = 1)
                    // C := 合成後の色
                    // a := 合成後のアルファ
                    // C' := ブレンド後コンポジット前の色
                    var factor = backAlphaVector * (one - frontAlphaVector);
                    // a = back.A * Fb + front.A * Ff
                    var newAlpha = factor + frontAlphaVector;
                    // C' = back.A * blended + (1 - back.A) * front
                    var color = backAlphaVector * blend.Blend(backVector, frontVector) + (one - backAlphaVector) * frontVector;
                    // C = (back.A * Fb * back + front.A * Ff * C') / a
                    color = (factor * backVector + frontAlphaVector * color) / (newAlpha + epsilon);
                    // アルファとそれ以外を分けて反映
                    backVector = Vector.ConditionalSelect(condition, newAlpha, color);
                    // byte値へ復元しつつ範囲も制限する
                    backVector = Vector.Clamp(backVector * floatFactor + roundOffset, Vector<float>.Zero, floatFactor);
                    // バッファへ書き戻す
                    backVector.CopyTo(backBuffer);
                    for (var i = 0; i < count; i++)
                    {
                        backP[i] = (byte)backBuffer[i];
                    }
                    backP += count;
                }
            });
        }

        internal static void BlendCore<T>(uint* backPointer, int backWidth, uint* frontPointer, int frontWidth, int width, int height, T blend)
            where T : IColorBlend
        {
            width *= 4;
            Parallel.For(0, height, y =>
            {
                var w = width;
                // byteの各要素をfloatへ変換するために、ポインタをbyte*に変換
                var backP = (byte*)(backPointer + y * backWidth);
                var frontP = (byte*)(frontPointer + y * frontWidth);
                // 定数ベクトル
                var count = Vector<float>.Count;
                var floatFactor = FloatFactor;
                var invertFactor = InvertFactor;
                var epsilon = Epsilon;
                var roundOffset = RoundOffset;
                var one = Vector<float>.One;
                var condition = AlphaSelector;
                // float変換用の作業バッファ
                var backBuffer = (stackalloc float[count]);
                var frontBuffer = (stackalloc float[count]);
                var backAlphaBuffer = (stackalloc float[count]);
                var frontAlphaBuffer = (stackalloc float[count]);
                // 切りの良い部分
                for (; w >= count; w -= count)
                {
                    Process(backBuffer, backAlphaBuffer, frontBuffer, frontAlphaBuffer, count);
                }
                // 余り
                if (w is > 0)
                {
                    Process(backBuffer, backAlphaBuffer, frontBuffer, frontAlphaBuffer, w);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Process(Span<float> backBuffer, Span<float> backAlphaBuffer, Span<float> frontBuffer, Span<float> frontAlphaBuffer, int count)
                {
                    // byteの値をそのままバッファへ格納
                    for (var i = 0; i < count; i++)
                    {
                        backBuffer[i] = backP[i];
                        frontBuffer[i] = frontP[i];
                        // alpha値は4の倍数ごとに同じ値になる
                        backAlphaBuffer[i] = backP[i / 4 * 4 + ColorUtils.ColorIndex_A];
                        frontAlphaBuffer[i] = frontP[i / 4 * 4 + ColorUtils.ColorIndex_A];
                    }
                    // 正規化してベクトル化
                    var backVector = Vector.Create(backBuffer) * invertFactor;
                    var backAlphaVector = Vector.Create(backAlphaBuffer) * invertFactor;
                    var frontVector = Vector.Create(frontBuffer) * invertFactor;
                    var frontAlphaVector = Vector.Create(frontAlphaBuffer) * invertFactor;
                    // Fb, Ff := Porter Duff 演算の定数 (ここでは Fb = 1 - front.A, Ff = 1)
                    // C := 合成後の色
                    // a := 合成後のアルファ
                    // C' := ブレンド後コンポジット前の色
                    var factor = backAlphaVector * (one - frontAlphaVector);
                    // a = back.A * Fb + front.A * Ff
                    var newAlpha = factor + frontAlphaVector;
                    // C' = back.A * blended + (1 - back.A) * front
                    var color = backAlphaVector * blend.Blend(backVector, frontVector) + (one - backAlphaVector) * frontVector;
                    // C = (back.A * Fb * back + front.A * Ff * C') / a
                    color = (factor * backVector + frontAlphaVector * color) / (newAlpha + epsilon);
                    // アルファとそれ以外を分けて反映
                    backVector = Vector.ConditionalSelect(condition, newAlpha, color);
                    // byte値へ復元しつつ範囲も制限する
                    backVector = Vector.Clamp(backVector * floatFactor + roundOffset, Vector<float>.Zero, floatFactor);
                    // バッファへ書き戻す
                    backVector.CopyTo(backBuffer);
                    for (var i = 0; i < count; i++)
                    {
                        backP[i] = (byte)backBuffer[i];
                    }
                    backP += count;
                    frontP += count;
                }
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blend<T>(LnBitmapData back, LnColor color, T blend)
            where T : IColorBlend
        {
            if (back.IsValid)
            {
                var (pb, wb, hb) = back;
                BlendCore(pb, wb, wb, hb, color, blend);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blend<T>(LnBitmapData back, Rectangle rect, LnColor color, T blend)
            where T : IColorBlend
        {
            if (back.IsValid && Structs.Adjust(ref rect, back))
            {
                BlendCore(back.Offset(rect.X, rect.Y), back.Width, rect.Width, rect.Height, color, blend);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blend<T>(LnBitmapData back, LnBitmapData front, T blend)
            where T : IColorBlend
        {
            if (back.IsValid && front.IsValid)
            {
                var (pb, wb, hb) = back;
                var (pf, wf, hf) = front;
                var width = Math.Min(wb, wf);
                var height = Math.Min(hb, hf);
                BlendCore(pb, wb, pf, wf, width, height, blend);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Blend<T>(LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect, T blend)
            where T : IColorBlend
        {
            var (backX, backY) = backPoint;
            var (frontX, frontY, width, height) = frontRect;
            if (back.IsValid && front.IsValid && Structs.Adjust(ref backX, ref backY, ref width, ref height, back) && Structs.Adjust(ref frontRect, front))
            {
                BlendCore(back.Offset(backX, backY), back.Width, front.Offset(frontX, frontY), front.Width, width, height, blend);
            }
        }
    }
}
