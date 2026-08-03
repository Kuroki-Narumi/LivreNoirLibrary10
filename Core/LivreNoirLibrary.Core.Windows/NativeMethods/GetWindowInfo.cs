using System;
using System.Collections.Generic;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.System.Threading;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Buffers;

namespace LivreNoirLibrary.Win32Api
{
    public record WindowInfo(nint Handle, string Title, string ExePath, Rectangle Rect)
    {
        public string ExeFileName => Path.GetFileName(ExePath);
        public string TitleAndExeName => $"{Title} ({ExeFileName})";
    }

    partial class NativeMethods
    {
        /// <summary>
        /// 現在のデスクトップに存在する全ての表示済みのウィンドウを列挙します。
        /// </summary>
        /// <param name="enumFunc">列挙されたウィンドウ情報を処理するデリゲート。列挙を継続する場合は<see langword="true"/>、終了する場合は<see langword="false"/>を返す。</param>
        public static void EnumerateWindowInfo(Func<WindowInfo, bool> enumFunc)
        {
            PInvoke.EnumWindows((hWnd, _) =>
            {
                try
                {
                    if (!IsCandidateWindow(hWnd, out var title) || !TryGetWindowRect(hWnd, out var rect))
                    {
                        return true;
                    }

                    var width = rect.Width;
                    var height = rect.Height;
                    if (width <= 0 || height <= 0)
                    {
                        return true;
                    }

                    var exePath = GetExePath(hWnd) ?? "";
                    return enumFunc(new WindowInfo(hWnd, title, exePath, rect));
                }
                catch
                {
                    // 個々のウィンドウでの例外は無視して列挙を継続する
                }
                return true;
            }, 0);
        }

        public static bool TryGetWindowInfo(Predicate<WindowInfo> predicate, [NotNullWhen(true)]out WindowInfo? info)
        {
            WindowInfo? result = null;
            EnumerateWindowInfo(info =>
            {
                if (predicate(info))
                {
                    result = info;
                    return false;
                }
                return true;
            });
            info = result;
            return result is not null;
        }

        public static ICollection<WindowInfo> GetWindowInfo(ICollection<WindowInfo>? results = null)
        {
            results ??= [];
            results.Clear();
            EnumerateWindowInfo(info =>
            {
                results.Add(info);
                return true;
            });
            return results;
        }

        private static bool IsCandidateWindow(HWND hWnd, [NotNullWhen(true)] out string? title)
        {
            title = null;

            if (!PInvoke.IsWindowVisible(hWnd))
            {
                return false;
            }

            /*
            // トップレベル(祖先が自分自身)のみ対象
            if (PInvoke.GetAncestor(hWnd, GET_ANCESTOR_FLAGS.GA_ROOT) != hWnd)
            {
                Console.WriteLine(hWnd);
                return false;
            }
            //*/

            /*
            // オーナーウィンドウを持つもの(ダイアログ等)は除外
            if (PInvoke.GetWindow(hWnd, GET_WINDOW_CMD.GW_OWNER) != 0)
            {
                return false;
            }
            //*/

            var exStyle = PInvoke.GetWindowLongPtr(hWnd, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
            var isToolWindow = (exStyle & (int)WS_EX.ToolWindow) != 0;
            var isAppWindow = (exStyle & (int)WS_EX.AppWindow) != 0;
            if (isToolWindow && !isAppWindow)
            {
                return false;
            }

            // 仮想デスクトップ切替や UWP サスペンドなどで見えないウィンドウを除外
            if (DwmGetWindowAttribute(hWnd, DWMWA.CLOAKED, out int value) && value is not 0)
            {
                return false;
            }

            title = GetWindowText(hWnd);
            return !string.IsNullOrWhiteSpace(title);
        }

        private static string? GetExePath(HWND hWnd)
        {
            PInvoke.GetWindowThreadProcessId(hWnd, out uint pid);
            if (pid == 0) return null;
            using var hProcess = PInvoke.OpenProcess_SafeHandle(PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_LIMITED_INFORMATION, false, pid);
            if (hProcess is { IsInvalid: false })
            {
                unsafe
                {
                    var buffer = (stackalloc char[1024]);
                    var len = (uint)buffer.Length;
                    if (PInvoke.QueryFullProcessImageName(hProcess, PROCESS_NAME_FORMAT.PROCESS_NAME_WIN32, buffer, ref len))
                    {
                        return buffer[..(int)len].ToString();
                    }
                    if ((WIN32_ERROR)Marshal.GetLastPInvokeError() is WIN32_ERROR.ERROR_HV_INSUFFICIENT_BUFFER)
                    {
                        var ary = ArrayPool<char>.Shared.Rent(32768);
                        len = (uint)ary.Length;
                        PInvoke.QueryFullProcessImageName(hProcess, PROCESS_NAME_FORMAT.PROCESS_NAME_WIN32, ary, ref len);
                        return buffer[..(int)len].ToString();
                    }
                }
            }
            return null;
        }

        public static bool TryGetWindowHandleByTitle(Predicate<string?> predicate, out nint handle) => TryGetWindowHandle(GetWindowText, predicate, out handle);

        public static bool TryGetWindowHandleByExePath(Predicate<string?> predicate, out nint handle) => TryGetWindowHandle(GetExePath, predicate, out handle);

        private static bool TryGetWindowHandle(Func<HWND, string?> getter, Predicate<string?> predicate, out nint handle)
        {
            nint h = 0;
            PInvoke.EnumWindows((hWnd, _) =>
            {
                var path = getter(hWnd);
                if (predicate(path))
                {
                    h = (nint)hWnd;
                    return false;
                }
                return true;
            }, 0);

            handle = h;
            return handle is not 0;
        }
    }
}
