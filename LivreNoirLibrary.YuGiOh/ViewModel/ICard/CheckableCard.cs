using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class CheckableCard(Card card) : CardWrapperBase(card), ICheckableObject
    {
        [ObservableProperty]
        private bool _isChecked;
    }
}
