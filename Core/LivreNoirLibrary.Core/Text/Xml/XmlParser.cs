using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace LivreNoirLibrary.Text.Xml
{
    public class XmlParseException(string? message = null, Exception? innerException = null) : Exception(message, innerException);

    public static class XmlParser
    {
        public static T Parse<T>(XmlReader reader, IXmlFactory<T> factory)
            where T : notnull
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        goto ElementDetected;
                }
            }
            throw new XmlParseException($"unsupported node type: {reader.NodeType}");
        ElementDetected:
            var className = reader.LocalName;
            if (factory.TryGetType(className, out var type) && factory.TryCreateInstance(type, out var instance))
            {
                ReadProperties(type, instance, reader, factory);
                return instance;
            }
            else
            {
                reader.Skip();
                throw new XmlParseException($"unknown type name: {className}");
            }
        }

        private static readonly Type[] _parsable_arg_types1 = [typeof(string), typeof(IFormatProvider)];
        private static readonly Type[] _parsable_arg_types2 = [typeof(string)];
        private static readonly object?[] _parsable_args1 = [null, null];
        private static readonly object?[] _parsable_args2 = [null];

        private static bool TryConvert<T>(IXmlFactory<T> factory, string text, Type targetType, [MaybeNullWhen(false)] out object value)
        {
            if (factory.TryConvert(text, targetType, out value))
            {
                return true;
            }
            if (targetType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, _parsable_arg_types1, null) is { } m1)
            {
                _parsable_args1[0] = text;
                value = m1.Invoke(null, _parsable_args1);
            }
            else if (targetType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, _parsable_arg_types2, null) is { } m2)
            {
                _parsable_args2[0] = text;
                value = m2.Invoke(null, _parsable_args2);
            }
            return value is not null;
        }

        private static void ReadProperties<T>(Type instanceType, T instance, XmlReader reader, IXmlFactory<T> factory)
            where T : notnull
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    var propertyName = reader.Name;
                    if (instanceType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) is not { } property)
                    {
                        throw new XmlParseException($"unknown property name: {instanceType}.{propertyName}");
                    }
                    if (!property.CanWrite)
                    {
                        throw new XmlParseException($"property is read-only: {instanceType}.{propertyName}");
                    }
                    if (!TryConvert(factory, reader.Value, property.PropertyType, out var value))
                    {
                        throw new XmlParseException($"failed to convert \"{reader.Value}\" to {property.PropertyType}");
                    }
                    property.SetValue(instance, value);
                }
                reader.MoveToElement();
            }
            if (!reader.IsEmptyElement)
            {
                PropertyInfo? property = null;
                Type? propertyType = null;
                if (instanceType.GetCustomAttribute<ContentPropertyAttribute>() is { } attr && 
                    instanceType.GetProperty(attr.PropertyName, BindingFlags.Instance) is { } p)
                {
                    property = p;
                    propertyType = p.PropertyType;
                }
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text:
                            if (propertyType is null)
                            {
                                throw new XmlParseException($"the instance doesn't have content proeprty: {instanceType}");
                            }
                            if (!TryConvert(factory, reader.Value, propertyType, out var value))
                            {
                                throw new XmlParseException($"failed to convert \"{reader.Value}\" to {propertyType}");
                            }
                            property!.SetValue(instance, value);
                            break;
                        case XmlNodeType.Element:
                            var name = reader.LocalName;
                            if (instanceType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) is { } p2)
                            {
                                property = p2;
                                propertyType = p2.PropertyType;
                            }
                            else if (!factory.TryGetType(name, out propertyType))
                            {
                                throw new XmlParseException($"unknown type name: {name}");
                            }
                            if (!factory.TryCreateInstance(propertyType, out var child))
                            {
                                throw new XmlParseException($"failed to create instance of type: {propertyType}");
                            }
                            ReadProperties(propertyType!, child, reader, factory);
                            if (property is not null)
                            {
                                property.SetValue(instance, child);
                            }
                            else if (!factory.TryAddChild(instance, child))
                            {
                                throw new XmlParseException($"failed to add child: {name}");
                            }
                            break;
                        case XmlNodeType.EndElement:
                            goto EndOfLoop;
                        default:
                            break;
                    }
                }
            EndOfLoop:
                ;
            }
        }
    }
}