using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public static class ScoreDefinitions
    {
        public static ScoreDefinition Beat_Default { get; } = new((JudgeType.Perfect, 2), (JudgeType.Great, 1));
    }
}
