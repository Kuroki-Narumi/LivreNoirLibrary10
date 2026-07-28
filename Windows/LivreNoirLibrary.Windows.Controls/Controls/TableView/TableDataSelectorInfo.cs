using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public readonly struct TableDataSelectorInfo(int key, string header, string? verticalHeader = null)
    {
        public readonly int Key = key;
        public readonly string Header = header;
        public readonly string? VerticalHeader = verticalHeader;
    }
}
