using System;

namespace LivreNoirLibrary.Media.Midi
{
    public static class MetaTypeExtensions
    {
        public static bool IsMetaText(this MetaType type) => type is >= MetaType.Text and <= MetaType.Device;
    }
}
