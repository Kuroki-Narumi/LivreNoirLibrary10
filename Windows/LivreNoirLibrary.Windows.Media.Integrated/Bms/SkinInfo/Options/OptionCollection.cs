using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public sealed class OptionCollection : KeyNodeCollection<OptionBase>
    {
        protected override string GetKey(OptionBase item) => item is Separator ? item.GetHashCode().ToString() : base.GetKey(item);
    }
}
