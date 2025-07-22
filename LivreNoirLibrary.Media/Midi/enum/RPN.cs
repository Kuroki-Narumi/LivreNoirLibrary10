
namespace LivreNoirLibrary.Media.Midi
{
    public enum RPN : short
    {
        PitchBendSensitivity = 0x0000,
        ChannelFineTune = 0x0001,
        ChannelCoarseTune = 0x0002,
        TuningProgramChange = 0x0003,
        TuningBankSelect = 0x0004,
        ModulationDepthRange = 0x0005,
        MPEConfigurationMessage = 0x0006,

        AzimuthAngle = 0x3D00,
        ElevationAngle = 0x3D01,
        Gain = 0x3D02,
        DistanceRatio = 0x3D03,
        MaxDistance = 0x3D04,
        GainAtMaxDistance = 0x3D05,
        ReferenceDistanceRatio = 0x3D06,
        PanSpreadAngle = 0x3D07,
        RollAngle = 0x3D08,

        NullFunction = 0x7F7F,
    }
}
