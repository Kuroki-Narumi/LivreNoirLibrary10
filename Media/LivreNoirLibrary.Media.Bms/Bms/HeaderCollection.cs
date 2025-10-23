using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Bms
{
    public class HeaderCollection : IJsonWriter, IHeaderCollection, IDumpable, ILoadable<HeaderCollection>
    {
        internal readonly Dictionary<HeaderType, double> _doubleValues = [];
        internal readonly Dictionary<HeaderType, string> _stringValues = [];
        internal readonly List<(string Key, string Value)> _sub = [];

        public List<(string Key, string Value)> SubHeaders => _sub;

        public bool HasValue => _doubleValues.Count is not 0 || _stringValues.Count is not 0 || _sub.Count is not 0;

        public void Clear()
        {
            _doubleValues.Clear();
            _stringValues.Clear();
            _sub.Clear();
        }

        public void SetDefault()
        {
            Clear();
            _stringValues[HeaderType.Title] = Constants.DefaultTitle;
            _doubleValues[HeaderType.Player] = (double)Constants.DefaultPlayer;
            _doubleValues[HeaderType.Bpm] = Constants.DefaultBpm;
            _stringValues[HeaderType.PlayLevel] = Constants.DefaultPlayLevel;
            _stringValues[HeaderType.Difficulty] = Constants.DefaultDifficulty;
            _doubleValues[HeaderType.Rank] = (double)Constants.DefaultRank;
            _doubleValues[HeaderType.Total] = Constants.DefaultTotal;
            _stringValues[HeaderType.StageFile] = Constants.DefaultStageFile;
            _stringValues[HeaderType.Banner] = Constants.DefaultBanner;
            _stringValues[HeaderType.BackBmp] = Constants.DefaultBackBmp;
            _stringValues[HeaderType.Preview] = Constants.DefaultPreview;
        }

        public bool TryGetNumber(HeaderType type, out double value) => _doubleValues.TryGetValue(type, out value);

        public bool TryGetInt(HeaderType type, out int value)
        {
            if (_doubleValues.TryGetValue(type, out var dValue))
            {
                value = (int)dValue;
                return true;
            }
            value = default;
            return false;
        }

        public bool TryGetEnum<T>(HeaderType type, out T value)
            where T : struct, Enum
        {
            if (_doubleValues.TryGetValue(type, out var dVal))
            {
                value = (T)Enum.ToObject(typeof(T), (long)dVal);
                return true;
            }
            value = default;
            return false;
        }

        public bool TryGetText(HeaderType type, [MaybeNullWhen(false)]out string value) => _stringValues.TryGetValue(type, out value);

        public bool Remove(HeaderType type) => _doubleValues.Remove(type) || _stringValues.Remove(type);

        public void Set(HeaderType type, double value)
        {
            if (BmsUtils.IsNumberHeader(type))
            {
                _doubleValues[type] = value;
            }
            else
            {
                _stringValues[type] = value.ToString();
            }
        }

        public void Set<T>(HeaderType type, T value) where T : struct, Enum => Set(type, (value as IConvertible).ToDouble(null));

        public void Set(HeaderType type, string value)
        {
            if (BmsUtils.IsNumberHeader(type))
            {
                if (double.TryParse(value, out var result))
                {
                    _doubleValues[type] = result;
                }
            }
            else
            {
                _stringValues[type] = value;
            }
        }

        public IEnumerable<(string, string)> EnumerateHeaders(int radix = 0)
        {
            for (var t = HeaderType.Player; t is <= HeaderType.Comment; t++)
            {
                var key = t.ToString().ToUpper();
                if (BmsUtils.IsNumberHeader(t))
                {
                    if (_doubleValues.TryGetValue(t, out var value))
                    {
                        if (radix is not 0 && t is HeaderType.LnObj)
                        {
                            yield return (key, BmsUtils.ToBased((int)value, radix));
                        }
                        else
                        {
                            yield return (key, value.ToString());
                        }
                    }
                }
                else if (_stringValues.TryGetValue(t, out var value))
                {
                    yield return (key, value);
                }
            }
            foreach (var (key, value) in _sub)
            {
                yield return (key.ToUpper(), value);
            }
            if (radix is > Constants.Base_Default)
            {
                yield return ("BASE", radix.ToString());
            }
        }

        public void TryEncode(Encoding encoding)
        {
            foreach (var (key, value) in EnumerateHeaders())
            {
                encoding.GetByteCount(key);
                encoding.GetByteCount(value);
            }
        }

        public void Dump(BmsTextWriter writer, bool isRoot)
        {
            var radix = writer.Radix;
            foreach (var (key, value) in EnumerateHeaders(radix))
            {
                writer.WriteLine($"#{key} {value}");
            }
            if (isRoot)
            {
                writer.WriteLine();
            }
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_doubleValues.Count);
            foreach (var (key, value) in _doubleValues)
            {
                writer.Write((byte)key);
                writer.Write(value);
            }
            writer.Write(_stringValues.Count);
            foreach (var (key, value) in _stringValues)
            {
                writer.Write((byte)key);
                writer.Write(value);
            }
            writer.Write(_sub.Count);
            foreach (var (key, value) in _sub)
            {
                writer.Write(key);
                writer.Write(value);
            }
        }

        public static HeaderCollection Load(BinaryReader reader)
        {
            HeaderCollection result = new();
            result.ProcessLoad(reader);
            return result;
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            var doubles = _doubleValues;
            var strings = _stringValues;
            var sub = _sub;
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = (HeaderType)reader.ReadByte();
                var value = reader.ReadDouble();
                doubles[key] = value;
            }
            count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = (HeaderType)reader.ReadByte();
                var value = reader.ReadString();
                strings[key] = value;
            }
            count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = reader.ReadString();
                var value = reader.ReadString();
                sub.Add((key, value));
            }
        }

        public void Merge(HeaderCollection src)
        {
            foreach (var (k, v) in src._doubleValues)
            {
                _doubleValues[k] = v;
            }
            foreach (var (k, v) in src._stringValues)
            {
                _stringValues[k] = v;
            }
            foreach (var header in CollectionsMarshal.AsSpan(src._sub))
            {
                _sub.Add(header);
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var (key, value) in EnumerateHeaders())
            {
                writer.WriteStringValue($"#{key} {value}");
            }
            writer.WriteEndArray();
        }
    }
}
