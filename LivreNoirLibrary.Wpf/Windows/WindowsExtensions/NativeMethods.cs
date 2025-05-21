using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Diagnostics;
using LivreNoirLibrary.Windows.NativeMethods;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static nint GetHandle(this Window window)
        {
            return new WindowInteropHelper(window).Handle;
        }
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
            small ? (Win32Api.GetSystemMetrics(SM.CXSMICON), Win32Api.GetSystemMetrics(SM.CYSMICON))
                  : (Win32Api.GetSystemMetrics(SM.CXICON), Win32Api.GetSystemMetrics(SM.CYICON));

        public static BitmapSource? GetIcon(nint handle, bool small = false)
        {
            var ptr = Win32Api.SendMessage(handle, WM.GetIcon, small ? 2 : 1, 0);
            if (ptr is 0)
            {
                ptr = Win32Api.GetClassLong(handle, small ? GCL.Handle_IconSmall : GCL.Handle_Icon);
            }
            if (ptr is not 0)
            {
                return BitmapFrame.Create(Imaging.CreateBitmapSourceFromHIcon(ptr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
            }
            return null;
        }

        public static BitmapSource? GetApplicationIcon(bool small = false) => GetIcon(GetCurrentHandle(), small);

        public static int SetSlipThrough(this Window window, bool through)
        {
            var handle = GetHandle(window);
            var style = Win32Api.GetWindowLong(handle, (int)GWL.ExStyle);
            var nStyle = through
                ? style | (int)WS_EX.Transparent
                : style & ~(int)WS_EX.Transparent;
            return Win32Api.SetWindowLong(handle, (int)GWL.ExStyle, nStyle);
        }

        public static bool SetRect(this Window window, double x, double y, double width, double height)
        {
            return Win32Api.SetWindowPos(GetHandle(window), nint.Zero, (int)x, (int)y, (int)Math.Ceiling(width), (int)Math.Ceiling(height), 0);
        }

        public static bool SetRect(this Window window, in Rect rect)
        {
            return SetRect(window, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void ShellExecute(this Window window, string? operation = null, string? file = null, string? parameters = null, string? directory = null, SW showCmd = SW.ShowNormal)
        {
            Win32Api.ShellExecute(GetHandle(window), operation, file, parameters, directory, showCmd);
        }

        public static void ShellOpen(this Window window, string path)
        {
            ShellExecute(window, "open", path, null, null, SW.ShowNormal);
        }

        public static Point GetCursorPos()
        {
            if (Win32Api.GetCursorPos(out var p))
            {
                return new(p.X, p.Y);
            }
            return default;
        }

        public static Point GetCursorPos(this DependencyObject obj) => GetCursorPos();
    }
}
