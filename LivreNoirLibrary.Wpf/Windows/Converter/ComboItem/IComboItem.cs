using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    public interface IComboItem : INamedObject
    {
        public static abstract Type KeyType { get; }
        public static abstract object GetItem(object value);
        public object Value { get; }
    }
}
