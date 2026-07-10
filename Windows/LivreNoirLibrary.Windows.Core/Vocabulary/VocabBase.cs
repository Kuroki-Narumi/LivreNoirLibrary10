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
using System.Linq;
using System;

namespace LivreNoirLibrary.Windows
{
    public partial class VocabBase : ObservableObjectBase
    {
        public const string DefaultLanguage = "default";
        public const string VocabDirname = "Vocab";
        public const string DefaultResourceName = $"/{VocabDirname}/{DefaultLanguage}.json";

        [JsonIgnore]
        public LanguageData? CurrentLanguage { get; set => SetValue(ref field, value); }

        protected static void SetupInstance<T>(string resourcePath = DefaultResourceName, string vocabDir = VocabDirname)
            where T : VocabBase, IVocabulary<T>
        {
            try
            {
                var text = ResourceManager.GetText(resourcePath);
                if (Json.TryParse<T>(text, out var source))
                {
                    T.Default.UpdateVocabData(source);
                }
            }
            catch
            {

            }

            var list = T.Languages;
            list.Clear();
            list.Add(LanguageData.CreateDefault());
            var dir = Path.Join(General.GetAssemblyDir(), vocabDir);
            if (Directory.Exists(dir))
            {
                foreach (var path in Directory.GetFiles(dir))
                {
                    if (LanguageData.TryGetData(path, out var data))
                    {
                        list.Add(data);
                    }
                }
            }

            OpenLanguageData<T>(null, true);
        }

        public string? Language { get; set => SetValue(ref field, value); }
        private readonly Dictionary<string, VocabData> _dictionary = [];

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

        protected bool SetData(string key, string? value, string? keyTip = null)
        {
            var current = GetData(key);
            if (!current.Equals(value, keyTip))
            {
                current.Update(value, keyTip);
                SendPropertyChanged(key);
                return true;
            }
            return false;
        }

        public virtual void UpdateVocabData<T>(T? source)
            where T : VocabBase
        {
            if (source is not null)
            {
                foreach (var (key, data) in source._dictionary)
                {
                    SetData(data, key);
                }
            }
        }

        public virtual void OnLanguageChanged() { }

        public static void CreateMenuItems<TVocab, TMenuItem>(ItemCollection items)
            where TVocab : VocabBase, IVocabulary<TVocab>
            where TMenuItem : MenuItem, new()
        {
            items.Clear();
            foreach (var data in TVocab.Languages)
            {
                TMenuItem m = new() { Header = data.Name };
                m.Click += (s, e) => OpenLanguageData<TVocab>(data);
                items.Add(m);
            }
        }

        public static void OpenLanguageData<T>(LanguageData? language, bool force = false)
            where T : VocabBase, IVocabulary<T>
        {
            var current = T.Current;
            if (!force && ReferenceEquals(current.CurrentLanguage, language))
            {
                return;
            }
            current.CurrentLanguage?.IsChecked = false;
            if (language is not null && Json.TryOpen<T>(language.Path, out var data))
            {
                current.UpdateVocabData(data);
            }
            else
            {
                language = T.Languages.First();
                current.UpdateVocabData(T.Default);
            }
            current.CurrentLanguage = language;
            language.IsChecked = true;
            current.OnLanguageChanged();
        }
    }
}
