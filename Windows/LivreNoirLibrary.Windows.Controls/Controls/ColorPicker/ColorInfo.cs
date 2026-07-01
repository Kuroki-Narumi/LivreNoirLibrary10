using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public partial class ColorInfo : ObservableObjectBase
    {
        public const float HueInvertFactor = 1f / 359f;

        public event Action<Color>? ColorChanged;

        public bool IsAlphaEnabled { get; set => SetValue(ref field, value); }

        public float A { get; private set => SetValue(ref field, value, [nameof(IntA)]); }
        public float R { get; private set => SetValue(ref field, value, [nameof(IntR)]); }
        public float G { get; private set => SetValue(ref field, value, [nameof(IntG)]); }
        public float B { get; private set => SetValue(ref field, value, [nameof(IntB)]); }
        public float H { get; private set => SetValue(ref field, value, [nameof(IntH), nameof(ScaledIntH)]); }
        public float S { get; private set => SetValue(ref field, value, [nameof(IntS)]); }
        public float V { get; private set => SetValue(ref field, value, [nameof(IntV)]); }

        public int IntA
        {
            get => ColorUtils.GetInt(A); 
            set
            {
                A = GetFloat(value);
                UpdateColor();
            }
        }
        public int IntR
        {
            get => ColorUtils.GetInt(R); 
            set
            {
                R = GetFloat(value);
                OnRgbChanged();
            }
        }
        public int IntG
        {
            get => ColorUtils.GetInt(G); 
            set
            {
                G = GetFloat(value);
                OnRgbChanged();
            }
        }
        public int IntB
        {
            get => ColorUtils.GetInt(B); 
            set
            {
                B = GetFloat(value);
                OnRgbChanged();
            }
        }
        public int IntH
        {
            get => (int)MathF.Round(H); 
            set
            {
                H = Math.Clamp(value, 0, 359);
                OnHsvChanged();
            }
        }
        public int ScaledIntH
        {
            get => ColorUtils.GetInt(H * HueInvertFactor);
            set
            {
                H = ColorUtils.GetFloat(value) * 359;
                OnHsvChanged();
            }
        }
        public int IntS
        {
            get => ColorUtils.GetInt(S); 
            set
            {
                S = GetFloat(value);
                OnHsvChanged();
            }
        }
        public int IntV
        {
            get => ColorUtils.GetInt(V); 
            set
            {
                V = GetFloat(value);
                OnHsvChanged();
            }
        }

        public Color Color { get; private set => SetValue(ref field, value); }

        private static float GetFloat(int value) => Math.Clamp(ColorUtils.GetFloat(value), 0, 1);

        private bool _updating;

        private void OnRgbChanged()
        {
            _updating = true;
            (H, S, V) = ColorUtils.CalcHSV(R, G, B, H, S);
            UpdateColor();
            _updating = false;
        }

        private void OnHsvChanged()
        {
            _updating = true;
            (R, G, B) = ColorUtils.CalcRGB(H, S, V);
            UpdateColor();
            _updating = false;
        }

        private void UpdateColor()
        {
            var a = IsAlphaEnabled ? ColorUtils.GetByte(A) : (byte)255;
            var r = ColorUtils.GetByte(R);
            var g = ColorUtils.GetByte(G);
            var b = ColorUtils.GetByte(B);
            var color = Color.FromArgb(a, r, g, b);
            Color = color;
            ColorChanged?.Invoke(color);
        }

        public void SetColor(Color color)
        {
            A = IsAlphaEnabled ? GetFloat(color.A) : 1f;
            R = ColorUtils.GetFloat(color.R);
            G = ColorUtils.GetFloat(color.G);
            B = ColorUtils.GetFloat(color.B);
            OnRgbChanged();
        }

        public string GetColorCode() => IsAlphaEnabled ? ColorUtils.GetColorCode(A, R, G, B) : ColorUtils.GetColorCode(R, G, B);

        public bool TrySetColorCode(string colorCode)
        {
            if (HsvColor.TryParseColorCode(colorCode, out var color, H, S))
            {
                if (!_updating)
                {
                    A = color.A;
                    R = color.R;
                    G = color.G;
                    B = color.B;
                    H = color.H;
                    S = color.S;
                    V = color.V;
                    UpdateColor();
                }
                return true;
            }
            return false;
        }
    }
}
