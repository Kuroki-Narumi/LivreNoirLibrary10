using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract class ObservableObjectBase : INotifyPropertyChanged
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> _args_cache = [];
        private static readonly Func<string, PropertyChangedEventArgs> _args_create = n => new(n);

        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName) => _args_cache.GetOrAdd(propertyName, _args_create);

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
