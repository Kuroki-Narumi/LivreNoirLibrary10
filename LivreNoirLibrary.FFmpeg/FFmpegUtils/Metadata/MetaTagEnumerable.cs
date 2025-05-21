using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly unsafe struct MetaTagEnumerable<T> : IEnumerable<MetaTag>
        where T : IMetaTag
    {
        private readonly AVDictionary** _dic;

        internal MetaTagEnumerable(T metaTag)
        {
            _dic = metaTag.GetDictPointer();
        }

        Enumerator GetEnumerator() => new(_dic);
        IEnumerator<MetaTag> IEnumerable<MetaTag>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public unsafe struct Enumerator : IEnumerator<MetaTag>
        {
            private readonly AVDictionary** _dic;
            private readonly int _count;
            private AVDictionaryEntry* _current_entry;
            private MetaTag _current;

            public readonly MetaTag Current => _current;
            readonly object IEnumerator.Current => Current;

            public readonly int Count => _count;

            internal Enumerator(AVDictionary** dic)
            {
                _dic = dic;
                if (dic is null || *dic is null)
                {
                    _count = 0;
                }
                else
                {
                    _count = ffmpeg.av_dict_count(*dic);
                }
            }

            public bool MoveNext()
            {
                AVDictionaryEntry* tag;
                if (_count is > 0 && (tag = ffmpeg.av_dict_iterate(*_dic, _current_entry)) is not null)
                {
                    var key = FFmpegUtils.GetString(tag->key) ?? "";
                    var value = FFmpegUtils.GetString(tag->value);
                    _current = new(key, value);
                    _current_entry = tag;
                    return true;
                }
                _current = default;
                return false;
            }

            public void Reset() => _current_entry = null;
            public readonly void Dispose() { }
        }
    }
}
