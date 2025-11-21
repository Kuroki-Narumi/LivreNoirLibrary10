using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Wave;
using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public partial class WavePlayer : DependencyObject, IDisposable, IWaveSourceProperty, ISamplePositionProperty, IIsPlayingProperty, IWaveSelectionProperty, IVolumeProperty
    {
        public static readonly DependencyProperty SourceProperty = WaveProperties.SourceProperty.AddOwner(typeof(WavePlayer));
        public static readonly DependencyProperty SamplePositionProperty = WaveProperties.SamplePositionProperty.AddOwner(typeof(WavePlayer));
        public static readonly DependencyProperty SelectionProperty = WaveProperties.SelectionProperty.AddOwner(typeof(WavePlayer));
        public static readonly DependencyProperty VolumeProperty = WaveProperties.VolumeProperty.AddOwner(typeof(WavePlayer));
        public static readonly DependencyProperty IsPlayingProperty = WaveProperties.IsPlayingProperty.AddOwner(typeof(WavePlayer));

        private readonly WaveBufferPlayer _player = new();
        private IWaveBuffer? _source;
        private long _samplePosition;
        private bool _isPositionChanging;
        private bool _isPlaying;
        private bool _isPlayingChanging;

        public IWaveBuffer? Source { get => _source; set => SetValue(SourceProperty, value); }
        public long SamplePosition { get => _samplePosition; set => SetValue(SamplePositionProperty, value); }
        public WaveSelection Selection { get => (WaveSelection)GetValue(SelectionProperty); set => SetValue(SelectionProperty, value); }
        public double Volume { get => (double)GetValue(VolumeProperty); set => SetValue(VolumeProperty, value); }
        public bool IsPlaying { get => _isPlaying; set => SetValue(IsPlayingProperty, value); }

        public WavePlayer()
        {
            _player.PlaybackStopped += OnPlayBackStopped;
        }

        ~WavePlayer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeSource();
            }
        }

        private void DisposeSource()
        {
            _player.ClearData();
        }

        void IWaveSourceProperty.OnSourceChanged(IWaveBuffer? value)
        {
            DisposeSource();
            _source = value;
            if (value is not null)
            {
                _player.SetData(value);
            }
            SamplePosition = 0;
            Selection = default;
        }

        void ISamplePositionProperty.OnSamplePositionChanged(long value)
        {
            _samplePosition = value;
            if (!_isPositionChanging)
            {
                _player.Position = value;
            }
        }

        void IWaveSelectionProperty.OnSelectionChanged(in WaveSelection value)
        {
            _player.SetRange(value.Left, value.Right);
        }

        void IVolumeProperty.OnVolumeChanged(double value)
        {
            _player.Volume = value;
        }

        void IIsPlayingProperty.OnIsPlayingChanged(bool value)
        {
            _isPlaying = value;
            if (!_isPlayingChanging)
            {
                if (value)
                {
                    Play();
                }
                else
                {
                    Pause();
                }
            }
        }

        private void UpdatePosition(long value)
        {
            _isPositionChanging = true;
            SamplePosition = value;
            _isPositionChanging = false;
        }

        private void UpdatePlaybackState(bool value)
        {
            _isPlayingChanging = true;
            IsPlaying = value;
            _isPlayingChanging = false;
        }

        public void SetLoop(bool loop = true)
        {
            _player.IsLoop = loop;
        }

        public void Play()
        {
            if (_isPlaying)
            {
                return;
            }
            if (_player.Play(_samplePosition))
            {
                CompositionTarget.Rendering += OnRenderingInvoked;
                UpdatePlaybackState(true);
            }
        }

        private void OnPlayBackStopped(object? sender, EventArgs e)
        {
            CompositionTarget.Rendering -= OnRenderingInvoked;
            UpdatePosition(_player.Position);
            UpdatePlaybackState(false);
        }

        private void OnRenderingInvoked(object? sender, EventArgs e)
        {
            UpdatePosition(_player.Position);
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Stop()
        {
            _player.Stop();
        }

        public void PlayPause()
        {
            if (_isPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
    }
}
