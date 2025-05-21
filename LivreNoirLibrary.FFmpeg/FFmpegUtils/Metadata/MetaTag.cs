using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly record struct MetaTag(string Key, string? Value)
    {
        public static implicit operator MetaTag((string, string?) tuple) => new(tuple.Item1, tuple.Item2);
        public static implicit operator MetaTag(KeyValuePair<string, string?> kv) => new(kv.Key, kv.Value);
        public static implicit operator (string, string?)(MetaTag tag) => (tag.Key, tag.Value);
        public static implicit operator KeyValuePair<string, string?>(MetaTag tag) => new(tag.Key, tag.Value);

        public void Deconstruct(out string key, out string? value)
        {
            key = Key;
            value = Value;
        }

        public override string ToString() => $"{Key}={Value}";
    }
}
