using LivreNoirLibrary.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace LivreNoirLibrary.Media.Effects
{
    public partial class BorderEffect : ShaderEffectBase
    {
        public static readonly DependencyProperty ColorProperty = RegisterParameter<BorderEffect, Color>(0, Colors.Black);

        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

        public BorderEffect() : base("border")
        {
            UpdateShaderValue(ColorProperty);
        }
    }
}
