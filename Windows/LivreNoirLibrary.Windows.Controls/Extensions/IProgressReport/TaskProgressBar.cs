using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class TaskProgressBar : Control
    {
        private CancellationTokenSource? _ct_source;

        static TaskProgressBar()
        {
            PropertyUtils.OverrideDefaultStyleKey<TaskProgressBar>();
        }

        public const double DefaultBarWidth = 200;
        public const double DefaultBarHeight = 24;
        public static readonly SolidColorBrush DefaultTextBackground = Media.MediaUtils.GetBrush(255, 241, 242, 247);
        public const bool DefaultIsAbortable = true;
        public const string DefaultAbortText = "Abort";
        public const double DefaultMinimum = 0;
        public const double DefaultMaximum = 1;

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _barWidth = DefaultBarWidth;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _barHeight = DefaultBarHeight;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private Brush? _textBackground = DefaultTextBackground;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string? _caption;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string? _message;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private bool _isAbortable = DefaultIsAbortable;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string? _abortText = DefaultAbortText;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _minimum = DefaultMinimum;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _maximum = DefaultMaximum;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private double _value = DefaultMinimum;

        public TaskProgressBar()
        {
            this.RegisterCommand(Commands.Cancel, OnExecuted_Cancel);
        }

        public void Prepare(in ProgressReport initialReport, bool abortable)
        {
            OnProgressChanged(initialReport);
            IsAbortable = abortable;
            Focus();
            Visibility = Visibility.Visible;
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            _ct_source = new();
            return _ct_source;
        }

        public void Terminate()
        {
            Visibility = Visibility.Collapsed;
            _ct_source?.Dispose();
            _ct_source = null;
        }

        internal void OnProgressChanged(ProgressReport p)
        {
            if (p.Caption is not null)
            {
                Caption = p.Caption;
            }
            if (p.Message is not null)
            {
                Message = p.Message;
            }
            if (p.Value is >= 0)
            {
                Value = p.Value;
            }
            if (p.Maximum is > 0)
            {
                Maximum = p.Maximum;
            }
        }

        private void OnExecuted_Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsAbortable)
            {
                _ct_source?.Cancel();
            }
            e.Handled = true;
        }
    }
}
