using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class Token : Card
    {
        public SortedCardList Generators { get; } = [];
        public SortedCardList Referers { get; } = [];

        public Token(int id, string name)
        {
            Id = id;
            Name = name;
            CardType = CardType.Token;
        }

        public void Clear()
        {
            Generators.Clear();
            Referers.Clear();
            Ruby = "";
            EnName = "";
            Text = "";
            PendulumText = "";
        }

        public void AddGenerator(Card card)
        {
            Generators.Add(card);
            UpdateText(card);
        }

        public void AddReferer(Card card)
        {
            Referers.Add(card);
            UpdateText(card);
        }

        private void UpdateText(Card card)
        {
            if (string.IsNullOrEmpty(Ruby))
            {
                Ruby = card.Ruby;
            }
            if (string.IsNullOrEmpty(EnName))
            {
                EnName = card.EnName;
            }
            if (string.IsNullOrEmpty(Text))
            {
                Text = card.Text;
            }
            if (string.IsNullOrEmpty(PendulumText))
            {
                PendulumText = card.PendulumText;
            }
        }
    }
}
