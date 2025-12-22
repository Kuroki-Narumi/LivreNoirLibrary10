using LivreNoirLibrary.Collections;
using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public sealed class LaneDefinitionCollection : StringKeyCollection<LaneDefinition>
    {
        protected override string GetKey(LaneDefinition item) => item.Channel;
    }
}
