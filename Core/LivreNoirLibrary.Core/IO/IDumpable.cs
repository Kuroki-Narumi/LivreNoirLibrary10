using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IDumpable
    {
        public void Dump(BinaryWriter writer);
    }
}
