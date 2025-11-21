using System;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        public static void Write(this BinaryWriter writer, Note note)
        {
            writer.Write((short)note.Channel);
            writer.Write((byte)note.Type);
            writer.Write(note.Value);
        }

        public static Note ReadNote(this BinaryReader reader)
        {
            var ch = (Channel)reader.ReadInt16();
            var type = (NoteType)reader.ReadByte();
            var value = reader.ReadDouble();
            return new(ch, type, value);
        }

        public static string Intern(this Note note) => $"{{{BmsUtils.GetChannelName(note.Channel)}, Type={note.Type}, Value={note.Value}}}";
        public static string GetValueText(this Note note, int radix) => note.Channel.IsDefValue() ? BmsUtils.ToBased((int)note.Value, radix) : note.Value.ToString();
        public static Note Clone(this Note note) => new(note.Channel, note.Type, note.Value);

        public static bool IsTempo(this Note note) => note.Channel is Channel.Bpm;
        public static bool IsStop(this Note note) => note.Channel is Channel.Stop;
        public static bool IsScroll(this Note note) => note.Channel is Channel.Scroll;
        public static bool IsSpeed(this Note note) => note.Channel is Channel.Speed;
        public static bool IsConductor(this Note note) => note.Channel.IsConductor();

        public static bool IsBga(this Note note) => BmsUtils.IsBga(note.Channel);

        public static bool IsKey(this Note note) => BmsUtils.IsKey(note.Channel);
        public static bool IsBgm(this Note note) => BmsUtils.IsBgm(note.Channel);
        public static bool IsNormal(this Note note) => note.Type is NoteType.Normal;
        public static bool IsNormal(this Note note, bool includeLongEnd) => note.Type is NoteType.Normal || (includeLongEnd && note.Type is NoteType.LongEnd);
        public static bool IsInvisible(this Note note) => note.Type is NoteType.Invisible;
        public static bool IsLongEnd(this Note note) => note.Type is NoteType.LongEnd;
        public static bool IsMine(this Note note) => note.Type is NoteType.Mine;
        public static bool IsVisibleKey(this Note note, bool includeLongEnd) => BmsUtils.IsVisible(note.Channel) && IsNormal(note, includeLongEnd);
        public static bool IsSoundLane(this Note note) => BmsUtils.IsSoundLane(note.Channel);
        public static bool IsSound(this Note note) => BmsUtils.IsWavDef(note.Channel);
        public static bool IsMainSound(this Note note, bool includeLongEnd) => note.IsBgm() || (note.IsKey() && note.IsNormal(includeLongEnd));
        public static bool IsPlayableSound(this Note note) => IsSound(note) && IsNormal(note);
        public static bool IsPlayableSound(this Note note, bool includeLongEnd) => IsSound(note) && IsNormal(note, includeLongEnd);
        public static bool IsInvalidMeta(this Note note) => !IsKey(note) && note.Type is not NoteType.Normal;

        public static bool IsDefValue(this Note note) => BmsUtils.IsDefValue(note.Channel);
        public static bool TryGetDefType(this Note note, out DefType type) => BmsUtils.TryGetDefType(note.Channel, out type);

        public static string GetLaneText(this Note note) => BmsUtils.GetChannelName(note.Channel);
    }
}
