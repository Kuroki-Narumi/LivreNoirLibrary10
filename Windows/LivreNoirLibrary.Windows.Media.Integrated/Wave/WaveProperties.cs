using System;
using System.Windows;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Wave;

namespace LivreNoirLibrary.Windows.Media
{
    public class WaveProperties : DependencyObject
    {
        public static readonly DependencyProperty PlayerProperty = PropertyUtils.RegisterAttachedTwoWay<WavePlayer>(typeof(WaveProperties), null, OnPlayerChanged);
        public static readonly DependencyProperty SourceProperty = PropertyUtils.RegisterAttachedTwoWay<IWaveBuffer>(typeof(WaveProperties), null, OnSourceChanged);

        public static readonly DependencyProperty SamplePositionProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(WaveProperties), 0L, OnSamplePositionChanged);
        public static readonly DependencyProperty SelectionProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(WaveProperties), default(WaveSelection), OnSelectionChanged);

        public static readonly DependencyProperty VolumeProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(WaveProperties), (double)AudioPlayerBase.DefaultVolume, OnVolumeChanged, CoerceVolumeProperty);
        public static readonly DependencyProperty IsPlayingProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(WaveProperties), false, OnIsPlayingChanged);

        public static readonly DependencyProperty PixelOffsetProperty = PropertyUtils.RegisterAttached(typeof(WaveProperties), 0d, OnPixelOffsetChanged);

        public static void OnPlayerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IWavePlayerProperty)?.OnPlayerChanged(e.OldValue as WavePlayer, e.NewValue as WavePlayer);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IWaveSourceProperty)?.OnSourceChanged(e.NewValue as WaveBuffer);
        }

        private static void OnSamplePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ISamplePositionProperty)?.OnSamplePositionChanged((long)e.NewValue);
        }

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IWaveSelectionProperty)?.OnSelectionChanged((WaveSelection)e.NewValue);
        }

        private static void OnVolumeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IVolumeProperty)?.OnVolumeChanged((double)e.NewValue);
        }

        private static object CoerceVolumeProperty(DependencyObject d, object baseValue)
        {
            var value = (double)baseValue;
            if (value is < AudioPlayerBase.MinVolume)
            {
                return AudioPlayerBase.MinVolume;
            }
            else if (value is > AudioPlayerBase.MaxVolume)
            {
                return AudioPlayerBase.MaxVolume;
            }
            return baseValue;
        }

        private static void OnIsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IIsPlayingProperty)?.OnIsPlayingChanged((bool)e.NewValue);
        }

        private static void OnPixelOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IPixelOffsetProperty)?.OnPixelOffsetChanged((double)e.NewValue);
        }
    }

    public interface IWavePlayerProperty
    {
        WavePlayer? Player { get; }
        void OnPlayerChanged(WavePlayer? oldValue, WavePlayer? newValue) { }
    }

    public interface IWaveSourceProperty
    {
        IWaveBuffer? Source { get; set; }
        void OnSourceChanged(IWaveBuffer? value) { }
    }

    public interface ISamplePositionProperty
    {
        long SamplePosition { get; set; }
        void OnSamplePositionChanged(long value) { }
    }

    public interface IWaveSelectionProperty
    {
        WaveSelection Selection { get; set; }
        void OnSelectionChanged(in WaveSelection value) { }
    }

    public interface IVolumeProperty
    {
        double Volume { get; set; }
        void OnVolumeChanged(double value) { }
    }

    public interface IIsPlayingProperty
    {
        bool IsPlaying { get; set; }
        void OnIsPlayingChanged(bool value) { }
    }

    public interface IPixelOffsetProperty
    {
        double PixelOffset { get; set; }
        void OnPixelOffsetChanged(double value) { }
    }
}
