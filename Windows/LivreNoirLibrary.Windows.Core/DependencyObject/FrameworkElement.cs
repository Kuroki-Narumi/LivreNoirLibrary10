using System;
using System.Windows;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows
{
    public static partial class PropertyUtils
    {
        public static void OverrideDefaultStyleKey<T>()
            where T : FrameworkElement
        {
            DummyElement.OverrideDefaultStyleKey(typeof(T));
        }

        private class DummyElement : FrameworkElement
        {
            public static void OverrideDefaultStyleKey(Type type)
            {
                DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            }
        }

        public static void UpdateBinding(this FrameworkElement d, DependencyProperty property, object? source)
        {
            BindingOperations.ClearBinding(d, property);
            if (source is not null)
            {
                d.SetBinding(property, new Binding(property.Name) { Source = source });
            }
        }
    }
}
