using LivreNoirLibrary.ObjectModel;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class TextSearchConditionsViewModel : ObservableObjectBase
    {
        public string? SearchText { get; set => SetValue(ref field, value, CheckRegex); }

        public bool IsTextValid { get; private set => SetValue(ref field, value); } = true;

        public bool CheckName { get; set => SetValue(ref field, value); }
        public bool CheckRuby { get; set => SetValue(ref field, value); }
        public bool CheckEnName { get; set => SetValue(ref field, value); }
        public bool CheckText { get; set => SetValue(ref field, value); }
        public bool CheckPText { get; set => SetValue(ref field, value); }
        public bool IgnoreCase { get; set => SetValue(ref field, value); }
        public bool TextIgnoreCase { get; set => SetValue(ref field, value); }
        public bool UseRegex { get; set => SetValue(ref field, value, CheckRegex); }
        public bool IgnoreSymbols { get; set => SetValue(ref field, value); }
        public bool TextIgnoreSymbols { get; set => SetValue(ref field, value); }

        private void CheckRegex()
        {
            var text = SearchText;
            if (!string.IsNullOrEmpty(text) && UseRegex)
            {
                try
                {
                    _ = new Regex(text);
                }
                catch
                {
                    IsTextValid = false;
                    return;
                }
            }
            IsTextValid = true;
        }

        public void SetTextFlags(TextSearchFlags flags)
        {
            CheckName = (flags & TextSearchFlags.Name) is not 0;
            CheckRuby = (flags & TextSearchFlags.Ruby) is not 0;
            CheckEnName = (flags & TextSearchFlags.EnName) is not 0;
            CheckText = (flags & TextSearchFlags.Text) is not 0;
            CheckPText = (flags & TextSearchFlags.PText) is not 0;
            IgnoreCase = (flags & TextSearchFlags.IgnoreCase) is not 0;
            TextIgnoreCase = (flags & TextSearchFlags.TextIgnoreCase) is not 0;
            UseRegex = (flags & TextSearchFlags.UseRegex) is not 0;
            IgnoreSymbols = (flags & TextSearchFlags.IgnoreSymbols) is not 0;
            TextIgnoreSymbols = (flags & TextSearchFlags.TextIgnoreSymbols) is not 0;
        }

        public TextSearchFlags GetTextFlags()
        {
            var flags = TextSearchFlags.None;
            if (CheckName) flags |= TextSearchFlags.Name;
            if (CheckRuby) flags |= TextSearchFlags.Ruby;
            if (CheckEnName) flags |= TextSearchFlags.EnName;
            if (CheckText) flags |= TextSearchFlags.Text;
            if (CheckPText) flags |= TextSearchFlags.PText;
            if (IgnoreCase) flags |= TextSearchFlags.IgnoreCase;
            if (TextIgnoreCase) flags |= TextSearchFlags.TextIgnoreCase;
            if (UseRegex) flags |= TextSearchFlags.UseRegex;
            if (IgnoreSymbols) flags |= TextSearchFlags.IgnoreSymbols;
            if (TextIgnoreSymbols) flags |= TextSearchFlags.TextIgnoreCase;
            return flags;
        }
    }
}
