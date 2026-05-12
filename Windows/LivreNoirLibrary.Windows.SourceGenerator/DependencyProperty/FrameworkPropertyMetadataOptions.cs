using System;

namespace LivreNoirLibrary.Windows.SourceGenerator
{
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
}
