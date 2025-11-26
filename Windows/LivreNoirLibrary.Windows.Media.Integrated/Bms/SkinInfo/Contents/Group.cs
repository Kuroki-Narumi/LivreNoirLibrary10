using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class Group : SkinElement
    {
        public bool ClipToBounds { get; set => SetValue(ref field, value); }
    }
}
