using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// ImageRectSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageRectSelector : UserControl, IHistoryOwner<Int32Rect>
    {
        public static readonly DependencyProperty SourceProperty = 
            ImageRectSelectorView.SourceProperty.AddOwnerTwoWay<BitmapSource>(typeof(ImageRectSelector), callback: OnSourceChanged);
        public static readonly DependencyProperty InitialRectProperty =
            ImageRectSelectorView.InitialRectProperty.AddOwnerTwoWay<Int32Rect>(typeof(ImageRectSelector), callback: OnSourceChanged);

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageRectSelector)!.ClearHistory();
        }

        [DependencyProperty]
        private string? _scaleText = "拡大率";
        [DependencyProperty]
        private string? _autoScaleText = "自動";
        [DependencyProperty]
        private string? _horizontalText = "横";
        [DependencyProperty]
        private string? _verticalText = "縦";

        private readonly History<Int32Rect> _history;
        IHistory IHistoryOwner.History => _history;
        private bool _needUpdateHistory = true;

        public BitmapSource? Source { get => SelectorView.Source; set => SetValue(SourceProperty, value); }
        public Int32Rect InitialRect { get => SelectorView.InitialRect; set => SetValue(InitialRectProperty, value); }

        public ImageRectSelector()
        {
            InitializeComponent();
            SetBinding(SourceProperty, new Binding(nameof(Source)) { Source = SelectorView, Mode = BindingMode.TwoWay });
            SetBinding(InitialRectProperty, new Binding(nameof(InitialRect)) { Source = SelectorView, Mode = BindingMode.TwoWay });
            _history = new(this);
            this.RegisterHistoryCommands();
            Text_Scale.SetBinding(TextBlock.TextProperty, new Binding(nameof(ScaleText)) { Source = this });
            Text_AutoScale.SetBinding(Button.ContentProperty, new Binding(nameof(AutoScaleText)) { Source = this });
            Text_Horizontal.SetBinding(TextBlock.TextProperty, new Binding(nameof(HorizontalText)) { Source = this });
            Text_Vertical.SetBinding(TextBlock.TextProperty, new Binding(nameof(VerticalText)) { Source = this });
        }

        public void SetVisualSource(Visual visual) => Source = Bitmap.GetSourceFromVisual(visual);
        public void SetDrawingSource(Drawing drawing) => Source = Bitmap.GetSourceFromDrawing(drawing);

        private void OnPreviewMouseWheel_Scale(object sender, MouseWheelEventArgs e) => (sender as ComboBox)!.ChangeByWheel(e);
        private void OnClick_Scale_Auto(object sender, RoutedEventArgs e) => SelectorView.AutoScale();
        private void OnClick_Scale_100(object sender, RoutedEventArgs e) => SelectorView.ScaleX = 1;

        public void ClearHistory() => _history.Initialize();

        private void OnValueChanged_Slider(object sender, RangeSliderValueChangedEventArgs e)
        {
            if (_needUpdateHistory)
            {
                _history.PushUndo();
            }
        }

        private void OnValueChanged_View(object sender, Int32Rect newValue)
        {
            if (_needUpdateHistory)
            {
                _history.PushUndo();
            }
        }

        Int32Rect IHistoryOwner<Int32Rect>.GetHistoryData() => SelectorView.Selection.GetRect();
        bool IHistoryOwner<Int32Rect>.NeedsUpdateHistory(Int32Rect historyData) => historyData != SelectorView.Selection.GetRect();
        void IHistoryOwner<Int32Rect>.ApplyHistory(Int32Rect historyData)
        {
            _needUpdateHistory = false;
            SelectorView.Selection.SetRect(historyData);
            _needUpdateHistory = true;
        }
    }
}
