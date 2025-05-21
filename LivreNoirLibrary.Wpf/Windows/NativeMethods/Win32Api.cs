using System;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Windows.NativeMethods
{
    internal static partial class Win32Api
    {
        [LibraryImport("user32")]
        internal static partial int RegisterHotKey(nint hWnd, int id, int modKey, int vKey);
        [LibraryImport("user32")]
        internal static partial int UnregisterHotKey(nint hWnd, int id);

        [LibraryImport("kernel32")]
        internal static partial void RtlMoveMemory(nint dest, nint source, int length);

        [LibraryImport("user32", EntryPoint = $"{nameof(SendMessage)}W")]
        internal static partial nint SendMessage(nint hWnd, WM msg, nint wParam, nint lParam);

        [LibraryImport("user32", EntryPoint = $"{nameof(GetClassLong)}PtrW")]
        internal static partial nint GetClassLong(nint hWnd, GCL nIndex);

        [LibraryImport("user32", EntryPoint = nameof(GetSystemMetrics))]
        internal static partial int GetSystemMetrics(SM index);

        [LibraryImport("user32", EntryPoint = $"{nameof(GetWindowLong)}PtrW")]
        internal static partial int GetWindowLong(nint hWnd, int index);
        [LibraryImport("user32", EntryPoint = $"{nameof(SetWindowLong)}PtrW")]
        internal static partial int SetWindowLong(nint hWnd, int index, int newLong);

        [LibraryImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool SetWindowPos(nint hWnd, nint hWndInsertAfter, int x, int y, int width, int height, uint uFlags);

        [LibraryImport("shell32", EntryPoint = $"{nameof(ShellExecute)}W", StringMarshalling = StringMarshalling.Utf16)]
        internal static partial nint ShellExecute(nint hwnd, string? operation, string? file, string? parameters, string? directory, SW showCmd);

        [LibraryImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool GetCursorPos(out Int32Point point);
        internal readonly struct Int32Point(int x, int y)
        {
            public readonly int X = x;
            public readonly int Y = y;
        }
    }
}
