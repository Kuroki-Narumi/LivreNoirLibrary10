using System;
using System.Collections.Generic;
using System.Drawing;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Devices.Display;

namespace LivreNoirLibrary.Win32Api
{
    public record MonitorInfo(nint Handle, Rectangle Rect, bool IsPrimary, string DeviceName)
    {
        public string? FriendlyName { get; internal set; }
    }

    partial class NativeMethods
    {
        public static ICollection<MonitorInfo> GetMonitorInfo(ICollection<MonitorInfo>? results = null)
        {
            results ??= [];
            results.Clear();

            unsafe
            {
                PInvoke.EnumDisplayMonitors(default, null, (hMonitor, _, rectPtr, data) =>
                {
                    var deviceName = "";
                    var isPrimary = false;
                    MONITORINFOEXW info = new();
                    info.monitorInfo.cbSize = (uint)sizeof(MONITORINFOEXW);
                    if (PInvoke.GetMonitorInfo(hMonitor, (MONITORINFO*)&info))
                    {
                        isPrimary = (info.monitorInfo.dwFlags & 1) is not 0;
                        deviceName = info.szDevice.ToString();
                    }
                    var rect = *rectPtr;
                    results.Add(new(hMonitor, new(rect.left, rect.top, rect.Width, rect.Height), isPrimary, deviceName));
                    return true;
                }, 0);
            }

            UpdateFriendlyName(results);

            return results;
        }

        private static readonly Dictionary<string, string> _device2friendly = [];

        public static unsafe void UpdateFriendlyName(IEnumerable<MonitorInfo> monitors)
        {
            var dic = _device2friendly;

            PInvoke.GetDisplayConfigBufferSizes(QUERY_DISPLAY_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, out var pathCount, out var modeCount);

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

                    if (PInvoke.DisplayConfigGetDeviceInfo(ref source.header) is not 0)
                    {
                        continue;
                    }
                    var deviceName = source.viewGdiDeviceName.ToString();

                    DISPLAYCONFIG_TARGET_DEVICE_NAME target = new();
                    target.header.size = (uint)sizeof(DISPLAYCONFIG_TARGET_DEVICE_NAME);
                    target.header.adapterId = path.targetInfo.adapterId;
                    target.header.id = path.targetInfo.id;
                    target.header.type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME;

                    if (PInvoke.DisplayConfigGetDeviceInfo(ref target.header) is not 0)
                    {
                        continue;
                    }
                    var friendlyName = target.monitorFriendlyDeviceName.ToString();

                    dic[deviceName] = friendlyName;
                }
            }

            foreach (var info in monitors)
            {
                info.FriendlyName = dic.GetValueOrDefault(info.DeviceName);
            }

            dic.Clear();
        }
    }
}
