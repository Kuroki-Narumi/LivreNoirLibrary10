using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Xml.Linq;

[Generator]
public class DependencyPropertyGenerator : PropertyGeneratorBase<DependencyPropertyGenerator.FieldInfo>
{
    public override string AttributeName { get; } = "LivreNoirLibrary.Windows.DependencyPropertyAttribute";
    public override string BaseTypeName { get; } = "System.Windows.DependencyObject";

    [Flags]
    public enum FrameworkPropertyMetadataOptions
    {
        AffectsMeasure = 1,
        AffectsArrange = 2,
        AffectsParentMeasure = 4,
        AffectsParentArrange = 8,
        AffectsRender = 16,
        Inherits = 32,
        OverridesInheritanceBehavior = 64,
        NotDataBindable = 128,
        BindsTwoWayByDefault = 256,
        Journal = 1024,
        SubPropertiesDoNotAffectRender = 2048,
    }

    public static bool IsFlagSet(FrameworkPropertyMetadataOptions options, FrameworkPropertyMetadataOptions flag) => (options & flag) == flag;

    public record FieldInfo : FieldInfoBase
    {
        public FrameworkPropertyMetadataOptions MetadataOptions { get; set; }
    }

    private const string MetadataOptions = nameof(MetadataOptions);

    protected override void ApplyAttributeArg(string argName, in TypedConstant value, FieldInfo info)
    {
        if (argName is MetadataOptions)
        {
            if (value.Value is int opt)
            {
                info.MetadataOptions |= (FrameworkPropertyMetadataOptions)opt;
            }
        }
        else if (Enum.TryParse<FrameworkPropertyMetadataOptions>(argName, out var option))
        {
            if (value.Value is true)
            {
                info.MetadataOptions |= option;
            }
            else
            {
                info.MetadataOptions &= ~option;
            }
        }
    }

    protected override FieldInfo? ApplyExtraInfo(IFieldSymbol fieldSymbol, FieldInfo info)
    {
        if (fieldSymbol.ContainingType.IsGenericType)
        {
            return null;
        }
        info.Property.Type = info.Field.Type.TrimEnd('?');
        if (!info.Usings.Contains("using System.Windows;"))
        {
            info.Usings = [.. info.Usings, "using System.Windows;"];
        }
        return base.ApplyExtraInfo(fieldSymbol, info);
    }

    protected override void AppendClass(StringBuilder sb, IEnumerable<FieldInfo?> infos)
    {
        StringBuilder dp = new();
        StringBuilder coerce = new();
        StringBuilder changed = new();
        StringBuilder clr = new();
        // プロパティ生成
        var second = false;
        var coerce_second = false;
        foreach (var info in infos)
        {
            if (second)
            {
                dp.AppendLine();
                changed.AppendLine();
                clr.AppendLine();
            }
            second = true;
            var className = info!.ClassName;
            var ((fieldType, fieldName, isValueType, isNullable), defaultValue, (propertyType, propertyName), setterScope, coerceHandler, changedHandler, _) = info;
            var conv = isValueType ? $"({fieldType}){{0}}" : $"{{0}} as {propertyType}";
            var defaultText = string.IsNullOrEmpty(defaultValue) ? $"default({propertyType})" : string.Format(conv, defaultValue);
            var options = info.MetadataOptions;
            var isPublic = info.IsSetterPublic;
            if (isPublic)
            {
                dp.AppendLine($$"""
                                public static readonly DependencyProperty {{propertyName}}Property = DependencyProperty.Register(
                        """);
            }
            else
            {
                dp.AppendLine($$"""
                                {{setterScope}}static readonly DependencyPropertyKey {{propertyName}}PropertyKey = DependencyProperty.RegisterReadOnly(
                        """);
            }
            dp.AppendLine($$"""
                                    "{{propertyName}}",
                                    typeof({{propertyType}}),
                                    typeof({{className}}),
                                    new FrameworkPropertyMetadata(
                                        {{defaultText}},
                                        (FrameworkPropertyMetadataOptions){{(int)options}},
                                        On{{propertyName}}Changed{{(coerceHandler is not CoerceType.None ? $", Coerce{propertyName}" : "")}}
                                        )
                        """);
            if (!isValueType && !isNullable)
            {
                dp.AppendLine($$"""
                                    , value => value is not null
                        """);
            }
            dp.AppendLine($$"""
                                    );
                        """);
            if (!info.IsSetterPublic)
            {
                dp.AppendLine($$"""
                                public static readonly DependencyProperty {{propertyName}}Property = {{propertyName}}PropertyKey.DependencyProperty;
                        """);
            }

            if (coerceHandler is not CoerceType.None)
            {
                if (coerce_second)
                {
                    coerce.AppendLine();
                }
                coerce_second = true;
                coerce.AppendLine($$"""
                                public static object Coerce{{propertyName}}(DependencyObject d, object baseValue)
                                {
                        """);
                if (coerceHandler is CoerceType.Static)
                {
                    coerce.AppendLine($$"""
                                    return Coerce{{propertyName}}({{string.Format(conv, "baseValue")}});
                        """);
                }
                else
                {
                    coerce.AppendLine($$"""
                                    return (d as {{className}})!.Coerce{{propertyName}}({{string.Format(conv, "baseValue")}});
                        """);
                }
                coerce.AppendLine($$"""
                                }
                        """);
            }

            changed.AppendLine($$"""  
                                private static void On{{propertyName}}Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
                                {
                                    if (d is {{className}} obj)
                                    {
                                        var value = {{string.Format(conv, "e.NewValue")}};
                                        obj.{{fieldName}} = value;
                        """);
            if (changedHandler is 1)
            {
                changed.AppendLine($$"""
                                        obj.On{{propertyName}}Changed(value);
                        """);
            }
            else if (changedHandler is 2)
            {
                changed.AppendLine($$"""
                                        obj.On{{propertyName}}Changed({{string.Format(conv, "e.OldValue")}}, value);
                        """);
            }
            if (IsFlagSet(options, FrameworkPropertyMetadataOptions.AffectsMeasure))
            {
                changed.AppendLine($$"""
                                        obj.InvalidateMeasure();
                        """);
            }
            else if (IsFlagSet(options, FrameworkPropertyMetadataOptions.AffectsArrange))
            {
                changed.AppendLine($$"""
                                        obj.InvalidateArrange();
                        """);
            }
            else if (IsFlagSet(options, FrameworkPropertyMetadataOptions.AffectsRender))
            {
                changed.AppendLine($$"""
                                        obj.InvalidateVisual();
                        """);
            }
            changed.AppendLine($$"""
                                    }
                                }
                        """);

            foreach (var attribute in info.Attributes)
            {
                clr.AppendLine($$"""
                                {{attribute}}
                        """);
            }
            clr.AppendLine($$"""
                                public {{fieldType}} {{propertyName}}
                                {
                                    get => {{fieldName}};
                                    {{setterScope}}set => SetValue({{propertyName}}Property{{(isPublic ? "" : "Key")}}, value);
                                }
                        """);
        }

        sb.AppendLine(dp.ToString());
        if (coerce.Length is > 0)
        {
            sb.AppendLine(coerce.ToString());
        }
        sb.AppendLine(changed.ToString());
        sb.Append(clr.ToString());
    }
}