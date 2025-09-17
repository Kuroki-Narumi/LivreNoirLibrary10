using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using static LivreNoirLibrary.Media.Bms.KeyLanes;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        public static int GetNoteCount(this IBmsData data, bool includeLongEnd = false)
        {
            var result = 0;
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsVisibleKey(includeLongEnd, out _))
                {
                    result++;
                }
            }
            return result;
        }

        public static int GetNoteCount(this IBmsData data, Predicate<INote> selector)
        {
            var result = 0;
            foreach (var (_, note) in data.Timeline)
            {
                if (selector(note))
                {
                    result++;
                }
            }
            return result;
        }

        public static int GetMaxBgmLane(this IBmsData data)
        {
            var max = 0;
            foreach (var (_, note) in data.Timeline)
            {
                if (note.IsBgm(out var n) && n.Lane < max)
                {
                    max = n.Lane;
                }
            }
            return 1 - max;
        }

        public static HashSet<int> GetUsedKeyLanes(this IRootData root, HashSet<int>? set = null)
        {
            set ??= [];
            foreach (var data in root.EachData())
            {
                foreach (var (_, note) in data.Timeline)
                {
                    if (note is ISoundNote s)
                    {
                        set.Add(s.Lane);
                    }
                }
            }
            return set;
        }

        public static int GetKeyCount(this IRootData root)
        {
            switch (root.ChartType)
            {
                case ChartType.Beat:
                    return GetKeyCount_Beat(root);
                case ChartType.Popn:
                    return GetKeyCount_Popn(root);
                case ChartType.Keyboard:
                    return GetKeyCount_Keyboard(root);
                default:
                    return GetUsedKeyLanes(root).Count;
            }
        }

        private static int GetKeyCount_Beat(IRootData root)
        {
            var like_7 = false;
            var like_10 = false;
            foreach (var data in root.EachData())
            {
                foreach (var (_, note) in data.Timeline)
                {
                    if (note is ISoundNote s)
                    {
                        switch (s.Lane)
                        {
                            case >= Beat_2P_6:
                                return 14;
                            case >= Beat_2P_1:
                                like_10 = true;
                                break;
                            case Beat_1P_6 or Beat_1P_7:
                                like_7 = true;
                                break;
                        }
                    }
                }
            }
            return like_7 ? (like_10 ? 14 : 7) : (like_10 ? 10 : 5);
        }

        private static int GetKeyCount_Popn(IRootData root)
        {
            var like_3 = false;
            var like_5 = false;
            var like_9 = false;
            foreach (var data in root.EachData())
            {
                foreach (var (_, note) in data.Timeline)
                {
                    if (note is ISoundNote s)
                    {
                        switch (s.Lane)
                        {
                            case (>= Pop_1P_8 and <= Pop_1P_6) or >= Pop_2P_8:
                                return 18;
                            case >= Pop_6:
                                like_9 = true;
                                break;
                            case >= Pop_4:
                                like_5 = true;
                                break;
                            case >= Pop_1 and <= Pop_3:
                                like_3 = true;
                                break;
                        }
                    }
                }
            }
            return like_3 ? (like_5 ? 9 : 3) : (like_9 ? 9 : 5);
        }

        private static int GetKeyCount_Keyboard(IRootData root)
        {
            foreach (var data in root.EachData())
            {
                foreach (var (_, note) in data.Timeline)
                {
                    if (note is ISoundNote { Lane: >= 25 })
                    {
                        return 48;
                    }
                }
            }
            return 24;
        }

        public static int GetMaxBarResolution(this IRootData root)
        {
            Dictionary<int, long> localMax = [];
            var max = 0;
            foreach (var data in root.EachData())
            {
                max = Math.Max(max, GetLocalMaxBarResolution(data, localMax));
            }
            return max;
        }

        public static int GetLocalMaxBarResolution(this IBmsData data, Dictionary<int, long>? localMax = null)
        {
            localMax ??= [];
            localMax.Clear();
            foreach (var (bar, beat) in data.Timeline.GetPositions())
            {
                var b = beat / data.GetBarLength(bar);
                if (localMax.TryGetValue(bar, out var current))
                {
                    localMax[bar] = current.LCM(b.Denominator);
                }
                else
                {
                    localMax.Add(bar, b.Denominator);
                }
            }
            return localMax.Count is 0 ? 1 : (int)localMax.Values.Max();
        }
    }
}
