using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Win32Api;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace LivreNoirLibrary.Media
{
    public static class CaptureService
    {
        public static ObservableList<WindowInfo> WindowInfos { get; } = [];
        public static ObservableList<MonitorInfo> MonitorInfos { get; } = [];

        public static void RefreshInfo()
        {
            NativeMethods.GetWindowInfo(WindowInfos);
            NativeMethods.GetMonitorInfo(MonitorInfos);
        }

        public static double GetDpiForWindow(nint handle) => PInvoke.GetDpiForWindow((HWND)handle);
    }
}
