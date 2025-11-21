using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public interface IVariableProvider
    {
        bool TryGetOption(string key, [MaybeNullWhen(false)]out string value);
        bool TryGetVariable(string key, [MaybeNullWhen(false)]out string value);
    }
}
