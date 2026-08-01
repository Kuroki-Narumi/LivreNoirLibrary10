using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    partial class ControlExtensions
    {
        [UnsafeAccessor(UnsafeAccessorKind.Method)]
        public static extern bool SetSelectedItems(this ListBox @this, IEnumerable items);

        extension(Viewbox)
        {
            [UnsafeAccessor(UnsafeAccessorKind.StaticMethod)]
            private static extern Size ComputeScaleFactor(Viewbox? _, Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection);

            public static Size ComputeScaleFactor(Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
                => ComputeScaleFactor(null, availableSize, contentSize, stretch, stretchDirection);
        }
    }
}
