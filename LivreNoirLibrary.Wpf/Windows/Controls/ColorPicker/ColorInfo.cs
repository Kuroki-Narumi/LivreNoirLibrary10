using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using LivreNoirLibrary.ObjectModel;
using static LivreNoirLibrary.Media.ColorUtils;

namespace LivreNoirLibrary.Media
{
    public class ColorInfo : ObservableObjectBase
    {
        private float _a;
        private float _r;
        private float _g;
        private float _b;
        private float _h;
        private float _s;
        private float _v;

        public float A { get => _a; set => SetPropertyA(ref _a, Math.Clamp(value, 0, 1)); }
        public float R { get => _r; set => SetProperty(ref _r, Math.Clamp(value, 0, 1)); }
        public float G  { get => _g; set => SetProperty(ref _g, Math.Clamp(value, 0, 1)); }
        public float B { get => _b; set => SetProperty(ref _b, Math.Clamp(value, 0, 1)); }
        public float H { get => _h; set => SetPropertyH(ref _h, Math.Clamp(value, 0, 360)); }
        public float S { get => _s; set => SetPropertySV(ref _s, Math.Clamp(value, 0, 1)); }
        public float V { get => _v; set => SetPropertySV(ref _v, Math.Clamp(value, 0, 1)); }

        public int IntA { get => GetInt(_a); set => A = GetFloat(value); }
        public int IntR { get => GetInt(_r); set => R = GetFloat(value); }
        public int IntG { get => GetInt(_g); set => G = GetFloat(value); }
        public int IntB { get => GetInt(_b); set => B = GetFloat(value); }
        public int IntH { get => (int)MathF.Round(_h); set => H = value; }
        public int IntS { get => GetInt(_s); set => S = GetFloat(value); }
        public int IntV { get => GetInt(_v); set => V = GetFloat(value); }

        public Color Color { get => GetColor(); set => SetColor(value); }
        public string ColorCode { get => GetColorCode(); set => SetColorCode(value); }

        public Color GetColor()
        {
            var a = GetByte(_a);
            var r = GetByte(_r);
            var g = GetByte(_g);
            var b = GetByte(_b);
            return Color.FromArgb(a, r, g, b);
        }

        public void SetColor(Color color)
        {
            _a = GetFloat(color.A);
            _r = GetFloat(color.R);
            _g = GetFloat(color.G);
            _b = GetFloat(color.B);
            CalcHSV();
        }

        public string GetColorCode(bool alpha = true) => alpha ? ColorUtils.GetColorCode(_a, _r, _g, _b) : ColorUtils.GetColorCode(_r, _g, _b);

        public bool SetColorCode(string colorCode)
        {
            if (HsvColor.TryParseColorCode(colorCode, out var color))
            {
                _a = color.A;
                _r = color.R;
                _g = color.G;
                _b = color.B;
                _h = color.H;
                _s = color.S;
                _v = color.V;
                SendAllPropertiesChanged();
                return true;
            }
            return false;
        }

        private void SendAllPropertiesChanged()
        {
            SendPropertyChanged(nameof(A));
            SendPropertyChanged(nameof(R));
            SendPropertyChanged(nameof(G));
            SendPropertyChanged(nameof(B));
            SendPropertyChanged(nameof(H));
            SendPropertyChanged(nameof(S));
            SendPropertyChanged(nameof(V));
            SendPropertyChanged(nameof(IntA));
            SendPropertyChanged(nameof(IntR));
            SendPropertyChanged(nameof(IntG));
            SendPropertyChanged(nameof(IntB));
            SendPropertyChanged(nameof(IntH));
            SendPropertyChanged(nameof(IntS));
            SendPropertyChanged(nameof(IntV));
            SendPropertyChanged(nameof(Color));
            SendPropertyChanged(nameof(ColorCode));
        }

        private void CalcRGB()
        {
            (_r, _g, _b) = ColorUtils.CalcRGB(_h, _s, _v);
            SendAllPropertiesChanged();
        }

        private void CalcHSV()
        {
            (_h, _s, V) = ColorUtils.CalcHSV(_r, _g, _b, _h, _s);
            SendAllPropertiesChanged();
        }

        private void SetPropertyA(ref float field, float value, [CallerMemberName]string propName = "")
        {
            if (value != field)
            {
                field = Math.Clamp(value, 0, 1);
                SendPropertyChanged(propName);
                SendPropertyChanged($"Int{propName}");
                SendPropertyChanged(nameof(Color));
                SendPropertyChanged(nameof(ColorCode));
            }
        }

        private void SetProperty(ref float field, float value)
        {
            value = Math.Clamp(value, 0, 1);
            if (value != field)
            {
                field = value;
                CalcHSV();
            }
        }

        private void SetPropertyH(ref float field, float value)
        {
            value = Math.Clamp(value, 0, 359);
            if (value != field)
            {
                field = value;
                CalcRGB();
            }
        }

        private void SetPropertySV(ref float field, float value)
        {
            value = Math.Clamp(value, 0, 1);
            if (value != field)
            {
                field = value;
                CalcRGB();
            }
        }

        public (byte R, byte G, byte B) GetBytes()
        {
            return (GetByte(_r), GetByte(_g), GetByte(_b));
        }
    }
}
