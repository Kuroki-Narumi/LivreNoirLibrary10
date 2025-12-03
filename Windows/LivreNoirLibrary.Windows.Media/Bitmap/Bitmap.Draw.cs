using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public static unsafe partial class Bitmap
    {
        extension (WriteableBitmap bitmap)
        {
            public void Fill(Color color)
            {
                using var p = bitmap.BeginWrite();
                p.Fill(color.ToLnColor());
            }

            public void Fill(Int32Rect rect, Color color)
            {
                using var p = bitmap.BeginWrite();
                p.Fill(rect.ToDrawingRect(), color.ToLnColor());
            }

            public void Fill(Int32Rect rect, Color color1, Color color2, bool vertical = false)
            {
                using var p = bitmap.BeginWrite();
                p.Fill(rect.ToDrawingRect(), color1.ToLnColor(), color2.ToLnColor(), vertical);
            }

            public void FillTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
            {
                using var p = bitmap.BeginWrite();
                p.FillTriangle(new(x0, y0, x1, y1, x2, y2), color.ToLnColor());
            }

            public void FillTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Color color1, Color color2, bool radial = false)
            {
                using var p = bitmap.BeginWrite();
                p.FillTriangle(new(x0, y0, x1, y1, x2, y2), color1.ToLnColor(), color2.ToLnColor(), radial);
            }

            public void DrawBorder(int thickness, Color color, bool keepSource = false, UnmanagedArray<uint>? buffer = null)
            {
                using var p = bitmap.BeginWrite();
                p.DrawBorder(thickness, color.ToLnColor(), keepSource, buffer);
            }

            public void DrawBorderSimple(int thickness, Color color, UnmanagedArray<uint>? buffer = null)
            {
                using var p = bitmap.BeginWrite();
                p.DrawBorderSimple(thickness, color.ToLnColor(), buffer);
            }
        }
    }
}
