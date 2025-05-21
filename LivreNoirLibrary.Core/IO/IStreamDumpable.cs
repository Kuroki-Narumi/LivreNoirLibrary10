using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IStreamDumpable<TSelf>
        where TSelf : IStreamDumpable<TSelf>
    {
        public void Dump(Stream stream);
        public static abstract TSelf Load(Stream stream);
    }
}
