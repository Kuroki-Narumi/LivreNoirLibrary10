using LivreNoirLibrary.Media.Capture;
using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class WindowCaptureControl : CaptureControlBase<WindowCapturer>
    {
        [DependencyProperty]
        private WindowSelector? _windowSelector;
        [DependencyProperty]
        private TimeSpan _searchInterval = WindowCapturer.DefaultSearchInterval;

        protected override WindowCapturer CreateCapturer() => new(this);

        public WindowCaptureControl()
        {
            _capturer.SearchInterval = SearchInterval;
        }

        private void OnWindowSelectorChanged(WindowSelector? value)
        {
            _capturer.Selector = value;
        }

        private void OnSearchIntervalChanged(TimeSpan value)
        {
            _capturer.SearchInterval = value;
        }
    }
}
