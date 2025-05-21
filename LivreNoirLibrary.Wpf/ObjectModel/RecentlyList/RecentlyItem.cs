using System;

namespace LivreNoirLibrary.ObjectModel
{
    public partial class RecentlyItem(string path) : ObservableObjectBase
    {
        [ObservableProperty(Related = [nameof(Basename), nameof(Dirname)])]
        private string _path = path;

        public string? Basename => System.IO.Path.GetFileName(_path);
        public string? Dirname => System.IO.Path.GetDirectoryName(_path);
    }
}
