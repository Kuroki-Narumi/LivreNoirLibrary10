
namespace LivreNoirLibrary.Media.Bms
{
    public enum Channel
    {
        None = 0,
        Bgm = 1,
        Bar = 2,

        Bpm_Base = 3,
        Bpm = 8,
        Stop = 9,

        Ext = 5,

        Bga_Base = 4,
        Bga_Layer1 = 7,
        Bga_Layer2 = 10,
        Bga_Poor = 6,
        Opacity_Base = 11,
        Opacity_Layer1 = 12,
        Opacity_Layer2 = 13,
        Opacity_Poor = 14,

        P1_Visible = 36,
        P2_Visible = 72,
        P1_Invisible = 108,
        P2_Invisible = 144,
        P1_Long = 180,
        P2_Long = 216,
        P1_Mine = 468,
        P2_Mine = 504,

        Bgm_Volume = 331,
        Key_Volume = 332,
        Text = 333,
        ExRank = 360,
        Argb_Base = 361,
        Argb_Layer1 = 362,
        Argb_Layer2 = 363,
        Argb_Poor = 364,
        SwBga = 365,
        ChangeOption = 366,

        Scroll = 1020,
        Speed = 1033,
    }
}
