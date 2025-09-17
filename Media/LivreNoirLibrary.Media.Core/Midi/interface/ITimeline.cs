using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ITimeline : IXMultiTimeline<Rational, IObject>
    {
        public void RemoveDuplicated(ISelection? selection);
    }
}
