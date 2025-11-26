using System;

namespace LivreNoirLibrary.Media.Bms
{
    public enum TimerId
    {
        None,
        Scene_Start,
        Scene_Fadeout,

        Play_LoadingStart = 1001,
        Play_LoadingFinished,
        Play_MusicStart,
        Play_FullCombo,
        Play_Miss,
        Play_Beat,

        Play_Button1_Press = 1011,
        Play_Button1_Release,
        Play_Button1_Bomb,
        Play_Button1_LongBomb,

        Play_Button2_Press = 1021,
        Play_Button2_Release,
        Play_Button2_Bomb,
        Play_Button2_LongBomb,

        Play_Button3_Press = 1031,
        Play_Button3_Release,
        Play_Button3_Bomb,
        Play_Button3_LongBomb,

        Play_Button4_Press = 1041,
        Play_Button4_Release,
        Play_Button4_Bomb,
        Play_Button4_LongBomb,

        Play_Button5_Press = 1051,
        Play_Button5_Release,
        Play_Button5_Bomb,
        Play_Button5_LongBomb,

        Play_Button6_Press = 1061,
        Play_Button6_Release,
        Play_Button6_Bomb,
        Play_Button6_LongBomb,

        Play_Button7_Press = 1071,
        Play_Button7_Release,
        Play_Button7_Bomb,
        Play_Button7_LongBomb,

        Play_Button8_Press = 1081,
        Play_Button8_Release,
        Play_Button8_Bomb,
        Play_Button8_LongBomb,

        Play_Button9_Press = 1091,
        Play_Button9_Release,
        Play_Button9_Bomb,
        Play_Button9_LongBomb,

        Play_Button10_Press = 1101,
        Play_Button10_Release,
        Play_Button10_Bomb,
        Play_Button10_LongBomb,

        Play_Button11_Press = 1111,
        Play_Button11_Release,
        Play_Button11_Bomb,
        Play_Button11_LongBomb,

        Play_Button12_Press = 1121,
        Play_Button12_Release,
        Play_Button12_Bomb,
        Play_Button12_LongBomb,

        Play_Button13_Press = 1131,
        Play_Button13_Release,
        Play_Button13_Bomb,
        Play_Button13_LongBomb,

        Play_Button14_Press = 1141,
        Play_Button14_Release,
        Play_Button14_Bomb,
        Play_Button14_LongBomb,

        Play_Button15_Press = 1151,
        Play_Button15_Release,
        Play_Button15_Bomb,
        Play_Button15_LongBomb,

        Play_Button16_Press = 1161,
        Play_Button16_Release,
        Play_Button16_Bomb,
        Play_Button16_LongBomb,

        Play_Button17_Press = 1171,
        Play_Button17_Release,
        Play_Button17_Bomb,
        Play_Button17_LongBomb,

        Play_Button18_Press = 1181,
        Play_Button18_Release,
        Play_Button18_Bomb,
        Play_Button18_LongBomb,

        Play_Button19_Press = 1191,
        Play_Button19_Release,
        Play_Button19_Bomb,
        Play_Button19_LongBomb,

        Play_Button20_Press = 1201,
        Play_Button20_Release,
        Play_Button20_Bomb,
        Play_Button20_LongBomb,

        Play_Button21_Press = 1211,
        Play_Button21_Release,
        Play_Button21_Bomb,
        Play_Button21_LongBomb,

        Play_Button22_Press = 1221,
        Play_Button22_Release,
        Play_Button22_Bomb,
        Play_Button22_LongBomb,

        Play_Button23_Press = 1231,
        Play_Button23_Release,
        Play_Button23_Bomb,
        Play_Button23_LongBomb,

        Play_Button24_Press = 1241,
        Play_Button24_Release,
        Play_Button24_Bomb,
        Play_Button24_LongBomb,

        Play_Button25_Press = 1251,
        Play_Button25_Release,
        Play_Button25_Bomb,
        Play_Button25_LongBomb,

        Play_Button26_Press = 1261,
        Play_Button26_Release,
        Play_Button26_Bomb,
        Play_Button26_LongBomb,

        Play_Button27_Press = 1271,
        Play_Button27_Release,
        Play_Button27_Bomb,
        Play_Button27_LongBomb,

        Play_Button28_Press = 1281,
        Play_Button28_Release,
        Play_Button28_Bomb,
        Play_Button28_LongBomb,

        Play_Button29_Press = 1291,
        Play_Button29_Release,
        Play_Button29_Bomb,
        Play_Button29_LongBomb,

        Play_Button30_Press = 1301,
        Play_Button30_Release,
        Play_Button30_Bomb,
        Play_Button30_LongBomb,

        Play_Button31_Press = 1311,
        Play_Button31_Release,
        Play_Button31_Bomb,
        Play_Button31_LongBomb,

        Play_Button32_Press = 1321,
        Play_Button32_Release,
        Play_Button32_Bomb,
        Play_Button32_LongBomb,

        Play_Button33_Press = 1331,
        Play_Button33_Release,
        Play_Button33_Bomb,
        Play_Button33_LongBomb,

        Play_Button34_Press = 1341,
        Play_Button34_Release,
        Play_Button34_Bomb,
        Play_Button34_LongBomb,

