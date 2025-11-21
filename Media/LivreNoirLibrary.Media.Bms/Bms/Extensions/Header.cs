using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension (SortedDictionary<HeaderType, string> headers)
        {
            public void SetDefault()
            {
                headers.Clear();
                headers.Set(HeaderType.Title, BmsConstants.DefaultTitle);
                headers.Set(HeaderType.Player, BmsConstants.DefaultPlayer);
                headers.Set(HeaderType.Bpm, BmsConstants.DefaultBpm);
                headers.Set(HeaderType.PlayLevel, BmsConstants.DefaultPlayLevel);
                headers.Set(HeaderType.Difficulty, BmsConstants.DefaultDifficulty);
                headers.Set(HeaderType.Rank, BmsConstants.DefaultRank);
                headers.Set(HeaderType.Total, BmsConstants.DefaultTotal);
                headers.Set(HeaderType.StageFile, BmsConstants.DefaultStageFile);
                headers.Set(HeaderType.Banner, BmsConstants.DefaultBanner);
                headers.Set(HeaderType.BackBmp, BmsConstants.DefaultBackBmp);
                headers.Set(HeaderType.Preview, BmsConstants.DefaultPreview);
            }

            public void Set(HeaderType type, string value) => headers[type] = value;
            public void Set(HeaderType type, double value) => headers[type] = value.ToString();
            public void Set<T>(HeaderType type, T value) where T : struct, Enum => headers[type] = Convert.ToInt32(value).ToString();

            public bool TryGetDouble(HeaderType type, out double value)
            {
                if (headers.TryGetValue(type, out var v) && double.TryParse(v, out value))
                {
                    return true;
                }
                value = default;
                return false;
            }

            public bool TryGetInt(HeaderType type, out int value)
            {
                if (headers.TryGetValue(type, out var v) && int.TryParse(v, out value))
                {
                    return true;
                }
                value = default;
                return false;
            }

            public bool TryGetEnum<T>(HeaderType type, out T value)
                where T : struct, Enum
            {
                if (headers.TryGetValue(type, out var v) && Enum.TryParse<T>(v, out value))
                {
                    return true;
                }
                value = default;
                return false;
            }

            public void Dump(BinaryWriter writer)
            {
                writer.Write(headers.Count);
                foreach (var (key, value) in headers)
                {
                    writer.Write((byte)key);
                    writer.Write(value);
                }
            }

            public void ProcessLoad(BinaryReader reader)
            {
                headers.Clear();
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var key = (HeaderType)reader.ReadByte();
                    var value = reader.ReadString();
                    headers[key] = value;
                }
            }

            public void TryEncode(Encoding encoding)
            {
                foreach (var (_, value) in headers)
                {
                    encoding.GetByteCount(value);
                }
            }

            public void Dump(BmsTextWriter writer)
            {
                foreach (var (key, value) in headers)
                {
                    writer.WriteLine($"#{key.ToString().ToUpper()} {value}");
                }
            }
        }

        extension (List<Header> headers)
        {
            public void Merge(List<Header> source)
            {
                foreach (var header in source.AsSpan())
                {
                    var key = header.Key;
                    var index = headers.FindIndex(header => string.Equals(header.Key, key, StringComparison.OrdinalIgnoreCase));
                    if (index is >= 0)
                    {
                        headers[index].Value = header.Value;
                    }
                    else
                    {
                        headers.Add(header);
                    }
                }
            }

            public void Dump(BinaryWriter writer)
            {
                writer.Write(headers.Count);
                foreach (var (key, value) in headers.AsSpan())
                {
                    writer.Write(key);
                    writer.Write(value);
                }
            }

            public void ProcessLoad(BinaryReader reader)
            {
                var c = headers.Count;
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var key = reader.ReadString();
                    var value = reader.ReadString();
                    if (i < c)
                    {
                        var header = headers[i];
                        header.Key = key;
                        header.Value = value;
                    }
                    else
                    {
                        headers.Add(new(key, value));
                    }
                }
                if (count < c)
                {
                    headers.RemoveRange(count, c - count);
                }
            }

            public void TryEncode(Encoding encoding)
            {
                foreach (var (key, value) in headers.AsSpan())
                {
                    encoding.GetByteCount(key);
                    encoding.GetByteCount(value);
                }
            }

            public void Dump(BmsTextWriter writer)
            {
                foreach (var (key, value) in headers.AsSpan())
                {
                    writer.WriteLine($"#{key} {value}");
                }
            }
        }
    }
}
