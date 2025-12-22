using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public abstract class OptionBase : SkinNode, IKeyNode
    {
        public string Key { get; set => SetValue(ref field, value); } = "";
        public abstract string? SelectedValue { get; }
        public string? ToolTip { get; set => SetValue(ref field, value); }
        public string? Suffix { get; set => SetValue(ref field, value); }

        public abstract void SetDefaultValue();
        public abstract void SetValue(string value);

        public override string ToString() => $"{GetType().Name}{{Key={Key}, SelectedValue={SelectedValue}}}";
    }
}
