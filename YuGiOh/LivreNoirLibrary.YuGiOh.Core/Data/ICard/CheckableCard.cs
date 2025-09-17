using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class CheckableCard(Card card) : CardWrapperBase(card), ICheckableObject
    {
        public bool IsChecked { get; set => SetValue(ref field, value); }
    }
}
