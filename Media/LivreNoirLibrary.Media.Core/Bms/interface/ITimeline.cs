using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ITimeline : IXMultiTimeline<BarPosition, Note>, IDumpable, ILoadable
    {
        void InsertBar(int number, int count);
        void DeleteBar(int number, int count);
    }
}
