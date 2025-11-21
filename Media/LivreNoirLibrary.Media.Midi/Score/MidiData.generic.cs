using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public class MidiData<TTrack> : IScore
        where TTrack : ITrack, new()
    {
        protected readonly SignatureTimeline _signatures = [];
        protected readonly List<TTrack> _tracks = [new()];

        public int TrackCount => _tracks.Count;
        public string? Title { get => _tracks[0].Title; set => _tracks[0].Title = value; }
        public string? Copyright { get; set; }

        public virtual void Clear()
        {
            _signatures.Clear();
            if (_tracks.Count is > 0)
            {
                _tracks.RemoveRange(1, _tracks.Count - 1);
            }
            _tracks[0].Clear();
        }

        public void InitializeTracks(int count)
        {
            for (var i = _tracks.Count; i < count; i++)
            {
                _tracks.Add(new());
            }
        }

        public TTrack GetTrack(int index)
        {
            InitializeTracks(index + 1);
            return _tracks[index];
        }
        ITrack IScore.GetTrack(int index) => GetTrack(index);

        public bool TryGetTrack(int index, [MaybeNullWhen(false)] out TTrack track)
        {
            if ((uint)index < (uint)_tracks.Count)
            {
                track = _tracks[index];
                return true;
            }
            track = default;
            return false;
        }

        bool IScore.TryGetTrack(int index, [MaybeNullWhen(false)] out ITrack track)
        {
            if (TryGetTrack(index, out var t))
            {
                track = t;
                return true;
            }
            track = default;
            return false;
        }

        public bool TryGetTrackByTitle(string? title, out int index, [MaybeNullWhen(false)] out TTrack track)
        {
            if (!string.IsNullOrEmpty(title))
            {
                for (index = 0; index < _tracks.Count; index++)
                {
                    track = _tracks[index];
                    if (track.Title == title)
                    {
                        return true;
                    }
                }
            }
            track = default;
            index = -1;
            return false;
        }

        bool IScore.TryGetTrackByTitle(string? title, out int index, [MaybeNullWhen(false)] out ITrack track)
        {
            if (TryGetTrackByTitle(title, out index, out var t))
            {
                track = t;
                return true;
            }
            track = default;
            return false;
        }

        public IEnumerable<(int Index, TTrack Track)> EachTrack()
        {
            for (int i = 0; i < _tracks.Count; i++)
            {
                yield return (i, _tracks[i]);
            }
        }

        IEnumerable<(int Index, ITrack Track)> IScore.EachTrack()
        {
            for (int i = 0; i < _tracks.Count; i++)
            {
                yield return (i, _tracks[i]);
            }
        }

        public Rational GetBarLength(int number) => _signatures.GetBarLength(number);
        public Rational GetAbsolutePosition(BarPosition position) => _signatures.GetAbsolutePosition(position);
        public BarPosition GetBarPosition(Rational absolutePosition) => _signatures.GetBarPosition(absolutePosition);

        public TimeSignature GetTimeSignature(Rational position) => _signatures.Get(position);
        public TimeSignature GetTimeSignatureByNumber(int number) => _signatures.GetByNumber(number);
        public void SetTimeSignature(Rational position, TimeSignature value) => _signatures.Set(position, value);
        public void SetTimeSignatureByNumber(int number, TimeSignature value) => _signatures.SetByNumber(number, value);
        public IEnumerable<BarInfo<Rational>> EnumerateBars(Rational end) => _signatures.EnumerateBars(end);
        public IEnumerable<BarLineInfo<Rational>> EnumerateLines(Rational end) => _signatures.EnumerateLines(end);
    }
}
