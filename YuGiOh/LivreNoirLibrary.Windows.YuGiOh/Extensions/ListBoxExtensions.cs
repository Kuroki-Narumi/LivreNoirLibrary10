using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class ListBoxExtensions
    {
        public static void SetCloningSource(this ItemsControl control, ICardProvider? provider)
        {
            if (provider is not null)
            {
                control.ItemsSource = new CloningCardList(provider);
            }
            else
            {
                control.ItemsSource = null;
            }
        }
    }
}
