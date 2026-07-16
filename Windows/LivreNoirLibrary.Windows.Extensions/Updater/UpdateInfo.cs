using LivreNoirLibrary.ObjectModel;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public class UpdateInfo
    {
        public Version Version { get; set; } = new(0, 0, 0);

        public string Url { get; set; } = "";

        [JsonConverter(typeof(Text.Base64JsonConverter))]
        public byte[] Updater { get; set; } = [];

        public static Version GetCurrentVersion()
        {
            var asm = Application.ResourceAssembly.GetName();
            return asm.Version ?? new(1, 0, 0);
        }

        public static async Task<UpdateInfo?> GetUpdateVersion(string infoUrl, CancellationToken c = default)
        {
            try
            {
                var info = await HttpClientJsonExtensions.GetFromJsonAsync<UpdateInfo>(HttpClientPool.Instance, infoUrl, c);
                return info;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<UpdateInfo?> CheckVersion(string infoUrl, CancellationToken c = default)
        {
            var info = await GetUpdateVersion(infoUrl, c);
            if (info is not null && !string.IsNullOrEmpty(info.Url) && info.Updater.Length > 0 && info.Version > GetCurrentVersion())
            {
                return info;
            }
            return null;
        }
    }
}
