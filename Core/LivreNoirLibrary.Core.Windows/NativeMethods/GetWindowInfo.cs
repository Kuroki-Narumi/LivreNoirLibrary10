using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Dwm;
using System.Runtime.InteropServices;
using System.IO;

namespace LivreNoirLibrary.Win32Api
{
    public record WindowInfo(nint Handle, string Title, string ExePath, Rectangle Rect)
    {
        public string ExeFileName => Path.GetFileName(ExePath);
    }

    partial class NativeMethods
    {
        public static ICollection<WindowInfo> GetWindowInfo(ICollection<WindowInfo>? results = null)
        {
            results ??= [];
            results.Clear();

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
                    results.Add(new WindowInfo(hWnd, title, exePath, rect));
                }
                catch
                {
                    // 個々のウィンドウでの例外は無視して列挙を継続する
                }
                return true;
            }, 0);

            return results;
        }

        private static bool IsCandidateWindow(HWND hWnd, out string title)
        {
            title = string.Empty;

            if (!PInvoke.IsWindowVisible(hWnd))
            {
                return false;
            }

            //*
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

            title = GetWindowText(hWnd) ?? "";
            return !string.IsNullOrWhiteSpace(title);
        }

        private static string? GetExePath(HWND hWnd)
        {
            PInvoke.GetWindowThreadProcessId(hWnd, out uint pid);
            if (pid == 0) return null;
            try
            {
                using var process = System.Diagnostics.Process.GetProcessById((int)pid);
                return process.MainModule?.FileName;
            }
            catch
            {
                return null;
            }
        }
    }
}
