using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class CheckableDeckTag : CheckableObject, IDeckTag, IClear
    {
        public string Name { get; internal set => SetValue(ref field, value); } = "";
        public string SearchHint { get; internal set => SetValue(ref field, value); } = "";

        public void Clear()
        {
            Name = "";
            SearchHint = "";
            IsChecked = false;
        }

        public void Update(DeckTag source)
        {
            Name = source.Name;
            SearchHint = source.SearchHint;
        }
    }
}
