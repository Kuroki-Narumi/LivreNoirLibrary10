using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows
{
    public static class CaptureService
    {
        public static ObservableList<WindowInfo> WindowInfos { get; } = [];
        public static ObservableList<MonitorInfo> MonitorInfos { get; } = [];

        public static void RefreshInfo()
        {
            NativeMethods.GetWindowInfo(WindowInfos);
            NativeMethods.GetMonitorInfo(MonitorInfos);
            MonitorInfos.NotifyCollectionReset();
        }
    }
}
