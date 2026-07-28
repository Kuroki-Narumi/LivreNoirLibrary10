using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract class WithVocabComboItem<T>(T value, IVocabData? name) : Windows.ComboItemBase<T>(value)
        where T : struct, Enum
    {
        public IVocabData? Name { get; } = name;

        public override string? ToString() => Name?.Value;
    }
}
