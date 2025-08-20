using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class BmScoreViewer : ScrollViewerBase, ISelection
    {
        public const SelectionType _SelectionType = SelectionType.Both | SelectionType.Hide;
        public static double[] ScaleYList { get; } = [4, 8, 16, 24, 32, 48, 64, 96, 128, 144, 160, 192, 240, 288, 320, 384, 480, 576, 640, 768, 960, 1152, 1280, 1536, 1920, 2304, 2560, 3072, 3840, 4608, 5120, 6144, 7680];
        public static double[] ScaleXList { get; } = [6, 8, 10, 12, 14, 16, 18, 20, 22, 24];

        public const double DefaultScaleY = 384;
        public const double DefaultScaleX = 14;

        public static readonly Rational DefaultQuantize = new(1, 16);
        public const bool DefaultSyncGridToQuantize = true;
        public const bool DefaultShiftToVerticalMove = true;
        public const EditMode DefaultEditMode = EditMode.Move;

        static BmScoreViewer()
        {
            ScaleXProperty.OverrideMetadata(typeof(BmScoreViewer), PropertyUtils.GetMetaTwoWay(DefaultScaleX, OnScaleXChanged));
            ScaleYProperty.OverrideMetadata(typeof(BmScoreViewer), PropertyUtils.GetMetaTwoWay(DefaultScaleY, OnScaleYChanged));
        }

        public static readonly DependencyProperty SmallGridProperty = BarLineCanvas.SmallGridProperty.AddOwner(typeof(BmScoreViewer), BmsViewModel.DefaultSmallGrid, OnSmallGridChanged);
        public static readonly DependencyProperty LargeGridProperty = BarLineCanvas.LargeGridProperty.AddOwner(typeof(BmScoreViewer));

        public static readonly RoutedEvent NoteClickedEvent = Events.Register<BmScoreViewer, NoteClickedEventHandler>();

        [DependencyProperty]
        private BmsViewModel? _viewModel;
        [DependencyProperty]
        private Rational _quantize = DefaultQuantize;
        [DependencyProperty]
        private bool _syncGridToQuantize = DefaultSyncGridToQuantize;
        [DependencyProperty]
        private bool _needsShiftToVerticalMove = DefaultShiftToVerticalMove;
        [DependencyProperty]
        private EditMode _editMode = DefaultEditMode;

        protected virtual bool CanChangeScaleX => true;
        private bool _changing_by_wheel;
        protected Rational _length;
        protected SelectionRect _selection = new();
        protected NoteCursor _cursor = new();

        public event NoteClickedEventHandler NoteClicked { add => AddHandler(NoteClickedEvent, value); remove => RemoveHandler(NoteClickedEvent, value); }

        public Rational SmallGrid { get => (Rational)GetValue(SmallGridProperty); set => SetValue(SmallGridProperty, value); }
        public Rational LargeGrid { get => (Rational)GetValue(LargeGridProperty); set => SetValue(LargeGridProperty, value); }

        ISelectionRect ISelection.SelectionRect => _selection;
        SelectionType ISelection.SelectionType => _SelectionType;
        bool ISelection.IsSelectionEmpty => _viewModel is not null && _viewModel.IsSelectionEmpty;

        protected readonly BarLineCanvas _barLines = new();
        protected readonly CanvasBackground _background = new();
        protected readonly CanvasHeader _header = new();

        public BmScoreViewer()
        {
            MinScaleX = ScaleXList[0];
            MaxScaleX = ScaleXList[^1];
            MinScaleY = ScaleYList[0];
            MaxScaleY = ScaleYList[^1];
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        protected Binding Binding_ViewModel => GetBindingCore(nameof(ViewModel), BindingMode.TwoWay);

        protected virtual void OnViewModelChanged(BmsViewModel? oldValue, BmsViewModel? newValue) { }

        private void OnQuantizeChanged(Rational value)
        {
            if (_syncGridToQuantize)
            {
                SmallGrid = value;
            }
        }

        private void OnSyncGridToQuantizeChanged(bool value)
        {
            if (value)
            {
                SmallGrid = _quantize;
            }
        }

        private static void OnSmallGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BmScoreViewer v && v._syncGridToQuantize && v._quantize != (Rational)e.NewValue)
            {
                v.SmallGrid = v._quantize;
            }
        }

        private void OnEditModeChanged(EditMode value)
        {
            if (value is EditMode.Add)
            {
                _cursor.Visibility = Visibility.Visible;
                Cursor = Cursors.Pen;
            }
            else
            {
                _cursor.Visibility = Visibility.Collapsed;
                Cursor = null;
            }
        }

        protected override void InitializeScrollableContents()
        {
            base.InitializeScrollableContents();
            AppendBaseCanvas();
            AppendContentCanvas();
            AppendSelectionCanvas();
        }

        protected virtual void AppendBaseCanvas()
        {
            AddChild(_barLines);
        }

        protected virtual void AppendContentCanvas()
        {
        }

        protected virtual void AppendSelectionCanvas()
        {
            AddChild(_selection.Rectangle);
            AddChild(_cursor);
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            _main_canvas.SetBinding(MinHeightProperty, Binding_ViewportHeight);

            var barLines = _barLines;
            barLines.SetBinding(BmsCanvasBase.ViewModelProperty, Binding_ViewModel);
            barLines.SetBinding(CanvasBase.ViewportWidthProperty, Binding_ViewportWidth);
            barLines.SetBinding(CanvasBase.ViewportHeightProperty, Binding_ViewportHeight);
            barLines.SetBinding(BmsCanvasBase.BottomProperty, Binding_ContentHeight);
            barLines.SetBinding(BmsCanvasBase.ScaleYProperty, Binding_ScaleY);
            barLines.SetBinding(Canvas.LeftProperty, Binding_HorizontalOffset);
            barLines.SetBinding(CanvasBase.ViewportTopProperty, Binding_VerticalOffset);

            SetBinding(SmallGridProperty, new Binding(nameof(SmallGrid)) { Mode = BindingMode.TwoWay, Source = barLines });
            SetBinding(LargeGridProperty, new Binding(nameof(LargeGrid)) { Mode = BindingMode.TwoWay, Source = barLines });
        }

        public static double ChangeScaleX(double current, int delta) => IScaleProperty.ChangeScaleCore(ScaleXList, current, delta);
        public static double ChangeScaleY(double current, int delta) => IScaleProperty.ChangeScaleCore(ScaleYList, current, delta);

        protected override void OnScaleXChanged(double oldValue, double newValue)
        {
            if (!_changing_by_wheel)
            {
                AdjustHorizontalScroll(0, oldValue, newValue);
            }
        }

        protected override void OnScaleYChanged(double oldValue, double newValue)
        {
            UpdateAreaHeight();
            if (!_changing_by_wheel)
            {
                AdjustVerticalScroll(ViewportHeight, oldValue, newValue);
            }
        }

        protected void UpdateAreaHeight()
        {
            ContentHeight = _length * _scale_y;
        }

        public (int Number, Rational Position, double ActualPosition) GetCurrentBar() => GetHeadBarInfo(VerticalOffset + ViewportHeight);
        public (int Number, Rational Position, double ActualPosition) GetHeadBarInfo() => GetHeadBarInfo(Mouse.GetPosition(_main_canvas).Y);

        public (int Number, Rational Position, double ActualPosition) GetHeadBarInfo(double y)
        {
            return _barLines.GetHeadPosition(_contentHeight - y);
        }

        public Rational GetPosition(MouseEventArgs e) => GetPosition(e.GetPosition(_main_canvas).Y);

        public Rational GetPosition(double y)
        {
            var offset = Math.Max(_contentHeight - y - RectBase.HeadHeight / 2, 0);
            var (_, r, d) = _barLines.GetHeadPosition(offset);
            return r + _quantize * (int)Math.Round((offset - d) / _scale_y / _quantize);
        }

        public virtual BarPosition GetBarPosition(Rational absolutePosition) => new(0, absolutePosition);

        public void ScrollIntoView(Rational position, double ratio = 1.0, bool force = false)
        {
            var min = VerticalOffset;
            var height = ViewportHeight;
            var max = min + height;
            var offset = _contentHeight - (double)position * _scale_y;
            if (force || offset < min || offset > max)
            {
                ScrollToVerticalOffset(offset - height * ratio);
            }
        }

        protected override bool ProcessWheel(int delta, bool ctrl, bool shift)
        {
            if (ctrl)
            {
                _changing_by_wheel = true;
                if (shift && CanChangeScaleX)
                {
                    var current = _scale_x;
                    ScaleX = ChangeScaleX(current, delta);
                    AdjustHorizontalScroll(current, _scale_x);
                }
                else
                {
                    var current = _scale_y;
                    ScaleY = ChangeScaleY(current, delta);
                    AdjustVerticalScroll(current, _scale_y);
                }
                _changing_by_wheel = false;
            }
            else if (shift)
            {
                ScrollToHorizontalOffset(HorizontalOffset - delta);
            }
            else
            {
                ScrollToVerticalOffset(VerticalOffset - GetScrollUnit(_scale_y, ViewportHeight) * (delta is > 0 ? 1 : -1));
            }
            return true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_editMode is EditMode.Add)
            {
                UpdateCursor();
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_editMode is EditMode.Add)
            {
                AddNoteAtCursor();
            }
            else
            {
                var pos = e.GetPosition(_main_canvas);
                if (!CheckNoteClick(pos, e))
                {
                    this.StartSelection(_main_canvas);
                }
            }
            e.Handled = true;
        }

        public void UpdateCursor() => UpdateCursorInternal();
        protected virtual void AddNoteAtCursor() { }

        protected virtual void RaiseNoteClicked(NoteRect rect, MouseButtonEventArgs? e = null)
        {
            RaiseEvent(new NoteClickedEventArgs(NoteClickedEvent, this, rect, e is not null && e.ClickCount is > 1));
        }

        protected virtual bool CheckNoteClick(Point point, MouseButtonEventArgs e)
        {
            if (SelectableCanvas.HitTest(point, out var rect))
            {
                OnNoteClicked(rect, e);
                return true;
            }
            return false;
        }
        public bool HitTest([MaybeNullWhen(false)] out NoteRect rect) => SelectableCanvas.HitTest(Mouse.GetPosition(_main_canvas), out rect);

        private void OnNoteClicked(NoteRect rect, MouseButtonEventArgs e)
        {
            if (SelectNote(rect, e))
            {
                if (e.ClickCount is 1 && _editMode is EditMode.Move)
                {
                    StartSelectionMove(rect);
                }
            }
        }

        protected virtual void UpdateCursorInternal()
        {
            var (x, y) = Mouse.GetPosition(_main_canvas);
            if (_editMode is EditMode.Add && LaneIndexConverter.TryGetPos2Info(x, out var ax, out var info))
            {
                var cursor = _cursor;
                var width = info.Width * _scale_x;
                var actPos = GetPosition(y);
                var pos = GetBarPosition(actPos);
                var lane = info.Lane;
                var color = info.NoteColor;
                var (nType, dType, text) = GetCursorTypes(lane);
                cursor.Update(width, pos, actPos, lane, nType, dType, text, color);
                UpdateCursorIndex();
                Canvas.SetLeft(cursor, ax);
                y = _contentHeight - actPos * _scale_y - RectBase.HeadHeight;
                Canvas.SetTop(cursor, y);
            }
        }

        protected virtual (NoteType NoteType, DefType DefType, string? TypeText) GetCursorTypes(int lane) => (NoteType.Normal, DefType.None, null);
        protected virtual void UpdateCursorIndex() => _cursor.SetIndexText(null);
    }
}
