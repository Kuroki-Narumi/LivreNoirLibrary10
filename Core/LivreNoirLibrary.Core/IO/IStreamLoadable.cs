using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface IStreamLoadable<TSelf>
        where TSelf : IStreamLoadable<TSelf>
    {
        public static abstract TSelf Load(Stream stream);
    }
}
