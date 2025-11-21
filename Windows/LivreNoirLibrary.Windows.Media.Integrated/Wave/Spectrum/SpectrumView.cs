using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Controls.Wave;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class SpectrumView : Canvas, IWaveSourceProperty, ISamplePositionProperty, ISpectrumProvider
    {
        public const double DefaultMinLevel = -88.0;
        public const double DefaultMaxLevel = 0.0;
        public const double DefaultFactor = 0.5;
        public static readonly int DefaultWindowWidth = int.Log2(FFT.DefaultWindowWidth);

        public static readonly DependencyProperty SourceProperty = WaveProperties.SourceProperty.AddOwner(typeof(SpectrumView));
        public static readonly DependencyProperty SamplePositionProperty = WaveProperties.SamplePositionProperty.AddOwner(typeof(SpectrumView));

        static SpectrumView()
        {
            PropertyUtils.OverrideDefaultStyleKey<SpectrumView>();
        }

        private static int CoerceWindowWidth(int value) => Math.Clamp(value, 1, 31);

        private IWaveBuffer? _source;
        private long _samplePosition;
        [DependencyProperty]
        private double _minLevel = DefaultMinLevel;
        [DependencyProperty]
        private double _maxLevel = DefaultMaxLevel;
        [DependencyProperty]
        private double _factor = DefaultFactor;
        [DependencyProperty]
        private int _windowWidth = DefaultWindowWidth;
        private int _ww_full = FFT.DefaultWindowWidth;
        private int _sampleRate = WaveBuffer.DefaultSampleRate;

        private readonly SpectrumImage _main;
        private readonly FreqElement _freq;
        private readonly GainElement _gain;

        private readonly List<double> _freqPositions = [];
        private double _maxFreqPosition;
        private readonly List<FreqGrid> _freqGrids = [];

        public Spectrum? SpectrumData { get; private set; }
        public IWaveBuffer? Source { get => _source; set => SetValue(SourceProperty, value); }
        public long SamplePosition { get => _samplePosition; set => SetValue(SamplePositionProperty, value); }
        public double LevelRange => _maxLevel - _minLevel;

        public SpectrumView()
        {
            ClipToBounds = true;
            _main = new(this);
            _freq = new(this);
            _gain = new(this);
            Children.Add(_main);
            Children.Add(_freq);
            Children.Add(_gain);
            RefreshFreqPosition();
            _main.SetBinding(WidthProperty, new Binding(nameof(ActualWidth)) { Source = this, Mode = BindingMode.OneWay });
            _main.SetBinding(HeightProperty, new Binding(nameof(ActualHeight)) { Source = this, Mode = BindingMode.OneWay });
        }

        private void OnMinLevelChanged() => UpdateLevel();
        private void OnMaxLevelChanged() => UpdateLevel();
        private void OnFactorChanged() => SpectrumData?.Factor = _factor;

        private void UpdateLevel()
        {
            if (SpectrumData is { } data)
            {
                data.MaxLevel = _maxLevel;
                data.MinLevel = _minLevel;
            }
            _gain.InvalidateVisual();
            _main.ReserveRefresh();
        }

        private void OnWindowWidthChanged(int value)
        {
            _ww_full = 1 << value;
            RefreshFreqPosition();
        }

        void IWaveSourceProperty.OnSourceChanged(IWaveBuffer? value)
        {
            _source = value;
            if (value is not null)
            {
                LoadData(value);
            }
            else
            {
                SpectrumData = null;
            }
        }

        void ISamplePositionProperty.OnSamplePositionChanged(long value)
        {
            _samplePosition = value;
            _main.ReserveRefresh();
        }

        public void LoadData(IWaveBuffer data)
        {
            var spc = SpectrumData = new(data, _ww_full)
            {
                Factor = _factor,
            };
            _sampleRate = spc.SampleRate;
            spc.Update(_samplePosition);
            UpdateLevel();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (sizeInfo.HeightChanged)
            {
                _gain.InvalidateVisual();
            }
            _freq.InvalidateVisual();
        }

        private void RefreshFreqPosition()
        {
            var list = _freqPositions;
            list.Clear();
            var den = _maxFreqPosition = _windowWidth - 1;
            var limit = _ww_full / 2;
            for (var i = 1; i < limit; i++)
            {
                list.Add(Math.Log2(i) / den);
            }

            var list2 = _freqGrids;
            list2.Clear();
            const double gridMax = 192000;
            void Create(double f, double mul, double th)
            {
                for (; f <= gridMax; f *= mul)
                {
                    list2.Add(new(f, th, den));
                }
            }
            /*
            Create(1.0, 10, 0);
            Create(3.0, 10, 300);
            Create(1.5, 10, 600);
            Create(5.0, 10, 600);
            Create(2.0, 10, 1200);
            Create(7.0, 10, 1200);
            //*/
            //*
            Create(440.0 / 64.0, 4, 0);
            Create(440.0 / 32.0, 4, 360);
            Create(660.0 / 64.0, 2, 720);
            Create(550.0 / 64.0, 2, 1440);
            //*/
        }

        public ReadOnlySpan<double> GetFrequencyPositions() => _freqPositions.AsSpan();
    }
}
