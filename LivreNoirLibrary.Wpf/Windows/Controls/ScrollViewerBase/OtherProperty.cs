using System;
using System.Runtime.CompilerServices;
using System.Windows;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ContentSizeChangedEventHandler(object sender, double width, double height);

    public partial class ScrollViewerBase
    {
        public event ContentSizeChangedEventHandler? ContentSizeChanged;

        [DependencyProperty(SetterScope = Scope.Protected)]
        protected double _contentWidth;
        [DependencyProperty(SetterScope = Scope.Protected)]
        protected double _contentHeight;

        protected virtual void OnContentWidthChanged(double value)
        {
            ContentSizeChanged?.Invoke(this, value, _contentHeight);
        }

        protected virtual void OnContentHeightChanged(double value)
        {
            ContentSizeChanged?.Invoke(this, _contentWidth, value);
        }

        protected static DependencyProperty RegisterBooleanProperty<T>(PropertyChangedCallback callback, bool defaultValue = true, [CallerMemberName] string caller = "")
            => DependencyProperty.Register(PropertyUtils.GetPropertyName(caller), typeof(bool?), typeof(T), PropertyUtils.GetMetaTwoWay(defaultValue, callback));
    }
}
