using System;

namespace LivreNoirLibrary.Media.Midi
{
    public static class IObjectExtensions
    {
        public static string GetIdentifier(this IObject obj) => $"{obj.ObjectName}{obj.ContentString}";
    }
}
