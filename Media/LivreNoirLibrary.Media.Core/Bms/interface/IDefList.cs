using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDefList : ICount, IEnumerable<(short, string)>, IDumpable, ILoadable, IClear
    {
        int MaxIndex { get; }
        IEnumerable<short> Keys { get; }
        bool ContainsKey(short key);
        bool TryGetValue(short key, [MaybeNullWhen(false)] out string value);
        bool TryGetKey(string value, out short key);
        void Set(short key, string? value);
        bool Remove(short key);

        void IDumpable.Dump(BinaryWriter writer)
        {
            writer.Write(Count);
            foreach (var (key, value) in this)
            {
                writer.Write(key);
                writer.Write(value);
            }
        }

        void ILoadable.ProcessLoad(BinaryReader reader)
        {
            Clear();
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = reader.ReadInt16();
                var value = reader.ReadString();
                Set(key, value);
            }
        }
    }
}
