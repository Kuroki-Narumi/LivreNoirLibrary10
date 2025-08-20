using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public static partial class ITrackExtensions
    {
        public static Rational GetFirstMetaPosition<T>(this T track, MetaType type)
            where T : ITrack
        {
            if (track.Timeline.Find((_, obj) => obj is IMetaObject m && m.Type == type, out var pos, out _))
            {
                return pos;
            }
            return new(-1);
        }

        public static void SetMetaText<T>(this T track, MetaType type, string? value)
            where T : ITrack
        {
            var timeline = track.Timeline;
            bool Check(Rational _, IObject obj) => obj is MetaText m && m.Type == type;
            if (string.IsNullOrEmpty(value))
            {
                timeline.RemoveIf(Check);
            }
            else
            {
                if (timeline.Find(Check, out _, out var obj))
                {
                    (obj as MetaText)!.Text = value;
                }
                else
                {
                    timeline.Add(Rational.Zero, new MetaText(type, value));
                }
            }
        }

        public static void SetMetaText<T>(this T track, Rational position, MetaType type, string? value)
            where T : ITrack
        {
            var timeline = track.Timeline;
            bool Check(IObject obj) => obj is MetaText m && m.Type == type;
            if (string.IsNullOrEmpty(value))
            {
                timeline.RemoveIf(position, Check);
            }
            else
            {
                if (timeline.Find(position, Check, out var obj))
                {
                    (obj as MetaText)!.Text = value;
                }
                else
                {
                    timeline.Add(position, new MetaText(type, value));
                }
            }
        }
    }
}
