using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class SelectedNoteCanvas : NoteCanvasBase<SelectedRect>
    {
        public Rational OffsetY { get; set; }

        private int _pivot_index;
        private int _index_offset_min;
        private int _index_offset_max;
        private Rational _offset_y_min;

        public SelectedNoteCanvas()
        {
            Visibility = Visibility.Collapsed;
            Opacity = 0.8;
        }

        protected override void RenderItem(DrawingContext drawingContext, SelectedRect item)
        {
            item.Render(drawingContext, SelectedColor, SelectedLongColor);
        }

        public void BeginMove(LaneIndexConverter converter, SelectableNoteCanvas source, NoteRect pivot)
        {
            Clear();
            var min = int.MaxValue;
            var max = 0;
            Rational yMin = Rational.MaxValue;
            foreach (var item in CollectionsMarshal.AsSpan(source._children))
            {
                if (item.IsSelected)
                {
                    if (!item.IsConductor)
                    {
                        var index = converter.Lane2Index(item.Lane);
                        if (index < min)
                        {
                            min = index;
                        }
                        if (index > max)
                        {
                            max = index;
                        }
                    }
                    if (item.ActualPosition < yMin)
                    {
                        yMin = item.ActualPosition;
                    }
                    _children.Add(new(item));
                }
            }
            _pivot_index = converter.Lane2Index(pivot.Lane);
            _index_offset_min = converter.ConductorLaneCount - min;
            _index_offset_max = converter.Count - max - 1;
            OffsetY = Rational.Zero;
            _offset_y_min = -yMin;
            RefreshVertical();
        }

        public void SetOffset(Rational offset)
        {
            if (offset < _offset_y_min)
            {
                offset = _offset_y_min;
            }
            if (OffsetY != offset)
            {
                OffsetY = offset;
                var sy = _scale_y;
                var bottom = _bottom;
                foreach (var item in CollectionsMarshal.AsSpan(_children))
                {
                    item.SetOffsetY(offset, bottom, sy);
                }
                ReserveViewportRefresh();
            }
        }

        public void RefreshX(LaneIndexConverter converter, double x, int scale)
        {
            var indexOffset = Math.Clamp(converter.Pos2Index(x) - _pivot_index, _index_offset_min, _index_offset_max);
            foreach (var item in CollectionsMarshal.AsSpan(_children))
            {
                var lane = item.InitialLane;
                if (!item.IsConductor)
                {
                    var index = converter.Lane2Index(lane);
                    lane = converter.Index2Lane(index + indexOffset);
                }
                if (converter.TryGetLane2Info(lane, out var newX, out var info))
                {
                    item.SetOffsetX(lane, newX, info.Width * scale);
                }
                else
                {
                    item.SetOffsetX(lane, _vw, 0);
                }
            }
            ReserveViewportRefresh();
        }

        public Dictionary<int, int> GetLaneMap()
        {
            Dictionary<int, int> result = [];
            foreach (var item in CollectionsMarshal.AsSpan(_children))
            {
                if (item.InitialLane != item.Lane)
                {
                    result.TryAdd(item.InitialLane, item.Lane);
                }
            }
            return result;
        }
    }
}
