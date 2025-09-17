using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class PropertyUtils
    {
        [GeneratedRegex(@"Property(Key)?$", RegexOptions.CultureInvariant)]
        public static partial Regex PropertyRegex { get; }

        public static string GetPropertyName(string caller) => PropertyRegex.Replace(caller, "");

        public static DependencyProperty AddOwner<T>(this DependencyProperty property,
            Type ownerType,
            T? defaultValue = default,
            PropertyChangedCallback? callback = null,
            CoerceValueCallback? coerce = null
            )
            => property.AddOwner(ownerType, GetMeta(defaultValue, callback, coerce));

        public static DependencyProperty AddOwnerTwoWay<T>(this DependencyProperty property,
            Type ownerType,
            T? defaultValue = default,
            PropertyChangedCallback? callback = null,
            CoerceValueCallback? coerce = null
            )
            => property.AddOwner(ownerType, GetMetaTwoWay(defaultValue, callback, coerce));

        public static DependencyProperty Register<T>(
            Type ownerType, 
            T? defaultValue = default, 
            PropertyChangedCallback? callback = null, 
            CoerceValueCallback? coerce = null, 
            ValidateValueCallback? validate = null, 
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.Register(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, callback, coerce), validate);

        public static DependencyProperty RegisterTwoWay<T>(
            Type ownerType, 
            T? defaultValue = default, 
            PropertyChangedCallback? callback = null, 
            CoerceValueCallback? coerce = null, 
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.Register(GetPropertyName(caller), typeof(T), ownerType, GetMetaTwoWay(defaultValue, callback, coerce), validate);

        public static DependencyPropertyKey RegisterReadOnly<T>(
            Type ownerType, 
            T? defaultValue = default,
            PropertyChangedCallback? callback = null, 
            CoerceValueCallback? coerce = null,
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterReadOnly(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, callback, coerce), validate);

        public static DependencyProperty RegisterAttached<T>(
            Type ownerType, 
            T? defaultValue = default, 
            PropertyChangedCallback? callback = null, 
            CoerceValueCallback? coerce = null, 
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterAttached(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, callback, coerce), validate);

        public static DependencyProperty RegisterAttachedTwoWay<T>(
            Type ownerType, 
            T? defaultValue = default,
            PropertyChangedCallback? callback = null,
            CoerceValueCallback? coerce = null,
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterAttached(GetPropertyName(caller), typeof(T), ownerType, GetMetaTwoWay(defaultValue, callback, coerce), validate);

        public static DependencyPropertyKey RegisterAttachedReadOnly<T>(
            Type ownerType,
            T? defaultValue = default,
            PropertyChangedCallback? callback = null,
            CoerceValueCallback? coerce = null,
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterAttachedReadOnly(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, callback, coerce), validate);

        public static DependencyProperty RegisterVisual<T>(
            Type ownerType, 
            T? defaultValue = default, 
            PropertyChangedCallback? callback = null, 
            CoerceValueCallback? coerce = null, 
            ValidateValueCallback? validate = null, 
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.Register(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, callback, coerce), validate);

        public static DependencyProperty RegisterVisualTwoWay<T>(
            Type ownerType, 
            T? defaultValue = default, 
            CoerceValueCallback? coerce = null, 
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.Register(GetPropertyName(caller), typeof(T), ownerType, GetMetaTwoWay(defaultValue, OnVisualPropertyChanged, coerce), validate);

        public static DependencyPropertyKey RegisterVisualReadOnly<T>(
            Type ownerType, 
            T? defaultValue = default,
            CoerceValueCallback? coerce = null,
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterReadOnly(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, OnVisualPropertyChanged, coerce), validate);

        public static DependencyProperty RegisterVisualAttached<T>(
            Type ownerType, 
            T? defaultValue = default, 
            CoerceValueCallback? coerce = null, 
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterAttached(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, OnVisualPropertyChanged, coerce), validate);

        public static DependencyProperty RegisterVisualAttachedTwoWay<T>(
            Type ownerType, 
            T? defaultValue = default,
            CoerceValueCallback? coerce = null,
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterAttached(GetPropertyName(caller), typeof(T), ownerType, GetMetaTwoWay(defaultValue, OnVisualPropertyChanged, coerce), validate);

        public static DependencyPropertyKey RegisterVisualAttachedReadOnly<T>(
            Type ownerType,
            T? defaultValue = default,
            CoerceValueCallback? coerce = null,
            ValidateValueCallback? validate = null,
            [CallerMemberName] string caller = ""
            )
            => DependencyProperty.RegisterAttachedReadOnly(GetPropertyName(caller), typeof(T), ownerType, GetMeta(defaultValue, OnVisualPropertyChanged, coerce), validate);
    }
}
