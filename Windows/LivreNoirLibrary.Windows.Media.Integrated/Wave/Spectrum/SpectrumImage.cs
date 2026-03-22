using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Wave
{
    public class SpectrumImage : LeftToRightBitmapView
    {
        private readonly ISpectrumProvider _owner;
        private readonly List<PosData> _posList = [];

        public SpectrumImage(ISpectrumProvider provider)
        {
            _owner = provider;
            Opacity = 0.8;
        }

        protected override void OnRequiredWidthChanged(double value)
        {
            ReserveRefresh();
        }

        protected override void OnRequiredHeightChanged(double value)
        {
            RefreshPosData();
            ReserveRefresh();
        }

        public void RefreshPosData()
        {
            var posList = _posList;
            posList.Clear();
            var lastPos = 0;
            var count = 1;
            void Add(double pos)
            {
                var p = (int)pos;
                if (lastPos != pos)
                {
                    posList.Add(new(p, p - lastPos, count));
                    lastPos = p;
                    count = 0;
                }
            }
            var h = RequiredHeight;
            var poss = _owner.GetFrequencyPositions();
            for (var i = 1; i < poss.Length; i++)
            {
                Add(poss[i] * h);
                count++;
            }
            Add(h);
        }

        protected override unsafe void Refresh()
        {
            if (Bitmap is not { } b || _owner.SpectrumData is not { } data)
            {
                return;
            }
            data.Update(_owner.SamplePosition);
            using var bitmap = b.BeginWrite();
            var w = (nuint)RequiredWidth;
            var stride = bitmap.Width;
            var count = 0;
            var channels = Math.Min(data.Channels, 2);
            // 描画用のピクセルデータ
            var colors = stackalloc uint[2];
            colors[0] = ColorUtils.GetMask(ColorFlags.R | ColorFlags.A);
            colors[1] = ColorUtils.GetMask(ColorFlags.B | ColorFlags.A);

            foreach (var (y, height, dataCount) in _posList.AsSpan())
            {
                var current = (uint*)bitmap.Offset(y);
                SimdOperations.Clear(current, w);
                for (var c = 0; c < channels; c++)
                {
                    var length = (Math.Clamp(data.Range(c, count, dataCount).Max(), 0, 1) * w).RoundToInt();
                    SimdOperations.Or(current, colors[c], w);
                }
                if (height is > 1)
                {
                    var target = (uint*)bitmap.Offset(y + 1);
                    for (var oy = 1; oy < height; oy++, target += stride)
                    {
                        SimdOperations.CopyFrom(target, current, w);
                    }
                }
                count += dataCount;
            }
        }

        private readonly record struct PosData(int Y, int Height, int DataCount);
    }
}
