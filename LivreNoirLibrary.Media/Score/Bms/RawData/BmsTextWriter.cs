using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    internal class BmsTextWriter : DisposableBase
    {
        private readonly StreamWriter _writer;
        private int _indent;
        private readonly Dictionary<int, string> _indent_strs = [];

        private BmsTextWriter(Stream stream, int indent, Encoding encoding)
        {
            _writer = new(stream, encoding, -1, true);
            _indent = indent;
        }

        public static BmsTextWriter Create(Stream stream, bool indent, Encoding encoding) => new(stream, indent ? 0 : -1, encoding);

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _writer.Dispose();
        }

        public void IndentRight()
        {
            if (_indent >= 0)
            {
                _indent++;
            }
        }

        public void IndentLeft()
        {
            if (_indent > 0)
            {
                _indent--;
            }
        }

        private string GetIndentPadding(int count)
        {
            if (!_indent_strs.TryGetValue(count, out var text))
            {
                text = new string(' ', count * 2);
                _indent_strs.Add(count, text);
            }
            return text;
        }

        public void Dump(string? str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (_indent is > 0)
                {
                    _writer.Write(GetIndentPadding(_indent));
                }
                _writer.WriteLine(str);
            }
        }

        public void Dump(string? format, params ReadOnlySpan<object?> arg)
        {
            if (!string.IsNullOrEmpty(format))
            {
                if (_indent is > 0)
                {
                    _writer.Write(GetIndentPadding(_indent));
                }
                _writer.WriteLine(format, arg);
            }
        }

        public void DumpEmpty() => _writer.WriteLine();
    }
}
