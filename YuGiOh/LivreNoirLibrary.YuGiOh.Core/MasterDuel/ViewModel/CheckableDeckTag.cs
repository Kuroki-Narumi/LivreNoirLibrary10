using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class CheckableDeckTag(DeckTag source) : CheckableObject
    {
        public DeckTag Source { get; } = source;
    }
}
