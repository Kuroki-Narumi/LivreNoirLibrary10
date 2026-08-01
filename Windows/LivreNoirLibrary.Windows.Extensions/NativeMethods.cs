using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Diagnostics;
using LivreNoirLibrary.Win32Api;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static nint GetHandle(this Window window) => new WindowInteropHelper(window).Handle;
        public static nint GetHandle(this DependencyObject depObj) => GetHandle(Window.GetWindow(depObj));

        public static nint GetCurrentHandle()
        {
            if (Application.Current is Application app && app.MainWindow is Window w && w.CheckAccess())
            {
                return GetHandle(w);
            }
            using var process = Process.GetCurrentProcess();
            return process.MainWindowHandle;
        }

        public static (int Width, int Height) GetSystemIconSize(bool small = false) => 
            small ? (NativeMethods.GetSystemMetrics(SM.CXSMICON), NativeMethods.GetSystemMetrics(SM.CYSMICON))
                  : (NativeMethods.GetSystemMetrics(SM.CXICON), NativeMethods.GetSystemMetrics(SM.CYICON));

        public static BitmapSource? GetIcon(nint handle, bool small = false)
        {
            var ptr = NativeMethods.SendMessage(handle, WM.GetIcon, small ? 2 : 1, 0);
            if (ptr is 0)
            {
                ptr = (nint)NativeMethods.GetClassLong(handle, small ? GCL.Handle_IconSmall : GCL.Handle_Icon);
            }
            if (ptr is not 0)
            {
                var source = Imaging.CreateBitmapSourceFromHIcon(ptr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return source;
            }
            return null;
        }

        public static BitmapSource? GetApplicationIcon(bool small = false) => GetIcon(GetCurrentHandle(), small);

        public static nint SetSlipThrough(this Window window, bool through)
        {
            var handle = GetHandle(window);
            var style = NativeMethods.GetWindowLong(handle, GWL.ExStyle);
            var nStyle = through
                ? style | (int)WS_EX.Transparent
                : style & ~(int)WS_EX.Transparent;
            return NativeMethods.SetWindowLong(handle, GWL.ExStyle, nStyle);
        }

        public static bool SetRect(this Window window, double x, double y, double width, double height)
        {
            return NativeMethods.SetWindowPos(GetHandle(window), nint.Zero, (int)x, (int)y, (int)Math.Ceiling(width), (int)Math.Ceiling(height), 0);
        }

        public static bool SetRect(this Window window, in Rect rect) => SetRect(window, rect.X, rect.Y, rect.Width, rect.Height);

        public static void ShellExecute(this Window window, string? operation = null, string? file = null, string? parameters = null, string? directory = null, SW showCmd = SW.ShowNormal)
        {
            NativeMethods.ShellExecute(GetHandle(window), operation, file, parameters, directory, showCmd);
        }

        public static void ShellOpen(this Window window, string path) => ShellExecute(window, "open", path, null, null, SW.ShowNormal);
    }
}
