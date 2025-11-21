using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class SpectrumView
    {
        private class GainElement(SpectrumView owner) : UIElement
        {
            private readonly SpectrumView _owner = owner;

            protected override void OnRender(DrawingContext dc)
            {
                base.OnRender(dc);

                var w = SystemParameters.VirtualScreenWidth;
                var h = _owner.ActualHeight;
                var minLevel = _owner._minLevel;
                var range = _owner.LevelRange;
                if (range is > 0)
                {
                    var step = h switch
                    {
                        <= 100 => 48d,
                        <= 200 => 24d,
                        <= 400 => 12d,
                        <= 800 => 6d,
                        _ => 3d,
                    };
                    for (var l = 0d; l >= minLevel; l -= step)
                    {
                        var y = Math.Round((minLevel - l) / range * h);
                        dc.DrawRectangle(WaveBrushes.GainDashed, null, new(0, y, w, 1));
                        RectangularText.Render(dc, 2, y - 5, $"{l}", WaveBrushes.GainText, WaveBrushes.TextOutline);
                    }
                }
            }
        }
    }
}
