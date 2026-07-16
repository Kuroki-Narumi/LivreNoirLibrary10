using System;
using NAudio.Wave;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public abstract class AudioPlayerBase : ObservableObjectBase, IDisposable
    {
        public const float DefaultVolume = 1;
        public const float MinVolume = 0;
        public const float MaxVolume = 1;

        public event EventHandler<StoppedEventArgs>? PlaybackStopped;

        protected float _volume = DefaultVolume;

        public PlaybackState PlaybackState
        {
            get;
            protected set => SetValue(ref field, value, [nameof(IsPlaying)]);
        }

        public bool IsPlaying => PlaybackState is PlaybackState.Playing;

        public double Volume
        {
            get => _volume;
            set
            {
                var v = Math.Clamp((float)value, MinVolume, MaxVolume);
                if (v != _volume)
                {
                    _volume = v;
                    this.NotifyPropertyChanged(nameof(Volume));
                    OnVolumeChanged(v);
                }
            }
        }

        protected virtual void OnVolumeChanged(float value) { }

        public abstract bool Play();
        public abstract long Pause();
        public abstract void Stop();

        protected void InvokePlaybackStopped(StoppedEventArgs? e = null)
        {
            PlaybackStopped?.Invoke(this, e ?? new());
        }

        ~AudioPlayerBase()
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
                Stop();
            }
        }
    }
}
