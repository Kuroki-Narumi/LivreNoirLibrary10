using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.IO;
using System.Runtime.InteropServices;
using System.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public class HeaderCollection : IEnumerable<HeaderBase>, IJsonWriter, IDumpable<HeaderCollection>
    {
        public HeaderCollection? Parent { get; set; }

        private readonly SortedDictionary<HeaderType, TypedHeader> _main = [];
        private readonly List<Header> _sub = [];

        public List<Header> SubHeaders => _sub;

        public bool IsEmpty() => _main.Count is 0 && _sub.Count is 0;

        public void Clear()
        {
            _main.Clear();
            _sub.Clear();
        }

        public bool IsDefined(HeaderType type) => _main.ContainsKey(type);

        public string? Get(HeaderType type)
        {
            return _main.TryGetValue(type, out var header) ? header.Value : null;
        }

        public string? GetParent(HeaderType type)
        {
            if (Parent is not null)
            {
                return Parent.Get(type) ?? Parent.GetParent(type);
            }
            return null;
        }

        public string? GetInherited(HeaderType type)
        {
            if (_main.TryGetValue(type, out var h))
            {
                return h.Value;
            }
            return Parent?.GetInherited(type);
        }

        public void Set(HeaderType type, string? value)
        {
            if (string.IsNullOrEmpty(value) || (GetParent(type) == value))
            {
                _main.Remove(type);
            }
            else if (_main.TryGetValue(type, out var h))
            {
                h.Value = value;
            }
            else
            {
                _main.Add(type, new(type, value));
            }
        }

        public void Set<T>(HeaderType type, T value)
            where T : struct
        {
            if (value is Enum e)
            {
                Set(type, Convert.ToInt32(e).ToString());
            }
            else
            {
                Set(type, value.ToString());
            }
        }

        public void Add(HeaderType type, string value)
        {
            if (_main.TryGetValue(type, out var h))
            {
                h.AddValue(value);
            }
            else
            {
                _main.Add(type, new(type, value));
            }
        }

        internal void Add(string type, string value)
        {
            if (HeaderBase.TryGetType(type, out var t))
            {
                Add(t, value);
            }
            else
            {
                _sub.Add(new(type, value));
            }
        }

        public bool Remove(HeaderType type) => _main.Remove(type);

        public string GetString(HeaderType type, string defaultValue = "") => GetInherited(type) ?? defaultValue;
        public T GetNumber<T>(HeaderType type, T defaultValue = default) where T : struct, INumber<T> => GetNumber(GetInherited(type), defaultValue);
        public T GetEnum<T>(HeaderType type, T defaultValue = default) where T : struct, Enum => GetEnum(GetInherited(type), defaultValue);

        public static T GetNumber<T>(string? text, T defaultValue = default)
            where T : struct, INumber<T>
        {
            if (!string.IsNullOrEmpty(text) && T.TryParse(text, null, out var v))
            {
                return v;
            }
            return defaultValue;
        }

        public static T GetEnum<T>(string? text, T defaultValue = default)
            where T : struct, Enum
        {
            if (!string.IsNullOrEmpty(text) && Enum.TryParse<T>(text, out var v))
            {
                return v;
            }
            return defaultValue;
        }

        public IEnumerator<HeaderBase> GetEnumerator()
        {
            foreach (var (_, h) in _main)
            {
                yield return h;
            }
            var c = _sub.Count;
            for (int i = 0; i < c; i++)
            {
                yield return _sub[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void Dump(RawData.BmsTextWriter writer)
        {
            foreach (var (_, h) in _main)
            {
                if (Parent is null || Parent.Get(h.Type) != h.Value)
                {
                    h.Dump(writer);
                }
            }
            foreach (var h in CollectionsMarshal.AsSpan(_sub))
            {
                h.Dump(writer);
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_main.Count + _sub.Count);
            foreach (var header in this)
            {
                header.Dump(writer);
            }
        }

        public static HeaderCollection Load(BinaryReader reader)
        {
            HeaderCollection result = [];
            result.ProcessLoad(reader);
            return result;
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var (k, v) = HeaderBase.LoadKV(reader);
                Add(k, v);
            }
        }

        public void Merge(HeaderCollection src)
        {
            foreach (var (k, v) in src._main)
            {
                _main[k] = v;
            }
            foreach (var header in CollectionsMarshal.AsSpan(src._sub))
            {
                _sub.Add(header);
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var h in this)
            {
                h.WriteJson(writer, options);
            }
            writer.WriteEndArray();
        }
    }
}
