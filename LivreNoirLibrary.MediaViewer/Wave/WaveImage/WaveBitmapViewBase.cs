using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Wave
{
    public abstract class WaveBitmapViewBase : WriteableBitmapView
    {
        private readonly MatrixTransform _transform = new();
        private Matrix _render_matrix = Matrix.Identity;

        public const int Index_Alpha = 3;
        public const int Index_Red = 2;
        public const int Index_Blue = 0;
        public const byte Byte_On = 255;

        public const int Bits_Red = Byte_On << (Index_Red * 8);
        public const int Bits_Blue = Byte_On << (Index_Blue * 8);
        public const int Bits_Alpha = Byte_On << (Index_Alpha * 8);

        public WaveBitmapViewBase()
        {
            RenderTransform = _transform;
        }

        protected override void UpdateRenderSize(double width, double height)
        {
            base.UpdateRenderSize(width, height);
            UpdateMatrix();
        }

        protected void UpdateMatrix()
        {
            _render_matrix.SetIdentity();
            UpdateMatrix(ref _render_matrix);
            _transform.Matrix = _render_matrix;
        }

        protected virtual void UpdateMatrix(ref Matrix matrix) { }
    }
}
