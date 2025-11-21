using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public static partial class IScoreExtensions
    {
        extension(IScore score)
        {
            public ITrack ConductorTrack => score.GetTrack(0);

            public int GetInitialTempo()
            {
                if (score.ConductorTrack.Timeline.TryGetValue(Rational.Zero, Collections.SearchMode.PreviousOrEqual, out _, out var list) && list.Find(obj => obj is TempoEvent) is TempoEvent t)
                {
                    return t.Value;
                }
                return Events.Tempo.DefaultValue;
            }

            public bool BulkEdit<T>(T trackIndexes, BulkEditOptions options)
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

            public string GetTrackTitle(int index)
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

            public Rational GetLastPosition()
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
}
