using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LivreNoirLibrary.Text.Xml
{
    public interface IXmlFactory<T>
    {
        bool TryGetType(string typeName, [MaybeNullWhen(false)] out Type type);
        bool TryCreateInstance(Type type, [MaybeNullWhen(false)] out T element);
        bool TryConvert(string value, Type targetType, [MaybeNullWhen(false)]out object obj);
        bool TryAddChild(T parent, T child);
    }
}
