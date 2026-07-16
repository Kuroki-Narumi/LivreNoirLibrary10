using LivreNoirLibrary.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Collections
{
    public interface IObservableCollection : IObservableObject, INotifyCollectionChanged, IClear
    {
        void RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }

    public static partial class IObservableCollectionExtensions
    {
        public static NotifyCollectionChangedEventArgs CollectionResetEventArgs { get; } = new(NotifyCollectionChangedAction.Reset);

        public static void NotifyCountChanged(this IObservableCollection obj) => obj.NotifyPropertyChanged(nameof(ICollection.Count));

        public static void NotifyCollectionAdded(this IObservableCollection obj, int index, object? addedItem)
        {
            NotifyCollectionChangedEventArgs e = new(NotifyCollectionChangedAction.Add, addedItem, index);
            obj.RaiseCollectionChanged(obj, e);
            obj.NotifyCountChanged();
        }

        public static void NotifyCollectionReplaced(this IObservableCollection obj, int index, object? oldItem, object? newItem)
        {
            NotifyCollectionChangedEventArgs e = new(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
            obj.RaiseCollectionChanged(obj, e);
        }

        public static void NotifyCollectionRemoved(this IObservableCollection obj, int index, object? removedItem = null)
        {
            NotifyCollectionChangedEventArgs e = new(NotifyCollectionChangedAction.Remove, removedItem, index);
            obj.RaiseCollectionChanged(obj, e);
            obj.NotifyCountChanged();
        }

        public static void NotifyCollectionMoved(this IObservableCollection obj, int oldIndex, int newIndex, object? movedItem)
        {
            NotifyCollectionChangedEventArgs e = new(NotifyCollectionChangedAction.Move, movedItem, newIndex, oldIndex);
            obj.RaiseCollectionChanged(obj, e);
        }

        public static void NotifyCollectionReset(this IObservableCollection obj)
        {
            obj.NotifyCountChanged();
            obj.RaiseCollectionChanged(obj, CollectionResetEventArgs);
        }
    }
}
