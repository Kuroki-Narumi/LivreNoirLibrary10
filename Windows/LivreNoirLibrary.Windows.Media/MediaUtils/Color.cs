using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class MediaUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt(this Color color) => ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | ((uint)color.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(this uint value) => unchecked(Color.FromArgb((byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value));
    }
}
