using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using LivreNoirLibrary.Windows;

namespace LivreNoirLibrary.Media.Effects
{
    public abstract class ShaderEffectBase : ShaderEffect
    {
        private const string ResourcePath_Base = "pack://application:,,,/LivreNoirLibrary.Wpf;component/Media/Effects/Shaders/";
        private static readonly Dictionary<string, PixelShader> _pixelShaders = [];

        protected static PixelShader GetPixelShader(string shaderName, string directory = ResourcePath_Base)
        {
            if (!_pixelShaders.TryGetValue(shaderName, out var shader))
            {
                shader = new() { UriSource = new Uri($"{directory}{shaderName}.ps") };
                shader.Freeze();
                _pixelShaders.Add(shaderName, shader);
            }
            return shader;
        }

        protected static DependencyProperty RegisterSampler<T>(int index, [CallerMemberName] string caller = "")
            => RegisterPixelShaderSamplerProperty(PropertyUtils.GetPropertyName(caller), typeof(T), index);

        protected static DependencyProperty RegisterParameter<T, TValue>(int index, TValue defaultValue, [CallerMemberName] string caller = "")
            => DependencyProperty.Register(
                PropertyUtils.GetPropertyName(caller),
                typeof(TValue),
                typeof(T),
                new UIPropertyMetadata(defaultValue, PixelShaderConstantCallback(index)));

        public static readonly DependencyProperty BaseProperty = RegisterSampler<ShaderEffectBase>(0);

        public Brush? Base { get => GetValue(BaseProperty) as Brush; set => SetValue(BaseProperty, value); }

        public ShaderEffectBase(string shaderName)
        {
            PixelShader = GetPixelShader(shaderName);
            UpdateShaderValue(BaseProperty);
        }
    }
}
