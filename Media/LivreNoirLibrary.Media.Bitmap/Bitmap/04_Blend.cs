using System;
using System.Drawing;
using System.Numerics;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        public static bool Adjust<T1, T2>(T1 source, T2 destination, ref Rectangle sourceRect, ref Point destLocation)
            where T1 : IBitmap
            where T2 : IBitmap
        {
            return source.Pointer is not 0 && destination.Pointer is not 0 && Structs.Adjust(ref sourceRect, ref destLocation, source.Width, source.Height, destination.Width, destination.Height);
        }

        extension<T>(T bitmap) where T : IBitmap
        {
            public void Blend(Rectangle destRect, BlendMode mode, LnColor color)
            {
                if (ColorBlend.TryGetBlendFunc(mode, out var func) && bitmap.Adjust(ref destRect))
                {
                    if (bitmap.IsFloat)
                    {
                        ColorBlend.BlendFloat((float*)bitmap.Offset(destRect.X, destRect.Y), bitmap.Width, destRect.Width, destRect.Height, func, color.ToFloatColor());
                    }
                    else
                    {
                        ColorBlend.BlendUInt((uint*)bitmap.Offset(destRect.X, destRect.Y), bitmap.Width, destRect.Width, destRect.Height, func, color);
                    }
                }
            }

            public void Blend(BlendMode mode, LnColor color) => bitmap.Blend(bitmap.Rect, mode, color);

            public void Blend(Rectangle destRect, BlendMode mode, FloatColor color)
            {
                if (ColorBlend.TryGetBlendFunc(mode, out var func) && bitmap.Adjust(ref destRect))
                {
                    if (bitmap.IsFloat)
                    {
                        ColorBlend.BlendFloat((float*)bitmap.Offset(destRect.X, destRect.Y), bitmap.Width, destRect.Width, destRect.Height, func, color);
                    }
                    else
                    {
                        ColorBlend.BlendUInt((uint*)bitmap.Offset(destRect.X, destRect.Y), bitmap.Width, destRect.Width, destRect.Height, func, color.ToByteColor());
                    }
                }
            }

            public void Blend(BlendMode mode, FloatColor color) => bitmap.Blend(bitmap.Rect, mode, color);
  
            public void Blend<TSource>(TSource source, Rectangle sourceRect, Point destLocation, BlendMode mode, Vector<float> colorCorrection, bool tweet = false)
                where TSource : IBitmap
            {
                if (ColorBlend.TryGetBlendFunc(mode, out var func) && Adjust(source, bitmap, ref sourceRect, ref destLocation))
                {
                    if (tweet)
                    {
                        Console.WriteLine("Blend");
                    }
                    var t0 = System.Diagnostics.Stopwatch.GetTimestamp();
                    var back = bitmap.Offset(destLocation.X, destLocation.Y);
                    var front = source.Offset(sourceRect.X, sourceRect.Y);
                    var backW = bitmap.Width;
                    var frontW = source.Width;
                    var width = sourceRect.Width;
                    var height = sourceRect.Height;

                    if (bitmap.IsFloat)
                    {
                        if (source.IsFloat)
                        {
                            ColorBlend.BlendFloatFloat((float*)back, backW, (float*)front, frontW, width, height, func, colorCorrection);
                        }
                        else
                        {
                            ColorBlend.BlendFloatUInt((float*)back, backW, (uint*)front, frontW, width, height, func, colorCorrection);
                        }
                    }
                    else if (source.IsFloat)
                    {
                        ColorBlend.BlendUIntFloat((uint*)back, backW, (float*)front, frontW, width, height, func, colorCorrection);
                    }
                    else
                    {
                        ColorBlend.BlendUIntUInt((uint*)back, backW, (uint*)front, frontW, width, height, func, colorCorrection);
                    }

                    if (tweet)
                    {
                        Console.WriteLine($"  processed in {(double)(System.Diagnostics.Stopwatch.GetTimestamp() - t0) / TimeSpan.TicksPerMillisecond:F3}ms");
                    }
                }
            }

            public void Blend<TSource>(TSource source, BlendMode mode, bool tweet = false) where TSource : IBitmap
                => bitmap.Blend(source, source.Rect, new(0, 0), mode, Vector<float>.One, tweet);

            public void Blend<TSource>(TSource source, Rectangle sourceRect, BlendMode mode, bool tweet = false) where TSource : IBitmap
                => bitmap.Blend(source, sourceRect, new(0, 0), mode, Vector<float>.One, tweet);

            public void Blend<TSource>(TSource source, Point destLocation, BlendMode mode, bool tweet = false) where TSource : IBitmap
                => bitmap.Blend(source, source.Rect, destLocation, mode, Vector<float>.One, tweet);

            public void Blend<TSource>(TSource source, BlendMode mode, Vector<float> colorCorrection, bool tweet = false) where TSource : IBitmap
                => bitmap.Blend(source, source.Rect, new(0, 0), mode, colorCorrection, tweet);

            public void Blend<TSource>(TSource source, Rectangle sourceRect, BlendMode mode, Vector<float> colorCorrection, bool tweet = false) where TSource : IBitmap
                => bitmap.Blend(source, sourceRect, new(0, 0), mode, colorCorrection, tweet);

            public void Blend<TSource>(TSource source, Point destLocation, BlendMode mode, Vector<float> colorCorrection, bool tweet = false) where TSource : IBitmap
                => bitmap.Blend(source, source.Rect, destLocation, mode, colorCorrection, tweet);
        }
    }
}
