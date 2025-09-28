using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public abstract class ScreenElementBase
    {
        public abstract void Render(DrawingContext drawingContext, long currentTick);
    }
}
