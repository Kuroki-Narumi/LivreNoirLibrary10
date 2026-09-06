using LivreNoirLibrary.Media.Capture;
using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class WindowCaptureControl : CaptureControlBase<WindowCapturer>
    {
        [DependencyProperty]
        private string? _searchingTitle;
        [DependencyProperty]
        private string? _searchingFile;
        [DependencyProperty]
        private WindowSearchMode _searchMode = WindowSearchMode.TitleAndFile;
        [DependencyProperty]
        private TimeSpan _searchInterval = WindowCapturer.DefaultSearchInterval;

        private bool _needRefresh = true;

        public WindowCaptureControl()
        {
            _capturer.SearchInterval = SearchInterval;
        }

        protected override WindowCapturer CreateCapturer() => new(this);

        private void ReserveRefresh()
        {
            _needRefresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_needRefresh)
            {
                _needRefresh = false;
                _capturer.SetSearchTarget(SearchingTitle, SearchingFile, SearchMode);
            }
            base.OnRender(drawingContext);
        }

        private void OnSearchingTitleChanged() => ReserveRefresh();
        private void OnSearchingFileChanged() => ReserveRefresh();
        private void OnSearchModeChanged() => ReserveRefresh();
        private void OnSearchIntervalChanged(TimeSpan value)
        {
            _capturer.SearchInterval = value;
        }
    }
}
