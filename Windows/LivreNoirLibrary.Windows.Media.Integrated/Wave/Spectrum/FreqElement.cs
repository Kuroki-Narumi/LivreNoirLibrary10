using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class SpectrumView
    {
        private class FreqElement(SpectrumView owner) : UIElement
        {
            private readonly SpectrumView _owner = owner;

            protected override void OnRender(DrawingContext dc)
            {
                base.OnRender(dc);
                var w = _owner.ActualWidth;
                var h = _owner.ActualHeight;
                var maxRate = _owner._sampleRate / 2.0;
                var den = Math.Log2(maxRate) / _owner._maxFreqPosition;
                var grids = _owner._freqGrids.AsSpan();
                for (var i = 0; i < grids.Length; i++)
                {
                    var grid = grids[i];
                    if (w < grid.Threshold)
                    {
                        break;
                    }
                    var x = Math.Round((grid.X - den + 1) * w);
                    if (x is >= 0 && x < w)
                    {
                        dc.DrawRectangle(WaveBrushes.FreqLineDashed, null, new(x, 0, 1, h));
                        RectangularText.Render(dc, x + 1, h - 12, grid.Text, WaveBrushes.FreqText, WaveBrushes.TextOutline);
                    }
                }
            }
        }
    }
}
