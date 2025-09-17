using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls
{
    public readonly struct TableHeaderInfo
    {
        public readonly int Key;
        public readonly string Header;
        public readonly string VerticalHeader;

        public TableHeaderInfo(int key, string name)
        {
            Key = key;
            Header = name;
            VerticalHeader = ToVertical(name);
        }

        public TableHeaderInfo(int key, string name, string vName)
        {
            Key = key;
            Header = name;
            VerticalHeader = vName;
        }

        static string ToVertical(string text) => string.Join('\n', text as IEnumerable<char>);
    }
}
