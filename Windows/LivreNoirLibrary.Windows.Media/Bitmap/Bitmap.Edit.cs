using LivreNoirLibrary.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class Bitmap
    {
        extension (WriteableBitmap bitmap)
        {
            public void SetColor(ColorFlags index, byte value)
            {
                using var p = bitmap.BeginWrite();
                p.SetColor(index, value);
            }

            public void SetColor(Int32Rect rect, ColorFlags index, byte value)
            {
                using var p = bitmap.BeginWrite();
                p.SetColor(rect.ToDrawingRect(), index, value);
            }

            public void SetColor(ColorIndex from, ColorIndex to)
            {
                if (to == from) { return; }
                using var p = bitmap.BeginWrite();
                p.SetColor(from, to);
            }

            public void SetColor(Int32Rect rect, ColorIndex from, ColorIndex to)
            {
                if (to == from) { return; }
                using var p = bitmap.BeginWrite();
                p.SetColor(rect.ToDrawingRect(), from, to);
            }

            public void SwapColor(ColorIndex index1, ColorIndex index2)
            {
                if (index1 == index2) { return; }
                using var p = bitmap.BeginWrite();
                p.SwapColor(index1, index2);
            }

            public void SwapColor(Int32Rect rect, ColorIndex index1, ColorIndex index2)
            {
                if (index1 == index2) { return; }
                using var p = bitmap.BeginWrite();
                p.SwapColor(rect.ToDrawingRect(), index1, index2);
            }

            public void InvertColor(ColorFlags flags = ColorFlags.RGB)
            {
                using var p = bitmap.BeginWrite();
                p.InvertColor(flags);
            }

            public void InvertColor(Int32Rect rect, ColorFlags flags = ColorFlags.RGB)
            {
                using var p = bitmap.BeginWrite();
                p.InvertColor(rect.ToDrawingRect(), flags);
            }

            public void SetTransparent(Color color)
            {
                using var p = bitmap.BeginWrite();
                p.SetTransparent(color.ToLnColor());
            }

            public void SetTransparent(Int32Rect rect, Color color)
            {
                using var p = bitmap.BeginWrite();
                p.SetTransparent(rect.ToDrawingRect(), color.ToLnColor());
            }
        }
    }
}