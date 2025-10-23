using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Destination : SkinNode, IRectNode
    {
        public ValueExpression? Time { get; set => SetValue(ref field, value); }
        public ValueExpression? Slope { get; set => SetValue(ref field, value); }
        public ValueExpression? X { get; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Y { get; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Width { get; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Height { get; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Opacity { get; set => SetValue(ref field, value); }
        public ValueExpression? OriginX { get; set => SetValue(ref field, value, [nameof(Origin)]); }
        public ValueExpression? OriginY { get; set => SetValue(ref field, value, [nameof(Origin)]); }
        public ValueExpression? Angle { get; set => SetValue(ref field, value); }
        public ValueExpression? RotateOriginX { get; set => SetValue(ref field, value, [nameof(RotateOrigin)]); }
        public ValueExpression? RotateOriginY { get; set => SetValue(ref field, value, [nameof(RotateOrigin)]); }

        public string Rect { get => this.GetRectText(); set => this.SetRectText(value); }

        public string Origin
        {
            get => $"{OriginX}, {OriginY}";
            set
            {
                StringConversion.GetTuple(value, out var v1, out var v2);
                OriginX = v1;
                OriginY = v2;
            }
        }

        public string RotateOrigin
        {
            get => $"{RotateOriginX}, {RotateOriginY}";
            set
            {
                StringConversion.GetTuple(value, out var v1, out var v2);
                RotateOriginX = v1;
                RotateOriginY = v2;
            }
        }
    }
}
