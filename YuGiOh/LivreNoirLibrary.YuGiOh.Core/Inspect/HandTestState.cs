using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandTestState(List<int> deck) : IClear
    {
        public readonly List<int> Deck = deck;
        public int DeckIndex;
        public readonly List<int> Hand = [];
        public readonly List<DrawSourceType> DrawSource = [];
        public double Value1;
        public double Value2;
        public readonly List<HandConditions> Matched = [];

        public int DeckRemain => Deck.Count - DeckIndex;

        public void Clear()
        {
            DeckIndex = 0;
            Hand.Clear();
            DrawSource.Clear();
            Value1 = 0;
            Value2 = 0;
            Matched.Clear();
        }

        public void Setup(int handCount, ReadOnlySpan<DrawSourceType> drawSource)
        {
            Hand.AddRange(Deck.AsSpan()[..handCount]);
            DeckIndex = handCount;
            DrawSource.AddRange(drawSource);
        }

        public void Draw(int count)
        {
            Hand.AddRange(Deck.Slice(DeckIndex, count));
            DeckIndex += count;
        }

        public HandTestState Clone()
        {
            HandTestState state = new(Deck)
            {
                DeckIndex = DeckIndex,
                Value1 = Value1,
                Value2 = Value2
            };
            state.Hand.AddRange(Hand);
            state.DrawSource.AddRange(DrawSource);
            state.Matched.AddRange(Matched);
            return state;
        }

        public void CopyFrom(HandTestState other)
        {
            Clear();
            DeckIndex = other.DeckIndex;
            Hand.AddRange(other.Hand);
            DrawSource.AddRange(other.DrawSource);
            Value1 = other.Value1;
            Value2 = other.Value2;
            Matched.AddRange(other.Matched);
        }

        public bool Update(double value1, double value2, List<HandConditions> matched)
        {
            if (value1 > Value1 || (value1 == Value1 && value2 > Value2))
            {
                Value1 = value1;
                Value2 = value2;
                Matched.Clear();
                Matched.AddRange(matched);
                return true;
            }
            return false;
        }
    }
}
