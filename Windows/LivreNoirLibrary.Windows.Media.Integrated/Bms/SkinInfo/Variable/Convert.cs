using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public sealed class Convert : SkinNode
    {
        public string From { get; set => SetValue(ref field, value); } = "";
        public string? To { get; set => SetValue(ref field, value); }
    }
}
