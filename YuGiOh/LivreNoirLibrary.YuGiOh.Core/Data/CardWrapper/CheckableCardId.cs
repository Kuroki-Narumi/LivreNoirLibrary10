using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class CheckableCardId(int id) : CardId(id), ICheckableObject
    {
        public bool IsChecked { get; set => SetValue(ref field, value); }
    }
}
