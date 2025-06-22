using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows
{
    public partial class LanguageData : ObservableObjectBase, INamedObject
    {
        public const string DefaultName = "(default)";

        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _path;
        [ObservableProperty]
        private bool _isChecked;

        private LanguageData(string name, string path)
        {
            _name = name;
            _path = path;
        }

        internal static LanguageData GetDefault() => new(DefaultName, "");
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
