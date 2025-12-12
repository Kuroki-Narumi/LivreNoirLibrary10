using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public static class JudgeDefinitions
    {
        public static JudgeDefinitionCollection Beat_Gambol { get; } = new(Beat_Poor, Beat_VeryHard_Perfect, Beat_Bad, Beat_BlankShot);
        public static JudgeDefinitionCollection Beat_VeryHard { get; } = new(Beat_Poor, Beat_VeryHard_Perfect, Beat_VeryHard_Great, Beat_VeryHard_Good, Beat_Bad, Beat_BlankShot);
        public static JudgeDefinitionCollection Beat_Hard { get; } = new(Beat_Poor, Beat_Hard_Perfect, Beat_Hard_Great, Beat_Hard_Good, Beat_Bad, Beat_BlankShot);
        public static JudgeDefinitionCollection Beat_Normal { get; } = new(Beat_Poor, Beat_Normal_Perfect, Beat_Normal_Great, Beat_Normal_Good, Beat_Bad, Beat_BlankShot);
        public static JudgeDefinitionCollection Beat_Easy { get; } = new(Beat_Poor, Beat_Easy_Perfect, Beat_Easy_Great, Beat_Easy_Good, Beat_Bad, Beat_BlankShot);
        public static JudgeDefinitionCollection Beat_VeryEasy { get; } = new(Beat_Poor, Beat_VeryEasy_Perfect, Beat_VeryEasy_Great, Beat_VeryEasy_Good, Beat_Bad, Beat_BlankShot);

        public static JudgeDefinition Beat_Bad { get; } = new()
        {
            Type = JudgeType.Bad,
            BeforeMargin = 0.2,
            AfterMargin = 0.2,
            ComboChange = ComboChange.Reset,
            IsMiss = true,
        };

        public static JudgeDefinition Beat_Poor { get; } = new()
        {
            Type = JudgeType.Through,
            AfterMargin = 0.2,
            ComboChange = ComboChange.Reset,
            IsMiss = true,
        };
        
        public static JudgeDefinition Beat_BlankShot { get; } = new()
        {
            Type = JudgeType.BlankShot,
            BeforeMargin = 0.8,
            AfterMargin = 0,
            ComboChange = ComboChange.Continue,
            IsMiss = true,
            IsRepeatable = true,
        };

        public static JudgeDefinition Beat_VeryHard_Perfect { get; } = new()
        {
            Type = JudgeType.Perfect,
            BeforeMargin = 0.008,
            AfterMargin = 0.008,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_VeryHard_Great { get; } = new()
        {
            Type = JudgeType.Great,
            BeforeMargin = 0.02,
            AfterMargin = 0.02,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_VeryHard_Good { get; } = new()
        {
            Type = JudgeType.Good,
            BeforeMargin = 0.04,
            AfterMargin = 0.04,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Hard_Perfect { get; } = new()
        {
            Type = JudgeType.Perfect,
            BeforeMargin = 0.013,
            AfterMargin = 0.013,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Hard_Great { get; } = new()
        {
            Type = JudgeType.Great,
            BeforeMargin = 0.03,
            AfterMargin = 0.03,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Hard_Good { get; } = new()
        {
            Type = JudgeType.Good,
            BeforeMargin = 0.06,
            AfterMargin = 0.06,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Normal_Perfect { get; } = new()
        {
            Type = JudgeType.Perfect,
            BeforeMargin = 0.017,
            AfterMargin = 0.017,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Normal_Great { get; } = new()
        {
            Type = JudgeType.Great,
            BeforeMargin = 0.045,
            AfterMargin = 0.045,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Normal_Good { get; } = new()
        {
            Type = JudgeType.Good,
            BeforeMargin = 0.09,
            AfterMargin = 0.09,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Easy_Perfect { get; } = new()
        {
            Type = JudgeType.Perfect,
            BeforeMargin = 0.021,
            AfterMargin = 0.021,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Easy_Great { get; } = new()
        {
            Type = JudgeType.Great,
            BeforeMargin = 0.06,
            AfterMargin = 0.06,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_Easy_Good { get; } = new()
        {
            Type = JudgeType.Good,
            BeforeMargin = 0.12,
            AfterMargin = 0.12,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_VeryEasy_Perfect { get; } = new()
        {
            Type = JudgeType.Perfect,
            BeforeMargin = 0.026,
            AfterMargin = 0.026,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_VeryEasy_Great { get; } = new()
        {
            Type = JudgeType.Great,
            BeforeMargin = 0.075,
            AfterMargin = 0.075,
            ComboChange = ComboChange.Increase,
        };

        public static JudgeDefinition Beat_VeryEasy_Good { get; } = new()
        {
            Type = JudgeType.Good,
            BeforeMargin = 0.15,
            AfterMargin = 0.15,
            ComboChange = ComboChange.Increase,
        };
    }
}
