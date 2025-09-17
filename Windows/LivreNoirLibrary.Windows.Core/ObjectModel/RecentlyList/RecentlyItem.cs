using System;

namespace LivreNoirLibrary.ObjectModel
{
    public partial class RecentlyItem(string path) : ObservableObjectBase
    {
        public string Path { get; set => SetValue(ref field, value, [nameof(Basename), nameof(Dirname)]); } = path;

        public string? Basename => System.IO.Path.GetFileName(Path);
        public string? Dirname => System.IO.Path.GetDirectoryName(Path);
    }
}
