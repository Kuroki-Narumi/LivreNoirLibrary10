using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Media
{
    public static partial class Icons
    {
        public static ElementGroup TunerIcon { get; } = CreateTunerIcon();

        private const string _tunerGeometry =
            "M10,4 v12 a4,4,0,0,0,4,4 v4 h-2 v4 h8 v-4 h-2 v-4 a4,4,0,0,0,4,-4 V4 h-4 v12 h-4 v-12 Z M8,8 a4,8,0,0,0,0,16 a4,12,0,0,1,0,-16 Z M24,8 a4,8,0,0,1,0,16 a4,12,0,0,0,0,-16 Z";

        private static ElementGroup CreateTunerIcon()
        {
            GeometryElement back = new(Geometries.Circle_16, CreateAttrBrush("#0ff", "#0aa", "#044"));
            GeometryElement front = new(_tunerGeometry, new SingleColorBrush("#fff"));
            ElementGroup g = new(back, front);
            return g;
        }
    }
}
