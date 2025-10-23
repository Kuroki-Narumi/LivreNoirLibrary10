using System;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    partial class IBmsDataExtensions
    {
        public static DiffCheckResult DiffCheck(this BmsData after, BmsData before)
        {
            DiffCheckResult result = new();
            DiffCheck(after, before, result);
            DiffCheck_Flow(after, before, result);
            return result;
        }

        private static void DiffCheck_Flow(BmsData after, BmsData before, DiffResultBase result)
        {
            var flows = result.Flows;
            var leftFlows = before.Flows;
            var rightFlows = after.Flows;
            var leftFlowCount = leftFlows.Count;
            var rightFlowCount = rightFlows.Count;
            var count = Math.Min(leftFlowCount, rightFlowCount);
            Dictionary<int, FlowBranch> rightBranches = [];
            for (var i = 0; i < count; i++)
            {
                FlowAddress flowAddress = new(i);
                var leftFlow = leftFlows[i];
                var rightFlow = rightFlows[i];
                FlowDiff flowDiff = new()
                {
                    Address = flowAddress,
                    OldValue = leftFlow.BmsHeader,
                    NewValue = rightFlow.BmsHeader
                };
                var branches = flowDiff.Branches;
                var leftBranches = leftFlow.Branches;
                var leftBranchCount = leftBranches.Count;
                foreach (var branch in rightFlow.Branches)
                {
                    rightBranches[branch.Condition] = branch;
                }
                for (var j = 0; j < leftBranchCount; j++)
                {
                    var leftBranch = leftBranches[j];
                    var index = leftBranch.Condition;
                    if (rightBranches.Remove(index, out var rightBranch))
                    {
                        DiffResultBase diff = new();
                        DiffCheck(leftBranch, rightBranch, diff);
                        if (!diff.IsEmpty)
                        {
                            branches.Add(index, new()
                            {
                                Address = flowAddress.Append(index),
                                OldValue = leftFlow.GetBranchHeader(leftBranch),
                                NewValue = rightFlow.GetBranchHeader(rightBranch),
                                DataDifference = diff
                            });
                        }
                    }
                    else
                    {
                        branches.Add(index, new()
                        {
                            Address = flowAddress.Append(index),
                            OldValue = leftFlow.GetBranchHeader(leftBranch),
                        });
                    }
                }
                foreach (var (index, rightBranch) in rightBranches)
                {
                    branches.Add(index, new()
                    {
                        Address = flowAddress.Append(index),
                        NewValue = rightFlow.GetBranchHeader(rightBranch),
                    });
                }
                rightBranches.Clear();
                if (branches.Count is > 0)
                {
                    flows.Add(flowDiff);
                }
            }
            if (leftFlowCount > count)
            {
                for (var i = count; i < leftFlowCount; i++)
                {
                    flows.Add(new() { Address = new(i), OldValue = leftFlows[i].BmsHeader });
                }
            }
            else if (rightFlowCount > count)
            {
                for (var i = count; i < rightFlowCount; i++)
                {
                    flows.Add(new() { Address = new(i), NewValue = rightFlows[i].BmsHeader });
                }
            }
        }

        private static void DiffCheck(BaseData after, BaseData before, DiffResultBase result)
        {
            DiffCheck_Header(after, before, result);
            DiffCheck_DefList(after, before, result);
            DiffCheck_BarDef(after, before, result);
            DiffCheck_Notes(after, before, result);
            foreach (var (_, list) in result.Notes)
            {
                list.CheckDigits();
            }
        }

        private static void DiffCheck_Header(BaseData after, BaseData before, DiffResultBase result)
        {
            var list = result.Headers;
            static Dictionary<string, string> CreateSet(HeaderCollection headers)
            {
                Dictionary<string, string> dic = [];
                dic.AddRange(headers.EnumerateHeaders());
                return dic;
            }
            var headerLeft = CreateSet(before.Headers);
            var headerRight = CreateSet(after.Headers);

            foreach (var key in headerLeft.Keys.Union(headerRight.Keys))
            {
                headerLeft.TryGetValue(key, out var valLeft);
                headerRight.TryGetValue(key, out var valRight);
                list.Add(new() { Key = key, OldValue = valLeft, NewValue = valRight });
            }
        }

        private static void DiffCheck_DefList(BaseData after, BaseData before, DiffResultBase result)
        {
            var dict = result.DefLists;
            var defLeft = before.DefLists;
            var defRight = after.DefLists;
            foreach (var type in Enum.GetValues<DefType>())
            {
                if (type.IsConductor())
                {
                    continue;
                }
                var l = defLeft.TryGetValue(type, out var listLeft);
                var r = defRight.TryGetValue(type, out var listRight);
                Dictionary<short, DefDiff> dic = [];
                if (l)
                {
                    if (r)
                    {
                        foreach (var key in listLeft!._keys.Union(listRight!._keys))
                        {
                            listLeft.TryGetValue(key, out var valLeft);
                            listRight.TryGetValue(key, out var valRight);
                            if (valLeft != valRight)
                            {
                                dic[key] = new() { OldValue = valLeft, NewValue = valRight };
                            }
                        }
                    }
                    else
                    {
                        foreach (var (key, value) in listLeft!)
                        {
                            dic.Add(key, new() { OldValue = value, NewValue = null });
                        }
                    }
                }
                else if (r)
                {
                    foreach (var (key, value) in listRight!)
                    {
                        dic.Add(key, new() { OldValue = null, NewValue = value });
                    }
                }
                if (dic.Count > 0)
                {
                    dict[type] = dic;
                }
            }
        }

        private static void DiffCheck_BarDef(BaseData after, BaseData before, DiffResultBase result)
        {
            var bars = result.BarDefs;
            var barLeft = before.Bars;
            var barRight = after.Bars;
            for (var i = 0; i <= Constants.MaxBarNumber; i++)
            {
                barLeft.TryGetValue(i, out var valLeft);
                barRight.TryGetValue(i, out var valRight);
                if (valLeft != valRight)
                {
                    bars.Add(i, new() { OldValue = valLeft, NewValue = valRight });
                }
            }
        }

        private static void DiffCheck_Notes(BaseData after, BaseData before, DiffResultBase result)
        {
            var dict = result.Notes;
            var leftTimeline = before.Timeline;
            var rightTimeline = after.Timeline;
            SortedSet<BarPosition> positions = [];
            foreach (var pos in leftTimeline.GetPositions())
            {
                positions.Add(pos);
            }
            foreach (var pos in rightTimeline.GetPositions())
            {
                positions.Add(pos);
            }

            List<(Channel Channel, IChannelNote Note)> leftNotes_Channel = [];
            List<ISoundNote> leftNotes_Sound = [];
            List<NoteDiff> buffer = [];
            static string? GetDefValue(BaseData data, INote note)
            {
                if (note.TryGetDefType(out var type, out var iNote) && data.DefLists.TryGetValue(type, iNote.Value, out var value))
                {
                    return value;
                }
                return null;
            }
            foreach (var pos in positions)
            {
                var l = leftTimeline.TryGet(pos, out var leftList);
                var r = rightTimeline.TryGet(pos, out var rightList);
                if (l)
                {
                    if (r)
                    {
                        buffer.Clear();
                        leftNotes_Channel.Clear();
                        leftNotes_Sound.Clear();
                        foreach (var note in leftList!)
                        {
                            if (note is IChannelNote c)
                            {
                                leftNotes_Channel.Add((c.Channel, c));
                            }
                            else if (note is ISoundNote s)
                            {
                                leftNotes_Sound.Add(s);
                            }
                        }
                        foreach (var rightNote in rightList!)
                        {
                            int index;
                            if (rightNote is IChannelNote c)
                            {
                                var channel = c.Channel;
                                index = leftNotes_Channel.FindIndex(n => n.Channel == channel);
                                if (index is >= 0)
                                {
                                    leftNotes_Channel.RemoveAt(index);
                                }
                            }
                            else if (rightNote is ISoundNote s)
                            {
                                var value = s.Value;
                                index = leftNotes_Sound.FindIndex(n => n.Value == s.Value);
                                if (index is >= 0)
                                {
                                    leftNotes_Sound.RemoveAt(index);
                                }
                            }
                            else
                            {
                                continue;
                            }
                            if (index is < 0)
                            {
                                buffer.Add(new() { DiffType = DiffType.Added, Position = pos.Offset, Note = rightNote, DefValue = GetDefValue(after, rightNote) });
                            }
                        }
                        foreach (var (_, leftNote) in leftNotes_Channel)
                        {
                            buffer.Add(new() { DiffType = DiffType.Removed, Position = pos.Offset, Note = leftNote, DefValue = GetDefValue(before, leftNote) });
                        }
                        foreach (var leftNote in leftNotes_Sound)
                        {
                            buffer.Add(new() { DiffType = DiffType.Removed, Position = pos.Offset, Note = leftNote, DefValue = GetDefValue(before, leftNote) });
                        }
                        if (buffer.Count is > 0)
                        {
                            buffer.Sort();
                            dict.GetOrAdd(pos.Bar).AddRange(buffer);
                        }
                    }
                    else
                    {
                        var list = dict.GetOrAdd(pos.Bar);
                        foreach (var note in leftList!)
                        {
                            list.Add(new() { DiffType = DiffType.Removed, Position = pos.Offset, Note = note, DefValue = GetDefValue(before, note) });
                        }
                    }
                }
                else if (r)
                {
                    var list = dict.GetOrAdd(pos.Bar);
                    foreach (var note in rightList!)
                    {
                        list.Add(new() { DiffType = DiffType.Added, Position = pos.Offset, Note = note, DefValue = GetDefValue(after, note) });
                    }
                }
            }
        }
    }
}
