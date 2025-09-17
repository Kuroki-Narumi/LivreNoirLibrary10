using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IStreamDumpable
    {
        public void Dump(Stream stream);
    }
}
