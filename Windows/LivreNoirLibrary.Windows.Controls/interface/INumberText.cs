using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface INumberText<T>
    {
        static void OnDefaultValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnDefaultValueChanged((T)e.NewValue);
        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnValueChanged((T)e.NewValue);
        static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnMinimumChanged((T)e.NewValue);
        static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnMaximumChanged((T)e.NewValue);
        static void OnWheelStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnWheelStepChanged((T)e.NewValue);

        T DefaultValue { get; set; }
        T Value { get; set; }
        T Minimum { get; set; }
        T Maximum { get; set; }
        T WheelStep { get; set; }

        void OnDefaultValueChanged(T value) { }
        void OnValueChanged(T value) { }
        void OnMinimumChanged(T value) { }
        void OnMaximumChanged(T value) { }
        void OnWheelStepChanged(T value) { }
        void OnStringFormatChanged(string? value) { }
    }
}
