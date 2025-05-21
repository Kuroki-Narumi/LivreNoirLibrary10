using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public readonly struct Marker(long position, string? name)
    {
        public const string LoopStartName = "LoopStart";
        public const string LoopEndName = "LoopEnd";
        public const string IgnoreName = "*tr*";

        public readonly long Position = position;
        public readonly string? Name = name;

        public static implicit operator Marker((long, string?) tuple) => new(tuple.Item1, tuple.Item2);
        public static implicit operator Marker(KeyValuePair<long, string?> kv) => new(kv.Key, kv.Value);
        public static implicit operator (long, string?)(Marker marker) => (marker.Position, marker.Name);
        public static implicit operator KeyValuePair<long, string?>(Marker marker) => new(marker.Position, marker.Name);

        public void Deconstruct(out long position, out string? name)
        {
            position = Position;
            name = Name;
        }
    }
}
