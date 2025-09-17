using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using LivreNoirLibrary.UnsafeOperations;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Ogg.Vorbis
{
    public class VorbisCommentList : List<VorbisComment>, IJsonWriter
    {
        public const string DefaultVendor = "LivreNoirLibrary";

        public string Vendor { get; set; } = DefaultVendor;

        public void Add(string key, string value) => Add(new VorbisComment(key, value));

        public bool Remove(string key)
        {
            var index = FindIndex(key);
            if (index is >= 0)
            {
                RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int RemoveAll(string key) => RemoveAll(c => c.Key == key);

        public string? GetOrNull(string key) => TryGetValue(key, out var value) ? value : null;

        public bool TryGetValue(string key, [MaybeNullWhen(false)]out string value)
        {
            foreach (var c in CollectionsMarshal.AsSpan(this))
            {
                if (c.Key == key)
                {
                    value = c.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public bool TryGet<T>(string key, [MaybeNullWhen(false)]out T value)
            where T : IParsable<T>
        {
            if (TryGetValue(key, out var str) && T.TryParse(str, null, out value))
            {
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool TryGetLong(string key, out long value) => TryGet(key, out value);
        public bool TryGetDouble(string key, out double value) => TryGet(key, out value);
        public bool TryGetDecimal(string key, out decimal value) => TryGet(key, out value);

        public int FindIndex(string key) => FindIndex(c => c.Key == key);

        public bool Set(string key, string? value)
        {
            var index = FindIndex(key);
            if (index is >= 0)
            {
                if (value is null)
                {
                    RemoveAt(index);
                }
                else
                {
                    this[index] = new(key, value);
                }
                return false;
            }
            else if (value is not null)
            {
                Add(key, value);
                return true;
            }
            return false;
        }

        public List<VorbisComment> FindAll(string key)
        {
            List<VorbisComment> result = [];
            foreach (var c in CollectionsMarshal.AsSpan(this))
            {
                if (c.Key == key)
                {
                    result.Add(c);
                }
            }
            return result;
        }

        public void LoadPacket(PacketInfo packet)
        {
            var data = packet.Data;
            var encoding = Encoding.UTF8;
            if (VorbisHeader.IsValidHeader(data, PacketType.Comment))
            {
                var length = data.Get<int>(VorbisHeader.Index_VendorLength);
                Vendor = encoding.GetString(data.AsSpan(VorbisHeader.Index_Vendor, length));
                var index = VorbisHeader.Index_Vendor + length;
                var count = data.Get<int>(index);
                index += sizeof(int);
                for (var i = 0; i < count; i++)
                {
                    length = data.Get<int>(index);
                    index += sizeof(int);
                    Add(VorbisComment.FromSpan(data.AsSpan(index, length), encoding));
                    index += length;
                }
            }
        }

        public static VorbisCommentList Create(in PacketInfo packet)
        {
            VorbisCommentList result = [];
            result.LoadPacket(packet);
            return result;
        }

        public PacketInfo ToPacket()
        {
            var encoding = Encoding.UTF8;
            var span = CollectionsMarshal.AsSpan(this);
            var vendor = Vendor;
            var venLen = vendor.Length;
            // calc required buffer length
            //        fixed length                    + framing bit
            var len = VorbisHeader.CommonHeaderLength + 1;
            //     vendor length + vendor string
            len += sizeof(int)   + encoding.GetMaxByteCount(venLen);
            //     number of comments
            len += sizeof(int);
            foreach (var comment in span)
            {
                //     comment length + comment string
                len += sizeof(int)    + comment.GetMaxByteCount(encoding);
            }
            var buffer = ArrayPool<byte>.Shared.Rent(len);
            try
            {
                // fixed header (header type 03 & "vorbis")
                VorbisHeader.InitializeHeader(buffer, PacketType.Comment);
                // vender length & vender string
                len = VorbisHeader.Index_VendorLength;
                var count = encoding.GetBytes(vendor, 0, vendor.Length, buffer, len + sizeof(int));
                buffer.Set(len, count);
                len += sizeof(int) + count;
                // number of comments
                buffer.Set(len, Count);
                len += sizeof(int);
                foreach (var comment in span)
                {
                    count = comment.CopyTo(buffer, len + sizeof(int), encoding);
                    buffer.Set(len, count);
                    len += sizeof(int) + count;
                }
                // framing bit
                buffer[len] = 1;
                len++;
                return new(buffer[0..len], 0, false);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("vendor", Vendor);
            var i = 0;
            foreach (var (key, value) in CollectionsMarshal.AsSpan(this))
            {
                writer.WriteString($"{++i}", $"{key}={value}");
            }
            writer.WriteEndObject();
        }

        public void GetLoopMarkers(MarkerCollection markers)
        {
            if (TryGetLong(StandardCommentKeys.LoopStart, out var start))
            {
                markers.Set(start, Marker.LoopStartName);
                if (TryGetLong(StandardCommentKeys.LoopLength, out var length))
                {
                    markers.Set(start + length, Marker.LoopEndName);
                }
            }
        }

        public void SetLoopMarkers(MarkerCollection markers)
        {
            if (markers.TryGetByName(Marker.LoopStartName, out var marker))
            {
                var pos = marker.Position;
                Set(StandardCommentKeys.LoopStart, $"{pos}");
                if (markers.TryGetByName(Marker.LoopEndName, out marker))
                {
                    Set(StandardCommentKeys.LoopLength, $"{marker.Position - pos}");
                }
            }
        }

        public IEnumerable<MetaTag> AsMetaTags()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                var (key, value) = this[i];
                yield return new(key, value);
            }
        }
    }
}
