using System;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ScrollViewerBase
    {
        protected Binding GetBinding_ViewportWidth() => new(nameof(ViewportWidth)) { Source = this, Mode = BindingMode.OneWay };
        protected Binding GetBinding_ViewportHeight() => new(nameof(ViewportHeight)) { Source = this, Mode = BindingMode.OneWay };

        protected Binding GetBinding_ContentWidth() => new(nameof(ContentWidth)) { Source = this, Mode = BindingMode.OneWay };
        protected Binding GetBinding_ContentHeight() => new(nameof(ContentHeight)) { Source = this, Mode = BindingMode.OneWay };

        protected Binding GetBinding_ScaleX() => new(nameof(ScaleX)) { Source = this, Mode = BindingMode.TwoWay };
        protected Binding GetBinding_ScaleY() => new(nameof(ScaleY)) { Source = this, Mode = BindingMode.TwoWay };

        protected Binding GetBinding_VerticalOffset(bool round = true) => new(nameof(VerticalOffset))
        {
            Source = this,
            Mode = BindingMode.OneWay,
            Converter = new DoubleRoundConverter() { Rounding = round ? DoubleRounding.Round : 0 }
        };

        protected Binding GetBinding_HorizontalOffset(bool round = true) => new(nameof(HorizontalOffset))
        {
            Source = this,
            Mode = BindingMode.OneWay,
            Converter = new DoubleRoundConverter() { Rounding = round ? DoubleRounding.Round : 0 }
        };
    }
}
