using System;

namespace LivreNoirLibrary.Windows
{
    public delegate void EditEventHandler(object sender);

    public interface IHistoryOwner
    {
        public IHistory History { get; }
    }

    public interface IHistoryOwner<T> : IHistoryOwner
    {
        public T GetHistoryData();
        public bool NeedsUpdateHistory(T historyData);
        public void ApplyHistory(T historyData);
    }

    public static class IHistoryOwnerExtensions
    {
        public static void OnEdit(this IHistoryOwner obj, object sender)
        {
            obj.History.PushUndo();
        }
    }
}
