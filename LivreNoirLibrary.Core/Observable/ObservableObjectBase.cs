using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Observable
{
    public abstract class ObservableObjectBase : INotifyPropertyChanged
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> _args_cache = [];
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            if (!_args_cache.TryGetValue(propertyName, out var args))
            {
                args = new(propertyName);
                _args_cache.Add(propertyName, args);
            }
            return args;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;
            SendPropertyChanged(propertyName);
            return true;
        }

        protected void SendPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, GetPropertyChangedEventArgs(propertyName));
        }
    }
}
