using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IStreamLoadable<TSelf>
        where TSelf : IStreamLoadable<TSelf>
    {
        abstract static TSelf Load(Stream stream);
    }
}
