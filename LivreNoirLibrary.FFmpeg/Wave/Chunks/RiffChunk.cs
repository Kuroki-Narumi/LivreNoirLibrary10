using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Media.Wave.Chunks;

namespace LivreNoirLibrary.Media.Wave
{
    public abstract class RiffChunk : IRiffChunk, IJsonWriter
    {
        public abstract string Chid { get; }
        public virtual uint ByteSize => 0;
        public abstract void DumpContents(BinaryWriter writer);

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => this.WriteJsonBasic(writer, options);
        public abstract void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options);

        public static RiffChunk Create(string chid, BinaryReader reader) => chid switch
        {
            ChunkIds.Cue => reader.ReadRiffChunk<Cue>(),
            ChunkIds.PlayList => reader.ReadRiffChunk<PlayList>(),
            ChunkIds.TList => reader.ReadRiffChunk<TList>(),
            ChunkIds.Acid => reader.ReadRiffChunk<Acid>(),
            ChunkIds.BExt => reader.ReadRiffChunk<BExt>(),
            ChunkIds.Fact => reader.ReadRiffChunk<Fact>(),
            ChunkIds.FILE => reader.ReadRiffChunk<FILE>(),
            ChunkIds.Inst => reader.ReadRiffChunk<Inst>(),
            ChunkIds.LIST => reader.ReadRiffChunk<LIST>(),
            ChunkIds.LTxt => reader.ReadRiffChunk<LTxt>(),
            ChunkIds.Sampler => reader.ReadRiffChunk<Sampler>(),
            ChunkIds.Struct => reader.ReadRiffChunk<Struct>(),
            ChunkIds.ID3 => reader.ReadRiffChunk<ID3>(),
            _ => Chunk.Load(chid, reader),
        };
    }
}
