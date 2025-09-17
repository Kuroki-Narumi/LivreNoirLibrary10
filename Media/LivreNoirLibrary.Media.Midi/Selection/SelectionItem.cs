using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public class SelectionItem(Rational position, IObject obj)
    {
        public Rational Position { get; } = position;
        public IObject Object { get; private set; } = obj;

        public void ReplaceToClone()
        {
            if (Object is not NoteGroup)
            {
                Object = Object.Clone();
            }
        }

        public static implicit operator (Rational, IObject)(SelectionItem item) => (item.Position, item.Object);

        public void Deconstruct(out Rational positoin, out IObject obj)
        {
            positoin = Position;
            obj = Object;
        }
    }
}
