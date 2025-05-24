using System;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IMetaObject : IObject
    {
        public MetaType Type { get; }
    }
}
