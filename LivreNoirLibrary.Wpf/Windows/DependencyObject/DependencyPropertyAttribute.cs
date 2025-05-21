using System;
using System.Windows;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DependencyPropertyAttribute : Attribute
    {
        public string Name { get; set; } = "";
        public Scope SetterScope { get; set; } = Scope.Public;
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions"/>
        public FrameworkPropertyMetadataOptions MetadataOptions { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.AffectsMeasure"/>
        public bool AffectsMeasure { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.AffectsArrange"/>
        public bool AffectsArrange { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.AffectsParentMeasure"/>
        public bool AffectsParentMeasure { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.AffectsParentArrange"/>
        public bool AffectsParentArrange { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.AffectsRender"/>
        public bool AffectsRender { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.Inherits"/>
        public bool Inherits { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior"/>
        public bool OverridesInheritanceBehavior { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.NotDataBindable"/>
        public bool NotDataBindable { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.BindsTwoWayByDefault"/>
        public bool BindsTwoWayByDefault { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.Journal"/>
        public bool Journal { get; set; }
        /// <inheritdoc cref="FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender"/>
        public bool SubPropertiesDoNotAffectRender { get; set; }
    }
}