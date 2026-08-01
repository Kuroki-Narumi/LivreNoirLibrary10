using System;
using System.Security.Principal;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Win32Api
{
    public static partial class NativeMethods
    {
        public static bool RunAsInvoker(string processPath, string args = "")
        {
            if (new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                return RunAsInvokerImpl(processPath, args);
            }
            else
            {
                var proc = Process.Start(processPath, args);
                return proc is not null;
            }
        }

        [LibraryImport("advapi32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool OpenProcessToken(nint hProcess, uint dwAccess, out nint hToken);

        [LibraryImport("advapi32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool DuplicateTokenEx(nint hToken, uint dwAccess, nint lpAttr, int impersonation, int tyokenType, out nint hNewToken);

        [LibraryImport("advapi32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool CreateProcessWithTokenW(
            nint hToken,
            uint dwLogonFlags,
            string? lpApplicationName,
            string? lpCommandLine,
            uint dwCreationFlags,
            nint lpEnvironment,
            string? lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInfo);

        [LibraryImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool CloseHandle(nint hObject);

        [LibraryImport("kernel32")]
        private static partial nint OpenProcess(uint access, [MarshalAs(UnmanagedType.Bool)] bool inherit, int pid);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct STARTUPINFO
        {
            public int cb;
            public nint lpReserved, lpDesktop, lpTitle;
            public uint dwX, dwY, dwXSize, dwYSize, dwXCountChars, dwYCountChars;
            public uint dwFillAttribute, dwFlags;
            public short wShowWindow, cbReserved2;
            public nint lpReserved2, hStdInput, hStdOutput, hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public nint hProcess, hThread;
            public int dwProcessId, dwThreadId;
        }

        const uint TOKEN_DUPLICATE = 0x0002;
        const uint TOKEN_QUERY = 0x0008;
        const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        const uint TOKEN_ADJUST_DEFAULT = 0x0080;
        const uint TOKEN_ADJUST_SESSIONID = 0x0100;
        const uint PROCESS_QUERY_INFO = 0x0400;

        static bool RunAsInvokerImpl(string exePath, string args = "")
        {
            // 1. Explorerのプロセスから非昇格トークンを取得
            if (Process.GetProcessesByName("explorer")[0] is not { } explorer)
            {
                return false;
            }
            nint hExplorer = 0, hToken = 0, hNewToken = 0;
            try
            {
                hExplorer = OpenProcess(PROCESS_QUERY_INFO, false, explorer.Id);
                OpenProcessToken(hExplorer, TOKEN_DUPLICATE | TOKEN_QUERY, out hToken);

                // 2. トークンを複製
                DuplicateTokenEx(hToken,
                    TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE |
                    TOKEN_QUERY | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID,
                    0, 2, 1, out hNewToken);

                // 3. 複製したトークンでプロセスを起動
                var si = new STARTUPINFO { cb = Marshal.SizeOf<STARTUPINFO>() };
                var ret = CreateProcessWithTokenW(
                    hNewToken, 
                    0, 
                    exePath,
                    $"\"{exePath}\" {args}", 
                    0, 0, null, ref si, out _);
                return ret;
            }
            finally
            {
                // 4. ハンドルを閉じる
                CloseHandleSafe(ref hNewToken);
                CloseHandleSafe(ref hToken);
                CloseHandleSafe(ref hExplorer);
            }
        }

        private static void CloseHandleSafe(ref nint handle)
        {
            if (handle is not 0)
            {
                CloseHandle(handle);
                handle = 0;
            }
        }
    }
}
