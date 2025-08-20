using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Lbm
{
    public interface ILinkable<TSelf> : IJsonWriter
        where TSelf : ILinkable<TSelf>
    {
        public static abstract TSelf Open(string uri);
        public static abstract bool TryParse(ref Utf8JsonReader reader, JsonSerializerOptions options, [MaybeNullWhen(false)]out TSelf self);
    }
}
