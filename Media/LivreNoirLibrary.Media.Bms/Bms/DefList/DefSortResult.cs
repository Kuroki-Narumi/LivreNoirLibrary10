using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefSortResult : List<(string Before, string After)>
    {
        public const int RemovedIndex = -1;
        public const int ExtendMask = 0xF000;
        public const string RemovedText = "Removed";
        public const string DefSortHadNoEffect = "DefSort had no effect.";

        private enum ContinueMode { None, Equal, Increment }

        internal DefSortResult(IDictionary<DefType, DefIndexMap> maps, int radix)
        {
            string GetHeader<T>(T value) where T : Enum => $"#{value.ToString().ToUpper()}";

            void Add(string before, string after)
            {
                base.Add((before.Shared(), after.Shared()));
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
                            beforeText = $"{header}{BmsUtils.ToBased(beforeStart, radix)}-{BmsUtils.ToBased(beforeCurrent, radix)}";
                            if (afterStart is RemovedIndex)
                            {
                                afterText = RemovedText;
                            }
                            else if (afterStart == afterCurrent)
                            {
                                afterText = BmsUtils.ToBased(afterCurrent, radix);
                            }
                            else
                            {
                                afterText = $"{BmsUtils.ToBased(afterStart, radix)}-{BmsUtils.ToBased(afterCurrent, radix)}";
                            }
                        }
                        else
                        {
                            beforeText = $"{header}{BmsUtils.ToBased(beforeCurrent, radix)}";
                            afterText = afterCurrent is RemovedIndex ? RemovedText : BmsUtils.ToBased(afterCurrent, radix);
                        }
                        Add(beforeText, afterText);
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
                using var o = ObjectPool.Rent<StringBuilder>();
                var sb = o.Value;
                sb.AppendLine("DefSort result:");
                foreach (var (Before, After) in this.AsSpan())
                {
                    sb.AppendLine($"  {Before} → {After}");
                }
                return sb.ToString();
            }
            else
            {
                return DefSortHadNoEffect;
            }
        }
    }
}
