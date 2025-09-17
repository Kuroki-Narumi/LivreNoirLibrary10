using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public interface INote : IDumpable
    {
        public string GetValueText(int radix);
        public void CopyFrom(INote source);
        public INote Clone();
    }

    public interface INote<T> : INote, ICloneable<T>, IDumpable, ILoadable<T>
        where T : INote<T>
    {

    }
}
