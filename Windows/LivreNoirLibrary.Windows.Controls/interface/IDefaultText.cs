using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IDefaultText
    {
        static readonly DependencyProperty DefaultTextProperty = PropertyUtils.RegisterAttached<string>(typeof(PropertyHolder));

        string? DefaultText { get; set; }
    }
}
