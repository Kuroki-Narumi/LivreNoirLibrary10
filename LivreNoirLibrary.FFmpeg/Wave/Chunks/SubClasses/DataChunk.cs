using System;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public abstract class DataChunk : RiffChunk, IDataChunk
    {
        public byte[] Data { get; set; } = [];

        public virtual void CopyTo<T>(T target) where T : DataChunk => IRiffChunkExtensions.CopyTo(this, target);
        public virtual string GetText() => IRiffChunkExtensions.GetText(this, null);
        public virtual void SetText(string text) => IRiffChunkExtensions.SetText(this, text, null);
    }
}
