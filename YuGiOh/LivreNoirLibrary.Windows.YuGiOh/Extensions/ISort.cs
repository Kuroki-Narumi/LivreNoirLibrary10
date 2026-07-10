using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public interface ISort
    {
        void SortInGridView(ListBox control, string propertyName);
    }
}
