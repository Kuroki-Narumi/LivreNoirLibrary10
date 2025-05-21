using System.IO;

namespace LivreNoirLibrary.IO
{
    public interface ICloneable<TSelf>
        where TSelf : ICloneable<TSelf>
    {
        public TSelf Clone();
    }
}
