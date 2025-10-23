using System;

namespace LivreNoirLibrary.Text.Xml
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ContentPropertyAttribute(string propertyName) : Attribute
    {
        public string PropertyName { get; } = propertyName;
    }
}
