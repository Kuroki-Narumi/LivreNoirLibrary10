using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace LivreNoirLibrary.Media
{
    using Wave;

    public class WaveBufferPlayer : AudioPlayerBase
    {
        private IWaveBuffer? _waveData;
        private WaveBufferSampleProvider? _provider;
        private WaveOutEvent? _waveOut;
        private CancellationTokenSource? _ct_source;

        private long _position;
        private long _start;
        private long _end;
        private bool _is_loop;

        private long _initial_position;
        private long _initial_waveOut_position;

        protected override void OnVolumeChanged(float value)
        {
            if (_provider is not null)
            {
                _provider.Volume = value;
            }
        }

        public long Position
        {
            get => GetPosition();
            set
            {
                if (_waveData is null)
                {
                    return;
                }
                value = Math.Clamp(value, 0, _waveData.SampleLength);
                if (_position != value)
                {
                    if (IsPlaying)
                    {
                        ProcessStop(PlaybackState.Playing, value);
                        ProcessPlay();
                    }
                    else
                    {
                        SetPosition(value);
                    }
                }
            }
        }

        private long GetPosition()
        {
            if (IsPlaying && _waveOut is not null && _provider is not null && _waveData is not null)
            {
                var pos = (_waveOut.GetPosition() - _initial_waveOut_position) / sizeof(float) / _waveData.Channels;
                return _provider.GetLoopedPosition(_initial_position + pos);
            }
            return _position;
        }

        private void SetPosition(long position) => SetProperty(ref _position, position, nameof(Position));

        public long RangeStart
        {
            get => _start;
            set
            {
                if (_waveData is null)
                {
                    return;
                }
                value = Math.Clamp(value, 0, _waveData.SampleLength);
                if (_start != value)
                {
                    ProcessPropertyChange(() =>
                    {
                        if (value > _end)
                        {
                            SetProperty(ref _start, _end, nameof(RangeStart));
                            SetProperty(ref _end, value, nameof(RangeEnd));
                        }
                        else
                        {
                            SetProperty(ref _start, value, nameof(RangeStart));
                        }
                    });
                }
            }
        }

        public long RangeEnd
        {
            get => _end;
            set
            {
                if (_waveData is null)
                {
                    return;
                }
                value = Math.Clamp(value, 0, _waveData.SampleLength);
                if (_end != value)
                {
                    ProcessPropertyChange(() =>
                    {
                        if (value < _start)
                        {
                            SetProperty(ref _end, _start, nameof(RangeEnd));
                            SetProperty(ref _start, value, nameof(RangeStart));
                        }
                        else
                        {
                            SetProperty(ref _end, value, nameof(RangeEnd));
                        }
                    });
                }
            }
        }

        public bool IsLoop
        {
            get => _is_loop;
            set
            {
                if (_is_loop != value)
                {
                    ProcessPropertyChange(() => SetProperty(ref _is_loop, value, nameof(IsLoop)));
                }
            }
        }

        private void ProcessPropertyChange(Action action)
        {
            var playing = IsPlaying;
            if (playing)
            {
                ProcessStop(PlaybackState.Playing, GetPosition());
            }
            action();
            if (playing)
            {
                ProcessPlay();
            }
        }

        public void ClearData()
        {
            DisposeWaveOut();
            _waveData = null;
            _position = 0;
        }

        public void SetData(IWaveBuffer waveData)
        {
            DisposeWaveOut();
            _waveData = waveData;
            _position = 0;
            InitializeWaveOut();
        }

        private void InitializeWaveOut()
        {
            if (_waveData is not null)
            {
                _provider = new(_waveData) { Volume = _volume };
                _waveOut = new();
                _waveOut.Init(_provider);
            }
        }

        public void DisposeWaveOut()
        {
            StopWaveOut();
            if (_waveOut is not null)
            {
                _waveOut.Dispose();
                _waveOut = null;
            }
            if (_provider is not null)
            {
                _provider = null;
            }
        }

        public void SetRange(long start, long end, bool? loop = null)
        {
            var playing = IsPlaying;
            if (playing)
            {
                ProcessStop(PlaybackState.Playing, GetPosition());
            }
            if (start > end)
            {
                (start, end) = (end, start);
            }
            SetProperty(ref _start, start, nameof(RangeStart));
            SetProperty(ref _end, end, nameof(RangeEnd));
            SetProperty(ref _is_loop, loop ?? _is_loop, nameof(IsLoop));
            if (playing)
            {
                ProcessPlay();
            }
        }

        private bool ProcessPlay()
        {
            if (_provider is null || _waveOut is null || _waveData is null)
            {
                return false;
            }
            long start, end;
            if (_start == _end)
            {
                start = _is_loop ? 0 : -1;
                end = _waveData.SampleLength;
            }
            else
            {
                start = _is_loop ? _start : -1;
                end = _end;
            }
            var (pos, length) = _provider.PrepareForPlayback(_position, end, start);
            if (length > 0)
            {
                _initial_position = pos;
                _initial_waveOut_position = _waveOut.GetPosition();
                _waveOut.Play();
                PlaybackState = PlaybackState.Playing;
                if (!_is_loop)
                {
                    _ct_source = new();
                    _ = RunPlayTask(length * 1000.0 / _waveData.SampleRate, _ct_source.Token);
                }
                return true;
            }
            return false;
        }

        private async Task RunPlayTask(double duration, CancellationToken token)
        {
            try
            {
                await Task.Delay((int)duration, token);
                token.ThrowIfCancellationRequested();
                Stop();
            }
            catch (TaskCanceledException) { }
        }

        private void ProcessStop(PlaybackState newState, long newPosition)
        {
            StopWaveOut();
            SetPosition(newPosition);
            PlaybackState = newState;
            if (newState is not PlaybackState.Playing)
            {
                InvokePlaybackStopped();
            }
        }

        private void StopWaveOut()
        {
            _ct_source?.Cancel();
            _ct_source = null;
            _waveOut?.Stop();
        }

        public override bool Play() => Play(_position);

        public bool Play(long from)
        {
            if (IsPlaying)
            {
                return false;
            }
            Position = from;
            return ProcessPlay();
        }

        public override long Pause()
        {
            ProcessStop(PlaybackState.Paused, GetPosition());
            return _position;
        }

        public override void Stop()
        {
            ProcessStop(PlaybackState.Stopped, _start);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeWaveOut();
            }
            base.Dispose(disposing);
        }
    }
}
