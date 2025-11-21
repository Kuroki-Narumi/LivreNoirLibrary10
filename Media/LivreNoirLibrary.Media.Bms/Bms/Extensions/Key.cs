using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension(IListEnumerable<double, Note> timeline)
        {
            public int GetNoteCount(bool includeLongEnd = false)
            {
                var count = 0;
                foreach (var (_, list) in timeline.EnumerateList())
                {
                    foreach (var note in list.AsSpan())
                    {
                        if (note.IsVisibleKey(includeLongEnd))
                        {
                            count++;
                        }
                    }
                }
                return count;
            }

            public int GetNoteCount(Predicate<Note> selector)
            {
                var count = 0;
                foreach (var (_, list) in timeline.EnumerateList())
                {
                    foreach (var note in list.AsSpan())
                    {
                        if (selector(note))
                        {
                            count++;
                        }
                    }
                }
                return count;
            }

            public int GetMaxBgmLane()
            {
                var max = 0;
                foreach (var (_, list) in timeline.EnumerateList())
                {
                    foreach (var note in list.AsSpan())
                    {
                        if (note.Channel.TryGetLane(out var lane) && lane < max)
                        {
                            max = lane;
                        }
                    }
                }
                return 1 - max;
            }
        }

        extension (IBmsData root)
        {
            public DefIndexCollection GetUsedDefList(DefIndexCollection? used = null)
            {
                used ??= [];
                foreach (var d in root.EnumerateAllData())
                {
                    foreach (var (_, note) in d.Timeline)
                    {
                        if (note.TryGetDefType(out var type) && note.Value is not 0)
                        {
                            used.Add(type, (int)note.Value);
                        }
                    }
                }
                return used;
            }

            public HashSet<Channel> GetUsedKeyLanes(HashSet<Channel>? set = null)
            {
                set ??= [];
                foreach (var data in root.EnumerateAllData())
                {
                    foreach (var (_, note) in data.Timeline)
                    {
                        if (note.IsKey())
                        {
                            set.Add(note.Channel);
                        }
                    }
                }
                return set;
            }

            public int GetKeyCount() => root.ChartType switch
            {
                ChartType.Beat => GetKeyCount_Beat(root),
                ChartType.Popn => GetKeyCount_Popn(root),
                ChartType.Keyboard => GetKeyCount_Keyboard(root),
                _ => GetUsedKeyLanes(root).Count,
            };

            private int GetKeyCount_Beat()
            {
                var like_7 = false;
                var like_10 = false;
                foreach (var data in root.EnumerateAllData())
                {
                    foreach (var (_, note) in data.Timeline)
                    {
                        if (note.IsKey())
                        {
                            switch (note.Channel)
                            {
                                case >= Channel.Beat_2P_6:
                                    return 14;
                                case >= Channel.Beat_2P_1:
                                    like_10 = true;
                                    break;
                                case Channel.Beat_1P_6 or Channel.Beat_1P_7:
                                    like_7 = true;
                                    break;
                            }
                        }
                    }
                }
                return like_7 ? (like_10 ? 14 : 7) : (like_10 ? 10 : 5);
            }

            private int GetKeyCount_Popn()
            {
                var like_3 = false;
                var like_5 = false;
                var like_9 = false;
                foreach (var data in root.EnumerateAllData())
                {
                    foreach (var (_, note) in data.Timeline)
                    {
                        if (note.IsKey())
                        {
                            switch (note.Channel)
                            {
                                case (>= Channel.Popn_1P_8 and <= Channel.Popn_1P_6) or >= Channel.Popn_2P_8:
                                    return 18;
                                case >= Channel.Popn_6:
                                    like_9 = true;
                                    break;
                                case >= Channel.Popn_4:
                                    like_5 = true;
                                    break;
                                case >= Channel.Popn_1 and <= Channel.Popn_3:
                                    like_3 = true;
                                    break;
                            }
                        }
                    }
                }
                return like_3 ? (like_5 ? 9 : 3) : (like_9 ? 9 : 5);
            }

            private int GetKeyCount_Keyboard()
            {
                foreach (var data in root.EnumerateAllData())
                {
                    foreach (var (_, note) in data.Timeline)
                    {
                        if (note.Channel.TryGetLane(out var lane) && lane is >= 25)
                        {
                            return 48;
                        }
                    }
                }
                return 24;
            }
        }
    }
}
