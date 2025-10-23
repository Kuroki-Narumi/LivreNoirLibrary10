using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public abstract class StringKeyCollection<T>() : ObservableSortedList<string, T>(StringConversion.DefaultComparer)
    {
    }
}
