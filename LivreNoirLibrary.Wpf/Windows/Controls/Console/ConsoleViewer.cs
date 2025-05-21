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
        private bool _initialized;
        private ScrollViewer? _viewer;
        private Grid? _grid;

        public ConsoleViewer()
        {
            this.RegisterCommand(ConsoleCommands.Copy, OnExecuted_Copy);
            this.RegisterCommand(ConsoleCommands.Flush, OnExecuted_Flush);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (!_initialized)
            {
                ExConsole.Log.CollectionChanged += OnLogChanged;
                _initialized = true;
            }
            _viewer = GetTemplateChild(PART_ScrollViewer) as ScrollViewer;
            _grid = GetTemplateChild(PART_Grid) as Grid;
            InitializeList();
        }

        private void InitializeList()
        {
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
            if (_grid is Grid grid)
            {
                grid.Children.Clear();
                grid.RowDefinitions.Clear();
            }
            _builder.Clear();
            e.Handled = true;
        }

        private void OnLogChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
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
                    if (_viewer is ScrollViewer viewer)
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
            if (_grid is Grid grid)
            {
                var row = grid.RowDefinitions.Count;
                TextBlock tt = new()
                {
                    Text = $"{time:HH:mm:ss} ",
                    Margin = new Thickness(0, 2, 0, 2),

                };
                tt.SetBinding(ForegroundProperty, new Binding(nameof(TimeForeground)) { Source = this });
                Grid.SetColumn(tt, 0);
                Grid.SetRow(tt, row);
                TextBlock tc = new()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 2, 0, 2),
                };
                tc.Inlines.Add(content);
                Grid.SetColumn(tc, 1);
                Grid.SetRow(tc, row);
                grid.RowDefinitions.Add(new() { Height = GridLength.Auto });
                grid.Children.Add(tt);
                grid.Children.Add(tc);
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
