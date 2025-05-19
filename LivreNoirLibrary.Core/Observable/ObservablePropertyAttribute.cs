using System;

namespace LivreNoirLibrary.Observable
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ObservablePropertyAttribute : Attribute
    {
        public string PropertyName { get; set; } = "";
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