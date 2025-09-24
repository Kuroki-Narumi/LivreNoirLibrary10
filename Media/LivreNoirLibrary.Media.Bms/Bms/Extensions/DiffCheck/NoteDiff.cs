using LivreNoirLibrary.Debug;
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
            foreach (var item in CollectionsMarshal.AsSpan(this))
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
            foreach (var item in CollectionsMarshal.AsSpan(this))
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
        public required INote Note { get; init; }
        public string? DefValue { get; init; }

        public int CompareTo(NoteDiff? other)
        {
            var c = DiffType.CompareTo(other!.DiffType);
            if (c is 0)
            {
                if (Note is ISoundNote ls)
                {
                    if (other.Note is ISoundNote rs)
                    {
                        return ls.Value.CompareTo(rs.Value);
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (other.Note is ISoundNote)
                    {
                        return -1;
                    }
                    else
                    {
                        return (Note as IChannelNote)!.Channel.CompareTo((other.Note as IChannelNote)!.Channel);
                    }
                }
            }
            return c;
        }

        public string GetChangeText(int radix)
        {
            var diffSymbol = GetSymbol(DiffType);
            var diffText = DiffType is DiffType.Added ? "added to" : "removed from";
            var defValue = string.IsNullOrEmpty(DefValue) ? "" : $"({DefValue})";
            string valueText, laneText;
            switch (Note)
            {
                case IConductorNote c:
                    valueText = c.Channel is Channel.Stop ? c.Value.ToString() : c.DecimalValue.ToString();
                    laneText = c.Channel.ToString();
                    break;
                case IMetaNote m:
                    valueText = m.Channel.IsDefChannel() ? BmsUtils.ToBased(m.Value, radix) : m.Value.ToString();
                    laneText = m.Channel.ToString();
                    break;
                case ISoundNote s:
                    string suffix;
                    if (s.IsMine())
                    {
                        valueText = s.Value.ToString();
                        suffix = "(mine)";
                    }
                    else
                    {
                        valueText = BmsUtils.ToBased(s.Value, radix);
                        suffix =
                            s.IsBgm() ? "(bgm)" :
                            s.IsInvisible() ? "(invisible)" :
                            s.IsLongEnd() ? "(ln end)" :
                            "";
                    }
                    laneText = $"{s.Lane}{suffix}";
                    break;
                default:
                    return "";
            }
            return $"{diffSymbol} {PositionText}: {valueText}{defValue} {diffText} {laneText}";
        }
    }
}
