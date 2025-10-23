using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Effects
{
    public partial class TransparentConverter : ShaderEffectBase
    {
        public static readonly DependencyProperty ColorProperty = RegisterParameter<TransparentConverter, Color>(0, Colors.Black);

        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

        public TransparentConverter() : base("transparent")
        {
            UpdateShaderValue(ColorProperty);
        }
    }
}
