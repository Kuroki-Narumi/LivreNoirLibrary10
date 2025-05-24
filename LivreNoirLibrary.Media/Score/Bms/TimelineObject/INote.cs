using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface INote
    {
        public NoteType Type { get; }
        public int Lane { get; }
        public int Id { get; }
    }

    public static class INoteExtensions
    {
        public static bool IsSound(this INote note) => BmsUtils.IsSoundLane(note.Lane);
        public static bool IsKey(this INote note) => BmsUtils.IsKeyLane(note.Lane);
        public static bool IsBgm(this INote note) => BmsUtils.IsBgmLane(note.Lane);

        public static bool IsTempo(this INote note) => note.Lane is BmsUtils.TempoLane;
        public static bool IsStop(this INote note) => note.Lane is BmsUtils.StopLane;
        public static bool IsConductor(this INote note) => note.Lane is BmsUtils.TempoLane or BmsUtils.StopLane;
        public static bool IsScroll(this INote note) => note.Lane is BmsUtils.ScrollLane;
        public static bool IsSpeed(this INote note) => note.Lane is BmsUtils.SpeedLane;
        public static bool IsIndex(this INote note, bool includesLongEnd) => IsWavObject(note, includesLongEnd) || BmsUtils.IsDefLane(note.Lane);

        public static bool IsWavObject(this INote note) => !IsMine(note) && IsSound(note);
        public static bool IsWavObject(this INote note, bool includesLongEnd) => !(IsMine(note) || !includesLongEnd && IsLongEnd(note)) && IsSound(note);
        public static bool IsPlayableSound(this INote note) => IsNormal(note) && IsSound(note);
        public static bool IsPlayableSound(this INote note, bool includesLongEnd) => (IsNormal(note) || includesLongEnd && IsLongEnd(note)) && IsSound(note);
        public static bool IsVisibleKey(this INote note) => IsNormal(note) && IsKey(note);
        public static bool IsVisibleKey(this INote note, bool includesLongEnd) => (IsNormal(note) || includesLongEnd && IsLongEnd(note)) && IsKey(note);

        public static bool IsNormal(this INote note) => note.Type is NoteType.Normal;
        public static bool IsInvisible(this INote note) => note.Type is NoteType.Invisible;
        public static bool IsMine(this INote note) => note.Type is NoteType.Mine;
        public static bool IsLongEnd(this INote note) => note.Type is NoteType.LongEnd;
        public static bool IsMetaKey(this INote note) => note.Type is NoteType.Invisible or NoteType.LongEnd or NoteType.Mine;
        public static bool IsDecimal(this INote note) => note.Type is NoteType.Decimal;
        public static bool IsRational(this INote note) => note.Type is NoteType.Rational;

        public static bool IsInvalidMeta(this INote note) => IsMetaKey(note) && !IsKey(note);
    }
}
