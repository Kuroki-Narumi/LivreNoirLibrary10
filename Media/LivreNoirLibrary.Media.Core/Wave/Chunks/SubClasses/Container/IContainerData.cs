using System;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public interface IContainerData<TSelf> : IDumpable, ILoadable<TSelf>, IWriteJson
        where TSelf : IContainerData<TSelf>
    {
        abstract static uint ByteSize { get; }
        int Id { get; }
    }
}
