using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.ObjectModel;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Media
{
    public static partial class Icons
    {
        public static readonly SingleColorBrush Link_On_Fill = new("#f00");
        public static readonly SingleColorBrush Link_On_Stroke = new("#c04040");
        public static readonly SingleColorBrush Link_Off_Fill = new("#80808080");
        public static readonly SingleColorBrush Link_Off_Stroke = new("#c0404040");

        private static readonly Dictionary<LinkDirection, ElementGroup> _link_icons = [];

        public static ElementGroup GetLinkIcon(LinkDirection dir)
        {
            if (!_link_icons.TryGetValue(dir, out var icon))
            {
                icon = new([
                        new(GetLinkGeometry(~dir, 32, 32), Link_Off_Stroke),
                        new(GetLinkGeometry(dir, 32, 32), Link_On_Fill),
                    ]);
                _link_icons.Add(dir, icon);
            }
            return icon;
        }

        public static string GetLinkGeometry(LinkDirection dir, double width, double height)
        {
            using var o = ObjectPool.RentStringBuilder(out var sb);
            foreach (var (refDir, geometry) in new LinkArrowEnumerator(width, height))
            {
                if ((dir & refDir) is not 0)
                {
                    sb.Append(geometry);
                }
            }
            return sb.ToString();
        }
    }
}
