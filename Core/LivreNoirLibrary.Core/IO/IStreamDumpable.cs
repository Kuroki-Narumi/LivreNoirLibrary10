using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IStreamDumpable
    {
        void Dump(Stream stream);
    }
}
