using System;

namespace LivreNoirLibrary.ObjectModel
{
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
        public static void OnEdit(this IHistoryOwner obj, bool force = false)
        {
            obj.History.PushUndo(force);
        }
    }
}
