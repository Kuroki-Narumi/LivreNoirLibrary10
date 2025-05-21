using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IDumpable<TSelf>
        where TSelf : IDumpable<TSelf>
    {
        public void Dump(BinaryWriter writer);
        public static abstract TSelf Load(BinaryReader reader);
    }
}
