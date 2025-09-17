using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IDefaultText
    {
        public static readonly DependencyProperty DefaultTextProperty = PropertyUtils.RegisterAttached<string>(typeof(PropertyHolder));

        public string? DefaultText { get; set; }
    }
}
