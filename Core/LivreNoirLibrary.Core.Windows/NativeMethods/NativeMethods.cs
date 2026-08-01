using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using System.Buffers;
using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Dwm;

namespace LivreNoirLibrary.Win32Api
{
    public static partial class NativeMethods
    {
        public static bool RegisterHotKey(nint hWnd, int id, int modKey, int vKey) => PInvoke.RegisterHotKey((HWND)hWnd, id, (HOT_KEY_MODIFIERS)modKey, (uint)vKey);
        public static bool UnregisterHotKey(nint hWnd, int id) => PInvoke.UnregisterHotKey((HWND)hWnd, id);

        public static bool GetKeyboardState<T>(Span<T> buffer)
            where T : unmanaged
        {
            unsafe
            {
                var byteSpan = MemoryMarshal.Cast<T, byte>(buffer);
                if (byteSpan.Length < 256)
                {
                    ThrowKeyboardBufferException();
                }
                return PInvoke.GetKeyboardState(MemoryMarshal.Cast<T, byte>(buffer));
            }
        }

        private static void ThrowKeyboardBufferException()
        {
            throw new ArgumentException($"the buffer must be >= 256 bytes.");
        }

        public static int GetSystemMetrics(SM index) => PInvoke.GetSystemMetrics((SYSTEM_METRICS_INDEX)index);

        public static nint SendMessage(nint hWnd, WM message, nint wParam, nint lParam) => PInvoke.SendMessage((HWND)hWnd, (uint)message, (WPARAM)(nuint)wParam, lParam);

        public static nuint GetClassLong(nint hWnd, GCL nIndex) => PInvoke.GetClassLongPtr((HWND)hWnd, (GET_CLASS_LONG_INDEX)nIndex);
        public static nint GetWindowLong(nint hWnd, GWL nIndex) => PInvoke.GetWindowLongPtr((HWND)hWnd, (WINDOW_LONG_PTR_INDEX)nIndex);
        public static nint SetWindowLong(nint hWnd, GWL nIndex, nint newLong) => PInvoke.SetWindowLongPtr((HWND)hWnd, (WINDOW_LONG_PTR_INDEX)nIndex, newLong);

        public static bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int x, int y, int width, int height, SWP uFlags) 
            => PInvoke.SetWindowPos((HWND)hWnd, (HWND)hWndInsertAfter, x, y, width, height, (SET_WINDOW_POS_FLAGS)uFlags);

        public static void ShellExecute(nint hWnd, string? operation, string? file, string? parameters, string? directory, SW showCommand) 
            => PInvoke.ShellExecute((HWND)hWnd, operation, file, parameters, directory, (SHOW_WINDOW_CMD)showCommand);

        public static string? GetWindowText(nint hWnd)
        {
            var length = PInvoke.GetWindowTextLength((HWND)hWnd);
            if (length <= 0)
            {
                return null;
            }
            var buffer = length < 512 ? (stackalloc char[length + 1]) : new char[length + 1];
            var written = PInvoke.GetWindowText((HWND)hWnd, buffer);
            if (written <= 0)
            {
                return null;
            }
            return buffer[..written].ToString();
        }

        public static bool IsWindow(nint hWnd) => PInvoke.IsWindow((HWND)hWnd);

        public static nint GetWindow(nint hWnd, GW uCmd) => PInvoke.GetWindow((HWND)hWnd, (GET_WINDOW_CMD)uCmd);
        public static nint GetAncestor(nint hWnd, GA gaFlags) => PInvoke.GetAncestor((HWND)hWnd, (GET_ANCESTOR_FLAGS)gaFlags);

        public static bool DwmGetWindowAttribute<T>(nint hWnd, DWMWA attribute, out T result)
            where T : unmanaged
        {
            unsafe
            {
                T r;
                if (PInvoke.DwmGetWindowAttribute((HWND)hWnd,(DWMWINDOWATTRIBUTE)attribute, &r, (uint)sizeof(T)).Succeeded)
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static unsafe bool TryGetWindowRect(nint hWnd, out Rectangle rect)
        {
            RECT r;
            var ret = PInvoke.DwmGetWindowAttribute((HWND)hWnd, DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, &r, (uint)sizeof(RECT));
            if (ret.Succeeded)
            {
                rect = new(r.left, r.top, r.Width, r.Height);
                return true;
            }
            rect = default;
            return false;
        }

        public static unsafe bool TryGetWindowRect(nint hWnd, out Rectangle windowRect, out Rectangle clientRect)
        {
            Point point;
            var h = (HWND)hWnd;
            if (TryGetWindowRect(hWnd, out windowRect) && PInvoke.GetClientRect(h, out var cr) && PInvoke.ClientToScreen(h, &point))
            {
                clientRect = new(point.X - windowRect.X, point.Y - windowRect.Y, cr.Width, cr.Height);
                return true;
            }
            windowRect = default;
            clientRect = default;
            return false;
        }

        public static bool TryGetClientToScreen(nint hWnd, out Point point)
        {
            point = default;
            return PInvoke.ClientToScreen((HWND)hWnd, ref point);
        }
    }
}
