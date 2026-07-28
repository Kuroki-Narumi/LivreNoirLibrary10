using System;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class DeckRecipeSeparatorItem(string name, string value)
    {
        public static DeckRecipeSeparatorItem[] Items { get; } = [new("_x_", " x "), new("×", "×"), new("tab", "\t")];

        public string Name { get; } = name;
        public string Value { get; } = value;
    }
}
