using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class FontSelector : ComboBox
    {
        private static readonly FontInfo[] _items = [.. Fonts.SystemFontFamilies.Select(font => new FontInfo(font))];

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
            if (!_fontFamilyUpdating && e.AddedItems[0] is FontInfo item)
            {
                _fontFamilyUpdating = true;
                SelectedFontFamily = item.FontFamily;
                _fontFamilyUpdating = false;
            }
        }
    }
}
