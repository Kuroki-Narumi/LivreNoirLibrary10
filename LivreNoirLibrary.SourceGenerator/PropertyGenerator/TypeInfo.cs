using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

public record TypeInfo
{
    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public bool IsValueType { get; set; }
    public bool IsNullable => Type.EndsWith("?");

    public TypeInfo() { }

    public TypeInfo(IFieldSymbol symbol)
    {
        Type = Utils.GetTypeFullname(symbol.Type);
        Name = symbol.Name;
        IsValueType = symbol.Type.IsValueType;
    }

    public TypeInfo(TypeInfo source)
    {
        Type = source.Type;
        Name = source.Name;
        IsValueType = source.IsValueType;
    }

    public void Deconstruct(out string type, out string name)
    {
        type = Type;
        name = Name;
    }

    public void Deconstruct(out string type, out string name, out bool isValueType)
    {
        type = Type;
        name = Name;
        isValueType = IsValueType;
    }

    public void Deconstruct(out string type, out string name, out bool isValueType, out bool isNullbale)
    {
        type = Type;
        name = Name;
        isValueType = IsValueType;
        isNullbale = IsNullable;
    }
}