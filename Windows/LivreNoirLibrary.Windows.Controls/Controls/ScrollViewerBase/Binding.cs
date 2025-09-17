using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using LivreNoirLibrary.Windows.Converters;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ScrollViewerBase
    {
        protected static readonly DoubleRoundConverter RoundConverter = new() { Rounding = DoubleRounding.Round };
        private readonly Dictionary<string, Binding> _binding_cache = [];

        protected Binding GetBindingCore(string propertyName, BindingMode mode = BindingMode.OneWay, IValueConverter? converter = null)
        {
            if (!_binding_cache.TryGetValue(propertyName, out var binding))
            {
                binding = new(propertyName)
                {
                    Source = this,
                    Mode = mode,
                    Converter = converter,
                };
                _binding_cache.Add(propertyName, binding);
            }
            return binding;
        }

        protected Binding Binding_ViewportWidth => GetBindingCore(nameof(ViewportWidth));
        protected Binding Binding_ViewportHeight => GetBindingCore(nameof(ViewportHeight));

        protected Binding Binding_ContentWidth => GetBindingCore(nameof(ContentWidth));
        protected Binding Binding_ContentHeight => GetBindingCore(nameof(ContentHeight));

        protected Binding Binding_ScaleX => GetBindingCore(nameof(ScaleX), BindingMode.TwoWay);
        protected Binding Binding_ScaleY => GetBindingCore(nameof(ScaleY), BindingMode.TwoWay);

        protected Binding Binding_VerticalOffset => GetBindingCore(nameof(VerticalOffset), converter: RoundConverter);
        protected Binding Binding_HorizontalOffset => GetBindingCore(nameof(HorizontalOffset), converter: RoundConverter);
    }
}
