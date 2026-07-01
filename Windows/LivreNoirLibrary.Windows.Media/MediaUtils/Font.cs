using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class MediaUtils
    {
        public static string GetFriendlyName(this FontFamily fontFamily, CultureInfo? culture = null)
        {
            var names = fontFamily.FamilyNames;
            culture ??= CultureInfo.CurrentCulture;
            if (names.TryGetValue(XmlLanguage.GetLanguage(culture.IetfLanguageTag), out var name))
            {
                return name;
            }
            if (names.TryGetValue(XmlLanguage.GetLanguage("en-us"), out name))
            {
                return name;
            }
            return "???";
        }
    }

    public class FontInfo(FontFamily fontFamily)
    {
        public FontFamily FontFamily { get; } = fontFamily;
        public string Source => FontFamily.Source;
        public string FriendlyName => FontFamily.GetFriendlyName();
    }
}
