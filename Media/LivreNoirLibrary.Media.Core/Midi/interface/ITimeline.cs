using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ITimeline : IXMultiTimeline<Rational, IObject>
    {
        void RemoveDuplicated(ISelection? selection);
    }
}
