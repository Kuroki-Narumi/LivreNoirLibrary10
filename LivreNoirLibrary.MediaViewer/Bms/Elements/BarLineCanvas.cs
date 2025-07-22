using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BarLineCanvas : BmsCanvasBase
    {
        [DependencyProperty]
        private Rational _smallGrid = BmsViewModel.DefaultSmallGrid;
        [DependencyProperty]
        private Rational _largeGrid = BmsViewModel.DefaultLargeGrid;
        [DependencyProperty(AffectsRender = true)]
        private Color _barLineColor = Colors.BarLine;
        [DependencyProperty(AffectsRender = true)]
        private Color _beatLineColor = Colors.BeatLine;
        [DependencyProperty(AffectsRender = true)]
        private Color _subBeatLineColor = Colors.SubBeatLine;

        private bool _need_refresh_line;
        private readonly List<double> _head_pos_list = [];
        private readonly List<double> _line_pos_list = [];
        private readonly List<BarLineType> _line_type_list = [];
        private (int Start, int Length) _head_range;
        private (int Start, int Length) _line_range;

        protected override void OnViewModelChanged(BmsViewModel? oldValue, BmsViewModel? newValue)
        {
            if (oldValue is not null)
            {
                oldValue.RequestRefreshBar -= OnRequestRefresh;
            }
            BindingOperations.ClearBinding(this, SmallGridProperty);
            BindingOperations.ClearBinding(this, LargeGridProperty);
            if (newValue is not null)
            {
                newValue.RequestRefreshBar += OnRequestRefresh;
                SetBinding(SmallGridProperty, new Binding(nameof(SmallGrid)) { Mode = BindingMode.TwoWay, Source = newValue });
                SetBinding(LargeGridProperty, new Binding(nameof(LargeGrid)) { Mode = BindingMode.TwoWay, Source = newValue });
                OnScaleYChanged();
            }
        }

        private void OnRequestRefresh(object? sender, EventArgs e)
        {
            _need_refresh_line = true;
            ReserveViewportRefresh();
        }

        protected override void OnScaleYChanged()
        {
            _need_refresh_line = true;
            base.OnScaleYChanged();
        }

        private void RefreshLinesIfNeeded()
        {
            if (_need_refresh_line)
            {
                _need_refresh_line = false;
                var headPos = _head_pos_list;
                var linePos = _line_pos_list;
                var lineType = _line_type_list;
                headPos.Clear();
                linePos.Clear();
                lineType.Clear();
                ViewModel?.RefreshLinePositions(headPos, linePos, lineType, ScaleY);
            }
        }

        protected override void RefreshVisible()
        {
            RefreshLinesIfNeeded();
            var end = Bottom - _vy;
            var start = end - _vh;
            _head_range = _head_pos_list.IndexRange(new Range<double>(start - NumberFontSize, end, true));
            _line_range = _line_pos_list.IndexRange(new Range<double>(start, end, true));
        }

        public const double NumberFontSize = 80;

        private static readonly FormattedTextOption _ft_options = new()
        {
            FontSize = NumberFontSize,
            FontStyle = FontStyles.Italic,
            FontWeight = FontWeights.Bold,
            Foreground = MediaUtils.GetBrush(Colors.BarText),
        };

        private static readonly Dictionary<int, RenderTargetBitmap> _number_cache = [];

        public static RenderTargetBitmap GetNumberBitmap(int number)
        {
            if (!_number_cache.TryGetValue(number, out var bitmap))
            {
                var ft = MediaUtils.CreateFormattedText(number.GetBarText(), _ft_options);
                DrawingVisual visual = new();
                using (var ctx = visual.RenderOpen())
                {
                    ctx.DrawText(ft, new(0, 0));
                }
                bitmap = new((int)(ft.Width + NumberFontSize / 4), (int)ft.Height, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(visual);
                bitmap.Freeze();
                _number_cache.Add(number, bitmap);
            }
            return bitmap;
        }

        protected override void OnRender(DrawingContext ctx)
        {
            base.OnRender(ctx);
            var vw = _vw;
            var bottom = Bottom;

            var headPos = _head_pos_list;
            var (s, l) = _head_range;
            var e = s + l;
            for (; s < e; s++)
            {
                var x = vw / 2;
                var y = (bottom - headPos[s]).RoundToInt();
                var bitmap = GetNumberBitmap(s);
                var w = bitmap.Width;
                var h = bitmap.Height;
                ctx.DrawImage(bitmap, new(x - w / 2, y - h, w, h));
            }

            var linePos = _line_pos_list;
            var lineType = _line_type_list;
            (s, l) = _line_range;
            e = s + l;
            var brushes = ArrayPool<SolidColorBrush>.Shared.Rent(3);
            try
            {
                brushes[0] = MediaUtils.GetBrush(BarLineColor);
                brushes[1] = MediaUtils.GetBrush(BeatLineColor);
                brushes[2] = MediaUtils.GetBrush(SubBeatLineColor);
                for (var i = s; i < e; i++)
                {
                    var y = (bottom - linePos[i]).RoundToInt() - 1;
                    ctx.DrawRectangle(brushes[(int)lineType[i]], null, new(0, y, vw, 1));
                }
            }
            finally
            {
                ArrayPool<SolidColorBrush>.Shared.Return(brushes);
            }
        }

        public (int Number, Rational Position, double ActualPosition) GetHeadPosition(double y)
        {
            RefreshLinesIfNeeded();
            var heads = _head_pos_list;
            var index = heads.BinarySearch(y);
            if (index < 0)
            {
                index = ~index;
            }
            if ((uint)index < (uint)heads.Count)
            {
                var info = ViewModel!.GetBarLineInfo(index);
                return (index, info.Position, heads[index]);
            }
            else
            {
                return (0, Rational.Zero, 0);
            }
        }
    }
}
