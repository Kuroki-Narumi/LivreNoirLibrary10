using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Windows.Win32;
using Windows.Win32.Devices.Display;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace LivreNoirLibrary.Win32Api
{
    public record MonitorInfo(nint Handle, Rectangle Rect, bool IsPrimary, string DeviceName, string? FriendlyName);

    partial class NativeMethods
    {
        /// <summary>
        /// 現在のシステムに接続されている全てのモニターを列挙します。
        /// </summary>
        /// <param name="enumFunc">列挙されたモニター情報を処理するデリゲート。列挙を継続する場合は<see langword="true"/>、終了する場合は<see langword="false"/>を返す。</param>
        public static void EnumerateMonitorInfo(Func<MonitorInfo, bool> enumFunc)
        {
            UpdateFriendlyNameMap();
            unsafe
            {
                PInvoke.EnumDisplayMonitors(default, null, (hMonitor, _, _, _) =>
                {
                    var info = CreateMonitorInfo(hMonitor);
                    return enumFunc(info);
                }, 0);
            }
        }

        public static MonitorInfo MonitorFromWindow(nint handle)
        {
            var hMonitor = PInvoke.MonitorFromWindow((HWND)handle, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);
            return CreateMonitorInfo(hMonitor);
        }

        private static MonitorInfo CreateMonitorInfo(HMONITOR handle)
        {
            unsafe
            {
                var deviceName = "";
                var isPrimary = false;
                MONITORINFOEXW info = new();
                info.monitorInfo.cbSize = (uint)sizeof(MONITORINFOEXW);
                Rectangle rect = default;
                if (PInvoke.GetMonitorInfo(handle, (MONITORINFO*)&info))
                {
                    isPrimary = (info.monitorInfo.dwFlags & 1) is not 0;
                    deviceName = info.szDevice.ToString();
                    var r = info.monitorInfo.rcMonitor;
                    rect = r.ToRectangle();
                }
                var friendlyName = _device2friendly.GetValueOrDefault(deviceName);
                return new((nint)handle, rect, isPrimary, deviceName, friendlyName);
            }
        }

        private static readonly Dictionary<string, string> _device2friendly = [];
        private static void UpdateFriendlyNameMap()
        {
            var dic = _device2friendly;

            PInvoke.GetDisplayConfigBufferSizes(QUERY_DISPLAY_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, out var pathCount, out var modeCount);
            unsafe
            {
                var paths = stackalloc DISPLAYCONFIG_PATH_INFO[(int)pathCount];
                var modes = stackalloc DISPLAYCONFIG_MODE_INFO[(int)modeCount];
                var result = PInvoke.QueryDisplayConfig(QUERY_DISPLAY_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, &pathCount, paths, &modeCount, modes, null);
                if (result is WIN32_ERROR.NO_ERROR)
                {
                    foreach (var path in new Span<DISPLAYCONFIG_PATH_INFO>(paths, (int)pathCount))
                    {
                        DISPLAYCONFIG_SOURCE_DEVICE_NAME source = new();
                        source.header.size = (uint)sizeof(DISPLAYCONFIG_SOURCE_DEVICE_NAME);
                        source.header.adapterId = path.sourceInfo.adapterId;
                        source.header.id = path.sourceInfo.id;
                        source.header.type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME;

                        if (PInvoke.DisplayConfigGetDeviceInfo(&source.header) is not 0)
                        {
                            continue;
                        }
                        var deviceName = source.viewGdiDeviceName.ToString();

                        DISPLAYCONFIG_TARGET_DEVICE_NAME target = new();
                        target.header.size = (uint)sizeof(DISPLAYCONFIG_TARGET_DEVICE_NAME);
                        target.header.adapterId = path.targetInfo.adapterId;
                        target.header.id = path.targetInfo.id;
                        target.header.type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME;

                        if (PInvoke.DisplayConfigGetDeviceInfo(&target.header) is not 0)
                        {
                            continue;
                        }
                        var friendlyName = target.monitorFriendlyDeviceName.ToString();

                        dic[deviceName] = friendlyName;
                    }
                }
            }
        }

        public static ICollection<MonitorInfo> GetMonitorInfo(ICollection<MonitorInfo>? results = null)
        {
            results ??= [];
            results.Clear();
            EnumerateMonitorInfo(info =>
            {
                results.Add(info);
                return true;
            });
            return results;
        }
    }
}
