using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public interface IVariableProvider
    {
        public bool TryGetOption(string key, [MaybeNullWhen(false)]out string value);
        public bool TryGetVariable(string key, [MaybeNullWhen(false)]out string value);
    }
}
