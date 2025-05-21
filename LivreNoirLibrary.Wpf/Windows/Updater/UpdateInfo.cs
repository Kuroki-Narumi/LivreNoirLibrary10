using System;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public class UpdateInfo
    {
        private static readonly HttpClient _client = new();

        [JsonPropertyName("version")]
        public Version Version { get; set; } = new(0, 0, 0);

        [JsonPropertyName("url")]
        public string Url { get; set; } = "";

        [JsonConverter(typeof(Text.Base64JsonConverter))]
        public byte[] Updater { get; set; } = [];

        public static Version GetCurrentVersion()
        {
            var asm = Application.ResourceAssembly.GetName();
            return asm.Version ?? new(1, 0, 0);
        }

        public static async Task<UpdateInfo?> GetUpdateVersion(string infoUrl)
        {
            try
            {
                var info = await HttpClientJsonExtensions.GetFromJsonAsync<UpdateInfo>(_client, infoUrl);
                return info;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<UpdateInfo?> CheckVersion(string infoUrl)
        {
            var info = await GetUpdateVersion(infoUrl);
            if (info is not null && !string.IsNullOrEmpty(info.Url) && info.Updater.Length > 0 && info.Version > GetCurrentVersion())
            {
                return info;
            }
            return null;
        }
    }
}
