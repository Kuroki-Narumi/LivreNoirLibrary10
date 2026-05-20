using System;
using System.Collections.Generic;
using System.Windows.Data;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    internal static class ColorSelectorPool
    {
        private static readonly List<ColorSelector> _list = [];
        private static readonly HashSet<ColorSelector> _using = [];

        public static ColorSelector Rent(ColorPicker owner)
        {
            foreach (var selector in _list.AsSpan())
            {
                if (_using.Add(selector))
                {
                    SetBinding(owner, selector);
                    return selector;
                }
            }
            ColorSelector sel = new();
            _list.Add(sel);
            _using.Add(sel);
            SetBinding(owner, sel);
            return sel;
        }

        private static void SetBinding(ColorPicker source, ColorSelector target)
        {
            target.SetBinding(ColorSelector.SelectedColorProperty, new Binding(nameof(ColorPicker.SelectedColor)) { Source = source, Mode = BindingMode.TwoWay });
            target.Setup(source.SelectedColor, source.IsAlphaEnabled);
        }

        public static void Return(ColorSelector selector)
        {
            BindingOperations.ClearAllBindings(selector);
            _using.Remove(selector);
        }
    }
}
