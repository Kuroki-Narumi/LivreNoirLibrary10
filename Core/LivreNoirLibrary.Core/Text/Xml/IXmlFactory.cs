using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LivreNoirLibrary.Text.Xml
{
    public interface IXmlFactory<T>
    {
        public bool TryGetType(string typeName, [MaybeNullWhen(false)] out Type type);
        public bool TryCreateInstance(Type type, [MaybeNullWhen(false)] out T element);
        public bool TryConvert(string value, Type targetType, [MaybeNullWhen(false)]out object obj);
        public bool TryAddChild(T parent, T child);
    }
}
