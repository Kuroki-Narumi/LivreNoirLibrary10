using System.IO;

namespace LivreNoirLibrary.ObjectModel
{
    /// <inheritdoc cref="System.ICloneable"/>
    public interface ICloneable<TSelf>
        where TSelf : ICloneable<TSelf>
    {
        public TSelf Clone();
    }
}
