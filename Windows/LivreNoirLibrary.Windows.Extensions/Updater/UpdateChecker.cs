using System;
using System.Diagnostics;
using System.Windows;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LivreNoirLibrary.Windows
{
    public static class UpdateChecker
    {
        public static string GetAssemblyPath() => Path.Join(General.GetAssemblyDir(), Application.ResourceAssembly.GetName().Name);
        public static string GetUpdaterPath(string settingName) => Path.Join(General.GetAppDataPath(settingName), UpdaterName);

        public const string UpdaterName = "updater.exe";
        private static bool _checking;

        public static async Task CheckUpdate<T>(this T window, bool force = true, CancellationToken c = default)
            where T : Window, IUpdateCheck
        {
            if (!_checking && (force || window.CheckUpdate))
            {
                c.ThrowIfCancellationRequested();
                var updater = GetUpdaterPath(window.UpdaterLocation);
                if (File.Exists(updater))
                {
                    File.Delete(updater);
                }
                _checking = true;
                window.CheckUpdate = true;
                c.ThrowIfCancellationRequested();
                var info = await UpdateInfo.CheckVersion(window.VersionUrl, c);
                if (info is not null)
                {
                    if (window.NotifyNewVersion(info.Version))
                    {
                        await window.SetDispatcher(() => ExecuteUpdate(window, updater, info));
                    }
                    else
                    {
                        window.CheckUpdate = false;
                    }
                }
                else if (force)
                {
                    await window.SetDispatcher(window.NotifyNoUpdate);
                }
                c.ThrowIfCancellationRequested();
                _checking = false;
            }
        }

        public static void SaveUpdater(this UpdateInfo target, string updaterPath)
        {
            if (File.Exists(updaterPath))
            {
                using var file = File.OpenRead(updaterPath);
                using var ms = new MemoryStream();
                IOExtensions.Deflate(file, ms);
                target.Updater = ms.ToArray();
            }
        }

        public static UpdateInfo CreateUpdateInfo(string infoPath, string updaterPath, UpdateInfo? info = null)
        {
            info ??= new();
            SaveUpdater(info, updaterPath);
            Json.Save(infoPath, info, true);
            return info;
        }

        public static void ExecuteUpdate(this Window window, string updaterPath, UpdateInfo info)
        {
            using (var stream = General.CreateSafe(updaterPath))
            {
                IOExtensions.Inflate(info.Updater, stream);
            }
            ProcessStartInfo prc = new()
            {
                UseShellExecute = true,
                FileName = updaterPath,
                Verb = "runas",
                Arguments = $"\"{info.Url}\" \"{GetAssemblyPath()}.exe\"",
            };
            try
            {
                Process.Start(prc);
                window.Close();
            }
            catch (System.ComponentModel.Win32Exception)
            {

            }
        }
    }
}
