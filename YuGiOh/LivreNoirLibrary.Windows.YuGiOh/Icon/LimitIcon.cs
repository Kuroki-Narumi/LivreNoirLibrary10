using System;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static partial class Icons
    {
        private static readonly Dictionary<int, GeometryDrawing> _limit_icons = new()
        {
            { LimitCount.Unusable, CreateLimitIcon("M2,0 l-2,2 l6,6 l-6,6 l2,2 l6,-6 l6,6 l2,-2 l-6,-6 l6,-6 l-2,-2 l-6,6 Z", 128, 0, 128) },
            { LimitCount.Forbidden, CreateLimitIcon("M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M4.877,2.877 A6,6,0,0,1,13.123,11.123 Z M2.877,4.877 A6,6,0,0,0,11.123,13.123 Z", 224, 0, 0) },
            { LimitCount.Limit1, CreateLimitIcon("M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M8,2 a6,6,0,0,0,0,12 a6,6,0,0,0,0,-12 Z M7,3 h2 v10 h-2 Z", 192, 96, 0) },
            { LimitCount.Limit2, CreateLimitIcon("M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M8,2 a6,6,0,0,0,0,12 a6,6,0,0,0,0,-12 Z M5,3 h2 v10 h-2 Z M9,3 h2 v10 h-2 Z", 128, 128, 0) },
            { LimitCount.Specified, CreateLimitIcon("M8,0 a8,8,0,0,0,0,16 a8,8,0,0,0,0,-16 Z M8,2 a6,6,0,0,0,0,12 a6,6,0,0,0,0,-12 Z M8,4 a4,4,0,0,0,0,8 a4,4,0,0,0,0,-8 Z M8,6 a2,2,0,0,0,0,4 a2,2,0,0,0,0,-4 Z", 0, 0, 192) },
        };

        private static GeometryDrawing CreateLimitIcon(string data, byte r, byte g, byte b)
        {
            GeometryDrawing dr = new(MediaUtils.GetBrush(255, r, g, b), null, MediaUtils.CreateGeometry(data));
            dr.Freeze();
            return dr;
        }

        public static GeometryDrawing GetLimitIcon(int limit) => _limit_icons[limit];
        public static GeometryDrawing? GetLimitIconOrNull(int limit) => _limit_icons.TryGetValue(limit, out var icon) ? icon : null;
    }
}
