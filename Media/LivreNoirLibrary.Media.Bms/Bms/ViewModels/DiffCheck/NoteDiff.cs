using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    using static IDifference;

    public class NoteDiffList : List<NoteDiff>
    {
        public int NumDigits { get; private set; }
        public int DenDigits { get; private set; }

        public void CheckDigits()
        {
            var num = 0L;
            var den = 0L;
            foreach (var item in this.AsSpan())
            {
                if (!item.Position.IsZero())
                {
                    num = Math.Max(num, item.Position.Numerator.ToString().Length);
                    den = Math.Max(den, item.Position.Denominator.ToString().Length);
                }
            }
            var format = $"{{0,{num}}}/{{1,-{den}}}";
            var padLeft = new string(' ', (int)num);
            var padRight = new string(' ', (int)den);
            foreach (var item in this.AsSpan())
            {
                if (item.Position.IsZero())
                {
                    item.PositionText = $"{padLeft}0{padRight}";
                }
                else
                {
                    item.PositionText = string.Format(format, item.Position.Numerator, item.Position.Denominator);
                }
            }
        }
    }

    public class NoteDiff : IDifference, IComparable<NoteDiff>
    {
        public required DiffType DiffType { get; init; }
        public required Rational Position { get; init; }
        public string PositionText { get; internal set; } = "";
        public required Channel Channel { get; init; }
        public NoteType NoteType { get; init; }
        public double Value { get; init; }
        public string? DefValue { get; init; }
        public double Value2 { get; init; }
        public string? DefValue2 { get; init; }

        public static string? GetDefValue(IBmsDataUnit data, Note note)
        {
            if (note.TryGetDefType(out var type) && data.DefLists.TryGetValue(type, (int)note.Value, out var value))
            {
                return value;
            }
            return null;
        }

        public static NoteDiff CreateAdded(Rational position, Note note, IBmsDataUnit data) => new()
        {
            DiffType = DiffType.Added,
            Position = position,
            Channel = note.Channel,
            NoteType = note.Type,
            Value = note.Value,
            DefValue = GetDefValue(data, note),
        };

        public static NoteDiff CreateRemoved(Rational position, Note note, IBmsDataUnit data) => new()
        {
            DiffType = DiffType.Removed,
            Position = position,
            Channel = note.Channel,
            NoteType = note.Type,
            Value = note.Value,
            DefValue = GetDefValue(data, note),
        };

        public static NoteDiff CreateChanged(Rational position, Note oldNote, Note newNote, IBmsDataUnit oldData, IBmsDataUnit newData) => new()
        {
            DiffType = DiffType.Removed,
            Position = position,
            Channel = oldNote.Channel,
            NoteType = oldNote.Type,
            Value = oldNote.Value,
            DefValue = GetDefValue(oldData, oldNote),
            Value2 = newNote.Value,
            DefValue2 = GetDefValue(newData, newNote),
        };

        public int CompareTo(NoteDiff? other)
        {
            var c = DiffType.CompareTo(other!.DiffType);
            if (c is 0)
            {
                var lch = Channel;
                var rch = other.Channel;
                return 
                    lch.IsSoundLane() 
                        ? rch.IsSoundLane() ? Value.CompareTo(other.Value) : 1 
                        : rch.IsSoundLane() ? -1 : lch.CompareTo(rch);
            }
            return c;
        }

        public string GetChangeText(int radix)
        {
            var diffSymbol = GetSymbol(DiffType);
            var defValue = string.IsNullOrEmpty(DefValue) ? "" : $"({DefValue})";
            var ch = Channel;
            var laneText = ch.GetChannelName();
            var valueText = ch.IsDefValue() ? BmsUtils.ToBased((int)Value, radix) : Value.ToString();
            if (ch.IsMine())
            {
                valueText = $"{Value}%";
            }
            else if (ch.IsWavDef())
            {
                var nt = NoteType;
                laneText = laneText + (ch.IsBgm() ? "(bgm)" :
                    nt is NoteType.Invisible ? "(invisible)" :
                    nt is NoteType.LongEnd ? "(ln end)" : 
                    "");
            }
            switch (DiffType)
            {
                case DiffType.Added:
                    return $"{diffSymbol} {PositionText}: {valueText}{defValue} added to {laneText}";
                case DiffType.Removed:
                    return $"{diffSymbol} {PositionText}: {valueText}{defValue} remove from {laneText}";
                default:
                    var valueText2 = ch.IsDefValue() ? BmsUtils.ToBased((int)Value2, radix) : Value2.ToString();
                    var defValue2 = string.IsNullOrEmpty(DefValue2) ? "" : $"({DefValue2})";
                    return $"{diffSymbol} {PositionText}: {valueText}{defValue} -> {valueText2}{defValue2} in {laneText}";
            }
        }
    }
}
