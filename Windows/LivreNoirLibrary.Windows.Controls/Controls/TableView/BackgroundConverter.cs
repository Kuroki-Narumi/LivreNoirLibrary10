using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    partial class TableView
    {
        private static readonly BackgroundConverter _bgConv = new();

        protected MultiBinding CreateCellBackgroundBinding(object obj, bool mouseOver)
        {
            MultiBinding binding = new() { Converter = _bgConv, Mode = BindingMode.OneWay };
            var bindings = binding.Bindings;
            bindings.Add(new Binding("(Grid.Row)") { Source = obj });
            bindings.Add(new Binding("(Grid.Column)") { Source = obj });
            bindings.Add(new Binding(nameof(VerticalBackground)) { Source = this });
            bindings.Add(new Binding(nameof(HorizontalBackground)) { Source = this });
            bindings.Add(new Binding(nameof(CrossedBackground)) { Source = this });
            if (mouseOver)
            {
                bindings.Add(new Binding(nameof(IsMouseOver)) { Source = obj });
                bindings.Add(new Binding(nameof(SelectedBackground)) { Source = this });
            }
            return binding;
        }

        internal class BackgroundConverter : IMultiValueConverter
        {
            public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                // values = [0: Grid.Row, 1: Grid.Column, 2: VerticalBackground, 3: HorizontalBackground, 4: CrossedBackground, 5: IsMouseOver, 6: SelectedBackground]
                if (values.Length >= 7 && values[5] is true)
                {
                    return values[6] as Brush;
                }
                if (values.Length < 5 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                {
                    return null;
                }
                if (values[0] is int row && values[1] is int col)
                {
                    if (row % 2 is 0)
                    {
                        if (col % 2 is 0)
                        {
                            return Brushes.Transparent;
                        }
                        else
                        {
                            return values[2] as Brush;
                        }
                    }
                    else
                    {
                        if (col % 2 is 0)
                        {
                            return values[3] as Brush;
                        }
                        else
                        {
                            return values[4] as Brush;
                        }
                    }
                }
                Console.WriteLine(string.Join(", ", values));
                throw new NotImplementedException();
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}