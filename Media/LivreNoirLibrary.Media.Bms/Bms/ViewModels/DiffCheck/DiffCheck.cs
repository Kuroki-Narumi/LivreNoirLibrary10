using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsExtensions
    {
        public static DiffCheckResult DiffCheck(this IBmsData after, IBmsData before)
        {
            DiffCheckResult result = new();
            DiffCheck(before, after, before.Root, after.Root, result);
            var leftLnObj = before.LnObj;
            var rightLnObj = after.LnObj;
            if (leftLnObj != rightLnObj)
            {
                result.Headers.Add(new() { Key = "LNOBJ", OldValue = leftLnObj.ToString(), NewValue = rightLnObj.ToString() });
            }
            return result;
        }

        private static void DiffCheck(IBmsData leftRoot, IBmsData rightRoot, IBmsDataUnit left, IBmsDataUnit right, DiffResultBase result)
        {
            DiffCheck_Header(left, right, result);
            DiffCheck_DefList(left, right, result);
            DiffCheck_BarDef(left, right, result);
            DiffCheck_Notes(left, right, result);
            DiffCheck_Flow(leftRoot, rightRoot, left, right, result);
            foreach (var (_, list) in result.Notes)
            {
                list.CheckDigits();
            }
        }

        private static void DiffCheck_Header(IBmsDataUnit left, IBmsDataUnit right, DiffResultBase result)
        {
            var list = result.Headers;
            var headerLeft = CreateSet(left);
            var headerRight = CreateSet(right);
            foreach (var key in headerLeft.Keys.Union(headerRight.Keys))
            {
                headerLeft.TryGetValue(key, out var valLeft);
                headerRight.TryGetValue(key, out var valRight);
                if (valLeft != valRight)
                {
                    list.Add(new() { Key = key, OldValue = valLeft, NewValue = valRight });
                }
            }
        }

        private static Dictionary<string, string> CreateSet(IBmsDataUnit data)
        {
            Dictionary<string, string> dic = [];
            dic.AddRange(data.MainHeaders.Select(kv => KeyValuePair.Create(kv.Key.ToString().ToUpper(), kv.Value)));
            dic.AddRange(data.SubHeaders.Select(h => KeyValuePair.Create(h.Key, h.Value)));
            return dic;
        }

        private static void DiffCheck_DefList(IBmsDataUnit left, IBmsDataUnit right, DiffResultBase result)
        {
            var dict = result.DefLists;
            var defLeft = left.DefLists;
            var defRight = right.DefLists;
            foreach (var type in Enum.GetValues<DefType>())
            {
                if (type.IsConductor())
                {
                    continue;
                }
                var l = defLeft.TryGetList(type, out var listLeft);
                var r = defRight.TryGetList(type, out var listRight);
                Dictionary<short, DefDiff> dic = [];
                if (l)
                {
                    if (r)
                    {
                        foreach (var key in listLeft!.Keys.Union(listRight!.Keys))
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

        private static void DiffCheck_BarDef(IBmsDataUnit left, IBmsDataUnit right, DiffResultBase result)
        {
            var bars = result.BarDefs;
            var barLeft = left.BarDefs;
            var barRight = right.BarDefs;
            for (var i = 0; i <= BmsConstants.MaxBarNumber; i++)
            {
                barLeft.TryGetValue(i, out var valLeft);
                barRight.TryGetValue(i, out var valRight);
                if (valLeft != valRight)
                {
                    bars.Add(i, new() { OldValue = valLeft, NewValue = valRight });
                }
            }
        }

        private static void DiffCheck_Notes(IBmsDataUnit left, IBmsDataUnit right, DiffResultBase result)
        {
            var dict = result.Notes;
            var leftTimeline = left.Timeline;
            var rightTimeline = right.Timeline;
            SortedSet<BarPosition> positions = [];
            foreach (var pos in leftTimeline.GetPositions())
            {
                positions.Add(pos);
            }
            foreach (var pos in rightTimeline.GetPositions())
            {
                positions.Add(pos);
            }
            List<(Channel Channel, Note Note)> leftNotes = [];
            List<NoteDiff> buffer = [];
            foreach (var pos in positions)
            {
                var ratOffset = pos.RationalOffset;
                var l = leftTimeline.TryGetValue(pos, SearchMode.Equal, out _, out var leftList);
                var r = rightTimeline.TryGetValue(pos, SearchMode.Equal, out _, out var rightList);
                if (l)
                {
                    if (r)
                    {
                        buffer.Clear();
                        leftNotes.Clear();
                        foreach (var note in leftList!)
                        {
                            leftNotes.Add((note.IsSound() ? 0 : note.Channel, note));
                        }
                        for (var i = 0; i < rightList!.Count;)
                        {
                            var rightNote = rightList[i];
                            var channel = rightNote.IsSound() ? 0 : rightNote.Channel;
                            var value = rightNote.Value;
                            var index = leftNotes.FindIndex(n => n.Channel == channel && n.Note.Value == value);
                            if (index is >= 0)
                            {
                                leftNotes.RemoveAt(index);
                                rightList.RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                        }
                        foreach (var rightNote in rightList)
                        {
                            var channel = rightNote.IsSound() ? 0 : rightNote.Channel;
                            var value = rightNote.Value;
                            if (channel is not 0)
                            {
                                var index = leftNotes.FindIndex(n => n.Channel == channel);
                                if (index is >= 0)
                                {
                                    buffer.Add(NoteDiff.CreateChanged(ratOffset, leftNotes[index].Note, rightNote, left, right));
                                    leftNotes.RemoveAt(index);
                                    continue;
                                }
                            }
                            buffer.Add(NoteDiff.CreateAdded(ratOffset, rightNote, right));
                        }
                        foreach (var (_, leftNote) in leftNotes)
                        {
                            buffer.Add(NoteDiff.CreateRemoved(ratOffset, leftNote, left));
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
                            buffer.Add(NoteDiff.CreateRemoved(ratOffset, note, left));
                        }
                    }
                }
                else if (r)
                {
                    var list = dict.GetOrAdd(pos.Bar);
                    foreach (var note in rightList!)
                    {
                        buffer.Add(NoteDiff.CreateAdded(ratOffset, note, right));
                    }
                }
            }
        }

        private static void DiffCheck_Flow(IBmsData leftRoot, IBmsData rightRoot, IBmsDataUnit left, IBmsDataUnit right, DiffResultBase result)
        {
            var parentAddress = result.FlowAddress;
            var flows = result.Flows;
            var leftFlows = left.Flows;
            var rightFlows = right.Flows;
            var leftFlowCount = leftFlows.Count;
            var rightFlowCount = rightFlows.Count;
            var count = Math.Min(leftFlowCount, rightFlowCount);
            Dictionary<int, FlowBranch> rightBranches = [];
            for (var i = 0; i < count; i++)
            {
                var newAddress = parentAddress.Append(i);
                var leftFlow = leftFlows[i];
                var rightFlow = rightFlows[i];
                FlowDiff flowDiff = new()
                {
                    Address = newAddress,
                    OldValue = leftFlow.BmsHeader,
                    NewValue = rightFlow.BmsHeader,
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
                    var condition = leftBranch.Condition;
                    var branchAddress = newAddress.Append(condition);
                    if (rightBranches.Remove(condition, out var rightBranch))
                    {
                        DiffResultBase diff = new(branchAddress);
                        var leftBranchData = leftRoot.GetBranchData(leftBranch);
                        var rightBranchData = rightRoot.GetBranchData(rightBranch);
                        DiffCheck(leftRoot, rightRoot, leftBranchData, rightBranchData, diff);
                        if (!diff.IsEmpty)
                        {
                            branches.Add(condition, new()
                            {
                                Address = diff.FlowAddress,
                                OldValue = leftFlow.GetBranchHeader(leftBranch.Condition),
                                NewValue = rightFlow.GetBranchHeader(rightBranch.Condition),
                                DataDifference = diff
                            });
                        }
                    }
                    else
                    {
                        branches.Add(condition, new()
                        {
                            Address = branchAddress,
                            OldValue = leftFlow.GetBranchHeader(leftBranch.Condition),
                        });
                    }
                }
                foreach (var (condition, rightBranch) in rightBranches)
                {
                    branches.Add(condition, new()
                    {
                        Address = newAddress.Append(condition),
                        NewValue = rightFlow.GetBranchHeader(rightBranch.Condition),
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
                    flows.Add(new() { Address = parentAddress.Append(i), OldValue = leftFlows[i].BmsHeader });
                }
            }
            else if (rightFlowCount > count)
            {
                for (var i = count; i < rightFlowCount; i++)
                {
                    flows.Add(new() { Address = parentAddress.Append(i), NewValue = rightFlows[i].BmsHeader });
                }
            }
        }
    }
}
