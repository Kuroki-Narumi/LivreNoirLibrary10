using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Windows.Win32.Foundation;

namespace LivreNoirLibrary.Win32Api
{
    partial class NativeMethods
    {
        internal static Rectangle ToRectangle(this RECT rect) => new(rect.left, rect.top, rect.Width, rect.Height);
    }
}
