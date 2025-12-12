using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Judge : SkinElement
    {
        public string? Perfect { get; set => SetValue(ref field, value); }
        public string? PerfectCombo { get; set => SetValue(ref field, value); }
        public string? Great { get; set => SetValue(ref field, value); }
        public string? GreatCombo { get; set => SetValue(ref field, value); }
        public string? Good { get; set => SetValue(ref field, value); }
        public string? GoodCombo { get; set => SetValue(ref field, value); }
        public string? Bad { get; set => SetValue(ref field, value); }
        public string? BadCombo { get; set => SetValue(ref field, value); }
        public string? Through { get; set => SetValue(ref field, value); }
        public string? ThroughCombo { get; set => SetValue(ref field, value); }
        public string? BlankShot { get; set => SetValue(ref field, value); }
        public string? BlankShotCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? Padding { get; set => SetValue(ref field, value); }
    }
}
