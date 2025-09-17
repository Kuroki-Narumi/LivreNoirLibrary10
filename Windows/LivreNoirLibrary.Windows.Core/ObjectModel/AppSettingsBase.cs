using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract class AppSettingsBase : ObservableObjectBase
    {
        public const string DefaultSettingFileName = "Settings";

        private static readonly Dictionary<string, string> _local_data_dir = [];

        protected static string GetFilePath(string appName, string settingName)
        {
            if (!_local_data_dir.TryGetValue(appName, out var dir))
            {
                dir = General.GetAppDataPath(appName);
                _local_data_dir.Add(appName, dir);
            }
            return Path.Join(dir, $"{settingName}.json");
        }

        protected static T Load<T>(string appName, string settingName = DefaultSettingFileName)
            where T : AppSettingsBase, new()
        {
            if (Json.TryOpen<T>(GetFilePath(appName, settingName), out var setting))
            {
                setting.OnLoad();
                return setting;
            }
            else
            {
                return new();
            }
        }

        protected static T Load<T>(string appName, params string[] settingNames)
            where T : AppSettingsBase, new()
        {
            foreach (var name in settingNames)
            {
                if (Json.TryOpen<T>(GetFilePath(appName, name), out var setting))
                {
                    setting.OnLoad();
                    return setting;
                }
            }
            return new();
        }

        protected void SaveInstance(string appName, string settingName = DefaultSettingFileName)
        {
            OnSave();
            Json.Save(GetFilePath(appName, settingName), this, true);
        }

        protected virtual void OnLoad() { }
        protected virtual void OnSave() { }
    }
}
