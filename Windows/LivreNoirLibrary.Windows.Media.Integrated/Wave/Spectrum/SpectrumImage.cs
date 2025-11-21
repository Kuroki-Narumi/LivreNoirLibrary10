using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;

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
            if (!TryGetBitmapPointer(out var b) || _owner.SpectrumData is not { } data)
            {
                return;
            }
            data.Update(_owner.SamplePosition);
            try
            {
                var bitmap = b.ToBitmapData();
                var w = (int)RequiredWidth;
                var count = 0;
                var channels = Math.Min(data.Channels, 2);
                // 描画用のピクセルデータ
                var colors = stackalloc uint[2];
                colors[0] = ColorUtils.Mask_R | ColorUtils.Mask_A;
                colors[1] = ColorUtils.Mask_B | ColorUtils.Mask_A;

                foreach (var (y, height, dataCount) in _posList.AsSpan())
                {
                    var current = bitmap.Offset(y);
                    SimdOperations.Clear(current, w);
                    for (var c = 0; c < channels; c++)
                    {
                        var length = (Math.Clamp(data.Range(c, count, dataCount).Max(), 0, 1) * w).RoundToInt();
                        SimdOperations.Or(current, colors[c], w);
                    }
                    if (height is > 1)
                    {
                        for (var oy = 1; oy < height; oy++)
                        {
                            SimdOperations.CopyFrom(bitmap.Offset(y + oy), current, w);
                        }
                    }
                    count += dataCount;
                }
            }
            finally
            {
                b.Dispose();
            }
        }

        private readonly record struct PosData(int Y, int Height, int DataCount);
    }
}
