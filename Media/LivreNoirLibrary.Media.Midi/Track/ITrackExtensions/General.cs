using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Midi
{
    public static partial class ITrackExtensions
    {
        public static void Clear<T>(this T track)
            where T : ITrack
        {
            track.Timeline.Clear();
            track.KeySwitchOptions.Clear();
        }

        public static void ClearKeySwitch<T>(this T track)
            where T : ITrack
        {
            track.KeySwitchOptions.Clear();
        }

        public static void SetKeySwitch<T>(this T track, ReadOnlySpan<KeySwitchOption> source)
            where T : ITrack
        {
            source.CopyTo(track.KeySwitchOptions);
        }

        public static Rational GetLastPosition<T>(this T track) where T : ITrack => track.Timeline.LastPosition;

        public static void Update<T>(this T track, ITrack source, bool restoreGroup = true)
            where T : ITrack
        {
            var timeline = track.Timeline;
            SortedDictionary<int, List<NoteGroup>> groups = [];
            if (restoreGroup)
            {
                foreach (var (_, obj) in timeline)
                {
                    if (obj is NoteGroup ng)
                    {
                        var count = -ng.Count;
                        if (!groups.TryGetValue(count, out var list))
                        {
                            list = [];
                            groups.Add(count, list);
                        }
                        list.Add(ng);
                    }
                }
            }
            track.Port = source.Port;
            track.Channel = source.Channel;
            track.Title = source.Title;
            timeline.Clear();
            source.Timeline.CopyTo(timeline);
            foreach (var (_, list) in groups)
            {
                foreach (var ng in CollectionsMarshal.AsSpan(list))
                {
                    track.AutoGroup(ng);
                }
            }
        }
    }
}
