using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICard
    {
        /// <summary>
        /// This property is used when you want to receive change notifications while binding the card itself.
        /// </summary>
        Card ThisCard { get; }

        int Id { get; }

        string Name { get; }
        string Ruby { get; }
        string EnName { get; }
        CardType CardType { get; }
        string Text { get; }
        bool Unusable { get; }

        Attribute Attribute { get; }
        MonsterType MonsterType { get; }
        bool HasEffect { get; }
        Ability Ability { get; }
        int Level { get; }
        int Atk { get; }
        int Def { get; }
        int PendulumScale { get; }
        string PendulumText { get; }

        PackInfoCollection PackInfo { get; }
    }
}
