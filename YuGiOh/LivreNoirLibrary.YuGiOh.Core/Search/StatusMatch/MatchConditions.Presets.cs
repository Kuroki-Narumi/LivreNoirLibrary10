using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class MatchConditions
    {
        public static MatchConditions SmallWorld { get; } = new()
        {
            Attribute = true,
            MonsterType = true,
            Level = true,
            Atk = true,
            Def = true,
            AtkDef = false,
            Count = 1,
            AllowsGreater = false,
            ExceptSelf = true,
            Candidate_Main = true,
            Target_Main = true,
        };

        public static MatchConditions Sculptor { get; } = new()
        {
            Attribute = false,
            MonsterType = false,
            Level = false,
            Atk = false,
            Def = false,
            AtkDef = true,
            Count = 1,
            AllowsGreater = true,
            ExceptSelf = false,
            Candidate_Main = false,
            Target_Main = true,
        };

        public static MatchConditions Nightmell { get; } = new()
        {
            Attribute = true,
            MonsterType = true,
            Level = true,
            Atk = true,
            Def = true,
            AtkDef = false,
            Count = 5,
            AllowsGreater = true,
            ExceptSelf = true,
            Candidate_Main = false,
            Target_Main = false,
        };

        public static MatchConditions Hedgehog { get; } = new()
        {
            Attribute = true,
            MonsterType = true,
            Level = true,
            Atk = false,
            Def = false,
            AtkDef = false,
            Count = 3,
            AllowsGreater = true,
            ExceptSelf = true,
            Candidate_Main = true,
            Target_Main = true,
        };

        public void CopyFrom(MatchConditions other)
        {
            Attribute = other.Attribute;
            MonsterType = other.MonsterType;
            Level = other.Level;
            Atk = other.Atk;
            Def = other.Def;
            AtkDef = other.AtkDef;
            Count = other.Count;
            CountMax = other.CountMax;
            AllowsGreater = other.AllowsGreater;
            ExceptSelf = other.ExceptSelf;
            Candidate_Main = other.Candidate_Main;
            Target_Main = other.Target_Main;
        }
    }
}
