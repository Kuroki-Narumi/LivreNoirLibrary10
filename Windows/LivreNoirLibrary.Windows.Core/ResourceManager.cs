using System;
using System.IO;
using System.Text;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static class ResourceManager
    {
        public static Stream GetStream(string path)
        {
            return Application.GetResourceStream(new Uri(path, UriKind.RelativeOrAbsolute)).Stream;
        }

        public static string GetText(string path)
        {
            using StreamReader reader = new(GetStream(path), Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}
