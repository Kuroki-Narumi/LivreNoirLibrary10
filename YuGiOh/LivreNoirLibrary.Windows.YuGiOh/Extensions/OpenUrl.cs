using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class Extensions
    {
        public static void OpenUrl_Card(this FrameworkElement element, int cid, bool tcg)
        {
            if (Window.GetWindow(element) is { } window)
            {
                window.ShellOpen(LivreNoirLibrary.YuGiOh.Scraping.Url.Card(cid, tcg));
            }
        }

        public static void OpenUrl_Pack(this FrameworkElement element, string pid)
        {
            if (Window.GetWindow(element) is { } window)
            {
                window.ShellOpen(LivreNoirLibrary.YuGiOh.Scraping.Url.Pack(pid));
            }
        }
    }
}
