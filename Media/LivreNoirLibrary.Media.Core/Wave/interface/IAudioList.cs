using System;

namespace LivreNoirLibrary.Media.Wave
{
    public readonly record struct TimelineItem(double Time, double Duration = 0, int Tag = -1);

    public interface IAudioList<TKey>
    {
        TKey Key { get; }
        TimelineItem FirstItem { get; }
        TimelineItem LastItem { get; }
        void Rewind();
        bool MoveNext(double untilExclusive, out TimelineItem current);
    }
}
