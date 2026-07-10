using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    public partial class LanguageData : CheckableObject, INamedObject
    {
        public const string DefaultName = "(default)";

        public string Name { get; set => SetValue(ref field, value); }
        public string Path { get; set => SetValue(ref field, value); }

        private LanguageData(string name, string path)
        {
            Name = name;
            Path = path;
        }

        internal static LanguageData CreateDefault() => new(DefaultName, "::invalid_path::");
        internal static bool TryGetData(string path, [MaybeNullWhen(false)]out LanguageData data)
        {
            if (Json.TryOpen<VocabBase>(path, out var obj))
            {
                data = new(obj.Language ?? "", path);
                return true;
            }
            data = null;
            return false;
        }
    }
}
