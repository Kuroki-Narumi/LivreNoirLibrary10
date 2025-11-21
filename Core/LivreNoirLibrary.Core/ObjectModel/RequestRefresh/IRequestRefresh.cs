using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IRequestRefresh
    {
        public event RequestRefreshEventHandler? RequestRefresh;
    }
}
