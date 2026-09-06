using LivreNoirLibrary.Win32Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.Media.Capture
{
    public delegate bool WindowSelector(WindowInfo info);

    public class WindowCapturer : ICapturer
    {
        public event EventHandler? CaptureTargetClosed { add => _capturer.CaptureTargetClosed += value; remove => _capturer.CaptureTargetClosed -= value; }

        public static TimeSpan DefaultSearchInterval { get; } = TimeSpan.FromMilliseconds(500);

        private readonly Capturer _capturer;
        private readonly SynchronizationContext? _syncContext = SynchronizationContext.Current;

        private Timer? _timer;
        private Action? _searchFunc;
        private WindowInfo? _captureTarget;
        private readonly Func<WindowInfo, bool> _checkTitleMatch;
        private readonly Func<WindowInfo, bool> _checkFileMatch;
        private readonly Func<WindowInfo, bool> _checkTitleAndFileMatch;

        public IImageAdapter Adapter { get => _capturer.Adapter; set => _capturer.Adapter = value; }

        public bool IsCursorCaptureEnabled { get => _capturer.IsCursorCaptureEnabled; set => _capturer.IsCursorCaptureEnabled = value; }

        public bool IsActive => _capturer.IsActive;

        public nint CapturingWindowHandle => _capturer.CapturingWindowHandle;
        public nint CapturingMonitorHandle => _capturer.CapturingMonitorHandle;

        /// <summary>
        /// キャプチャ候補のウィンドウタイトル
        /// </summary>
        public string? SearchingTitle { get; private set; }

        /// <summary>
        /// キャプチャ候補の実行ファイル名
        /// </summary>
        public string? SearchingFile { get; private set; }

        /// <summary>
        /// キャプチャ候補の検出方法
        /// </summary>
        public WindowSearchMode SearchMode { get; private set; } = WindowSearchMode.TitleAndFile;

        /// <summary>
        /// キャプチャ対象が存在しない場合に探す間隔。
        /// </summary>
        public TimeSpan SearchInterval
        {
            get;
            set
            {
                field = value;
                RefreshTimer();
            }
        } = DefaultSearchInterval;

        public WindowCapturer(IImageAdapter adapter)
        {
            _capturer = new(adapter);
            _capturer.CaptureTargetClosed += OnCaptureTargetClosed;
            _checkTitleMatch = CheckTitleMatch;
            _checkFileMatch = CheckFileMatch;
            _checkTitleAndFileMatch = CheckTitleAndFileMatch;
        }

        private void StopTimer()
        {
            _timer?.Dispose();
            _timer = null;
        }

        public void SetSearchTarget(string? title, string? file, WindowSearchMode mode = WindowSearchMode.TitleAndFile)
        {
            if (TextEquals(SearchingTitle, title) && TextEquals(SearchingFile, file) && SearchMode == mode)
            {
                return;
            }

            _capturer.Stop();
            SearchingTitle = title;
            SearchingFile = file;
            SearchMode = mode;

            Action? searchFunc = null;
            if (!string.IsNullOrEmpty(title) && (mode & WindowSearchMode.Title) is not 0)
            {
                if (!string.IsNullOrEmpty(file) && (mode & WindowSearchMode.File) is not 0)
                {
                    if ((mode & WindowSearchMode.Complete) is not 0)
                    {
                        searchFunc = SearchByTitleAndName;
                    }
                    else
                    {
                        searchFunc = SearchByTitleOrName;
                    }
                }
                else
                {
                    searchFunc = SearchByTitle;
                }
            }
            else if (!string.IsNullOrEmpty(file) && (mode & WindowSearchMode.File) is not 0)
            {
                searchFunc = SearchByFile;
            }
            _searchFunc = searchFunc;
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            StopTimer();
            if (!_capturer.IsActive && _searchFunc is { } func)
            {
                OnTick(null);
                var interval = SearchInterval;
                if (!_capturer.IsActive && interval.Ticks > 0)
                {
                    _timer = new(OnTick, null, default, interval);
                }
            }
        }

        private void OnTick(object? state)
        {
            _captureTarget = null;
            _searchFunc?.Invoke();
            if (_captureTarget is { } info)
            {
                if (_syncContext is { } ctx)
                {
                    ctx.Post(StartCapture, info.Handle);
                }
                else
                {
                    StartCapture(info.Handle);
                }
            }
        }

        private static bool TextEquals(string? left, string? right) => left.AsSpan().Equals(right, StringComparison.OrdinalIgnoreCase);

        private bool CheckTitleMatch(WindowInfo info)
        {
            if (TextEquals(info.Title, SearchingTitle!))
            {
                _captureTarget = info;
                return false;
            }
            return true;
        }

        private bool CheckFileMatch(WindowInfo info)
        {
            if (TextEquals(info.ExeFileName, SearchingFile!))
            {
                _captureTarget = info;
                return false;
            }
            return true;
        }

        private bool CheckTitleAndFileMatch(WindowInfo info)
        {
            if (TextEquals(info.Title, SearchingTitle!) && TextEquals(info.ExeFileName, SearchingFile!))
            {
                _captureTarget = info;
                return false;
            }
            return true;
        }

        private void SearchByTitle() => NativeMethods.EnumerateWindowInfo(_checkTitleMatch);
        private void SearchByFile() => NativeMethods.EnumerateWindowInfo(_checkFileMatch);
        private void SearchByTitleAndName() => NativeMethods.EnumerateWindowInfo(_checkTitleAndFileMatch);
        private void SearchByTitleOrName()
        {
            NativeMethods.EnumerateWindowInfo(_checkTitleMatch);
            if (_captureTarget is null)
            {
                NativeMethods.EnumerateWindowInfo(_checkFileMatch);
            }
        }

        private void StartCapture(object? state)
        {
            if (state is nint handle)
            {
                _capturer.StartWindowCapture(handle);
            }
            if (_capturer.IsActive)
            {
                StopTimer();
            }
        }

        private void OnCaptureTargetClosed(object? sender, EventArgs e)
        {
            RefreshTimer();
        }
    }
}
