using System;

namespace LivreNoirLibrary.Windows.Input
{
    public enum KeyType : int
    {
        None = 0,

        Letter = 1,
        Number = 2,
        NumPad = 3,
        Move = 4,
        Function = 5,

        Shift = 11,
        Ctrl = 12,
        Alt = 13,

        System = 21,
        Windows = System,
        Application = System,

        Browser = System,
        Volume = System,
        Media = System,

        Other = -1,
    }
}
