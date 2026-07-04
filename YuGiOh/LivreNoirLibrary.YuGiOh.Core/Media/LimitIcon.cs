using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Media
{
    public static partial class Icons
    {
        public static ElementGroup? GetLimitIcon(int limit) => _limit_icons.GetValueOrDefault(limit);

        private static readonly Dictionary<int, ElementGroup> _limit_icons = new()
        {
            { LimitCount.Unusable, CreateLimitIcon(
                "M2,0 l-2,2 l6,6 l-6,6 l2,2 l6,-6 l6,6 l2,-2 l-6,-6 l6,-6 l-2,-2 l-6,6 Z", "#800080") },
            { LimitCount.Forbidden, CreateLimitIcon(
                "M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M4.877,2.877 A6,6,0,0,1,13.123,11.123 Z M2.877,4.877 A6,6,0,0,0,11.123,13.123 Z", "#e00000") },
            { LimitCount.Limit1, CreateLimitIcon(
                "M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M8,2 a6,6,0,0,0,0,12 a6,6,0,0,0,0,-12 Z M7,3 h2 v10 h-2 Z", "#C06000") },
            { LimitCount.Limit2, CreateLimitIcon(
                "M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M8,2 a6,6,0,0,0,0,12 a6,6,0,0,0,0,-12 Z M5,3 h2 v10 h-2 Z M9,3 h2 v10 h-2 Z", "#808000") },
            { LimitCount.Specified, CreateLimitIcon(
                "M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M8,2 a6,6,0,0,0,0,12 a6,6,0,0,0,0,-12 Z M8,4 a4,4,0,0,0,0,8 a4,4,0,0,0,0,-8 Z M8,6 a2,2,0,0,0,0,4 a2,2,0,0,0,0,-4 Z", "#0000C0") },
        };

        private static ElementGroup CreateLimitIcon(string geometry, string color) => new([new GeometryElement(geometry, new SingleColorBrush(color))]);
    }
}
