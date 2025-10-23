using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public abstract class OptionBase : SkinNode, IKeyNode
    {
        public string Key { get; set => SetValue(ref field, value); } = "";
        public abstract string? SelectedValue { get; }
    }
}
