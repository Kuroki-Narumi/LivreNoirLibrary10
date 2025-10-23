using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public sealed class ConvertCollection : StringKeyCollection<Convert>
    {
        protected override string GetKey(Convert item) => item.From;
    }
}
