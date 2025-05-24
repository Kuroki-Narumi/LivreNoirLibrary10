using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public class SysExPrefixCollection : ObservableList<SysExPrefix>
    {
        public int FindIndex(byte[] data)
        {
            var dataSpan = new ReadOnlySpan<byte>(data);
            for (var i = 0; i < _list.Count; i++)
            {
                var item = _list[i].Prefix;
                if (dataSpan[..item.Length].SequenceEqual(item))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public partial class SysExPrefix : ObservableObjectBase, INamedObject
    {
        public const int MaxPrefixLength = 256;

        [ObservableProperty]
        private string _name = "";
        [JsonIgnore]
        [ObservableProperty(Related = [nameof(Prefix_String)])]
        private byte[] _prefix = [];

        string? INamedObject.Name => _name;

        [JsonPropertyName(nameof(Prefix))]
        public string Prefix_String { get => BitConverter.ToString(_prefix); set => Prefix = GetBytes(value); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParseOne(ReadOnlySpan<char> span, Range range, out byte value)
        {
            if (MemoryExtensions.Trim(span[range]).TryParseToLong(16, out var v) && v is >= byte.MinValue and <= byte.MaxValue)
            {
                value = (byte)v;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public static byte[] GetBytes(string source)
        {
            var span = source.AsSpan();
            var buffer = (stackalloc byte[MaxPrefixLength]);
            var i = 0;
            foreach (var match in Regex_Delimiter.EnumerateSplits(span))
            {
                _ = TryParseOne(span, match, out buffer[i]);
                i++;
                if (i is >= MaxPrefixLength)
                {
                    break;
                }
            }
            return buffer[..i].ToArray();
        }

        public static bool IsValid(string source)
        {
            var span = source.AsSpan();
            foreach (var match in Regex_Delimiter.EnumerateSplits(span))
            {
                if (!TryParseOne(span, match, out _))
                {
                    return false;
                }
            }
            return true;
        }

        [GeneratedRegex(@"[\s-.,]")]
        private static partial Regex Regex_Delimiter { get; }
    }
}
