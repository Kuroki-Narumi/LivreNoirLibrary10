using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Controls.Wave;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class UnuniformedWaveImage : WriteableBitmapView, IPixelOffsetProperty
    {
        public static readonly Color DefaultColor = Colors.WaveForm;
        public static readonly Color DefaultHighLevelColor = Color.FromArgb(192, 255, 0, 0);
        public static readonly Color DefaultLowLevelColor = Color.FromArgb(192, 0, 192, 128);

        public static readonly DependencyProperty PixelOffsetProperty = WaveProperties.PixelOffsetProperty.AddOwner(typeof(WaveImage));

        private double _pixelOffset;
        [DependencyProperty]
        private TimeCounter? _timeCounter;
        /// <summary>
        /// Represents pixels per whole note.
        /// </summary>
        [DependencyProperty]
        private double _scaleY = IScaleProperty.DefaultScale;
        [DependencyProperty]
        private double _bottom;
        [DependencyProperty]
        private Color _color = DefaultColor;
        [DependencyProperty]
        private bool _showLevelLine;
        [DependencyProperty]
        private double _highLevel;
        [DependencyProperty]
        private double _lowLevel;
        [DependencyProperty]
        private Color _highLevelColor = DefaultHighLevelColor;
        [DependencyProperty]
        private Color _lowLevelColor = DefaultLowLevelColor;

        private double _lastPixelOffset = -1;
        private double _lastHeight = -1;
        private readonly List<UnuniformedWaveImageData> _children = [];

        public double PixelOffset { get => _pixelOffset; set => SetValue(PixelOffsetProperty, value); }

        void IPixelOffsetProperty.OnPixelOffsetChanged(double value)
        {
            _pixelOffset = value;
            ReserveRefresh_Partial();
        }

        public override void ReserveRefresh()
        {
            _lastPixelOffset = -1;
            base.ReserveRefresh();
        }

        public void ReserveRefresh_Partial() => base.ReserveRefresh();

        protected override void Refresh()
        {
            if (!TryGetBitmapPointer(out var b))
            {
                return;
            }
            var offset = _pixelOffset;
            try
            {
                var bitmap = b.ToBitmapData();
                var h = (int)RequiredHeight;
                WaveImage.AdjustRefreshArea((int)_lastHeight, h, offset, _lastPixelOffset, bitmap, out var top, out var bottom);
            }
            finally
            {
                b.Dispose();
                _lastHeight = RequiredHeight;
                _lastPixelOffset = offset;
            }
        }
    }
}