        Play_Button35_Press = 1351,
        Play_Button35_Release,
        Play_Button35_Bomb,
        Play_Button35_LongBomb,

        Play_Button37_Press = 1371,
        Play_Button37_Release,
        Play_Button37_Bomb,
        Play_Button37_LongBomb,

        Play_Button38_Press = 1381,
        Play_Button38_Release,
        Play_Button38_Bomb,
        Play_Button38_LongBomb,

        Play_Button39_Press = 1391,
        Play_Button39_Release,
        Play_Button39_Bomb,
        Play_Button39_LongBomb,

        Play_Button40_Press = 1401,
        Play_Button40_Release,
        Play_Button40_Bomb,
        Play_Button40_LongBomb,

        Play_Button41_Press = 1411,
        Play_Button41_Release,
        Play_Button41_Bomb,
        Play_Button41_LongBomb,

        Play_Button42_Press = 1421,
        Play_Button42_Release,
        Play_Button42_Bomb,
        Play_Button42_LongBomb,

        Play_Button43_Press = 1431,
        Play_Button43_Release,
        Play_Button43_Bomb,
        Play_Button43_LongBomb,

        Play_Button44_Press = 1441,
        Play_Button44_Release,
        Play_Button44_Bomb,
        Play_Button44_LongBomb,

        Play_Button45_Press = 1451,
        Play_Button45_Release,
        Play_Button45_Bomb,
        Play_Button45_LongBomb,

        Play_Button46_Press = 1461,
        Play_Button46_Release,
        Play_Button46_Bomb,
        Play_Button46_LongBomb,

        Play_Button47_Press = 1471,
        Play_Button47_Release,
        Play_Button47_Bomb,
        Play_Button47_LongBomb,

        Play_Button48_Press = 1481,
        Play_Button48_Release,
        Play_Button48_Bomb,
        Play_Button48_LongBomb,

        Play_Button49_Press = 1491,
        Play_Button49_Release,
        Play_Button49_Bomb,
        Play_Button49_LongBomb,

        Play_Button50_Press = 1501,
        Play_Button50_Release,
        Play_Button50_Bomb,
        Play_Button50_LongBomb,

        Play_Button51_Press = 1511,
        Play_Button51_Release,
        Play_Button51_Bomb,
        Play_Button51_LongBomb,

        Play_Button52_Press = 1521,
        Play_Button52_Release,
        Play_Button52_Bomb,
        Play_Button52_LongBomb,

        Play_Button53_Press = 1531,
        Play_Button53_Release,
        Play_Button53_Bomb,
        Play_Button53_LongBomb,

        Play_Button54_Press = 1541,
        Play_Button54_Release,
        Play_Button54_Bomb,
        Play_Button54_LongBomb,

        Play_Button55_Press = 1551,
        Play_Button55_Release,
        Play_Button55_Bomb,
        Play_Button55_LongBomb,

        Play_Button56_Press = 1561,
        Play_Button56_Release,
        Play_Button56_Bomb,
        Play_Button56_LongBomb,

        Play_Button57_Press = 1571,
        Play_Button57_Release,
        Play_Button57_Bomb,
        Play_Button57_LongBomb,

        Play_Button58_Press = 1581,
        Play_Button58_Release,
        Play_Button58_Bomb,
        Play_Button58_LongBomb,

        Play_Button59_Press = 1591,
        Play_Button59_Release,
        Play_Button59_Bomb,
        Play_Button59_LongBomb,

        Play_Button60_Press = 1601,
        Play_Button60_Release,
        Play_Button60_Bomb,
        Play_Button60_LongBomb,

        Play_Button61_Press = 1611,
        Play_Button61_Release,
        Play_Button61_Bomb,
        Play_Button61_LongBomb,

        Play_Button62_Press = 1621,
        Play_Button62_Release,
        Play_Button62_Bomb,
        Play_Button62_LongBomb,

        Play_Button63_Press = 1631,
        Play_Button63_Release,
        Play_Button63_Bomb,
        Play_Button63_LongBomb,

        Play_Button64_Press = 1641,
        Play_Button64_Release,
        Play_Button64_Bomb,
        Play_Button64_LongBomb,

        Play_Button65_Press = 1651,
        Play_Button65_Release,
        Play_Button65_Bomb,
        Play_Button65_LongBomb,

        Play_Button66_Press = 1661,
        Play_Button66_Release,
        Play_Button66_Bomb,
        Play_Button66_LongBomb,

        Play_Button67_Press = 1671,
        Play_Button67_Release,
        Play_Button67_Bomb,
        Play_Button67_LongBomb,

        Play_Button68_Press = 1681,
        Play_Button68_Release,
        Play_Button68_Bomb,
        Play_Button68_LongBomb,

        Play_Button69_Press = 1691,
        Play_Button69_Release,
        Play_Button69_Bomb,
        Play_Button69_LongBomb,

        Play_Button70_Press = 1701,
        Play_Button70_Release,
        Play_Button70_Bomb,
        Play_Button70_LongBomb,

        Play_Button71_Press = 1711,
        Play_Button71_Release,
        Play_Button71_Bomb,
        Play_Button71_LongBomb,
    }
}
