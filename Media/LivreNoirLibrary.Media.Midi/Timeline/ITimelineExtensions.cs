using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Midi
{
    public static class ITimelineExtensions
    {
        public static void SetTempo(this ITimeline timeline, Rational position, int value)
        {
            if (timeline.Find(position, obj => obj is TempoEvent, out var obj))
            {
                (obj as TempoEvent)!.Value = value;
            }
            else
            {
                timeline.Add(position, new TempoEvent(value));
            }
        }

        public static void ExtendToRawTimeline(this ITimeline timeline, RawTimeline target, int channel, long ticksPerWholeNote)
        {
            foreach (var (pos, item) in timeline)
            {
                var tick = IObject.GetTick(pos, ticksPerWholeNote);
                item.ExtendToEvent(target, channel, tick, pos, ticksPerWholeNote);
            }
        }
    }
}
