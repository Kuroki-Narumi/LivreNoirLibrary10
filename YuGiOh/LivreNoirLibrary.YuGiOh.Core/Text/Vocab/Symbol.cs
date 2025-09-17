using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        [GeneratedRegex(@"[\s""-'*-/:->[-`{-~・“”‘’＝－―★☆×『』【】《》]")]
        static partial Regex Regex_Symbol { get; }

        public static string RemoveSymbol(string text)
        {
            return Regex_Symbol.Replace(text, "");
        }
    }
}
