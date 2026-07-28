using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandTestResult : IClear
    {
        public int TotalCount { get; private set; }
        public ValueResult Value1 { get; } = new();
        public ValueResult Value2 { get; } = new();
        public List<HandConditionResult> Conditions { get; } = [];
        public List<HandConditionResult> GroupResults { get; } = [];

        private readonly Dictionary<int, int> _sumBuffer = [];

        public void Clear()
        {
            TotalCount = 0;
            Value1.Values.Clear();
            Value2.Values.Clear();
            Conditions.Clear();
            GroupResults.Clear();
        }

        public void AddValue(double value1, double value2)
        {
            TotalCount++;
            Value1.Values.Add(value1);
            Value2.Values.Add(value2);
        }

        public void EndInit(HandConditionsCollection conditions)
        {
            var total = TotalCount;
            Value1.EndInit();
            Value2.EndInit();
            var sum = _sumBuffer;
            var conds = Conditions;
            foreach (var cond in conditions.AsSpan())
            {
                conds.Add(new(cond, total));
                var g = cond.GroupId;
                sum[g] = (sum.TryGetValue(g, out var current) ? current : 0) + cond.Count;
            }
            var groups = GroupResults;
            foreach (var (group, count) in sum)
            {
                groups.Add(new(null, group, count, total));
            }
            sum.Clear();
        }
    }
}
