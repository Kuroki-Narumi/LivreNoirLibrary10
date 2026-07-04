using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [Flags]
    public enum TextSearchFlags
    {
        None = 0,

        Name = 1,
        Ruby = 2,
        EnName = 4,
        Text = 8,
        PText = 16,
        IgnoreCase = 32,
        TextIgnoreCase = 64,
        UseRegex = 128,
        IgnoreSymbols = 256,
        TextIgnoreSymbols = 512,

        Default = Name | Ruby | Text | PText | IgnoreCase | IgnoreSymbols,
        CheckText = Name | Ruby | EnName | Text | PText,
    }
}
