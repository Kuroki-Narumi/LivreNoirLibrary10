using System;
using System.Windows.Media;

namespace LivreNoirLibrary.YuGiOh.Controls
{
    public static partial class Icons
    {
        public const int IconWidth = 16;
        public const int IconHeight = 16;

        private static Geometry CreateGeometry(string data)
        {
            var g = Geometry.Parse(data);
            g.Freeze();
            return g;
        }
    }
}
