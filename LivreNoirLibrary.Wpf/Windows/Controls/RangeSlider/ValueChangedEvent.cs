using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void RangeSliderValueChangedEventHandler(object sender, RangeSliderValueChangedEventArgs e);

    public class RangeSliderValueChangedEventArgs : RoutedEventArgs
    {
        public double Value1 { get; init; }
        public double Value2 { get; init; }

        public RangeSliderValueChangedEventArgs() : base() { }
        public RangeSliderValueChangedEventArgs(RoutedEvent routedEvent, RangeSlider source) : base(routedEvent, source)
        {
            Value1 = source.Value1;
            Value2 = source.Value2;
        }
    }

    public partial class RangeSlider
    {
        public static readonly RoutedEvent ValueChangedEvent = EventRegister.Register<RangeSlider, RangeSliderValueChangedEventHandler>(RoutingStrategy.Direct);

        public event RangeSliderValueChangedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        protected void RaiseValueChanged()
        {
            RangeSliderValueChangedEventArgs args = new(ValueChangedEvent, this);
            RaiseEvent(args);
        }
    }
}
