using System;

namespace LivreNoirLibrary.ObjectModel
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ObservablePropertyAttribute : Attribute
    {
        public Type? Type { get; set; }
        public string Name { get; set; } = "";
        public Scope SetterScope { get; set; } = Scope.Public;
        public string[] Related { get; set; } = [];
    }

    public enum Scope
    {
        Public,
        Private,
        Protected,
        Internal,
        InternalProtected,
    }
}