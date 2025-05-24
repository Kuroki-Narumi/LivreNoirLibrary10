using System;
using System.IO;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using NAudio.Wave;

namespace LivreNoirLibrary.Media
{
    public partial class AudioFilePlayer : AudioPlayerBase
    {
        [ObservableProperty]
        private string? _filename;
        private WaveOutEvent? _waveOut;
        private AudioFileReader? _stream;

        private void OnFilenameChanged(string? value)
        {
            Stop(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeStream();
            }
            base.Dispose(disposing);
        }

        public void Rewind(long ticks = 0)
        {
            Stop();
            _stream?.SeekByTicks(ticks);
        }

        private void InitWaveOut()
        {
            var stream = _stream;
            if (stream is null && AudioUtils.TryGetWaveStream(_filename, out stream))
            {
                _stream = stream;
            }
            if (_waveOut is null && stream is not null)
            {
                _waveOut = new();
                _waveOut.PlaybackStopped += OnPlaybackStopped;
                _waveOut.Init(stream);
            }
        }

        public override bool Play()
        {
            DisposeWaveOut();
            InitWaveOut();
            if (_waveOut is not null)
            {
                _waveOut.Play();
                PlaybackState = PlaybackState.Playing;
                return true;
            }
            return false;
        }

        public bool Play(string? filename)
        {
            if (_filename == filename)
            {
                Rewind(0);
            }
            else
            {
                Filename = filename;
            }
            return Play();
        }

        public override long Pause()
        {
            _waveOut?.Pause();
            return 0;
        }

        public void Stop(bool disposeStream)
        {
            Stop();
            if (disposeStream)
            {
                DisposeStream();
            }
        }

        public override void Stop()
        {
            DisposeWaveOut();
        }

        private void DisposeStream()
        {
            _stream?.Dispose();
            _stream = null;
        }

        private void DisposeWaveOut()
        {
            PlaybackState = PlaybackState.Stopped;
            if (_waveOut is not null)
            {
                _waveOut.PlaybackStopped -= OnPlaybackStopped;
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            DisposeStream();
            DisposeWaveOut();
            InvokePlaybackStopped(e);
        }
    }
}
