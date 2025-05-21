using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

[Generator]
public class ObservablePropertyGenerator : PropertyGeneratorBase<ObservablePropertyGenerator.FieldInfo>
{
    const string _attr_name = "LivreNoirLibrary.ObjectModel.ObservablePropertyAttribute";

    public override string AttributeName => _attr_name;
    public override string BaseTypeName { get; } = "LivreNoirLibrary.ObjectModel.ObservableObjectBase";

    public record FieldInfo : FieldInfoBase
    {
        public string[] Related { get; set; } = [];

        public void SetRelated(IEnumerable<string?>? related)
        {
            Related = related is null ? [] : [.. related.Where(v => !string.IsNullOrEmpty(v) && v != Property.Name).Select(v => v!)];
        }

        public void Deconstruct(
            out TypeInfo field,
            out TypeInfo property,
            out string setterScope,
            out CoerceType coerceHandler,
            out int onChangedHandler,
            out string[] related,
            out string[] attributes)
        {
            base.Deconstruct(out field, out _, out property, out setterScope, out coerceHandler, out onChangedHandler, out _);
            related = Related;
            attributes = Attributes;
        }
    }

    private const string Type = nameof(Type);
    private const string Related = nameof(Related);

    protected override void ApplyAttributeArg(string argName, in TypedConstant value, FieldInfo info)
    {
        switch (argName)
        {
            case Type:
                if (value.Value is ITypeSymbol tt)
                {
                    info.Property.Type = Utils.GetTypeFullname(tt);
                }
                break;
            case Related:
                info.SetRelated(value.Values.Select(v => v.Value as string));
                break;
        }
    }

    protected override void AppendProperty(StringBuilder sb, FieldInfo info)
    {
        var ((fieldType, fieldName), (propertyType, propertyName), setterScope, coerceHandler, onChangedHandler, related, attributes) = info;
        var needCast = fieldType != propertyType;
        foreach (var attribute in attributes)
        {
            sb.AppendLine($$"""
                            {{attribute}}
                    """);
        }
        sb.AppendLine($$"""
                            public {{propertyType}} {{propertyName}}
                            {
                                get => {{(needCast ? $"({propertyType})" : "")}}{{fieldName}};
                                {{setterScope}}set
                                {
                    """);
        string valueText;
        if (onChangedHandler is 2)
        {
            sb.AppendLine($"                var oldValue = {fieldName};");
        }
        if (needCast)
        {
            valueText = "v";
            if (coerceHandler is not CoerceType.None)
            {
                sb.AppendLine($"                var v = Coerce{propertyName}(value);");
            }
            else
            {
                sb.AppendLine($"                var v = ({fieldType})value;");
            }
        }
        else
        {
            valueText = "value";
            if (coerceHandler is not CoerceType.None)
            {
                sb.AppendLine($"                value = Coerce{propertyName}(value);");
            }
        }
        if (related.Length is > 0 || onChangedHandler is > 0)
        {
            sb.AppendLine($$"""
                                    if (SetProperty(ref {{fieldName}}, {{valueText}}))
                                    {
                    """);
            foreach (var name in related)
            {
                sb.AppendLine($"                    SendPropertyChanged(\"{name}\");");
            }
            if (onChangedHandler is 1)
            {
                sb.AppendLine($"                    On{propertyName}Changed({valueText});");
            }
            else if (onChangedHandler is 2)
            {
                sb.AppendLine($"                    On{propertyName}Changed(oldValue, {valueText});");
            }
            sb.AppendLine("                }");
        }
        else
        {
            sb.AppendLine($"                SetProperty(ref {fieldName}, {valueText});");
        }
        sb.AppendLine("""
                                }
                            }
                    """);
    }
}