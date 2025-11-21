using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IOptionMark
    {
        const bool DefaultIsOptionMarkVisible = true;

        static readonly DependencyProperty IsOptionMarkVisibleProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIsOptionMarkVisible);

        bool IsOptionMarkVisible { get; set; }
    }
}
