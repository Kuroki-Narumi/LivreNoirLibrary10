using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Markup;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class FontSelector : ComboBox
    {
        private static readonly FontSelectorItem[] _items = [.. Fonts.SystemFontFamilies.Select(font => new FontSelectorItem(font))];

        static FontSelector()
        {
            PropertyUtils.OverrideDefaultStyleKey<FontSelector>();
        }

        [DependencyProperty]
        private FontFamily? _selectedFontFamily;
        [DependencyProperty]
        private bool _displayFontSource;

        private bool _fontFamilyUpdating;

        public FontSelector()
        {
            ItemsSource = _items;
        }

        private void OnSelectedFontFamilyChanged(FontFamily? value)
        {
            if (!_fontFamilyUpdating)
            {
                var selectedItem = value is not null ? _items.FirstOrDefault(item => item.Source == value.Source) : null;
                SelectedItem = selectedItem;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (!_fontFamilyUpdating && e.AddedItems[0] is FontSelectorItem item)
            {
                _fontFamilyUpdating = true;
                SelectedFontFamily = item.FontFamily;
                _fontFamilyUpdating = false;
            }
        }
    }

    public class FontSelectorItem(FontFamily fontFamily)
    {
        public FontFamily FontFamily { get; } = fontFamily;
        public string Source => FontFamily.Source;
        public string FriendlyName => GetName(FontFamily);

        private static string GetName(FontFamily fontFamily)
        {
            var names = fontFamily.FamilyNames;
            if (names.TryGetValue(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag), out var name))
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
}
