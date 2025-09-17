using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface ILoadable<TSelf>
        where TSelf : ILoadable<TSelf>
    {
        public static abstract TSelf Load(BinaryReader reader);
    }
}
