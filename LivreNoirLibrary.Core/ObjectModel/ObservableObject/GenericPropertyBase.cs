using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.ObjectModel
{
    public class GenericPropertyBase<T> : ObservableObjectBase
        where T : IEquatable<T>
    {
        public virtual T? DefaultValue { get; } = default;
        public virtual Dictionary<string, T> DefaultValues { get; } = [];

        protected readonly Dictionary<string, T> _values = [];

        [return: MaybeNull]
        public T GetValue([CallerMemberName] string key = "")
        {
            return _values.TryGetValue(key, out var value) || DefaultValues.TryGetValue(key, out value) ? value : DefaultValue;
        }

        public bool SetValue(T value, [CallerMemberName] string key = "")
        {
            if (!_values.TryAdd(key, value))
            {
                if (_values[key].Equals(value))
                {
                    return false;
                }
                _values[key] = value;
            }
            SendPropertyChanged(key);
            return true;
        }

        public void Load<TDic>(TDic source)
            where TDic : IDictionary<string, T>
        {
            foreach (var (key, value) in source)
            {
                SetValue(value, key);
            }
        }
    }
}
