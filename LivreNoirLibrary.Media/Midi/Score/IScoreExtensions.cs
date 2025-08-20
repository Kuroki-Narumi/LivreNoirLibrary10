using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public static class IScoreExtensions
    {
        public static int GetInitialTempo(this IScore score)
        {
            if (score.ConductorTrack.Timeline.TryGet(Rational.Zero, out var list) && list.Find(obj => obj is TempoEvent) is TempoEvent t)
            {
                return t.Value;
            }
            return RawData.Tempo.DefaultValue;
        }

        public static bool BulkEdit<T>(this IScore score, T trackIndexes, BulkEditOptions options)
            where T : ICollection<int>
        {
            var flag = false;
            foreach (var (i, t) in score.EachTrack())
            {
                if (trackIndexes.Contains(i) && t.BulkEdit(options, null, out _))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static string GetTrackTitle(this IScore score, int index)
        {
            var title = score.TryGetTrack(index, out var track) ? track.Title : null;
            if (string.IsNullOrEmpty(title))
            {
                return index is 0 ? "(Conductor)" : $"(Track {index})";
            }
            else
            {
                return index is 0 ? $"{title} (Conductor)" : title;
            }
        }

        public static Rational GetLastPosition(this IScore score)
        {
            var pos = Rational.Zero;
            foreach (var (_, track) in score.EachTrack())
            {
                var p = track.GetLastPosition();
                if (p > pos)
                {
                    pos = p;
                }
            }
            return pos;
        }
    }
}
