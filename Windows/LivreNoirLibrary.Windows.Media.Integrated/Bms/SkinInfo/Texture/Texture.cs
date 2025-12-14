using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Texture : SkinNode, IKeyNode, IRectNode
    {
        public const string Key_StageFile = Tags.StageFile;
        public const string Key_Banner = Tags.Banner;
        public const string Key_BackBmp = Tags.BackBmp;
        public const string Key_Bmp00 = "#BMP00";
        public static bool IsReservedKey(string? key) => key is Key_StageFile or Key_Banner or Key_BackBmp or Key_Bmp00;

        public string Key { get; set => SetValue(ref field, value); } = "";
        public string? BasedOn { get; set => SetValue(ref field, value); }
        public ValueExpression? Source { get => field ?? _base?.Source; set => SetValue(ref field, value); }
        public ValueExpression? X { get => field ?? _base?.X; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Y { get => field ?? _base?.Y; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Width { get => field ?? _base?.Width; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? Height { get => field ?? _base?.Height; set => SetValue(ref field, value, [nameof(Rect)]); }
        public ValueExpression? DivX { get => field ?? _base?.DivX; set => SetValue(ref field, value, [nameof(Division)]); }
        public ValueExpression? DivY { get => field ?? _base?.DivY; set => SetValue(ref field, value, [nameof(Division)]); }
        public ValueExpression? LoopPeriod { get => field ?? _base?.LoopPeriod; set => SetValue(ref field, value); }

        public override string ToString() => $"{nameof(Texture)}{{Key={Key}, {(string.IsNullOrEmpty(BasedOn) ? "" : $"BasedOn={BasedOn}, ")}Source={Source}, Rect=({X}, {Y}, {Width}, {Height}), Div=({DivX}, {DivY}), LoopPeriod={LoopPeriod}}}";

        public string Rect { get => this.GetRectText(); set => this.SetRectText(value); }

        public string Division
        {
            get => $"{DivX}, {DivY}";
            set
            {
                StringConversion.GetTuple(value, out var v1, out var v2);
                DivX = v1;
                DivY = v2;
            }
        }

        internal Texture? _base;
        internal bool IsCircularReference()
        {
            var parent = _base;
            while (parent is not null)
            {
                if (ReferenceEquals(parent, this))
                {
                    return true;
                }
                parent = parent._base;
            }
            return false;
        }
        internal string? _baseDirectory;
    }
}
