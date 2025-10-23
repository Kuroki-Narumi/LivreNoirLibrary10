using LivreNoirLibrary.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class INoteExtensions
    {
        public const byte NoteType_Conductor = 1;
        public const byte NoteType_Meta = 2;
        public const byte NoteType_Sound = 3;

        public static void Write(this BinaryWriter writer, INote item)
        {
            writer.Write(item switch
            {
                IConductorNote => NoteType_Conductor,
                IMetaNote => NoteType_Meta,
                ISoundNote => NoteType_Sound,
                _ => throw new NotSupportedException($"unknown data type: {item.GetType()}"),
            });
            item.Dump(writer);
        }

        public static INote ReadINote(this BinaryReader reader)
        {
            var type = reader.ReadByte();
            return type switch
            {
                NoteType_Conductor => ConductorNote.Load(reader),
                NoteType_Sound => SoundNote.Load(reader),
                NoteType_Meta => MetaNote.Load(reader),
                _ => throw new NotSupportedException($"unknown data prefix: {type}"),
            };
        }

        private static bool TryGetCore<T>(INote note, Predicate<T> selector, [MaybeNullWhen(false)] out T actual)
            where T : INote
        {
            if (note is T n && selector(n))
            {
                actual = n;
                return true;
            }
            actual = default;
            return false;
        }

        extension(IDecimalValueNote note)
        {
            public double DoubleValue { get => (double)note.Value; set => note.Value = (decimal)value; }
            public Rational RationalValue { get => Rational.ConvertBySBT(note.Value); set => note.Value = (decimal)value; }
        }

        extension(INote note)
        {
            public bool IsConductor(Predicate<IConductorNote> selector) => TryGetCore(note, selector, out _);
            public bool IsMeta(Predicate<IMetaNote> selector) => TryGetCore(note, selector, out _);
            public bool IsSound(Predicate<ISoundNote> selector) => TryGetCore(note, selector, out _);

            public bool IsTempo([MaybeNullWhen(false)] out IConductorNote actual) => TryGetCore(note, IsTempo, out actual);
            public bool IsStop([MaybeNullWhen(false)] out IConductorNote actual) => TryGetCore(note, IsStop, out actual);
            public bool IsScroll([MaybeNullWhen(false)] out IConductorNote actual) => TryGetCore(note, IsScroll, out actual);
            public bool IsSpeed([MaybeNullWhen(false)] out IConductorNote actual) => TryGetCore(note, IsSpeed, out actual);

            public bool IsBga([MaybeNullWhen(false)] out IMetaNote actual) => TryGetCore(note, IsBga, out actual);
            public bool IsDef([MaybeNullWhen(false)] out IMetaNote actual) => TryGetCore(note, IsDef, out actual);

            public bool IsKey([MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, IsKey, out actual);
            public bool IsBgm([MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, IsBgm, out actual);
            public bool IsNormal(bool includeLongEnd, [MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, n => IsNormal(n, includeLongEnd), out actual);
            public bool IsInvisible([MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, IsInvisible, out actual);
            public bool IsLongEnd([MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, IsLongEnd, out actual);
            public bool IsMine([MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, IsMine, out actual);
            public bool IsVisibleKey(bool includeLongEnd, [MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, n => IsVisibleKey(n, includeLongEnd), out actual);
            public bool IsPlayableSound(bool includeLongEnd, [MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, n => IsPlayableSound(n, includeLongEnd), out actual);
            public bool IsInvalidMeta([MaybeNullWhen(false)] out ISoundNote actual) => TryGetCore(note, IsInvalidMeta, out actual);

            public bool TryGetDefType(out DefType type, [MaybeNullWhen(false)] out IIntValueNote actual)
            {
                if (note is ISoundNote s)
                {
                    type = DefType.Wav;
                    actual = s;
                    return true;
                }
                if (note is IMetaNote m)
                {
                    type = BmsUtils.GetDefType(m.Channel);
                    actual = m;
                    return type is not 0;
                }
                type = default;
                actual = default;
                return false;
            }

            public bool IsDefType(DefType type, [MaybeNullWhen(false)] out IIntValueNote actual) => note.TryGetDefType(out var t, out actual) && t == type;
        }

        extension(IConductorNote note)
        {
            public bool IsTempo() => note.Channel is Channel.Bpm;
            public bool IsStop() => note.Channel is Channel.Stop;
            public bool IsScroll() => note.Channel is Channel.Scroll;
            public bool IsSpeed() => note.Channel is Channel.Speed;

            public HistoryNote ToHistory() => new(note);
        }

        extension(IMetaNote note)
        {
            public bool IsBga() => BmsUtils.IsBga(note.Channel);
            public bool IsDef() => BmsUtils.IsDefChannel(note.Channel);

            public HistoryNote ToHistory() => new(note);
        }

        extension(ISoundNote note)
        {
            public bool IsKey() => BmsUtils.IsKeyLane(note.Lane);
            public bool IsBgm() => BmsUtils.IsBgmLane(note.Lane);
            public bool IsNormal(bool includeLongEnd = false) => note.Type is NoteType.Normal || (includeLongEnd && IsLongEnd(note));
            public bool IsInvisible() => note.Type is NoteType.Invisible;
            public bool IsLongEnd() => note.Type is NoteType.LongEnd;
            public bool IsMine() => note.Type is NoteType.Mine;
            public bool IsVisibleKey(bool includeLongEnd = false) => BmsUtils.IsKeyLane(note.Lane) && IsNormal(note, includeLongEnd);
            public bool IsPlayableSound(bool includeLongEnd = false) => BmsUtils.IsBgmLane(note.Lane) || IsNormal(note, includeLongEnd);
            public bool IsInvalidMeta() => BmsUtils.IsBgmLane(note.Lane) && note.Type is not NoteType.Normal;

            public HistoryNote ToHistory() => new(note);
        }
    }
}
