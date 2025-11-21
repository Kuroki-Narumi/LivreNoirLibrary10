using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Data;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    internal static class ColorSelectorPool
    {
        private static readonly Lock _lock = new();
        private static readonly List<ColorSelector> _list = [];
        private static readonly HashSet<ColorSelector> _used = [];

        public static ColorSelector Rent(ColorPicker owner)
        {
            lock (_lock)
            {
                foreach (var selector in _list.AsSpan())
                {
                    if (_used.Add(selector))
                    {
                        SetBinding(owner, selector);
                        return selector;
                    }
                }
                ColorSelector sel = new();
                _list.Add(sel);
                _used.Add(sel);
                SetBinding(owner, sel);
                return sel;
            }
        }

        private static void SetBinding(ColorPicker source, ColorSelector target)
        {
            target.SetBinding(ColorSelector.SelectedColorProperty, new Binding(nameof(ColorPicker.SelectedColor)) { Source = source, Mode = BindingMode.TwoWay });
            target.SetColor(source.SelectedColor, source.IsAlphaEnabled);
        }

        public static void Return(ColorSelector selector)
        {
            lock (_lock)
            {
                BindingOperations.ClearAllBindings(selector);
                _used.Remove(selector);
            }
        }
    }
}
