
namespace LivreNoirLibrary.Media.Bms.RawData
{
    internal partial class BmsTextReader
    {
        private enum Command
        {
            Random, SetRandom, EndRandom, If, ElseIf, Else, EndIf,
            Switch, SetSwitch, EndSwitch, Case, Skip, Default,
            WavDef, BmpDef, BgaDef, BpmDef, StopDef,
            ExWavDef, ExBmpDef, AtBgaDef, ExRankDef, TextDef, ArgbDef, SwBgaDef, OptionDef,
            ScrollDef, SpeedDef,
            Bgm, Bar, Channel,
            Base, Header
        }
    }
}
