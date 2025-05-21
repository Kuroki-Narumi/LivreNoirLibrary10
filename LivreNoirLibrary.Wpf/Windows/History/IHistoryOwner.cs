using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows
{
    public delegate void EditEventHandler(object sender);

    public interface IHistoryOwner<TSelf, TData>
        where TSelf : IHistoryOwner<TSelf, TData>
    {
        public History<TSelf, TData> History { get; }

        public TData GetHistoryData();
        public void ApplyHistoryData(TData historyData);
    }

    public static class IHistoryOwnerExtensions
    {
        public static void OnBeforeEdit<TSelf, TData>(this TSelf obj, object sender)
            where TSelf : IHistoryOwner<TSelf, TData>
        {
            obj.History.BeforeEdit();
        }

        public static void OnAfterEdit<TSelf, TData>(this TSelf obj, object sender)
            where TSelf : IHistoryOwner<TSelf, TData>
        {
            obj.History.AfterEdit();
        }
    }
}
