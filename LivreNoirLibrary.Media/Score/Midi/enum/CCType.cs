
namespace LivreNoirLibrary.Media.Midi
{
    public enum CCType : short
    {
        BankSelect = 0,
        Modulation = 1,
        Breath = 2,

        FootPedal = 4,
        Portamento = 5,
        DataEntry = 6,
        Volume = 7,
        Balance = 8,

        Pan = 10,
        Expression = 11,
        Effect_1 = 12,
        Effect_2 = 13,

        GeneralPurpose_1 = 16,
        GeneralPurpose_2 = 17,
        GeneralPurpose_3 = 18,
        GeneralPurpose_4 = 19,

        BankSelect_LSB = 32,
        Modulation_LSB = 33,
        Breath_LSB = 34,

        FootPedal_LSB = 36,
        Portamento_LSB = 37,
        DataEntry_LSB = 38,
        Volume_LSB = 39,
        Balance_LSB = 40,

        Pan_LSB = 42,
        Expression_LSB = 43,
        Effect_1_LSB = 44,
        Effect_2_LSB = 45,

        GeneralPurpose_1_LSB = 48,
        GeneralPurpose_2_LSB = 49,
        GeneralPurpose_3_LSB = 50,
        GeneralPurpose_4_LSB = 51,

        DamperPedal_Flag = 64,
        Portamento_Flag = 65,
        Sostenuto_Flag = 66,
        SoftPedal_Flag = 67,
        Legato_Flag = 68,
        Hold_Flag = 69,
        Variation = 70,
        Timbre = 71,
        Release = 72,
        Attack = 73,
        Brightness = 74,
        Decay = 75,
        Vibrato_Rate = 76,
        Vibrato_Depth = 77,
        Vibrato_Delay = 78,

        GeneralPurpose_5 = 80,
        GeneralPurpose_6 = 81,
        GeneralPurpose_7 = 82,
        GeneralPurpose_8 = 83,

        Portamento_Control = 84,

        VelocityPrefix = 88,

        Reverb = 91,
        Tremolo = 92,
        Chorus = 93,
        Detune = 94,
        Phaser = 95,
        DataIncrement = 96,
        DataDecrement = 97,
        NRPN_LSB = 98,
        NRPN_MSB = 99,
        RPN_LSB = 100,
        RPN_MSB = 101,

        AllSoundOff = 120,
        ResetAllControllers = 121,
        LocalControl_Flag = 122,
        AllNotesOff = 123,
        OmniModeOff = 124,
        OmniModeOn = 125,
        MonoModeOn = 126,
        PolyModeOn = 127,

        PolyphonicKeyPressure = 256 + 0xA0,
        ProgramChange = 256 + 0xC0,
        ChannelPressure = 256 + 0xD0,
        PitchBend = 256 + 0xE0,

        SequenceNumber = 256 + 0x00,
        ChannelPrefix = 256 + 0x20,
        Port = 256 + 0x21,

        KeySwitch = 512,

        RPN = 0x7F00,
        NRPN = 0x7F01,
        Data = 0x7F02,
    }
}
