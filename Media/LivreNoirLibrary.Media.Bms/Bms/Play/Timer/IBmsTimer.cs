using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public interface IBmsTimer : IClear
    {
        void Set(TimerId id, double time);
        bool Remove(TimerId id);
        double Get(TimerId id, double time);
        bool TryGet(TimerId id, double time, out double relativeTime);
    }
}
