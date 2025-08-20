using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using LivreNoirLibrary.Windows;

namespace LivreNoirLibrary.Media.Effects
{
    [ContentProperty(nameof(Front))]
    public partial class BlendEffect : ShaderEffectBase
    {
        public const CompositeMode DefaultCompositeMode = CompositeMode.SourceOver;
        public const BlendMode DefaultBlendMode = BlendMode.Alpha;

        /*
        * コンポジットモードとPorter Duff定数の対応
        *        mode     : [D1, Dx, S1, Sx] -> [  DstF  ,  SrcF   ]
        *       Clear     : [ 0,  0,  0,  0] -> [ 0      , 0       ]
        *      Source     : [ 0,  0,  1,  0] -> [ 0      , 1       ]
        * Destination     : [ 1,  0,  0,  0] -> [ 1      , 0       ]
        *      Source Over: [ 1, -1,  1,  0] -> [ 1 - S.a, 1       ]
        * Destination Over: [ 1,  0,  1, -1] -> [ 1      , 1 - D.a ]
        *      Source In  : [ 0,  0,  0,  1] -> [ 0      ,     D.a ]
        * Destination In  : [ 0,  1,  0,  0] -> [     S.a, 0       ]
        *      Source Out : [ 0,  0,  1, -1] -> [ 0      , 1 - D.a ]
        * Destination Out : [ 1, -1,  0,  0] -> [ 1 - S.a, 0       ]
        *      Source Atop: [ 1, -1,  0,  1] -> [ 1 - S.a,     D.a ]
        * Destination Atop: [ 0,  1,  1, -1] -> [     S.a, 1 - D.a ]
        *         Xor     : [ 1, -1,  1, -1] -> [ 1 - S.a, 1 - D.a ]
        *     Lighter     : [ 0,  1,  0,  1] -> [ 1      , 1       ]
        */
        private static readonly Dictionary<CompositeMode, Point4D> _d1dxs1sx = new()
        {
            [CompositeMode.Source] = new(0, 0, 1, 0),
            [CompositeMode.Destination] = new(1, 0, 0, 0),
            [CompositeMode.SourceOver] = new(1, -1, 1, 0),
            [CompositeMode.DestinationOver] = new(1, 0, 1, -1),
            [CompositeMode.SourceIn] = new(0, 0, 0, 1),
            [CompositeMode.DestinationIn] = new(0, 1, 0, 0),
            [CompositeMode.SourceOut] = new(0, 0, 1, -1),
            [CompositeMode.DestinationOut] = new(1, -1, 0, 0),
            [CompositeMode.SourceAtop] = new(1, -1, 0, 1),
            [CompositeMode.DestinationAtop] = new(0, 1, 1, -1),
            [CompositeMode.Xor] = new(1, -1, 1, -1),
            [CompositeMode.Lighter] = new(0, 1, 0, 1),
        };

        private static readonly Dictionary<BlendMode, string> _blender = new()
        {
            [BlendMode.Alpha] = "alpha",
            [BlendMode.Add] = "add",
            [BlendMode.Subtract] = "subtract",
            [BlendMode.Multiply] = "multiply",
            [BlendMode.Screen] = "screen",
            [BlendMode.Overlay] = "overlay",
            [BlendMode.Darken] = "darken",
            [BlendMode.Lighten] = "lighten",
            [BlendMode.ColorDodge] = "color_dodge",
            [BlendMode.ColorBurn] = "color_burn",
            [BlendMode.HardLight] = "hard_light",
            [BlendMode.SoftLight] = "soft_light",
            [BlendMode.Difference] = "difference",
            [BlendMode.Exclusion] = "exclusion",
        };

        public static readonly DependencyProperty FrontProperty = RegisterSampler<BlendEffect>(1);
        public Brush? Front { get => GetValue(FrontProperty) as Brush; set => SetValue(FrontProperty, value); }

        private static readonly DependencyProperty InternalCompositeModeProperty = RegisterParameter<BlendEffect, Point4D>(0, _d1dxs1sx[DefaultCompositeMode]);

        [DependencyProperty]
        private CompositeMode _compositeMode = DefaultCompositeMode;
        [DependencyProperty]
        private BlendMode _blendMode = DefaultBlendMode;

        public BlendEffect() : base(_blender[DefaultBlendMode])
        {
            UpdateShaderValue(FrontProperty);
            UpdateShaderValue(InternalCompositeModeProperty);
        }

        private void OnCompositeModeChanged(CompositeMode value)
        {
            SetValue(InternalCompositeModeProperty, _d1dxs1sx.GetValueOrDefault(value));
        }

        private void OnBlendModeChanged(BlendMode value)
        {
            PixelShader = _blender.TryGetValue(value, out var name) ? GetPixelShader(name) : null;
        }
    }
}
