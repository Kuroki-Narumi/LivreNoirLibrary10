using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IAudioTimeline<TKey> : IEnumerable<IAudioList<TKey>>
    {
        int KeyCount { get; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class IAudioTimelineExtensions
    {
        extension<TKey>(IAudioTimeline<TKey> obj)
        {
            public void Rewind()
            {
                foreach (var list in obj)
                {
                    list.Rewind();
                }
            }
        }
    }
}
