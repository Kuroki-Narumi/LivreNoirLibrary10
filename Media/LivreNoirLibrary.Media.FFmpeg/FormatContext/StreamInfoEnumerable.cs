using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly unsafe struct StreamInfoEnumerable : ISafeEnumerable<StreamInfo>
    {
        private readonly AVFormatContext* _context;
        private readonly AVMediaType _type;

        internal StreamInfoEnumerable(AVFormatContext* context, AVMediaType type)
        {
            _context = context;
            _type = type;
        }

        private Enumerator GetEnumerator() => new(_context, _type);
        IEnumerator<StreamInfo> IEnumerable<StreamInfo>.GetEnumerator() => GetEnumerator();

        public struct Enumerator : ISafeEnumerator<StreamInfo>
        {
            private readonly AVFormatContext* _context;
            private readonly AVMediaType _type;
            private readonly int _count;
            private int _next_index = 0;
            private StreamInfo _current;

            public readonly StreamInfo Current => _current;

            internal Enumerator(AVFormatContext* context, AVMediaType type)
            {
                _context = context;
                _type = type;
                _count = context is not null ? (int)context->nb_streams : 0;
            }

            public bool MoveNext()
            {
                while (_next_index < _count)
                {
                    _current = new(_context->streams[_next_index]);
                    _next_index++;
                    if (_type is <= 0 || _current.MediaType == _type)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void Reset() => _next_index = 0;
        }
    }
}
