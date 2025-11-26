using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IRequestRefresh
    {
        event RequestRefreshEventHandler? RequestRefresh;
    }
}
