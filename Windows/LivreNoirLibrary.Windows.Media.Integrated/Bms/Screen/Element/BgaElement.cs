using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BgaElement : ScreenElementBase
    {
        [DependencyProperty]
        private VisualBrush? _brush;

        public BgaElement(Bga source, BgaImageSource imageSource) : base(source)
        {
            SetBinding(BrushProperty, new Binding(nameof(BgaImageSource.VisualBrush)) { Source = imageSource });
        }

        public void Update(BmsTimer timer, long absoluteTick)
        {
            ViewModel.Update(timer, absoluteTick);
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(_brush, null, new(0, 0, Width, Height));
        }
    }
}
