using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class LIST(string type) : RiffChunk, IRiffChunk<LIST>, ICollection<DataChunk>
    {
        public override string Chid => ChunkIds.LIST;
        public string Type { get; set; } = type;
        public List<DataChunk> SubChunks { get; } = [];

        public int Count => SubChunks.Count;
        public bool IsReadOnly => false;

        public static LIST LoadContents(BinaryReader reader, uint length)
        {
            var stream = reader.BaseStream;
            var limit = stream.Position + length;
            var type = ChunkIds.Read(reader);
            LIST data = new(type);
            var list = data.SubChunks;
            while (stream.Position < limit)
            {
                var chid = ChunkIds.Read(reader);
                switch (chid)
                {
                    case ChunkIds.Label:
                    case ChunkIds.Note:
                        list.Add(IdChunk.Load(chid, reader));
                        break;
                    case ChunkIds.LTxt:
                        list.Add(reader.ReadRiffChunk<LTxt>());
                        break;
                    case ChunkIds.FILE:
                        list.Add(reader.ReadRiffChunk<FILE>());
                        break;
                    default:
                        list.Add(Chunk.Load(chid, reader));
                        break;
                }
            }
            return data;
        }

        public override void DumpContents(BinaryWriter writer)
        {
            ChunkIds.Write(writer, Type);
            foreach (var chunk in CollectionsMarshal.AsSpan(SubChunks))
            {
                writer.WriteRiffChunk(chunk);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteString("type", Type);
            writer.WritePropertyName("chunks");
            writer.WriteStartArray();
            foreach (var chunk in CollectionsMarshal.AsSpan(SubChunks))
            {
                chunk.WriteJson(writer, options);
            }
            writer.WriteEndArray();
        }

        public void Clear()
        {
            SubChunks.Clear();
        }

        public DataChunk? Find(string chid) => SubChunks.Find(c => c.Chid == chid);
        public DataChunk? Find(string chid, int id) => SubChunks.Find(c => c is IIdChunk ic && ic.Id == id && c.Chid == chid);
        public List<DataChunk> FindAll(string chid) => SubChunks.FindAll(c => c.Chid == chid);
        public List<DataChunk> FindAll(string chid, int id) => SubChunks.FindAll(c => c is IIdChunk ic && ic.Id == id && c.Chid == chid);

        public bool TryFind<T>([MaybeNullWhen(false)]out T chunk, string? chid = null)
            where T : DataChunk
        {
            var nocheck = string.IsNullOrEmpty(chid);
            foreach (var c in CollectionsMarshal.AsSpan(SubChunks))
            {
                if (c is T ch && (nocheck || c.Chid == chid))
                {
                    chunk = ch;
                    return true;
                }
            }
            chunk = null;
            return false;
        }

        public bool TryFind<T>([MaybeNullWhen(false)]out T chunk, int id, string? chid = null)
            where T : DataChunk, IIdChunk
        {
            var nocheck = string.IsNullOrEmpty(chid);
            foreach (var c in CollectionsMarshal.AsSpan(SubChunks))
            {
                if (c is T ch && ch.Id == id && (nocheck || c.Chid == chid))
                {
                    chunk = ch;
                    return true;
                }
            }
            chunk = null;
            return false;
        }

        public List<T> FindAll<T>(string? chid = null)
            where T : DataChunk
        {
            var nocheck = string.IsNullOrEmpty(chid);
            List<T> result = [];
            foreach (var c in CollectionsMarshal.AsSpan(SubChunks))
            {
                if (c is T ch && (nocheck || c.Chid == chid))
                {
                    result.Add(ch);
                }
            }
            return result;
        }

        public List<T> FindAll<T>(int id, string? chid = null)
            where T : DataChunk, IIdChunk
        {
            var nocheck = string.IsNullOrEmpty(chid);
            List<T> result = [];
            foreach (var c in CollectionsMarshal.AsSpan(SubChunks))
            {
                if (c is T ch && ch.Id == id && (nocheck || c.Chid == chid))
                {
                    result.Add(ch);
                }
            }
            return result;
        }

        public string? GetText(string tag, Encoding? encoding = null) => Find(tag)?.GetText(encoding);
        public string? GetText(string tag, int id, Encoding? encoding = null) => Find(tag, id)?.GetText(encoding);

        public bool TryGetText(string tag, [MaybeNullWhen(false)]out string text, Encoding? encoding = null)
        {
            text = GetText(tag, encoding);
            return !string.IsNullOrEmpty(text);
        }

        public bool TryGetText(string tag, int id, [MaybeNullWhen(false)]out string text, Encoding? encoding = null)
        {
            text = GetText(tag, id, encoding);
            return !string.IsNullOrEmpty(text);
        }

        public bool Remove(DataChunk item) => SubChunks.Remove(item);
        public void RemoveAll(string chid) => SubChunks.RemoveAll(c => c.Chid == chid);
        public void RemoveAll(string chid, int id) => SubChunks.RemoveAll(c => c is IIdChunk ic && ic.Id == id && c.Chid == chid);

        public void Set(string chid, byte[] data)
        {
            if (Find(chid) is DataChunk c)
            {
                c.Data = data;
                return;
            }
            Add(chid, data);
        }

        public void Set(string chid, int id, byte[] data)
        {
            if (Find(chid, id) is DataChunk c)
            {
                c.Data = data;
                return;
            }
            Add(chid, id, data);
        }

        public void SetText(string chid, string text, Encoding? encoding = null) => Set(chid, (encoding ?? Encoding.UTF8).GetBytes(text));
        public void SetText(string chid, int id, string text, Encoding? encoding = null) => Set(chid, id, (encoding ?? Encoding.UTF8).GetBytes(text));

        public void Set<T>(T chunk)
            where T : DataChunk
        {
            if (chunk is IIdChunk idc)
            {
                if (Find(chunk.Chid, idc.Id) is DataChunk ic)
                {
                    chunk.CopyTo(ic);
                }
                return;
            }
            if (Find(chunk.Chid) is DataChunk d)
            {
                chunk.CopyTo(d);
                return;
            }
            Add(chunk);
        }

        public void Add(DataChunk item) => SubChunks.Add(item);
        public void Add(string chid, byte[] data) => SubChunks.Add(new Chunk(chid, data));
        public void Add(string chid, string text, Encoding? encoding = null) => SubChunks.Add(new Chunk(chid, (encoding ?? Encoding.UTF8).GetBytes(text)));
        public void Add(string chid, int id, byte[] data) => SubChunks.Add(new IdChunk(chid, id, data));
        public void Add(string chid, int id, string text, Encoding? encoding = null) => SubChunks.Add(new IdChunk(chid, id, (encoding ?? Encoding.UTF8).GetBytes(text)));

        public bool Contains(DataChunk item) => SubChunks.Contains(item);
        public void CopyTo(DataChunk[] array, int arrayIndex) => SubChunks.CopyTo(array, arrayIndex);
        public List<DataChunk>.Enumerator GetEnumerator() => SubChunks.GetEnumerator();
        IEnumerator<DataChunk> IEnumerable<DataChunk>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
