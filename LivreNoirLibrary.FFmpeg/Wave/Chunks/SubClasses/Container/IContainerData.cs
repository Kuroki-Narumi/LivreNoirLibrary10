using System;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public interface IContainerData<TSelf> : IDumpable<TSelf>, IJsonWriter
        where TSelf : IContainerData<TSelf>
    {
        public static abstract uint ByteSize { get; }
        public int Id { get; }
    }
}
