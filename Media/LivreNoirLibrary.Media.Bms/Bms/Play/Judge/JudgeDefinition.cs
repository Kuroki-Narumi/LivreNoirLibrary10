using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public readonly struct JudgeDefinition : IComparable<JudgeDefinition>
    {
        public JudgeType Type { get; init; }
        public double BeforeMargin { get; init; }
        public double AfterMargin { get; init; }
        public ComboChange ComboChange { get; init; }
        public bool IsMiss { get; init; }
        public bool IsRepeatable { get; init; }

        public int CompareTo(JudgeDefinition other)
        {
            var c = BeforeMargin.CompareTo(other.BeforeMargin);
            return c is 0 ? AfterMargin.CompareTo(other.AfterMargin) : c;
        }
    }
}
