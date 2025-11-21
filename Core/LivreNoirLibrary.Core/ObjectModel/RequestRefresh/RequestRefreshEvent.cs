using System;

namespace LivreNoirLibrary.ObjectModel
{
    public delegate void RequestRefreshEventHandler(object sender, RequestRefreshEventArgs e);

    public class RequestRefreshEventArgs(RequestRefreshType type) : EventArgs
    {
        public RequestRefreshType Type { get; } = type;

        public static RequestRefreshEventArgs RefreshAll { get; } = new(RequestRefreshType.RefreshAll);
        public static RequestRefreshEventArgs RefreshPosition { get; } = new(RequestRefreshType.RefreshPosition);
        public static RequestRefreshEventArgs Redraw { get; } = new(RequestRefreshType.Redraw);
    }
}
