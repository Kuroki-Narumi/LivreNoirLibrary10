using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface ILoadable
    {
        void ProcessLoad(BinaryReader reader);
    }

    public interface ILoadable<TSelf>
        where TSelf : ILoadable<TSelf>
    {
        abstract static TSelf Load(BinaryReader reader);
    }
}
