using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Media.VectorGraphics;

namespace LivreNoirLibrary.YuGiOh.Data
{
    using LivreNoirLibrary.YuGiOh.Media;

    public partial class Card
    {
        public ElementGroup Icon => Icons.GetCardIcon(this.GetFrameType());
        public ElementGroup AttributeIcon => Icons.GetAttributeIcon(Attribute);
        public ElementGroup? TunerIcon => this.IsTuner() ? Icons.TunerIcon : null;
        public ElementGroup LinkIcon => this.IsLink() ? Icons.GetLinkIcon(this.GetLinkDirections()) : Icon;
        public IBrush FrameBrush => Icons.GetFrameBrush(this.GetFrameType());
    }
}
