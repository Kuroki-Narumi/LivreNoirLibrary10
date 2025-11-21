using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IScore : IBarPositionProvider<Rational>, IClear
    {
        string? Title { get; set; }
        string? Copyright { get; set; }

        void InitializeTracks(int count);
        int TrackCount { get; }
        ITrack GetTrack(int index);
        bool TryGetTrack(int index, [MaybeNullWhen(false)] out ITrack track);
        bool TryGetTrackByTitle(string? title, out int index, [MaybeNullWhen(false)] out ITrack track);
        IEnumerable<(int Index, ITrack Track)> EachTrack();

        TimeSignature GetTimeSignature(Rational position);
        TimeSignature GetTimeSignatureByNumber(int number);
        void SetTimeSignature(Rational position, TimeSignature value);
        void SetTimeSignatureByNumber(int number, TimeSignature value);
        IEnumerable<BarInfo<Rational>> EnumerateBars(Rational end);
        IEnumerable<BarLineInfo<Rational>> EnumerateLines(Rational end);
    }
}
