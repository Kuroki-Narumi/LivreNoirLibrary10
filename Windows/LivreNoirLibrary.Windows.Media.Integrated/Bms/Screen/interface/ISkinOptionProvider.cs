using System;
using System.Collections.Generic;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface ISkinOptionProvider
    {
        IDictionary<string, string>? GetSkinOptions(Skin? skin);
    }
}
