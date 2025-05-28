using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace LivreNoirLibrary.YuGiOh.Controls
{
    public static partial class Icons
    {
        private static DrawingImage CreateTunerIcon()
        {
            DrawingGroup dg = new();
            using (var ctx = dg.Open())
            {
                double s = IconWidth / 2;
                //ctx.DrawEllipse(Brush_CardFrame, null, new(s, s), s, s);
                ctx.DrawEllipse(CreateAttrBrush("#0ff", "#0aa", "#044"), null, new(s, s), s, s);
                ctx.DrawGeometry(Brushes.White, null, CreateGeometry("M5,2 v6 a2,2,0,0,0,2,2 v2 h-1 v2 h4 v-2 h-1 v-2 a2,2,0,0,0,2,-2 V2 h-2 v6 h-2 v-6 Z M4,4 a2,4,0,0,0,0,8 a2,6,0,0,1,0,-8 Z M12,4 a2,4,0,0,1,0,8 a2,6,0,0,0,0,-8 Z"));
            }
            dg.Freeze();
            DrawingImage di = new(dg);
            di.Freeze();
            return di;
        }

        public static DrawingImage TunerIcon { get; } = CreateTunerIcon();
    }
}
