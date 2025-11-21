using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsExtensions
    {
        public delegate bool TryGetFunc<T>(IBmsDataUnit data, out T value);

        extension (IBmsViewModel vm)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryGetInheritedValue<T>(TryGetFunc<T> func, [MaybeNullWhen(false)] out T value, bool containsCurrent = true)
            {
                if (containsCurrent && func(vm.CurrentData, out value))
                {
                    return true;
                }
                foreach (var data in vm.EnumerateParents())
                {
                    if (func(data, out value))
                    {
                        return true;
                    }
                }
                value = default;
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: NotNullIfNotNull(nameof(ifNone))]
            public T? GetInheritedValue<T>(TryGetFunc<T> func, T? ifNone = default, bool containsCurrent = true)
            {
                if (containsCurrent && func(vm.CurrentData, out var value))
                {
                    return value;
                }
                foreach (var data in vm.EnumerateParents())
                {
                    if (func(data, out value))
                    {
                        return value;
                    }
                }
                return ifNone;
            }
        }
    }
}
