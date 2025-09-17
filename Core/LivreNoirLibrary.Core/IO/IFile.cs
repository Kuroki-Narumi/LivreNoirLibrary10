using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IFile<TSelf>
        where TSelf : IFile<TSelf>
    {
        public static abstract TSelf Open(string path);
        public void Save(string path);
    }
}
