using System;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IMetaObject : IObject
    {
        MetaType Type { get; }
    }
}
