using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public interface IJudgeProvider
    {
        JudgeDefinitionCollection Judges { get; }
        ScoreDefinition ScoreDefinition { get; }
        GaugeDefinition GaugeDefinition { get; }
        double GaugeGainBase { get; set; }
    }
}
