using System;
using System.Windows;

namespace LivreNoirLibrary.Media
{
    public readonly record struct TextureCacheKey(string Path, Int32Rect Rect);
}
