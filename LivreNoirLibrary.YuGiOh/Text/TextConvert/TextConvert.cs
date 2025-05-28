using System;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class TextConvert
    {
        public static string Wide2Half(string text) => Text.TextConvert.Wide2Half(text);
        public static string Kana2Hiragana(string text) => Text.TextConvert.Kana2Hiragana(text);

        [GeneratedRegex(@"[！-），．-［］-｝]")]
        static partial Regex Regex_Wide_CardInfo { get; }
        public static string Wide2Half_CardInfo(string text) => Regex_Wide_CardInfo.Replace(text, m => m.Value.Wide2Half_General());

        [GeneratedRegex(@"(?:([^「」]+)|^)(「[^」]+」)?")]
        static partial Regex Regex_SpecifyName { get; }
        public static string Wide2Half_CardText(string text) => Regex_SpecifyName.Replace(text, m => $"{Wide2Half_CardInfo(m.Groups[1].Value)}{m.Groups[2].Value}");

        [GeneratedRegex("[！＄（）＊＋，．＜＝＞？＼＾｛｜｝]")]
        static partial Regex Regex_Wide_Regex { get; }
        public static string Wide2Half_Regex(string text) => Regex_Wide_Regex.Replace(text, m => $"\\{m.Value.Wide2Half_General()}");

        public static (string, string) StrConv(string text)
        {
            text = Wide2Half(text);
            return (text, Kana2Hiragana(Vocab.RemoveSymbol(text)));
        }

        public static string StrConv2(string text)
        {
            return Wide2Half(Wide2Half_Regex(text));
        }
    }
}
