using System;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public class JsonViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? CollectionTemplate { get; set; }
        public DataTemplate? ValueTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            return item is JsonCollectionNode ? CollectionTemplate : ValueTemplate;
        }
    }
}
