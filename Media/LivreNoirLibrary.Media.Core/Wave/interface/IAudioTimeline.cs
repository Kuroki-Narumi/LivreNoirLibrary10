using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public readonly record struct TimelineItem<TKey>(TKey Key, double Time, double Duration = 0, int Tag = -1);

    public interface IAudioTimeline<TKey>
    {
        public int AudioItemCount { get; }
        IEnumerable<TimelineItem<TKey>> Range(double time, double duration);
    }

    public static class IAudioTimelineExtensions
    {
        extension<T>(IAudioTimeline<T> obj)
        {
            public bool TryGetFirstItem(out TimelineItem<T> item)
            {
                foreach (var i in obj.Range(0, double.PositiveInfinity))
                {
                    item = i;
                    return true;
                }
                item = default;
                return false;
            }
        }
    }
}
