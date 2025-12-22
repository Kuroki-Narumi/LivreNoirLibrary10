using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media.Bms;

namespace LivreNoirLibrary.SandBox
{
    internal class AppSettings : AppSettingsBase
    {
        public static AppSettings Instance { get; } = Load<AppSettings>(nameof(SandBox));
        public static void Save() => Instance.SaveInstance(nameof(SandBox));

        public int SkinIndex { get; set; } = 0;
        public Dictionary<string, Dictionary<string, string>> BmsSkinOptions { get; set; } = [];
        public BmsVideoCreatorOptions BmsVideoCreatorOptions { get; set; } = new();
    }
}
