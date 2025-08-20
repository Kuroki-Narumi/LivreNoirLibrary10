using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public class HeaderCollection : IJsonWriter, IDumpable<HeaderCollection>
    {
        public HeaderCollection? Parent { get; set; }

        private readonly HashSet<HeaderType> _double_headers = [
            HeaderType.Player, HeaderType.Bpm, HeaderType.PlayLevel, HeaderType.Rank, HeaderType.Total, HeaderType.LnObj, HeaderType.LnMode, HeaderType.DefExRank
            ];

        private readonly SortedDictionary<HeaderType, double> _doubleValues = [];
        private readonly SortedDictionary<HeaderType, string> _stringValues = [];
        private readonly List<(string Key, string Value)> _sub = [];

        public PlayerType Player { get => (PlayerType)GetInt(HeaderType.Player, (int)Constants.DefaultPlayer); set => SetInternal(HeaderType.Player, (double)value); }
        public string? Genre { get => GetString(HeaderType.Genre); set => SetInternal(HeaderType.Genre, value); }
        public string? Title { get => GetString(HeaderType.Title); set => SetInternal(HeaderType.Title, value); }
        public string? SubTitle { get => GetString(HeaderType.SubTitle); set => SetInternal(HeaderType.SubTitle, value); }
        public string? Artist { get => GetString(HeaderType.Artist); set => SetInternal(HeaderType.Artist, value); }
        public string? SubArtist { get => GetString(HeaderType.SubArtist); set => SetInternal(HeaderType.SubArtist, value); }
        public double Bpm { get => GetDouble(HeaderType.Bpm, Constants.DefaultBpm); set => SetInternal(HeaderType.Bpm, value); }
        public int PlayLevel { get => GetInt(HeaderType.PlayLevel, Constants.DefaultLevel); set => SetInternal(HeaderType.PlayLevel, value); }
        public string? Difficulty { get => GetString(HeaderType.Difficulty); set => SetInternal(HeaderType.Difficulty, value); }
        public Rank Rank { get => (Rank)GetInt(HeaderType.Rank, (int)Constants.DefaultRank); set => SetInternal(HeaderType.Rank, (double)value); }
        public double Total { get => GetDouble(HeaderType.Total, Constants.DefaultTotal); set => SetInternal(HeaderType.Total, value); }
        public string? StageFile { get => GetString(HeaderType.StageFile); set => SetInternal(HeaderType.StageFile, value); }
        public string? Banner { get => GetString(HeaderType.Banner); set => SetInternal(HeaderType.Banner, value); }
        public string? BackBmp { get => GetString(HeaderType.BackBmp); set => SetInternal(HeaderType.BackBmp, value); }
        public string? Preview { get => GetString(HeaderType.Preview); set => SetInternal(HeaderType.Preview, value); }
        public int LnObj { get => GetInt(HeaderType.LnObj); set => SetInternal(HeaderType.LnObj, value); }
        public LongNoteMode LnMode { get => (LongNoteMode)GetInt(HeaderType.LnMode, (int)Constants.DefaultLnMode); set => SetInternal(HeaderType.LnMode, (double)value); }
        public double ExRank { get => GetDouble(HeaderType.DefExRank, Constants.DefaultExRank); set => SetInternal(HeaderType.DefExRank, value); }
        public string? Comment { get => GetString(HeaderType.Comment); set => SetInternal(HeaderType.Comment, value); }

        public List<(string Key, string Value)> SubHeaders => _sub;

        public bool IsEmpty() => _doubleValues.Count is 0 && _stringValues.Count is 0 && _sub.Count is 0;

        public void Clear()
        {
            _doubleValues.Clear();
            _stringValues.Clear();
            _sub.Clear();
        }

        public void SetDefault()
        {
            Clear();
            Title = "(untitled)";
            Player = Constants.DefaultPlayer;
            Bpm = Constants.DefaultBpm;
            PlayLevel = Constants.DefaultLevel;
            Difficulty = Constants.DefaultDifficulty;
            Rank = Constants.DefaultRank;
            SetInternal(HeaderType.Total, Constants.DefaultTotal);
            StageFile = Constants.DefaultStageFile;
            Banner = Constants.DefaultBanner;
            BackBmp = Constants.DefaultBackBmp;
        }

        public int GetInt(HeaderType type, int ifNone = 0, Inheritance inheritance = Inheritance.Inherited)
        {
            if (inheritance is not Inheritance.Parent && _doubleValues.TryGetValue(type, out var value))
            {
                return (int)value;
            }
            if (inheritance is not Inheritance.Actual && Parent is not null)
            {
                return Parent.GetInt(type, ifNone, inheritance);
            }
            return ifNone;
        }

        public double GetDouble(HeaderType type, double ifNone = 0, Inheritance inheritance = Inheritance.Inherited)
        {
            if (inheritance is not Inheritance.Parent && _doubleValues.TryGetValue(type, out var value))
            {
                return value;
            }
            if (inheritance is not Inheritance.Actual && Parent is not null)
            {
                return Parent.GetDouble(type, ifNone, inheritance);
            }
            return ifNone;
        }

        public string? GetString(HeaderType type, Inheritance inheritance = Inheritance.Inherited)
        {
            if (inheritance is not Inheritance.Parent && _stringValues.TryGetValue(type, out var value))
            {
                return value;
            }
            if (inheritance is not Inheritance.Actual && Parent?.GetString(type, inheritance) is string parent)
            {
                return parent;
            }
            return null;
        }

        private void SetInternal(HeaderType type, double value)
        {
            if (value is < 0 || (GetDouble(type, 0, Inheritance.Parent) == value))
            {
                _doubleValues.Remove(type);
            }
            else
            {
                _doubleValues[type] = value;
            }
        }

        private void SetInternal(HeaderType type, string? value)
        {
            if (string.IsNullOrEmpty(value) || (GetString(type, Inheritance.Parent) == value))
            {
                _stringValues.Remove(type);
            }
            else
            {
                _stringValues[type] = value;
            }
        }

        internal void SetAuto(string key, string value)
        {
            if (Enum.TryParse<HeaderType>(key, true, out var type))
            {
                if (_double_headers.Contains(type))
                {
                    if (double.TryParse(value, out var v))
                    {
                        _doubleValues[type] = v;
                    }
                }
                else
                {
                    if (_stringValues.TryGetValue(type, out var current))
                    {
                        _stringValues[type] = $"{current}, {value}";
                    }
                    else
                    {
                        _stringValues.Add(type, value);
                    }
                }
            }
            else
            {
                _sub.Add((key, value));
            }
        }

        internal void Dump(RawData.BmsTextWriter writer, int radix)
        {
            for (var t = HeaderType.Player; t is <= HeaderType.Comment; t++)
            {
                var key = t.ToString().ToUpper();
                if (_double_headers.Contains(t))
                {
                    if (_doubleValues.TryGetValue(t, out var value) && GetDouble(t, 0, Inheritance.Parent) != value)
                    {
                        if (t is HeaderType.LnObj)
                        {
                            writer.Dump($"#{key} {BmsUtils.ToBased((int)value, radix)}");
                        }
                        else
                        {
                            writer.Dump($"#{key} {value}");
                        }
                    }
                }
                else
                {
                    if (_stringValues.TryGetValue(t, out var value) && GetString(t, Inheritance.Parent) != value)
                    {
                        writer.Dump($"#{key} {value}");
                    }
                }
            }
            foreach (var (key, value) in CollectionsMarshal.AsSpan(_sub))
            {
                writer.Dump($"#{key.ToUpper()} {value}");
            }
            if (radix is not Constants.Base_Default)
            {
                writer.Dump($"#BASE {radix}");
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
            for (var t = HeaderType.Player; t is <= HeaderType.Comment; t++)
            {
                var key = t.ToString().ToUpper();
                if (_double_headers.Contains(t))
                {
                    if (_doubleValues.TryGetValue(t, out var value) && GetDouble(t, 0, Inheritance.Parent) != value)
                    {
                        writer.WriteStringValue($"#{key} {value}");
                    }
                }
                else
                {
                    if (_stringValues.TryGetValue(t, out var value) && GetString(t, Inheritance.Parent) != value)
                    {
                        writer.WriteStringValue($"#{key} {value}");
                    }
                }
            }
            foreach (var (key, value) in CollectionsMarshal.AsSpan(_sub))
            {
                writer.WriteStringValue($"#{key.ToUpper()} {value}");
            }
            writer.WriteEndArray();
        }
    }
}
