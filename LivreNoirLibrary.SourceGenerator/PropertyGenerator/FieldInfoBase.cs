using Microsoft.CodeAnalysis;
using System;

public record FieldInfoBase
{
    public string FilePath { get; set; } = "";
    public string[] Usings { get; set; } = [];
    public string Namespace { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string[] Attributes { get; set; } = [];
    public TypeInfo Field { get; set; } = new();
    public string DefaultValue { get; set; } = "";
    public TypeInfo Property { get; set; } = new();
    public int SetterScope { get; set; }
    public CoerceType CoerceHandler { get; set; }
    public int OnChangedHandler { get; set; }
    public bool HasDefault { get; set; }

    public bool IsSetterPublic => SetterScope is 0;
    public string SetterScopeText => SetterScope switch
    {
        1 => "private ",
        2 => "protected ",
        3 => "internal ",
        4 => "internal protected ",
        _ => "",
    };

    public void Deconstruct(
        out TypeInfo field,
        out string defaultValue,
        out TypeInfo property,
        out string setterScope,
        out CoerceType coerceHandler,
        out int onChangedHandler,
        out bool hasDefault)
    {
        field = Field;
        defaultValue = DefaultValue;
        property = Property;
        setterScope = SetterScopeText;
        coerceHandler = CoerceHandler;
        onChangedHandler = OnChangedHandler;
        hasDefault = HasDefault;
    }
}