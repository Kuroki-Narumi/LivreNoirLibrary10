using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// ImageRectSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class ImageRectSelector : UserControl, IHistoryOwner<Int32Rect>
    {
        public static readonly DependencyProperty SourceProperty = 
            ImageRectSelectorView.SourceProperty.AddOwnerTwoWay<BitmapSource>(typeof(ImageRectSelector), callback: OnSourceChanged);

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageRectSelector selector && e.NewValue is BitmapSource bitmap)
            {
                var selection = selector.SelectorView;
                if (selection.SelectedWidth is <= 0 && selection.SelectedHeight is <= 0)
                {
                    selector.SetDispatcher(() =>
                    {
                        selection.SetOriginalRect();
                        selector.OnEdit();
                    });
                }
            }
        }

        [DependencyProperty]
        private string? _scaleText = "拡大率";
        [DependencyProperty]
        private string? _autoScaleText = "自動";
        [DependencyProperty]
        private string? _horizontalText = "横";
        [DependencyProperty]
        private string? _verticalText = "縦";
        [DependencyProperty]
        private string? _widthText = "幅";
        [DependencyProperty]
        private string? _heightText = "高さ";

        private readonly History<Int32Rect> _history;
        IHistory IHistoryOwner.History => _history;

        public BitmapSource? Source { get => SelectorView.Source; set => SetValue(SourceProperty, value); }
        public UIElementCollection LeftPanel => LeftStackPanel.Children;
        public UIElementCollection RightPanel => RightStackPanel.Children;

        public ImageRectSelector()
        {
            InitializeComponent();
            SelectorView.SetBinding(ImageRectSelectorView.SourceProperty, new Binding(nameof(Source)) { Source = this, Mode = BindingMode.TwoWay });
            _history = new(this);
            this.RegisterHistoryCommands();
            Text_Scale.SetBinding(TextBlock.TextProperty, new Binding(nameof(ScaleText)) { Source = this });
            Text_AutoScale.SetBinding(Button.ContentProperty, new Binding(nameof(AutoScaleText)) { Source = this });
            Text_Horizontal.SetBinding(TextBlock.TextProperty, new Binding(nameof(HorizontalText)) { Source = this });
            Text_Vertical.SetBinding(TextBlock.TextProperty, new Binding(nameof(VerticalText)) { Source = this });
            Text_Width.SetBinding(TextBlock.TextProperty, new Binding(nameof(WidthText)) { Source = this });
            Text_Height.SetBinding(TextBlock.TextProperty, new Binding(nameof(HeightText)) { Source = this });
            Text_AutoRange.SetBinding(Button.ContentProperty, new Binding(nameof(AutoScaleText)) { Source = this });
            this.AddModifiedHandler(OnModified);
        }

        public Int32Rect GetRect() => SelectorView.GetRect();
        public void SetRect(Int32Rect rect) => SelectorView.SetRect(rect);

        public void SetVisualSource(Visual visual) => Source = Bitmap.GetSourceFromVisual(visual);
        public void SetDrawingSource(Drawing drawing) => Source = Bitmap.GetSourceFromDrawing(drawing);

        private void OnPreviewMouseWheel_Scale(object sender, MouseWheelEventArgs e) => (sender as ComboBox)!.ChangeByWheel(e);

        private void OnClick_Scale_Auto(object sender, RoutedEventArgs e)
        {
            SelectorView.AutoScale();
            e.Handled = true;
        }

        private void OnClick_Scale_100(object sender, RoutedEventArgs e)
        {
            SelectorView.ScaleX = 1;
            e.Handled = true;
        }

        private void OnClick_Range_Auto(object sender, RoutedEventArgs e)
        {
            SelectorView.SetOpaqueRect();
            this.OnEdit();
            e.Handled = true;
        }

        public void ClearHistory() => _history.Clear();

        Int32Rect IHistoryOwner<Int32Rect>.GetHistoryData() => SelectorView.GetRect();
        bool IHistoryOwner<Int32Rect>.HistoryEquals(Int32Rect previous, Int32Rect current) => previous == current;
        void IHistoryOwner<Int32Rect>.ApplyHistory(Int32Rect historyData) => SelectorView.SetRect(historyData);

        private void OnModified(object sender, RoutedEventArgs e)
        {
            this.OnEdit();
            e.Handled = true;
        }
    }
}
