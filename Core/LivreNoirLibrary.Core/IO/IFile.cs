using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IFile<TSelf>
        where TSelf : IFile<TSelf>
    {
        abstract static TSelf Open(string path);
        void Save(string path);
    }
}
