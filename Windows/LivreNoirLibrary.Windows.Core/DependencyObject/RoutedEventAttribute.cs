using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Event, AllowMultiple = false, Inherited = false)]
    public class RoutedEventAttribute(Type ownerType, Type handlerType) : Attribute
    {
        public Type? OwnerType { get; } = ownerType;
        public Type? HandlerType { get; } = handlerType;
        public string EventName { get; set; } = "";
        public RoutingStrategy Strategy { get; set; } = RoutingStrategy.Bubble;

        public RoutedEventAttribute() : this(null!, null!) { }
        public RoutedEventAttribute(Type ownerType) : this(ownerType, null!) { }
    }
}
