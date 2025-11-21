using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract class ObservableObjectBase : INotifyPropertyChanged
    {
        private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> _args_cache = [];
        private readonly Func<string, PropertyChangedEventArgs> _args_create = n => new(n);

        public PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName) => _args_cache.GetOrAdd(propertyName, _args_create);

        public event PropertyChangedEventHandler? PropertyChanged;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            SendPropertyChanged(propertyName);
            return true;
        }

        protected bool SetValue<T>(ref T field, T value, ReadOnlySpan<string> relatedProperties, [CallerMemberName] string propertyName = "")
        {
            var result = SetValue(ref field, value, propertyName);
            if (result)
            {
                foreach (var prop in relatedProperties)
                {
                    SendPropertyChanged(prop);
                }
            }
            return result;
        }

        protected bool SetValue<T>(ref T field, T value, Action<T, T> changedHandler, [CallerMemberName] string propertyName = "")
        {
            var oldValue = field;
            var result = SetValue(ref field, value, propertyName);
            if (result)
            {
                changedHandler(oldValue, value);
            }
            return result;
        }

        protected bool SetValue<T>(ref T field, T value, ReadOnlySpan<string> relatedProperties, Action<T, T> changedHandler, [CallerMemberName] string propertyName = "")
        {
            var oldValue = field;
            var result = SetValue(ref field, value, propertyName);
            if (result)
            {
                changedHandler(oldValue, value);
                foreach (var prop in relatedProperties)
                {
                    SendPropertyChanged(prop);
                }
            }
            return result;
        }

        protected bool SetValue<T>(ref T field, T value, Action changedHandler, [CallerMemberName] string propertyName = "")
        {
            var result = SetValue(ref field, value, propertyName);
            if (result)
            {
                changedHandler();
            }
            return result;
        }

        protected bool SetValue<T>(ref T field, T value, ReadOnlySpan<string> relatedProperties, Action changedHandler, [CallerMemberName] string propertyName = "")
        {
            var result = SetValue(ref field, value, propertyName);
            if (result)
            {
                changedHandler();
                foreach (var prop in relatedProperties)
                {
                    SendPropertyChanged(prop);
                }
            }
            return result;
        }

        protected void SendPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, GetPropertyChangedEventArgs(propertyName));
        }
    }
}
