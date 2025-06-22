using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class BmsCanvasBase : CanvasBase
    {
        public static readonly DependencyProperty ScaleYProperty = IScaleProperty.RegisterScaleY<BmsCanvasBase>(OnScaleYChanged);

        private static void OnScaleYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BmsCanvasBase c)
            {
                c._scaleY = (double)e.NewValue;
                c.OnScaleYChanged();
            }
        }

        [DependencyProperty]
        private BmsViewModel? _viewModel;
        /// <summary>
        /// Represents pixels per beat.
        /// </summary>
        private double _scaleY = IScaleProperty.DefaultScale;
        [DependencyProperty]
        private double _bottom;

        /// <summary>
        /// Represents pixels per beat.
        /// </summary>
        public double ScaleY { get => _scaleY; set => SetValue(ScaleYProperty, value); }

        protected virtual void OnViewModelChanged(BmsViewModel? oldValue, BmsViewModel? newValue) { }

        protected virtual void OnScaleYChanged()
        {
            RefreshVertical();
            ReserveViewportRefresh();
        }

        protected void OnBottomChanged() => OnScaleYChanged();
        protected virtual void RefreshVertical() { }
    }
}
