using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.ObjectModel
{
    public class GenericPropertyBase<T> : ObservableObjectBase
        where T : IEquatable<T>
    {
        private readonly T _defaultValue;
        private readonly Dictionary<string, T> _defaultValues = [];
        private readonly Dictionary<string, T> _values = [];

        public GenericPropertyBase()
        {
            _defaultValue = GetDefaultValue();
            InitializeDefaultValues(_defaultValues);
        }

        protected virtual T GetDefaultValue() => default!;
        protected virtual void InitializeDefaultValues(Dictionary<string, T> defaultValues) { }

        protected T GetValue([CallerMemberName] string key = "") => _values.TryGetValue(key, out var value) || _defaultValues.TryGetValue(key, out value) ? value : _defaultValue;

        protected bool SetValue(T value, [CallerMemberName] string key = "")
        {
            if (!_values.TryAdd(key, value))
            {
                if (_values[key].Equals(value))
                {
                    return false;
                }
                _values[key] = value;
            }
            this.NotifyPropertyChanged(key);
            return true;
        }

        protected bool SetValue(T value, Action<T>? callback, [CallerMemberName] string key = "")
        {
            var result = SetValue(value, key);
            if (result)
            {
                callback?.Invoke(value);
            }
            return true;
        }

        protected bool SetValue(T value, ReadOnlySpan<string> relatedProperties, [CallerMemberName] string key = "")
        {
            var result = SetValue(value, key);
            if (result)
            {
                foreach (var prop in relatedProperties)
                {
                    this.NotifyPropertyChanged(prop);
                }
            }
            return true;
        }

        protected bool SetValue(T value, ReadOnlySpan<string> relatedProperties, Action<T>? callback, [CallerMemberName] string key = "")
        {
            var result = SetValue(value, key);
            if (result)
            {
                callback?.Invoke(value);
                foreach (var prop in relatedProperties)
                {
                    this.NotifyPropertyChanged(prop);
                }
            }
            return true;
        }

        public void ClearValues()
        {
            _values.Clear();
        }

        public void Load<TDic>(TDic source)
            where TDic : IDictionary<string, T>
        {
            foreach (var (key, value) in source)
            {
                SetValue(value, key);
            }
        }

        public IEnumerable<(string, T)> EnumerateValues()
        {
            foreach (var (k, v) in _values)
            {
                yield return (k, v);
            }
        }
    }
}
