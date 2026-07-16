using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract class ObservableObjectBase : IObservableObject
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            this.NotifyPropertyChanged(propertyName);
            return true;
        }

        protected bool SetValue<T>(ref T field, T value, ReadOnlySpan<string> relatedProperties, [CallerMemberName] string propertyName = "")
        {
            var result = SetValue(ref field, value, propertyName);
            if (result)
            {
                foreach (var prop in relatedProperties)
                {
                    this.NotifyPropertyChanged(prop);
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
                    this.NotifyPropertyChanged(prop);
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
                    this.NotifyPropertyChanged(prop);
                }
            }
            return result;
        }

        void IObservableObject.RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
    }
}
