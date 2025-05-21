using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IOptionMark
    {
        public const bool DefaultIsOptionMarkVisible = true;

        public static readonly DependencyProperty IsOptionMarkVisibleProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIsOptionMarkVisible);

        public bool IsOptionMarkVisible { get; set; }
    }
}
