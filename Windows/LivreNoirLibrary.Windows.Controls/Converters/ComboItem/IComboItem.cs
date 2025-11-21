using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    public interface IComboItem : INamedObject
    {
        abstract static Type KeyType { get; }
        abstract static object GetItem(object value);
        object Value { get; }
    }
}
