using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IObservableObject : INotifyPropertyChanged
    {
        void RaisePropertyChanged(object sender, PropertyChangedEventArgs e);
    }

    public static partial class IObservableExtensions
    {
        internal static readonly Dictionary<string, PropertyChangedEventArgs> _cache = [];

        public static void NotifyPropertyChanged(this IObservableObject obj, [CallerMemberName] string proeprtyName = "")
        {
            var e = _cache.GetOrAdd(proeprtyName, static n => new(n));
            obj.RaisePropertyChanged(obj, e);
        }
    }
}
