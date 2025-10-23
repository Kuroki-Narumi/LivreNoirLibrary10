using System;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public partial class PlaySkin : Skin
    {
        public Int32Collection KeyCount { get; set => SetValue(ref field, value); } = [];
    }
}
