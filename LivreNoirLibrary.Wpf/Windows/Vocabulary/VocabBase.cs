using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows
{
    public partial class VocabBase : ObservableObjectBase
    {
        public const string DefaultLanguage = "default";
        public const string VocabDirname = "Vocab";
        public const string DefaultResourceName = $"/{VocabDirname}/{DefaultLanguage}.json";

        public static string VocabDir { get; private set; } = "";
        public static ObservableCollection<LanguageData> LanguageList { get; } = [];

        [JsonIgnore]
        [ObservableProperty]
        private LanguageData? _currentLanguage;

        protected static void SetupInstance<T>(string resourcePath = DefaultResourceName)
            where T : VocabBase, IVocabulary<T>, new()
        {
            try
            {
                var text = ResourceManager.GetText(resourcePath);
                T.Default = Json.Parse<T>(text);
                T.Current.UpdateVocabData(T.Default);
            }
            catch
            {
                T.Default = new();
            }

            var list = LanguageList;
            list.Clear();
            list.Add(LanguageData.GetDefault());
            VocabDir = Path.Join(General.GetAssemblyDir(), VocabDirname);
            if (Directory.Exists(VocabDir))
            {
                foreach (var path in Directory.GetFiles(VocabDir))
                {
                    if (LanguageData.TryGetData(path, out var data))
                    {
                        list.Add(data);
                    }
                }
            }
        }

        [ObservableProperty]
        protected string? _language;
        protected readonly Dictionary<string, VocabData> _dictionary = [];

        protected VocabData GetData([CallerMemberName] string key = "") => _dictionary.GetOrAdd(key);

        protected bool SetData(VocabData data, [CallerMemberName]string key = "")
        {
            var current = GetData(key);
            if (!current.Equals(data))
            {
                current.Update(data);
                SendPropertyChanged(key);
                return true;
            }
            return false;
        }

        protected internal void UpdateVocabData<T>(T source)
            where T : VocabBase
        {
            foreach (var (key, data) in source._dictionary)
            {
                SetData(data, key);
            }
        }

        public virtual void OnLoadVocabData() { }
    }

    public static class VocabExtension
    {
        public static void LinkToMenu<TVocab, TMenuItem>(this TVocab vocab, TMenuItem menu)
            where TVocab : VocabBase, IVocabulary<TVocab>
            where TMenuItem : MenuItem, new()
        {
            var items = menu.Items;
            items.Clear();
            foreach (var data in VocabBase.LanguageList)
            {
                TMenuItem m = new() { Header = data.Name };
                m.Click += (s, e) => OpenLanguageData(vocab, data);
                items.Add(m);
            }
        }

        public static void OpenLanguageData<T>(this T vocab, LanguageData language)
            where T : VocabBase, IVocabulary<T>
        {
            var current = T.Current;
            if (ReferenceEquals(current.CurrentLanguage, language))
            {
                return;
            }
            if (current.CurrentLanguage is not null)
            {
                current.CurrentLanguage.IsChecked = false;
            }
            if (Json.TryOpen<T>(language.Path, out var data))
            {
                current.UpdateVocabData(data);
            }
            else
            {
                language = VocabBase.LanguageList[0];
                current.UpdateVocabData(T.Default);
            }
            vocab.CurrentLanguage = language;
            language.IsChecked = true;
            vocab.OnLoadVocabData();
        }
    }
}
