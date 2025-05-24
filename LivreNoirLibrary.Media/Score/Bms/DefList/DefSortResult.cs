using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefSortResult : List<DefSortResultItem>
    {
        public const int RemovedIndex = -1;
        public const int ExtendMask = 0xF000;
        public const string RemovedText = "Removed";
        public const string DefSortHadNoEffect = "DefSort had no effect.";

        private enum ContinueMode { None, Equal, Increment }

        internal DefSortResult(DefIndexMapCollection maps, int oldRadix, int newRadix)
        {
            string GetHeader<T>(T value) where T : Enum => $"#{value.ToString().ToUpper()}";

            if (oldRadix != newRadix)
            {
                Add(new($"{GetHeader(HeaderType.Base)} {oldRadix}", $"{newRadix}"));
            }
            foreach (var (type, map) in maps)
            {
                var header = GetHeader(type);
                short beforeStart, beforeCurrent, afterStart, afterCurrent;
                beforeStart = beforeCurrent = afterStart = afterCurrent = RemovedIndex;
                ContinueMode mode = 0;
                void AddLine(short before, short after)
                {
                    if (beforeStart is > 0)
                    {
                        string beforeText, afterText;
                        if (beforeCurrent > beforeStart)
                        {
                            beforeText = $"{header}{BmsUtils.ToBased(beforeStart, oldRadix)}-{BmsUtils.ToBased(beforeCurrent, oldRadix)}";
                            if (afterStart is RemovedIndex)
                            {
                                afterText = RemovedText;
                            }
                            else if (afterStart == afterCurrent)
                            {
                                afterText = BmsUtils.ToBased(afterCurrent, newRadix);
                            }
                            else
                            {
                                afterText = $"{BmsUtils.ToBased(afterStart, newRadix)}-{BmsUtils.ToBased(afterCurrent, newRadix)}";
                            }
                        }
                        else
                        {
                            beforeText = $"{header}{BmsUtils.ToBased(beforeCurrent, oldRadix)}";
                            afterText = afterCurrent is RemovedIndex ? RemovedText : BmsUtils.ToBased(afterCurrent, newRadix);
                        }
                        Add(new(beforeText, afterText));
                    }
                    beforeStart = before;
                    afterStart = after;
                }
                foreach (var (before, after) in map)
                {
                    var beforeIsContinuous = before == beforeCurrent + 1;
                    var afterIsEqual = after == afterCurrent;
                    var afterIsIncrement = after == afterCurrent + 1;
                    if (!(beforeIsContinuous && ((afterIsEqual && mode is not ContinueMode.Increment) || (afterIsIncrement && mode is not ContinueMode.Equal))))
                    {
                        AddLine(before, after);
                    }
                    mode = beforeIsContinuous 
                            ? afterIsEqual ? ContinueMode.Equal 
                            : afterIsIncrement ? ContinueMode.Increment 
                            : ContinueMode.None
                        : ContinueMode.None;
                    beforeCurrent = before;
                    afterCurrent = after;
                }
                AddLine(RemovedIndex, RemovedIndex);
            }
        }

        public override string ToString()
        {
            if (Count is > 0)
            {
                StringBuilder sb = new();
                sb.AppendLine("DefSort result:");
                foreach (var item in CollectionsMarshal.AsSpan(this))
                {
                    sb.AppendLine($"  {item.Before} → {item.After}");
                }
                return sb.ToString();
            }
            else
            {
                return DefSortHadNoEffect;
            }
        }
    }

    public class DefSortResultItem(string before, string after)
    {
        public string Before { get; } = before.Shared();
        public string After { get; } = after.Shared();
    }
}
