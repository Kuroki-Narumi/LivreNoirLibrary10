using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class Judge : SkinElement
    {
        public ValueExpression? Perfect { get; set => SetValue(ref field, value); }
        public ValueExpression? PerfectCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? Great { get; set => SetValue(ref field, value); }
        public ValueExpression? GreatCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? Good { get; set => SetValue(ref field, value); }
        public ValueExpression? GoodCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? Bad { get; set => SetValue(ref field, value); }
        public ValueExpression? BadCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? Through { get; set => SetValue(ref field, value); }
        public ValueExpression? ThroughCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? BlankShot { get; set => SetValue(ref field, value); }
        public ValueExpression? BlankShotCombo { get; set => SetValue(ref field, value); }
        public ValueExpression? Padding { get; set => SetValue(ref field, value); }
    }
}
