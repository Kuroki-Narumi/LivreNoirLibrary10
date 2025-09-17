using System;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// ConsoleViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class ConsoleViewer : Control
    {
        public const string PART_ScrollViewer = nameof(PART_ScrollViewer);
        public const string PART_Grid = nameof(PART_Grid);

        public const double DefaultBackgroundOpacity = 0.5;
        public const string DefaultCopyText = "Copy Log";
        public const string DefaultFlushText = "Flush";

        [DependencyProperty]
        private double _backgroundOpacity = DefaultBackgroundOpacity;
        [DependencyProperty]
        private Brush? _timeForeground;
        [DependencyProperty]
        private string? _copyText = DefaultCopyText;
        [DependencyProperty]
        private string? _flushText = DefaultFlushText;

        public double ViewportWidth => _viewer is not null ? _viewer.ViewportWidth : ActualWidth;

        private readonly StringBuilder _builder = new();
        private ScrollViewer? _viewer;
        private Panel? _panel;

        public ConsoleViewer()
        {
            this.RegisterCommand(ConsoleCommands.Copy, OnExecuted_Copy);
            this.RegisterCommand(ConsoleCommands.Flush, OnExecuted_Flush);
            ExConsole.Log.CollectionChanged += OnLogChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _viewer = GetTemplateChild(PART_ScrollViewer) as ScrollViewer;
            _panel = GetTemplateChild(PART_Grid) as Panel;
            InitializeList();
        }

        private void InitializeList()
        {
            _builder.Clear();
            _panel?.Children.Clear();
            foreach (var item in ExConsole.Log)
            {
                AddChild(item);
            }
        }

        private void OnExecuted_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(_builder.ToString());
            e.Handled = true;
        }

        private void OnExecuted_Flush(object sender, ExecutedRoutedEventArgs e)
        {
            _builder.Clear();
            _panel?.Children.Clear();
            e.Handled = true;
        }

        private void OnLogChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (_panel is not null && e.NewItems is not null)
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var o in e.NewItems)
                    {
                        if (o is LogItem l)
                        {
                            AddChild(l);
                        }
                    }
                    if (_viewer is { } viewer)
                    {
                        var pos = viewer.VerticalOffset;
                        var max = viewer.ScrollableHeight;
                        if (pos == max)
                        {
                            viewer.ScrollToEnd();
                        }
                    }
                });
            }
        }

        private void AddChild(LogItem item)
        {
            var time = item.Time;
            var content = item.Content;
            if (_panel is { } panel)
            {
                Grid grid = new();
                grid.ColumnDefinitions.Add(new() { Width = GridLength.Auto, SharedSizeGroup = "DateTime" });
                grid.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
                TextBlock tt = new()
                {
                    Text = $"{time:HH:mm:ss} ",
                    Margin = new Thickness(0, 2, 0, 2),
                };
                tt.SetBinding(ForegroundProperty, new Binding(nameof(TimeForeground)) { Source = this });
                grid.Children.Add(tt);
                TextBlock tc = new()
                {
                    Text = content,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 2, 0, 2),
                };
                Grid.SetColumn(tc, 1);
                grid.Children.Add(tc);
                panel.Children.Add(grid);
            }
            _builder.Append($"{time:yyyy-MM-dd HH:mm:ss.fff}\t{content}\n");
        }
    }

    public static class ConsoleCommands
    {
        public static RoutedCommand Copy => Commands.Copy;
        public static RoutedCommand Flush { get; } = Commands.Create(Key.F, ModifierKeys.Control);
    }
}
