using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class JudgeDefinitionCollection
    {
        private readonly JudgeDefinition[] _definitions;

        public JudgeDefinition ThroughJudge { get; }

        public JudgeDefinitionCollection(JudgeDefinition through, params ReadOnlySpan<JudgeDefinition> judges)
        {
            ThroughJudge = through;
            _definitions = [.. judges];
            Array.Sort(_definitions);
        }

        public bool TryGetJudge(double error, out JudgeDefinition judge)
        {
            foreach (var j in _definitions)
            {
                if (j.BeforeMargin >= -error && j.AfterMargin <= error)
                {
                    judge = j;
                    return true;
                }
            }
            judge = default;
            return false;
        }
    }
}
