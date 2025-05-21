using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// JsonView.xaml の相互作用ロジック
    /// </summary>
    public partial class JsonView : UserControl
    {
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private object? _source;
        private byte[]? _buffer;

        public JsonView()
        {
            InitializeComponent();
        }

        private void OnSourceChanged(object? value)
        {
            if (value is not null)
            {
                _buffer = JsonNode.CreateBuffer(value);
                TreeView.ItemsSource = JsonNode.CreateNode(_buffer);
            }
            else
            {
                _buffer = null;
                TreeView.ItemsSource = null;
            }
        }

        public string GetText()
        {
            if (_buffer is not null)
            {
                return Encoding.UTF8.GetString(_buffer);
            }
            return "";
        }

        private void OnExpanded_Json(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem item && item.Header is JsonNode node)
            {
                node.IsExpanded = item.IsExpanded;
            }
        }
    }
}
