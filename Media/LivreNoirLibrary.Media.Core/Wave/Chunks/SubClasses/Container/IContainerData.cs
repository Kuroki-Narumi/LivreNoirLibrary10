using System;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public interface IContainerData<TSelf> : IDumpable, ILoadable<TSelf>, IWriteJson, IId
        where TSelf : IContainerData<TSelf>
    {
        abstract static uint ByteSize { get; }
    }
}
