
namespace LivreNoirLibrary.Media.Midi
{
    public enum KeySwitchMode : byte
    {
        None,
        Toggle = 0x10,
        Once = 0x20,
        Hold = 0x40,

        HoldOn = 0x50,
        HoldOff = 0x60,
    }
}
