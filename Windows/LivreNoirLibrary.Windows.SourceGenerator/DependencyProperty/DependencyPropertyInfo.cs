
namespace LivreNoirLibrary.Windows.SourceGenerator
{
    public record DependencyPropertyInfo
    {
        public string Filename { get; set; } = "";
        public string[] Usings { get; set; } = [];
        public string Namespace { get; set; } = "";
        public string ContainingType { get; set; } = "";
        public TypeInfo Property { get; set; } = null!;
        public string[] Attributes { get; set; } = [];
        public string? DefaultValue { get; set; }
        public string? GetterScope { get; set; }
        public string? SetterScope { get; set; }
        public string FieldName { get; set; } = "";
        public CoerceType CoerceType { get; set; }
        public int OnChangedArgCount { get; set; }
        public FrameworkPropertyMetadataOptions Options { get; set; }
    }
}
