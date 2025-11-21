using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IDumpable
    {
        void Dump(BinaryWriter writer);
    }
}
