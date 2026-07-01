using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [Flags]
    public enum TextSearchFlags
    {
        Name = 1,
        Ruby = 2,
        EnName = 4,
        Text = 8,
        PText = 16,
        IgnoreCase = 32,
        IgnoreTextCase = 64,
        Regex = 128,
        IgnoreSymbols = 256,
        IgnoreTextSymbols = 512,

        Default = Name | Ruby | Text | PText | IgnoreCase | IgnoreSymbols,
        CheckText = Name | Ruby | EnName | Text | PText,
    }
}
