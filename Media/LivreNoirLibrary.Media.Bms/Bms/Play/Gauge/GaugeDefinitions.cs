using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public static class GaugeDefinitions
    {
        public static GaugeDefinition Beat_Normal { get; } = new(
            (JudgeType.Perfect, GaugeGain.Relative(1)),
            (JudgeType.Great, GaugeGain.Relative(1)),
            (JudgeType.Good, GaugeGain.Relative(0.5)),
            (JudgeType.Bad, -4),
            (JudgeType.Through, -6),
            (JudgeType.BlankShot, -2)
            )
        {
            InitialValue = 22,
            MinimumValue = 2,
            MaximumValue = 100,
            PassingValue = 80,
        };

        public static GaugeDefinition Beat_Easy { get; } = new(
            (JudgeType.Perfect, GaugeGain.Relative(1.2)),
            (JudgeType.Great, GaugeGain.Relative(1.2)),
            (JudgeType.Good, GaugeGain.Relative(0.6)),
            (JudgeType.Bad, -3.2),
            (JudgeType.Through, -4.8),
            (JudgeType.BlankShot, -1.6)
            )
        {
            InitialValue = 22,
            MinimumValue = 2,
            MaximumValue = 100,
            PassingValue = 80,
        };

        public static GaugeDefinition Beat_Hard { get; } = new(
            (JudgeType.Perfect, 0.1),
            (JudgeType.Great, 0.1),
            (JudgeType.Good, 0.05),
            (JudgeType.Bad, -6),
            (JudgeType.Through, -10),
            (JudgeType.BlankShot, -2)
            )
        {
            InitialValue = 100,
            MinimumValue = 0,
            MaximumValue = 100,
            PassingValue = 0,
            LowValue = 30,
            Endurance = true,
        };

        public static GaugeDefinition Beat_ExHard { get; } = new(
            (JudgeType.Perfect, 0.1),
            (JudgeType.Great, 0.1),
            (JudgeType.Good, 0.05),
            (JudgeType.Bad, -12),
            (JudgeType.Through, -16),
            (JudgeType.BlankShot, -6)
            )
        {
            InitialValue = 100,
            MinimumValue = 0,
            MaximumValue = 100,
            PassingValue = 0,
            Endurance = true,
        };

        public static GaugeDefinition Beat_Hazard { get; } = new(
            (JudgeType.Perfect, 0.1),
            (JudgeType.Great, 0.1),
            (JudgeType.Good, 0.05),
            (JudgeType.Bad, -100),
            (JudgeType.Through, -100),
            (JudgeType.BlankShot, -10)
            )
        {
            InitialValue = 100,
            MinimumValue = 0,
            MaximumValue = 100,
            PassingValue = 0,
            Endurance = true,
        };

        public static GaugeDefinition Beat_Expert { get; } = new(
            (JudgeType.Perfect, 0.06),
            (JudgeType.Great, 0.06),
            (JudgeType.Good, 0.03),
            (JudgeType.Bad, -4),
            (JudgeType.Through, -6),
            (JudgeType.BlankShot, -2)
            )
        {
            InitialValue = 100,
            MinimumValue = 0,
            MaximumValue = 100,
            PassingValue = 0,
            LowValue = 30,
            Endurance = true,
        };
    }
}
