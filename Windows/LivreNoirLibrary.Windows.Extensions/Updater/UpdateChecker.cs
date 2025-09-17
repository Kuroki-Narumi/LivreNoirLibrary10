using System;
using System.Diagnostics;
using System.Windows;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Windows
{
    public static class UpdateChecker
    {
        public static string GetAssemblyPath() => Path.Join(General.GetAssemblyDir(), Application.ResourceAssembly.GetName().Name);
        public static string GetUpdaterPath(string settingName) => Path.Join(General.GetAppDataPath(settingName), UpdaterName);

        public const string UpdaterName = "updater.exe";
        private static bool _checking;

        public static async void CheckUpdate<T>(this T window, bool force = true)
            where T : Window, IUpdateCheck
        {
            if ((force || window.CheckUpdate) && !_checking)
            {
                var updater = GetUpdaterPath(window.SettingName);
                if (File.Exists(updater))
                {
                    File.Delete(updater);
                }
                _checking = true;
                window.CheckUpdate = true;
                var info = await UpdateInfo.CheckVersion(window.VersionUrl);
                if (info is not null)
                {
                    if (window.ShowMessage_YesNo(string.Format(window.GetMessage_NewVersion(), info.Version), MessageBoxImage.Information) is MessageBoxResult.Yes)
                    {
                        window.CheckUpdate = true;
                        window.SetDispatcher(() => ExecuteUpdate(window, updater, info));
                    }
                    else
                    {
                        window.CheckUpdate = false;
                    }
                }
                else if (force)
                {
                    window.ShowMessage_OK(window.GetMessage_NoUpdate(), MessageBoxImage.Information);
                }
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
