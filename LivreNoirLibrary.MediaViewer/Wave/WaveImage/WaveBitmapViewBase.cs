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
