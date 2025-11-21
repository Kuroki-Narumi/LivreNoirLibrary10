using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public class LeftToRightBitmapView : WriteableBitmapView
    {
        private readonly RotateTransform _rotate = new(-90);
        private readonly TranslateTransform _translate = new();

        public LeftToRightBitmapView()
        {
            TransformGroup transform = new();
            transform.Children.Add(_rotate);
            transform.Children.Add(_translate);
            RenderTransform = transform;
        }

        protected override void UpdateRenderSize(double width, double height)
        {
            base.UpdateRenderSize(height, width);
            _translate.Y = height;
        }
    }
}
