using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class ComboItemBase<T>(T value, IVocabData? name) : Windows.ComboItemBase<T>(value)
        where T : struct, Enum
    {
        public IVocabData? Name { get; } = name;
        protected override Brush? GetBackground(int row, int column) => AttributeItem.GetBackgroundStatic(row, column);

        public override string? ToString() => Name?.Value;
    }
}
