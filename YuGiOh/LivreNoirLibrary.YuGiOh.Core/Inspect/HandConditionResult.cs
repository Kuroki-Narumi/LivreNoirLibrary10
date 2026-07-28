using System;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public readonly struct HandConditionResult(string? name, int groupId, int count, int totalCount)
    {
        public string? Name { get; } = name;
        public int GroupId { get; } = groupId;
        public int Count { get; } = count;
        public double Probability { get; } = (double)count / totalCount;
        public string ProbText => $"{Probability:P2}";

        public HandConditionResult(HandConditions condition, int totalCount) : this(condition.Name, condition.GroupId, condition.Count, totalCount) { }
    }
}
