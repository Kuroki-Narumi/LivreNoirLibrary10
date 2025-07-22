using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IScore : IBarPositionProvider
    {
        public string? Title { get; set; }
        public string? Copyright { get; set; }

        public void InitializeTracks(int count);
        public int TrackCount { get; }
        public ITrack ConductorTrack { get; }
        public ITrack GetTrack(int index);
        public bool TryGetTrack(int index, [MaybeNullWhen(false)] out ITrack track);
        public string GetTrackTitle(int index);
        public bool TryGetTrackByTitle(string? title, out int index, [MaybeNullWhen(false)] out ITrack track);
        public IEnumerable<(int Index, ITrack Track)> EachTrack();

        public Rational GetLastPosition();

        public TimeSignature GetTimeSignature(Rational position);
        public TimeSignature GetTimeSignatureByNumber(int number);
        public void SetTimeSignature(Rational position, TimeSignature value);
        public void SetTimeSignatureByNumber(int number, TimeSignature value);
        public IEnumerable<BarInfo> EachBar(Rational end);
        public IEnumerable<BarLineInfo> EachLine(Rational end);

        public bool BulkEdit<T>(T trackIndexes, BulkEditOptions options) where T : ICollection<int>;
    }

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
    }
}
