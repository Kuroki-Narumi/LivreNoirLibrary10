using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension(IFlowContainer obj)
        {
            public string BmsHeader => $"{(obj.Type is FlowType.Random
                ? (obj.IsFixed ? Tags.SetRandom : Tags.Random)
                : (obj.IsFixed ? Tags.SetSwitch : Tags.Switch))} {obj.Max}";

            public string BmsFooter => obj.Type is FlowType.Random ? Tags.EndRandom : Tags.EndSwitch;

            public string GetBranchHeader(int condition) => obj.Type is FlowType.Random
                    ? condition is BmsConstants.DefaultCondition ? Tags.Else : $"{Tags.If} {condition}"
                    : condition is BmsConstants.DefaultCondition ? Tags.Default : $"{Tags.Case} {condition}";

            public string GetBranchFooter() => obj.Type is FlowType.Random ? Tags.EndIf : Tags.Skip;

            public FlowBranch? GetBranch(int condition)
            {
                if (obj.Branches.Find(branch => branch.Condition == condition) is { } branch)
                {
                    return branch;
                }
                return obj.DefaultBranch;
            }

            public FlowBranch GetOrAddBranch(int condition)
            {
                if (condition is BmsConstants.DefaultCondition)
                {
                    return (obj.DefaultBranch ??= new(condition));
                }
                else if (obj.Branches.Find(branch => branch.Condition == condition) is { } branch)
                {
                    return branch;
                }
                else
                {
                    branch = new(condition);
                    obj.Branches.Add(branch);
                    return branch;
                }
            }

            public void EnsureBranches(bool ensureDefault = false)
            {
                var set = ObjectPool.Rent<HashSet<int>>();
                try
                {
                    var branches = obj.Branches;
                    foreach (var branch in branches.AsSpan())
                    {
                        set.Add(branch.Condition);
                    }
                    var max = obj.Max;
                    for (var i = 1; i <= max; i++)
                    {
                        if (set.Add(i))
                        {
                            branches.Add(new(i));
                        }
                    }
                    if (ensureDefault)
                    {
                        obj.DefaultBranch ??= new(BmsConstants.DefaultCondition);
                    }
                }
                finally
                {
                    ObjectPool.Return(set);
                }
            }

            public void SortBranches(bool ascending = true)
            {
                Comparison<FlowBranch> comparison = ascending
                    ? (a, b) => a.Condition.CompareTo(b.Condition)
                    : (a, b) => b.Condition.CompareTo(a.Condition);
                obj.Branches.Sort(comparison);
            }

            public bool DeleteBranch(IBmsData root, FlowBranch branch)
            {
                if (obj.DefaultBranch == branch)
                {
                    obj.DefaultBranch = null;
                }
                else if (!obj.Branches.Remove(branch))
                {
                    return false;
                }
                root.InsulateBranch(branch);
                return true;
            }

            public IEnumerable<FlowBranch> EnumerateBranches()
            {
                var list = obj.Branches;
                var count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    yield return list[i];
                }
                if (obj.DefaultBranch is { } branch)
                {
                    yield return branch;
                }
            }

            public void CopyFrom(IFlowContainer source)
            {
                obj.Note = source.Note;
                obj.Type = source.Type;
                obj.IsFixed = source.IsFixed;
                obj.Max = source.Max;
            }

            public void Dump(BinaryWriter writer)
            {
                writer.WriteNullable(obj.Note);
                writer.Write((byte)obj.Type);
                writer.Write(obj.Max);
                writer.Write(obj.IsFixed);
                var defaultBranch = obj.DefaultBranch;
                writer.Write(obj.Branches.Count + (defaultBranch is not null ? 1 : 0));
                foreach (var branch in obj.Branches.AsSpan())
                {
                    branch.Dump(writer);
                }
                defaultBranch?.Dump(writer);
            }

            public void ProcessLoad(BinaryReader reader)
            {
                obj.Note = reader.ReadStringOrNull();
                obj.Type = (FlowType)reader.ReadByte();
                obj.Max = reader.ReadInt32();
                obj.IsFixed = reader.ReadBoolean();
                var branches = obj.Branches;
                branches.Clear();
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var branch = FlowBranch.Load(reader);
                    if (branch.Condition is BmsConstants.DefaultCondition)
                    {
                        obj.DefaultBranch = branch;
                    }
                    else
                    {
                        branches.Add(branch);
                    }
                }
            }
        }
    }
}
