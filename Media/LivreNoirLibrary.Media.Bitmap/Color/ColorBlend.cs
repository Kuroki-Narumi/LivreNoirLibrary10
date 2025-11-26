using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class ColorBlend
    {
        public static readonly Vector<float> FloatFactor = Vector.Create(ColorUtils.FloatFactor);
        public static readonly Vector<float> InvertFactor = Vector.Create(ColorUtils.InvertFactor);

        public static readonly Vector<float> Epsilon = Vector.Create(ColorUtils.Epsilon);
        public static readonly Vector<int> AlphaSelector = Vector.Equals(Vector<float>.One, VectorUtils.CreateRepeating([0, 0, 0, 1f]));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void BlendCore(ref Vector<float> back, in Vector<float> backAlpha, in Vector<float> front, in Vector<float> frontAlpha, BlendFunc blend)
        {
            // Fb, Ff := Porter Duff 演算の定数 (ここでは Fb = 1 - front.A, Ff = 1)
            // C := 合成後の色
            // a := 合成後のアルファ
            // C' := ブレンド後コンポジット前の色
            var factor = backAlpha * (Vector<float>.One - frontAlpha);
            // a = back.A * Fb + front.A * Ff
            var newAlpha = factor + frontAlpha;
            // C' = back.A * blended + (1 - back.A) * front
            var color = backAlpha * blend(back, front) + (Vector<float>.One - backAlpha) * front;
            // C = (back.A * Fb * back + front.A * Ff * C') / a
            color = (factor * back + frontAlpha * color) / (newAlpha + Epsilon);
            // アルファとそれ以外を分けて反映
            back = Vector.ConditionalSelect(AlphaSelector, newAlpha, color);
        }

        public static void BlendUInt(uint* pointer, int targetWidth, int width, int height, BlendFunc blend, LnColor color)
        {
            var count = Vector<float>.Count;
            width *= 4;
            var invertFactor = InvertFactor;
            // 前景ベクトル
            var ca = ColorUtils.GetFloat(color.A);
            var cr = ColorUtils.RgbToScRgb(color.R);
            var cg = ColorUtils.RgbToScRgb(color.G);
            var cb = ColorUtils.RgbToScRgb(color.B);
            var frontVector = VectorUtils.CreateRepeating([cb, cg, cr, ca]);
            var frontAlphaVector = Vector.Create(ca);

            Parallel.For(0, height, y =>
            {
                var w = width;
                // byteの各要素をfloatへ変換するために、ポインタをbyte*に変換
                var backP = (byte*)(pointer + y * targetWidth);
                // float変換用の作業バッファ
                var backBuffer = stackalloc float[count];
                var backAlphaBuffer = stackalloc float[count];

                for (; w >= count; w -= count)
                {
                    Process(backBuffer, backAlphaBuffer, count);
                }
                if (w is > 0)
                {
                    Process(backBuffer, backAlphaBuffer, w);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Process(float* backBuffer, float* backAlphaBuffer, int count)
                {
                    // byteの値をそのままバッファへ格納
                    for (var i = 0; i < count; i++)
                    {
                        backBuffer[i] = ColorUtils.RgbToScRgb(backP[i]);
                        // alpha値は4の倍数ごとに同じ値になる
                        backAlphaBuffer[i] = backP[i / 4 * 4 + ColorUtils.ColorIndex_A];
                    }
                    // 正規化してベクトル化
                    var backVector = *(Vector<float>*)backBuffer;
                    var backAlphaVector = *(Vector<float>*)backAlphaBuffer * invertFactor;

                    BlendCore(ref backVector, backAlphaVector, frontVector, frontAlphaVector, blend);

                    // バッファへ書き戻す
                    *(Vector<float>*)backBuffer = backVector;
                    for (var i = 0; i < count; i += 4)
                    {
                        backP[i + ColorUtils.ColorIndex_B] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_B]);
                        backP[i + ColorUtils.ColorIndex_G] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_G]);
                        backP[i + ColorUtils.ColorIndex_R] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_R]);
                        backP[i + ColorUtils.ColorIndex_A] = ColorUtils.GetByte(backBuffer[i + ColorUtils.ColorIndex_A]);
                    }
                    backP += count;
                }
            });
        }

        public static void BlendFloat(float* pointer, int targetWidth, int width, int height, BlendFunc blend, Vector<float> color)
        {
            var count = Vector<float>.Count;
            var invertFactor = InvertFactor;
            var frontAlpha = FloatColor.FillAlphaToAll(color);
            targetWidth *= 4;
            width *= 4;

            Parallel.For(0, height, y =>
            {
                var w = width;
                var backP = (Vector<float>*)(pointer + y * targetWidth);
                for (; w >= count; w -= count, backP++)
                {
                    var back = *backP;
                    BlendCore(ref back, FloatColor.FillAlphaToAll(back), color, frontAlpha, blend);
                    *backP = back;
                }
                if (w is > 0)
                {
                    var back = VectorUtils.CreateRepeating(new ReadOnlySpan<float>(backP, w));
                    BlendCore(ref back, FloatColor.FillAlphaToAll(back), color, frontAlpha, blend);
                    var singleBack = (float*)backP;
                    for (var i = 0; i < w; i++, singleBack++)
                    {
                        *singleBack = back[i];
                    }
                }
            });
        }

        public static void BlendUIntUInt(uint* back, int backWidth, uint* front, int frontWidth, int width, int height, BlendFunc blend, Vector<float> colorCorrection)
        {
            // 定数ベクトル
            var count = Vector<float>.Count;
            var invertFactor = InvertFactor;
            width *= 4;

            Parallel.For(0, height, y =>
            {
                var w = width;
                // byteの各要素をfloatへ変換するために、ポインタをbyte*に変換
                var backP = (byte*)(back + y * backWidth);
                var frontP = (byte*)(front + y * frontWidth);
                // float変換用の作業バッファ
                var backBuffer = stackalloc float[count];
                var backAlphaBuffer = stackalloc float[count];
                var frontBuffer = stackalloc float[count];
                var frontAlphaBuffer = stackalloc float[count];

                for (; w >= count; w -= count)
                {
                    Process(backBuffer, backAlphaBuffer, frontBuffer, frontAlphaBuffer, count);
                }
                if (w is > 0)
                {
                    Process(backBuffer, backAlphaBuffer, frontBuffer, frontAlphaBuffer, w);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Process(float* backBuffer, float* backAlphaBuffer, float* frontBuffer, float* frontAlphaBuffer, int count)
                {
                    // byteの値をそのままバッファへ格納
                    for (var i = 0; i < count; i++)
                    {
                        backBuffer[i] = ColorUtils.RgbToScRgb(backP[i]);
                        frontBuffer[i] = ColorUtils.RgbToScRgb(frontP[i]);
                        // alpha値は4の倍数ごとに同じ値になる
                        var alphaIndex = i / 4 * 4 + ColorUtils.ColorIndex_A;
                        backAlphaBuffer[i] = backP[alphaIndex];
                        frontAlphaBuffer[i] = frontP[alphaIndex];
                    }
                    // 正規化してベクトル化
                    var backVector = *(Vector<float>*)backBuffer;
                    var backAlphaVector = *(Vector<float>*)backAlphaBuffer * invertFactor;
                    var frontVector = *(Vector<float>*)frontBuffer * colorCorrection;
                    var frontAlphaVector = *(Vector<float>*)frontAlphaBuffer * invertFactor * colorCorrection[ColorUtils.ColorIndex_A];

                    BlendCore(ref backVector, backAlphaVector, frontVector, frontAlphaVector, blend);

                    // バッファへ書き戻す
                    *(Vector<float>*)backBuffer = backVector;
                    for (var i = 0; i < count; i += 4)
                    {
                        backP[i + ColorUtils.ColorIndex_B] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_B]);
                        backP[i + ColorUtils.ColorIndex_G] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_G]);
                        backP[i + ColorUtils.ColorIndex_R] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_R]);
                        backP[i + ColorUtils.ColorIndex_A] = ColorUtils.GetByte(backBuffer[i + ColorUtils.ColorIndex_A]);
                    }
                    backP += count;
                    frontP += count;
                }
            });
        }

        public static void BlendUIntFloat(uint* back, int backWidth, float* front, int frontWidth, int width, int height, BlendFunc blend, Vector<float> colorCorrection)
        {
            // 定数ベクトル
            var count = Vector<float>.Count;
            var invertFactor = InvertFactor;
            frontWidth *= 4;
            width *= 4;

            Parallel.For(0, height, y =>
            {
                var w = width;
                // byteの各要素をfloatへ変換するために、ポインタをbyte*に変換
                var backP = (byte*)(back + y * backWidth);
                var frontP = (Vector<float>*)(front + y * frontWidth);
                // float変換用の作業バッファ
                var backBuffer = stackalloc float[count];
                var backAlphaBuffer = stackalloc float[count];

                for (; w >= count; w -= count, frontP++)
                {
                    var front = *frontP * colorCorrection;
                    Process(backBuffer, backAlphaBuffer, front, FloatColor.FillAlphaToAll(front), count);
                }
                if (w is > 0)
                {
                    var front = VectorUtils.CreateRepeating(new ReadOnlySpan<float>(frontP, w)) * colorCorrection;
                    Process(backBuffer, backAlphaBuffer, front, FloatColor.FillAlphaToAll(front), w);
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Process(float* backBuffer, float* backAlphaBuffer, Vector<float> front, Vector<float> frontAlpha, int count)
                {
                    // byteの値をそのままバッファへ格納
                    for (var i = 0; i < count; i++)
                    {
                        backBuffer[i] = ColorUtils.RgbToScRgb(backP[i]);
                        // alpha値は4の倍数ごとに同じ値になる
                        backAlphaBuffer[i] = backP[i / 4 * 4 + ColorUtils.ColorIndex_A];
                    }
                    // 正規化してベクトル化
                    var backVector = *(Vector<float>*)backBuffer;
                    var backAlphaVector = *(Vector<float>*)backAlphaBuffer * invertFactor;

                    BlendCore(ref backVector, backAlphaVector, front, frontAlpha, blend);

                    // バッファへ書き戻す
                    *(Vector<float>*)backBuffer = backVector;
                    for (var i = 0; i < count; i += 4)
                    {
                        backP[i + ColorUtils.ColorIndex_B] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_B]);
                        backP[i + ColorUtils.ColorIndex_G] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_G]);
                        backP[i + ColorUtils.ColorIndex_R] = ColorUtils.ScRgbToRgb(backBuffer[i + ColorUtils.ColorIndex_R]);
                        backP[i + ColorUtils.ColorIndex_A] = ColorUtils.GetByte(backBuffer[i + ColorUtils.ColorIndex_A]);
                    }
                    backP += count;
                }
            });
        }

        public static void BlendFloatUInt(float* back, int backWidth, uint* front, int frontWidth, int width, int height, BlendFunc blend, Vector<float> colorCorrection)
        {
            var count = Vector<float>.Count;
            var invertFactor = InvertFactor;
            backWidth *= 4;
            width *= 4;

            Parallel.For(0, height, y =>
            {
                var w = width;
                var backP = (Vector<float>*)(back + y * backWidth);
                var frontP = (byte*)(front + y * frontWidth);
                // float変換用の作業バッファ
                var frontBuffer = stackalloc float[count];
                var frontAlphaBuffer = stackalloc float[count];

                for (; w >= count; w -= count, backP++)
                {
                    var back = *backP;
                    var (front, frontAlpha) = GetFrontVector(frontBuffer, frontAlphaBuffer, count);
                    BlendCore(ref back, FloatColor.FillAlphaToAll(back), front, frontAlpha, blend);
                    *backP = back;
                }
                if (w is > 0)
                {
                    var back = VectorUtils.CreateRepeating(new ReadOnlySpan<float>(backP, w));
                    var (front, frontAlpha) = GetFrontVector(frontBuffer, frontAlphaBuffer, w);
                    BlendCore(ref back, FloatColor.FillAlphaToAll(back), front, frontAlpha, blend);
                    var singleBack = (float*)backP;
                    for (var i = 0; i < w; i++, singleBack++)
                    {
                        *singleBack = back[i];
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                (Vector<float>, Vector<float>) GetFrontVector(float* frontBuffer, float* frontAlphaBuffer, int count)
                {
                    // byteの値をそのままバッファへ格納
                    for (var i = 0; i < count; i++)
                    {
                        frontBuffer[i] = ColorUtils.RgbToScRgb(frontP[i]);
                        // alpha値は4の倍数ごとに同じ値になる
                        frontAlphaBuffer[i] = frontP[i / 4 * 4 + ColorUtils.ColorIndex_A];
                    }
                    // 正規化してベクトル化
                    var frontVector = *(Vector<float>*)frontBuffer * colorCorrection;
                    var frontAlphaVector = *(Vector<float>*)frontAlphaBuffer * invertFactor * colorCorrection[ColorUtils.ColorIndex_A];
                    frontP += count;
                    return (frontVector, frontAlphaVector);
                }
            });
        }

        public static void BlendFloatFloat(float* back, int backWidth, float* front, int frontWidth, int width, int height, BlendFunc blend, Vector<float> colorCorrection)
        {
            var count = Vector<float>.Count;
            backWidth *= 4;
            frontWidth *= 4;
            width *= 4;

            Parallel.For(0, height, y =>
            {
                var w = width;
                var backP = (Vector<float>*)(back + y * backWidth);
                var frontP = (Vector<float>*)(front + y * frontWidth);
                for (; w >= count; w -= count, backP++, frontP++)
                {
                    var back = *backP;
                    var front = *frontP * colorCorrection;
                    BlendCore(ref back, FloatColor.FillAlphaToAll(back), front, FloatColor.FillAlphaToAll(front), blend);
                    *backP = back;
                }
                if (w is > 0)
                {
                    var back = VectorUtils.CreateRepeating(new ReadOnlySpan<float>(backP, w));
                    var front = VectorUtils.CreateRepeating(new ReadOnlySpan<float>(frontP, w)) * colorCorrection;
                    BlendCore(ref back, FloatColor.FillAlphaToAll(back), front, FloatColor.FillAlphaToAll(front), blend);
                    var singleBack = (float*)backP;
                    for (var i = 0; i < w; i++, singleBack++)
                    {
                        *singleBack = back[i];
                    }
                }
            });
        }
    }
}
