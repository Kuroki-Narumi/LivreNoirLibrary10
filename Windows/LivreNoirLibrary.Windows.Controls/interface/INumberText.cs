using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface INumberText<T>
    {
        public static void OnDefaultValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnDefaultValueChanged((T)e.NewValue);
        public static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnValueChanged((T)e.NewValue);
        public static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnMinimumChanged((T)e.NewValue);
        public static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnMaximumChanged((T)e.NewValue);
        public static void OnWheelStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<T>)?.OnWheelStepChanged((T)e.NewValue);

        public T DefaultValue { get; set; }
        public T Value { get; set; }
        public T Minimum { get; set; }
        public T Maximum { get; set; }
        public T WheelStep { get; set; }

        void OnDefaultValueChanged(T value) { }
        void OnValueChanged(T value) { }
        void OnMinimumChanged(T value) { }
        void OnMaximumChanged(T value) { }
        void OnWheelStepChanged(T value) { }
        void OnStringFormatChanged(string? value) { }
    }
}
