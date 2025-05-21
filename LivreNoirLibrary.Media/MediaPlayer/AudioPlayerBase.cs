using System;
using NAudio.Wave;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public abstract partial class AudioPlayerBase : ObservableObjectBase, IDisposable
    {
        public const float DefaultVolume = 1;
        public const float MinVolume = 0;
        public const float MaxVolume = 1;

        public event EventHandler<StoppedEventArgs>? PlaybackStopped;

        [ObservableProperty(Related = [nameof(IsPlaying)], SetterScope = Scope.Protected)]
        private PlaybackState _playbackState = PlaybackState.Stopped;
        [ObservableProperty(Type = typeof(double))]
        protected float _volume = DefaultVolume;

        public bool IsPlaying => _playbackState is PlaybackState.Playing;

        protected static float CoerceVolume(double value) => Math.Clamp((float)value, MinVolume, MaxVolume);
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
