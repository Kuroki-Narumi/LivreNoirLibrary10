using Microsoft.CodeAnalysis;
using System;

namespace LivreNoirLibrary.Windows.SourceGenerator
{
    public class RoutedEventInfo
    {
        public string FilePath { get; set; } = "";
        public string Namespace { get; set; } = "";
        public string ContainingType { get; set; } = "";
        public bool IsStatic { get; set; }
        public bool IsEvent { get; set; }
        public string OwnerType { get; set; } = "";
        public string HandlerType { get; set; } = "";
        public string EventName { get; set; } = "";
        public RoutingStrategy RoutingStragegy { get; set; } = RoutingStrategy.Bubble;
    }

    public enum RoutingStrategy
    {
        Tunnel,
        Bubble,
        Direct,
    }
}
