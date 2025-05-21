using System;
using NAudio.Wave;

namespace LivreNoirLibrary.Media.Wave
{
    public class WaveBufferSampleProvider(IWaveBuffer source) : ISampleProvider
    {
        public const float MaxVolume = 2;

        private readonly WeakReference<IWaveBuffer> _source = new(source);
        private readonly WaveFormat _format = WaveFormat.CreateIeeeFloatWaveFormat(source.SampleRate, source.Channels);

        private float _volume = 1.0f;
        private long _position;
        private long _end_position;
        private bool _loop;
        private long _loop_start;
        private long _loop_length;

        public WaveFormat WaveFormat => _format;

        public float Volume
        {
            get => _volume;
            set => _volume = Math.Clamp(value, 0, MaxVolume);
        }

        /// <summary>
        /// Prepare for play by <see cref="IWavePlayer"/> as <see cref="ISampleProvider"/>
        /// </summary>
        /// <param name="startPosition">
        /// Specifies the sample position playback starts from.
        /// </param>
        /// <param name="endPosition">
        /// Specifies the sample position playback ends. For playing to the end of data, set this 0 or negative.
        /// </param>
        /// <param name="loopStart">
        /// Specifies the sample position of loop start. Set negative to NOT loop playback.
        /// </param>
        public (long ActualStart, long ActualLength) PrepareForPlayback(long startPosition = 0, long endPosition = 0, long loopStart = -1)
        {
            if (!_source.TryGetTarget(out var source))
            {
                return (0, 0);
            }
            var ch = source.Channels;
            var rawLength = source.TotalSample;
            _end_position = endPosition is <= 0 ? rawLength : Math.Min(endPosition * ch, rawLength);
            _loop_start = loopStart * ch;
            if (loopStart is >= 0 && _loop_start < rawLength)
            {
                _loop_length = _end_position - _loop_start;
                _loop = _loop_length is > 0;
            }
            else
            {
                _loop = false;
            }
            startPosition *= ch;
            if (_loop)
            {
                _position = GetLoopedPositionPrivate(startPosition);
            }
            else
            {
                _position = Math.Clamp(startPosition, 0, rawLength);
                if (_position >= _end_position)
                {
                    _end_position = rawLength;
                }
                _loop_length = _end_position - _position;
            }
            return (_position / ch, _loop_length / ch);
        }

        public long GetLoopedPosition(long sampleOffset)
        {
            if (_loop && _source.TryGetTarget(out var source))
            {
                var ch = source.Channels;
                sampleOffset = GetLoopedPositionPrivate(sampleOffset * ch) / ch;
            }
            return sampleOffset;
        }

        private long GetLoopedPositionPrivate(long sampleOffset)
        {
            if (sampleOffset < 0)
            {
                sampleOffset = _end_position + (sampleOffset - _loop_start) % _loop_length;
            }
            if (sampleOffset > _end_position)
            {
                sampleOffset = _loop_start + (sampleOffset - _loop_start) % _loop_length;
            }
            return sampleOffset;
        }

        public unsafe int Read(float[] buffer, int offset, int count)
        {
            if (!_source.TryGetTarget(out var source))
            {
                return 0;
            }
            var pos = _position;
            var end = _end_position;
            var vol = _volume;
            fixed (float* src = source.Data)
            fixed (float* dst = buffer)
            {
                var dstPtr = dst + offset;
                if (_loop)
                {
                    var loop = _loop_length;
                    for (var i = 0; i < count; i++, pos++)
                    {
                        if (pos >= end)
                        {
                            pos -= loop;
                        }
                        dstPtr[i] = src[pos] * vol;
                    }
                }
                else
                {
                    var dif = (int)(end - pos);
                    if (dif is <= 0)
                    {
                        return 0;
                    }
                    if (dif < count)
                    {
                        count = dif;
                    }
                    for (var i = 0; i < count; i++, pos++)
                    {
                        dstPtr[i] = src[pos] * vol;
                    }
                }
            }
            _position = pos;
            return count;
        }
    }
}
